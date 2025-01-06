using System;
using System.Collections.Generic;
using System.Threading;
using Android.Content;
using CloudBanking.BaseControl;
using CloudBanking.Common;
using CloudBanking.Entities;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
using CloudBanking.ServiceLocators;
using CloudBanking.ShellContainers;
using CloudBanking.Utilities;
using Plugin.CurrentActivity;
using static CloudBanking.Utilities.UtilEnum;

namespace CloudBanking.UITestApp
{
    public class ShellClient : BaseShellClient.BaseShellClient, IShellClient
    {
        public AppState CurrentAppState { get; set; }

        const string TAG = "ShellClient";

        MainApplication _context;
        bool _showMsgBox;
        bool _processing = false;
        ProcessingData _processingData;

        ShellContainers.ApplicationFlow _applicationFlow
        {
            get
            {
                if (_context == null)
                    return null;

                return _context.PaymentFlow;
            }
        }

        public ShellClient(MainApplication context, IDialogBuilder dialgoBuilder) : base(dialgoBuilder)
        {
            _context = context;
        }

        public IDictionary<string, string> GetInfo(TransData transData, bool fShowMsgBox = false)
        {
            _showMsgBox = fShowMsgBox;

            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_GET_INFO, true);

            _showMsgBox = false;

            if (data == null)
                return null;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS)
            {
                if (data.ResponseData != null && data.ResponseData is IDictionary<string, string>)
                    return (IDictionary<string, string>)data.ResponseData;

                if (fShowMsgBox) ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR, StringIds.STRING_ERRORDATA, false, GlobalResource.MB_OK, GlobalResource.MB_ICONDECLINED_BMP);

                return null;
            }

            return null;
        }

        public bool IsTransactionFound(TransData transData, bool fShowMsgBox = false)
        {
            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_CHECK_TRANSACTION, true);

            if (data == null)
                return false;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS)
                return ((bool)data.ResponseData);

            return false;
        }

        public IList<BatchReport> GetBatchReport(TransData transData, bool fShowMsgBox = false)
        {
            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_GET_BATCH, true);

            if (data == null)
                return null;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS)

                if (data.ResponseData != null && data.ResponseData is IList<BatchReport>)

                    return ((IList<BatchReport>)data.ResponseData);

            return null;
        }

        public bool IsPendingRecordExist(TransData transData, ref string responseMesage, bool fShowMsgBox = false)
        {
            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_CHECK_PENDING_RECORD_EXIST, true);

            if (data == null)
            {
                responseMesage = StringIds.STRING_UNKNOWN_ERROR;

                return true;
            }

            switch (data.usResult)
            {
                case TransResponse.SHELLERROR_SUCCESS:

                    if (data.ResponseData != null && data.ResponseData is bool)

                        return ((bool)data.ResponseData);

                    else

                        responseMesage = StringIds.STRING_ERRORDATA;

                    break;

                case TransResponse.SHELLERROR_SHELL_NOT_INSTALLED:
                case TransResponse.SHELLERROR_SHELL_NOT_READY:

                    responseMesage = StringIds.STRING_SHELL_IS_NOT_READY;

                    break;

                default:

                    responseMesage = string.IsNullOrEmpty(data.ErrorMessage) ? StringIds.STRING_UNKNOWN_ERROR : data.ErrorMessage;

                    break;
            }

            return true;
        }

        public TransResponse DoTransaction(TransData transData, bool fShowMsgBox = false)
        {
            _showMsgBox = fShowMsgBox;

            var res = GetData(transData, GlobalConstants.SHELL_COMMAND_DO_TRANSACTION, false);

            _showMsgBox = false;

            return res;
        }

        public bool IsShellReady(bool fShowMsgBox = false)
        {
            return IsShellReady();
        }

        public bool IsShellInstalled()
        {
            return true;
        }

        public void OpenShell()
        {
            return;
        }

        TransResponse GetData(TransData request, string command, bool fWithLoading)
        {
            ProcessingData process = null;
            TransResponse res = null;

            if (fWithLoading)
            {
                process = CreateProcessingData();
            }

            if (command == GlobalConstants.SHELL_COMMAND_DO_TRANSACTION)
            {
                var tokenSource = new CancellationTokenSource();

                try
                {
                    //disble thread in processor because we are in a transaction
                    //if (PaymentProcessors.GlobalProcessorTimerChecking != null)
                    //{
                    //    PaymentProcessors.GlobalProcessorTimerChecking.Elapsed -= PaymentProcessors.GlobalProcessorTimerChecking_Elapsed;
                    //}

                    res = _applicationFlow.ProcessTrans(request, tokenSource.Token, (m) =>
                    {
                    }, (sm) =>
                    {
                    });

                    var fContinue = true;

                    while (fContinue && res.usResult == TransResponse.SHELLERROR_SUCCESS)
                    {
                        switch (res.iCommand)
                        {
                            case TransResponse.APPCOMMAND_CONTINUE_FINALCOMPLETION:

                                request.FunctionType = FunctionType.PreAuthComplete;

                                res = _applicationFlow.ProcessTrans(request, tokenSource.Token, (m) =>
                                {
                                }, (sm) =>
                                {
                                });

                                break;

                            default:

                                fContinue = false;
                                break;
                        }
                    }

                    process?.OnDismiss?.Invoke(false);

                    DialogBuilder.fInPaymentProcess = false;

                    if (res.fCheckCardRemoved)
                        _applicationFlow.CheckCardInserted(ENTRYMODE.EM_SMC, true);

                    ServiceLocator.Instance.Get<ILoggerService>()?.ClearState();

                    //Enable thread in processor because we complete in a transaction
                    //if (PaymentProcessors.GlobalProcessorTimerChecking != null)
                    //{
                    //    PaymentProcessors.GlobalProcessorTimerChecking.Elapsed -= PaymentProcessors.GlobalProcessorTimerChecking_Elapsed;
                    //    PaymentProcessors.GlobalProcessorTimerChecking.Elapsed += PaymentProcessors.GlobalProcessorTimerChecking_Elapsed;
                    //}

                    GC.Collect();

                    ServiceLocator.Instance.Get<ILoggerService>()?.TrackMemory("Complete a transaction");

                }
                catch (Exception ex)
                {
                    _context.Handler.Post(() =>
                    {
                        throw ex;
                    });

                    ServiceLocator.Instance.Get<ILoggerService>().Error(ex, "_paymentFlow.ProcessTrans");

                    tokenSource.Cancel();

                    if (res == null)
                        res = new TransResponse();

                    res.usResult = TransResponse.SHELLERROR_CANNOTPROCESS;
                    res.lzResult = "Payment can't process";
                }
            }
            else
            {
                res = _applicationFlow?.GetData(command, request);
            }

            if (res == null)
            {
                return new TransResponse()
                {
                    usResult = TransResponse.SHELLERROR_CANCELLED
                };
            }

            if (res.usResult != TransResponse.SHELLERROR_SUCCESS)
            {
                if (!string.IsNullOrEmpty(res.ErrorMessage) && _showMsgBox)
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR, res.ErrorMessage, true, GlobalResource.MB_OK, GlobalResource.MB_ICONDECLINED_BMP);

                return res;
            }

            return res;
        }

        bool IsShellReady()
        {
            if (_applicationFlow == null)
                return false;

            if (_applicationFlow.IsInitializating || !_applicationFlow.IsInitialized)
                return false;

            return true;
        }

        public IList<PaymentViewModel> GetPaymentRecords(TransData transData, bool fShowMsgBox = false)
        {
            _showMsgBox = fShowMsgBox;

            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_GET_PAYMENT_RECORDS, true);

            _showMsgBox = false;

            if (data == null)
                return null;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS)
            {
                if (data.ResponseData != null && data.ResponseData is IList<PaymentViewModel>)
                    return (IList<PaymentViewModel>)data.ResponseData;

                if (fShowMsgBox) ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR, StringIds.STRING_ERRORDATA, true, GlobalResource.MB_OK, GlobalResource.MB_ICONDECLINED_BMP);
            }

            return null;
        }

        public IList<PrintJobViewModel> GetPrintJobRecords(TransData transData, bool fShowMsgBox = false)
        {
            _showMsgBox = fShowMsgBox;

            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_GET_PRINTJOB_RECORDS, true);

            _showMsgBox = false;

            if (data == null)
                return null;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS)
            {
                if (data.ResponseData != null && data.ResponseData is IList<PrintJobViewModel>)
                    return (IList<PrintJobViewModel>)data.ResponseData;

                if (fShowMsgBox)
                {
                    data.fShowErrorMessage = false;

                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR, StringIds.STRING_ERRORDATA, true, GlobalResource.MB_OK, GlobalResource.MB_ICONDECLINED_BMP);
                }
            }

            return null;
        }

        public IList<RecordViewModel> GetPreAuthPendingRecords(TransData transData, bool fShowMsgBox = false)
        {
            _showMsgBox = fShowMsgBox;

            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_GET_PREAUTH_PENDING_RECORDS, true);

            _showMsgBox = false;

            if (data == null)
                return null;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS)
            {
                if (data.ResponseData != null && data.ResponseData is IList<RecordViewModel>)
                    return (IList<RecordViewModel>)data.ResponseData;

                if (fShowMsgBox)
                {
                    data.fShowErrorMessage = false;

                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR, StringIds.STRING_ERRORDATA, true, GlobalResource.MB_OK, GlobalResource.MB_ICONDECLINED_BMP);
                }
            }

            return null;
        }

        public bool IsAdviceRecordExist(TransData transData, ref string responseMesage, bool fShowMsgBox = false)
        {
            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_CHECK_ADVICE_RECORD_EXIST, true);

            if (data == null)
            {
                responseMesage = StringIds.STRING_UNKNOWN_ERROR;

                return true;
            }

            switch (data.usResult)
            {
                case TransResponse.SHELLERROR_SUCCESS:

                    if (data.ResponseData != null && data.ResponseData is bool)

                        return ((bool)data.ResponseData);

                    else

                        responseMesage = StringIds.STRING_ERRORDATA;

                    break;

                case TransResponse.SHELLERROR_SHELL_NOT_INSTALLED:
                case TransResponse.SHELLERROR_SHELL_NOT_READY:

                    responseMesage = StringIds.STRING_SHELL_IS_NOT_READY;

                    break;

                default:

                    responseMesage = string.IsNullOrEmpty(data.ErrorMessage) ? StringIds.STRING_UNKNOWN_ERROR : data.ErrorMessage;

                    break;
            }

            return true;
        }

        public IList<EnabledMerchant> GetEnabledMerchants(TransData transData, ref ushort usResult)
        {
            var data = GetData(transData, GlobalConstants.SHELL_COMMAND_GET_ENABLED_MERCHANTS, true);

            if (data == null)
                return null;

            usResult = data.usResult;

            if (data.usResult == TransResponse.SHELLERROR_SUCCESS && data.ResponseData is IList<EnabledMerchant>)
                return ((IList<EnabledMerchant>)data.ResponseData);

            return null;
        }

        public IList<PrintJobViewModel> GetAllPrintJobRecords(TransData transData, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public TransResponse DoTransaction(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetInfo(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public bool IsTransactionFound(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IList<PaymentViewModel> GetPaymentRecords(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IList<PrintJobViewModel> GetPrintJobRecords(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IList<PrintJobViewModel> GetAllPrintJobRecords(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public PrintJob GetPrintJob(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IList<RecordViewModel> GetPreAuthPendingRecords(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IList<BatchReport> GetBatchReport(TransData transData, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public bool IsPendingRecordExist(TransData transData, ref string responseMesage, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public bool IsAdviceRecordExist(TransData transData, ref string responseMesage, out bool isIdleTimeout, bool fShowMsgBox = false)
        {
            throw new NotImplementedException();
        }

        public IList<EnabledMerchant> GetEnabledMerchants(TransData transData, ref ushort usResult, out bool isIdleTimeout)
        {
            throw new NotImplementedException();
        }
    }
}

