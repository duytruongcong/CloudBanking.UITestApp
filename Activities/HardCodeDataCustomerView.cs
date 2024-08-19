using CloudBanking.BaseControl;
using CloudBanking.Common;
using CloudBanking.Entities;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
using CloudBanking.ShellContainers;
using CloudBanking.ShellModels;
using CloudBanking.ShellUI;
using CloudBanking.UI;
using CloudBanking.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using static CloudBanking.Utilities.UtilEnum;
using static Java.Util.Jar.Attributes;

namespace CloudBanking.UITestApp
{
    public partial class TestActivity : BaseActivity
    {
        private void ShowCusViewRequestCardDialog(CaseDialog caseDialog)
        {
            var RequestDlgData = new RequestCardDlgData();
            var pInitProcessData = new ShellInitProcessData()
            {
                lAmount = 10000,
                lDiscount = 0,
                lDiscountPercent = 0,
                PaymentDonations = new PaymentDonations()
                {
                    lTotalDonations = 0,
                },
                lTipAmount = 0,
                lCashOut = 2000,
                lCashOutFee = 0,
                lSurChargeFee = 0,
                lSurChargePercent = 0,
                lAccountSurChargeFee = 0,
                lAccountSurChargePercent = 0,
                PaymentVouchers = new PaymentVouchers()
                {
                },
            };

            RequestDlgData.fNoPresentCard = false;
            //RequestDlgData.fShowCardFees = true;
            RequestDlgData.pInitProcessData = pInitProcessData;
            RequestDlgData.fMultiplePayments = false;
            RequestDlgData.fCanCancel = true;
            RequestDlgData.lTotal = pInitProcessData.lAmount
                                    + pInitProcessData.lTipAmount
                                    + pInitProcessData.lCashOut
                                    + (pInitProcessData.PaymentDonations != null ? pInitProcessData.PaymentDonations.lTotalDonations : 0)
                                    + (pInitProcessData.PaymentVouchers != null ? pInitProcessData.PaymentVouchers.lTotalVouchers : 0);

            RequestDlgData.PresentCardTitleId = StringIds.STRING_PRESENTCARD_TITLE;
            RequestDlgData.fAliPay = false;
            RequestDlgData.fWePay = false;
            RequestDlgData.IsEmulator = true;
            RequestDlgData.fShowMenu = false;
            RequestDlgData.fMultiTender = false;

            RequestDlgData.fVisa = true;
            RequestDlgData.fMasterCard = true;
            RequestDlgData.fDiners = true;
            RequestDlgData.fAmex = true;
            RequestDlgData.fJBC = true;
            RequestDlgData.fUnionPay = true;
            RequestDlgData.fTroy = true;
            RequestDlgData.fDiscover = true;
            RequestDlgData.fOtherPay = false;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    RequestDlgData.iFunctionButton = FunctionType.Purchase;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.fManualPay = true;
                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    break;

                case CaseDialog.CASE2:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = false;
                    RequestDlgData.fManualPay = false;

                    break;

                case CaseDialog.CASE3:

                    RequestDlgData.iFunctionButton = FunctionType.Cash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.fManualPay = false;
                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    break;

                case CaseDialog.CASE4:

                    RequestDlgData.iFunctionButton = FunctionType.Refund;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.fManualPay = true;

                    break;

                case CaseDialog.CASE5:

                    RequestDlgData.iFunctionButton = FunctionType.PreAuth;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = false;
                    RequestDlgData.fManualPay = false;
                    RequestDlgData.PresentCardSubTitleId = StringIds.STRING_OPEN_PREAUTH_UPCASE;

                    break;

                case CaseDialog.CASE6:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = false;
                    RequestDlgData.fManualPay = true;

                    break;

                case CaseDialog.CASE7:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.fManualPay = false;

                    break;

                case CaseDialog.CASE8:

                    RequestDlgData.iFunctionButton = FunctionType.CardStatusCheck;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.fManualPay = true;
                    RequestDlgData.PresentCardSubTitleId = StringIds.STRING_CARD_STATUS_CHECK_UPCASE;

                    break;
            }

            var lszTitle = string.Empty;
            var totalTitleId = string.Empty;
            switch (RequestDlgData.iFunctionButton)
            {
                case FunctionType.Refund: lszTitle = StringIds.STRING_FUNCTIONTYPES_REFUND; totalTitleId = StringIds.STRING_FUNCTIONTYPES_REFUND; break;
                case FunctionType.Purchase: lszTitle = StringIds.STRING_PRESENTCARD_TITLE; totalTitleId = StringIds.STRING_PURCHASE; break;
                case FunctionType.PurchaseCash: lszTitle = StringIds.STRING_PRESENTCARD_TITLE; totalTitleId = StringIds.STRING_PURCHASE_AND_CASH; break;
                case FunctionType.Deposit: lszTitle = StringIds.STRING_FUNCTIONTYPES_DEPOSIT; totalTitleId = StringIds.STRING_DEPOSIT; break;
                case FunctionType.CardAuthentication: lszTitle = StringIds.STRING_FUNCTIONTYPES_CARDAUTHENTICATION; totalTitleId = StringIds.STRING_CARD_AUTH; break;
                case FunctionType.TestComm: lszTitle = StringIds.STRING_FUNCTIONTYPES_TESTCOMM; totalTitleId = StringIds.STRING_TESTCOMM; break;
                case FunctionType.CardStatusCheck: lszTitle = StringIds.STRING_FUNCTIONTYPES_CARDSTATUSCHECK; totalTitleId = StringIds.STRING_CARD_CHECK_OPTIONS; break;
                case FunctionType.PreAuth: lszTitle = StringIds.STRING_FUNCTIONTYPES_PREAUTH; totalTitleId = StringIds.STRING_PRE_AUTH; break;
                case FunctionType.CashOut: lszTitle = StringIds.STRING_CASHOUT; totalTitleId = StringIds.STRING_CASHOUT; break;
                case FunctionType.Void: lszTitle = StringIds.STRING_FUNCTIONTYPES_VOID; totalTitleId = StringIds.STRING_VOID; break;
                case FunctionType.Adjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_ADJUST; totalTitleId = StringIds.STRING_ADJUST; break;
                case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; totalTitleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
            }

            RequestDlgData.szTotalTitle = totalTitleId;

            if (RequestDlgData.fSmart && RequestDlgData.fMSR && RequestDlgData.fRfid)
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_INSERT_SWIPE_TAP_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_PRESENT_INSERT_OR_SWIPE_CARD_UPCASE;
            }
            else if (RequestDlgData.fSmart && RequestDlgData.fRfid)
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_INSERT_TAP_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_INSERTORTAPCARD;
            }
            else if (RequestDlgData.fSmart && RequestDlgData.fMSR)
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_INSERT_SWIPE_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_INSERTORSWIPECARD;
            }
            else if (RequestDlgData.fSmart)
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_INSERT_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_INSERTCARD;
            }
            else if (RequestDlgData.fMSR && RequestDlgData.fRfid)
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_SWIPE_TAP_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_TAPORSWIPECARD;
            }
            else if (RequestDlgData.fRfid)
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_TAP_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_PLEASE_PRESENT_CARD_UPCASE;
            }
            else
            {
                RequestDlgData.CardIconResId = IconIds.VECTOR_SWIPE_CARD;

                RequestDlgData.PresentCardTitleId = StringIds.STRING_SWIPECARD;
            }

            //var requestCardDialog = new CusViewRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            //{
            //}, RequestDlgData);

            //requestCardDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //requestCardDialog.Show(this);
        }

        private void ShowCusViewEnterPinDialog(CaseDialog caseDialog)
        {
            GetPinNumberDlgData data = new GetPinNumberDlgData();

            long lTotal = 12000;

            data.lAmount = lTotal;

            data.fShowBackButton = true;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    data.AccountType = AccountType.ACCOUNT_TYPE_CREDIT;
                    data.IsEmulator = false;
                    data.fPinByPass = false;
                    break;

                case CaseDialog.CASE2:

                    data.AccountType = AccountType.ACCOUNT_TYPE_SAVINGS;
                    data.IsEmulator = false;
                    data.fPinByPass = true;
                    break;

                case CaseDialog.CASE3:

                    data.AccountType = 0;
                    data.IsEmulator = true;
                    data.fPinByPass = false;
                    break;

                case CaseDialog.CASE4:

                    data.AccountType = 0;
                    data.IsEmulator = true;
                    data.fPinByPass = true;
                    break;
            }

            //var enterPinDialog = new CusViewEnterPinDialog(null, data);
            //enterPinDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //enterPinDialog.Show(this);
        }

        private void ShowCusViewEntryCVVDialog()
        {
            var entrySecurityData = new EntryCVVDlgData();

            //var entryCVVDialog = new ShellUI.CusViewEntryCVVDialog(StringIds.STRING_CSC_CODE, null, entrySecurityData);
            //entryCVVDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //entryCVVDialog.Show(this);
        }

        private void ShowCusViewApprovalDialog(CaseDialog caseDialog)
        {
            string lpszEntryModeString = "";

            ApprovalDlgData DlgData = new ApprovalDlgData();

            DlgData.lBalance = 111111;

            DlgData.iEntryMode = ENTRYMODE.EM_MOTO;

            var lpszAboveMainString = StringIds.STRING_PURCHASE;
            if (!string.IsNullOrEmpty(lpszAboveMainString))
            {
                switch (DlgData.iEntryMode)
                {
                    case ENTRYMODE.EM_MOTO:

                        lpszEntryModeString = Localize.GetString(StringIds.STRING_EM_MOTO).ToUpper() + " ";

                        break;

                    case ENTRYMODE.EM_MANUAL:

                        lpszEntryModeString = Localize.GetString(StringIds.STRING_EM_MANUAL).ToUpper() + " ";

                        break;

                    default:
                        break;
                }


                DlgData.lpszAboveMainString = lpszEntryModeString + Localize.GetString(lpszAboveMainString).ToUpper();
            }

            bool fPrintMerchantFirst = true;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    DlgData.lPurchaseApproval = 12000;
                    DlgData.PrintStage = PrintStage.PrintComplete;
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);
                    DlgData.fApproved = true;
                    fPrintMerchantFirst = true;
                    DlgData.lpszThirdResult = fPrintMerchantFirst ? Localize.GetString(StringIds.STRING_PRINT_MERCHANT_COPY).ToUpper() : Localize.GetString(StringIds.STRING_PRINT_CUSTOMER_COPY).ToUpper();
                    DlgData.fCustomerDisplay = false;
                    DlgData.IdBitmap = GlobalResource.MB_ICONAPPROVAL_BMP;
                    DlgData.FunctionType = FunctionType.Purchase;
                    break;

                case CaseDialog.CASE2:

                    DlgData.lPurchaseApproval = 12000;
                    DlgData.PrintStage = PrintStage.PrintPrompt;
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_DECLINED);
                    DlgData.fApproved = false;
                    fPrintMerchantFirst = true;
                    DlgData.lpszThirdResult = fPrintMerchantFirst ? Localize.GetString(StringIds.STRING_PRINT_MERCHANT_COPY).ToUpper() : Localize.GetString(StringIds.STRING_PRINT_CUSTOMER_COPY).ToUpper();
                    DlgData.fCustomerDisplay = false;
                    DlgData.lpszResult = "TRANSACTION CANCELED";
                    DlgData.lpszSecondaryResult = "Signature didn't match";
                    DlgData.IdBitmap = GlobalResource.MB_ICONDECLINED_BMP;
                    DlgData.FunctionType = FunctionType.PurchaseCash;

                    break;

                case CaseDialog.CASE3:

                    DlgData.lPurchaseApproval = 12000;
                    DlgData.PrintStage = PrintStage.Printing;
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);
                    DlgData.fApproved = true;
                    fPrintMerchantFirst = true;
                    DlgData.lpszThirdResult = fPrintMerchantFirst ? Localize.GetString(StringIds.STRING_PRINTING_MERCHANT_COPY).ToUpper() : Localize.GetString(StringIds.STRING_PRINTING_CUSTOMER_COPY).ToUpper();
                    DlgData.fCustomerDisplay = false;
                    DlgData.IdBitmap = GlobalResource.MB_ICONAPPROVAL_BMP;
                    DlgData.FunctionType = FunctionType.Refund;

                    break;

                case CaseDialog.CASE4:

                    DlgData.lPurchaseApproval = 0;
                    DlgData.PrintStage = PrintStage.PrintComplete;
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_DECLINED);
                    DlgData.fApproved = false;
                    fPrintMerchantFirst = false;
                    DlgData.lpszThirdResult = fPrintMerchantFirst ? Localize.GetString(StringIds.STRING_PRINT_MERCHANT_COPY).ToUpper() : Localize.GetString(StringIds.STRING_PRINT_CUSTOMER_COPY).ToUpper();
                    DlgData.fCustomerDisplay = true;
                    DlgData.lpszResult = "TRANSACTION CANCELED";
                    DlgData.lpszSecondaryResult = "Signature didn't match";
                    DlgData.IdBitmap = GlobalResource.MB_ICONDECLINED_BMP;
                    DlgData.FunctionType = FunctionType.PreAuth;

                    break;

                case CaseDialog.CASE5:

                    DlgData.lPurchaseApproval = 0;
                    DlgData.PrintStage = PrintStage.PrintPrompt;
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);
                    DlgData.fApproved = true;
                    fPrintMerchantFirst = false;
                    DlgData.lpszThirdResult = fPrintMerchantFirst ? Localize.GetString(StringIds.STRING_PRINT_MERCHANT_COPY).ToUpper() : Localize.GetString(StringIds.STRING_PRINT_CUSTOMER_COPY).ToUpper();
                    DlgData.fCustomerDisplay = true;
                    DlgData.IdBitmap = GlobalResource.MB_ICONAPPROVAL_BMP;
                    DlgData.FunctionType = FunctionType.PreAuthCancel;
                    DlgData.Amount = 0;
                    DlgData.CardInfo = Localize.GetString(StringIds.STRING_CARDTYPE_VISA);
                    DlgData.CardInfo += $" *{"8870"}";
                    DlgData.AuthCode = "2895647";

                    break;

                case CaseDialog.CASE6:

                    DlgData.lPurchaseApproval = 0;
                    DlgData.PrintStage = PrintStage.Printing;
                    fPrintMerchantFirst = false;
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_DECLINED);
                    DlgData.fApproved = false;
                    DlgData.lpszThirdResult = fPrintMerchantFirst ? Localize.GetString(StringIds.STRING_PRINTING_MERCHANT_COPY).ToUpper() : Localize.GetString(StringIds.STRING_PRINTING_CUSTOMER_COPY).ToUpper();
                    DlgData.fCustomerDisplay = true;
                    DlgData.lpszResult = "TRANSACTION CANCELED";
                    DlgData.lpszSecondaryResult = "Signature didn't match";
                    DlgData.IdBitmap = GlobalResource.MB_ICONDECLINED_BMP;
                    DlgData.FunctionType = FunctionType.CardStatusCheck;

                    break;

                case CaseDialog.CASE7:

                    DlgData.lPurchaseApproval = 12000;
                    fPrintMerchantFirst = true;
                    DlgData.PrintStage = PrintStage.Printing;
                    DlgData.fApproved = true;
                    DlgData.lpszThirdResult = Localize.GetString(!fPrintMerchantFirst ? StringIds.STRING_PRINTING_CUSTOMER_COPY : StringIds.STRING_PRINTING_MERCHANT_COPY);
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_SIGNATURE_REQUIRED);
                    DlgData.IdBitmap = GlobalResource.MB_ICON_SIGNATURE_RESULT;
                    DlgData.FunctionType = FunctionType.Purchase;

                    break;
            }

            DlgData.TransactionTypeStringId = GetStringId(DlgData.FunctionType);

            //var approvalDialog = new ShellUI.CusViewApprovalDialog(StringIds.STRING_TRANSACTION, null, DlgData);
            //_baseDialog = approvalDialog;
            //approvalDialog.OnResult += (iResult, args) =>
            //{
            //    approvalDialog.Dismiss();
            //};
            //approvalDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //approvalDialog.Show(this);
        }

        void ShowCusViewSelectTipDialog()
        {
            double x = 5.00;
            long y = 1000;
            long amount = 10000;
            var keyValue = new KeyValuePair<double, long>(x, y);
            var data = new List<KeyValuePair<double, long>>() { keyValue, keyValue, keyValue, keyValue };

            //var dialog = new CusViewSelectTipDialog(StringIds.STRING_STARTAPP, null, data, true, amount);
            //dialog.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog.Show(this);
        }

        void ShowCusViewAdjustDonationDialog()
        {
            long amount = 38000;

            List<long> donation = new List<long> { 50, 200, 500, 1000 };
            string iconId = "donation_port_demo_02.png";
            string noBtnTitleId = "STRING_NO_DONATION";
            int noBtnCommand = 5016;

            //var dialog = new CusViewAdjustDonationDialog(StringIds.STRING_DONATION, null, amount, donation, iconId, noBtnTitleId, noBtnCommand);
            //dialog.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog.Show(this);
        }

        private void ShowCusViewMessageDialog(CaseDialog caseDialog)
        {
            IBaseDialog dialog = null;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    CusViewCustomStringMessageBox(false, StringIds.STRING_EMV_REMOVECARD, StringIds.STRING_EMV_REMOVECARD, false, GlobalResource.MB_NONE, GlobalResource.ICON_ICC_CARD_REMOVED, fAutoDismiss: true);
                    break;

                case CaseDialog.CASE2:

                    CusViewCustomStringMessageBox(true, StringIds.STRING_EMV_REMOVECARD, StringIds.STRING_CARD_REMOVE_TOO_SOON, false, GlobalResource.MB_RETRYCANCEL, GlobalResource.ICON_ICC_CARD_REMOVED);
                    break;

                case CaseDialog.CASE3:

                    CusViewErrorMessage(string.Empty, Localize.GetString(StringIds.STRING_TRANSACTIONCANCELLED), false, string.Empty, false, Localize.GetString(StringIds.STRING_PLEASEREMOVECARD), GlobalResource.MB_NONE);
                    break;

                case CaseDialog.CASE4:

                    CusViewCustomStringMessageBox(true, StringIds.STRING_CONFIRM_SIGNATURE, StringIds.STRING_CONFIRM_SIGNATURE_IS_CORRECT_UPCASE, false, GlobalResource.MB_YESNO, ref dialog, GlobalResource.MB_ICON_SIGNATURE_RESULT, aboveMsg: StringIds.STRING_SIGNATURE_REQUIRED, fAboveMsgActualText: false);
                    break;
            }
        }

        private void CusViewErrorMessage(string IdDlgTitle, string lpszMainResult, bool fActualText, string lpszSecondaryResult, bool fSubActualText, string bottomWarningId, int uType)
        {
            CusViewCustomStringMessageBox(true, IdDlgTitle, lpszMainResult, fActualText, uType, GlobalResource.MB_ICONDECLINED_BMP, subMsg: lpszSecondaryResult, fSubActualText: fSubActualText, bottomWarningId: bottomWarningId);
        }

        private void CusViewCustomStringMessageBox(bool BlockUI, string IdDlgTitleText, string strContent, bool fActualText, int uType, int idImg = 0, EvtMessage Evt = null, string subMsg = "", bool fSubActualText = false, string bottomWarningId = "",
                string strSubMessageColor = "", int iSubMessageTextSize = 0, string thirdbMsg = "", bool fThirdActualText = false, bool isShowBackBtn = false, string aboveMsg = "", bool fAboveMsgActualText = false, bool fAutoDismiss = false, string thirdbMsgColor = "")
        {
            IBaseDialog dialog = null;

            CusViewCustomStringMessageBox(BlockUI, IdDlgTitleText, strContent, fActualText, uType, ref dialog, idImg, Evt, subMsg, fSubActualText, bottomWarningId,
              strSubMessageColor, iSubMessageTextSize, thirdbMsg, fThirdActualText, isShowBackBtn, aboveMsg, fAboveMsgActualText, fAutoDismiss, thirdbMsgColor);
        }

        private void CusViewCustomStringMessageBox(bool BlockUI, string IdDlgTitleText, string strContent, bool fActualText, int uType, ref IBaseDialog dialog, int idImg = 0, EvtMessage Evt = null, string subMsg = "", bool fSubActualText = false, string bottomWarningId = "",
                string strSubMessageColor = "", int iSubMessageTextSize = 0, string thirdbMsg = "", bool fThirdActualText = false, bool isShowBackBtn = false, string aboveMsg = "", bool fAboveMsgActualText = false, bool fAutoDismiss = false, string thirdbMsgColor = "")
        {
            MessageType data = new MessageType();

            data.IsShowBackBtn = true;
            data.IsShowCancelBtn = true;
            data.IsAboveMsgActualText = fAboveMsgActualText;
            data.IsActualText = fActualText;
            data.IsSubActualText = fSubActualText;
            data.IsThirdActualText = fThirdActualText;

            data.AboveMessage = aboveMsg;
            data.Message = strContent;
            data.SubMessage = subMsg;
            data.ThirdMessage = thirdbMsg;

            data.iType = uType;
            data.idImg = idImg;
            data.BottomWarningId = bottomWarningId;

            if (!string.IsNullOrWhiteSpace(strSubMessageColor))
            {
                data.strSubMessageColor = strSubMessageColor;
            }

            if (!string.IsNullOrWhiteSpace(thirdbMsgColor))
            {
                data.thirdbMsgColor = thirdbMsgColor;
            }

            if (iSubMessageTextSize != 0)
            {
                data.iSubMessageTextSize = iSubMessageTextSize;
            }

            data.Evt = Evt;

            //var messageDialog = new CusViewMessageDialog(IdDlgTitleText, null, data);
            //messageDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //messageDialog.Show(this);
        }

        private void ShowCusViewStandardSetupDialog()
        {
            var noCVVStatusDlgData = new StandardSetupDialogModel()
            {
                OkBtnTitleId = StringIds.STRING_SELECT,
                OKBtnCommandId = GlobalResource.OK_BUTTON,
                Items = new List<BaseEditModel>()
                    {
                        new RadioEditModel()
                        {
                            GroupName = "NoCVV",
                            TitleId = StringIds.STRING_CVV_NOT_PRESENT,
                            Identifier = GlobalResource.NOT_PRESENT,
                            Value = false
                        },
                        new RadioEditModel()
                        {
                            GroupName = "NoCVV",
                            TitleId = StringIds.STRING_CVV_UNREADABLE,
                            Identifier = GlobalResource.CVV_UNREADABLE,
                            Value = false
                        },
                        new RadioEditModel()
                        {
                            GroupName = "NoCVV",
                            TitleId = StringIds.STRING_NO_CVV_ON_CARD,
                            Identifier = GlobalResource.NO_CVV_ON_CARD,
                            Value = false
                        }
                    }
            };

            //var standardSetupDialog = new ShellUI.CusViewStandardSetupDialog(StringIds.STRING_NO_CVV, null, noCVVStatusDlgData);
            //standardSetupDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //standardSetupDialog.Show(this);
        }

        void ShowCusViewConfirmServiceDialog()
        {
            object[] param = new object[1];
            var data = new List<ServiceItem>();

            data.Add(new ServiceItem()
            {
                Title = "Excellent",
                Icon = "vector_excellent",
                RatingScore = 5

            });

            data.Add(new ServiceItem()
            {
                Title = "Fair",
                Icon = "vector_fair",
                RatingScore = 3

            });
            data.Add(new ServiceItem()
            {
                Title = "Good",
                Icon = "vector_good",
                RatingScore = 4

            });

            data.Add(new ServiceItem()
            {
                Title = "Poor",
                Icon = "vector_poor",
                RatingScore = 3

            });
          
            //var dialog4 = new CusViewConfirmServiceDialog(StringIds.STRING_CUSTOMER_RATING, null, data, true);
            //dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog4.Show(this);
        }

        void ShowCusViewConfirmSurveyDialog()
        {
            var item1 = new SurveyItem() { Title = "Service Quality", Icon = "vector_service_quality", StarMaxRating = 5 };
            var item2 = new SurveyItem() { Title = "Employee Efficiency", Icon = "vector_employee_efficiency", StarMaxRating = 5 };
            var item3 = new SurveyItem() { Title = "Product Quality", Icon = "vector_product_quality", StarMaxRating = 5 };
            var item4 = new SurveyItem() { Title = "Return Likelihood", Icon = "vector_return_likelihood", StarMaxRating = 5 };
            var item5 = new SurveyItem() { Title = "Rated to Competitors", Icon = "vector_rated_to_competitors", StarMaxRating = 5 };
            var data = new List<SurveyItem>() { item1, item2, item3, item4, item5 };

            //var dialog4 = new CusViewConfirmSurveyDialog(StringIds.STRING_CUSTOMER_SURVEY, null, data);
            //dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog4.Show(this);
        }

        void ShowCusViewBasketItemReviewDialog()
        {
            var baskets = new List<BasketItemModel>()
            {
                new BasketItemModel()
                {
                    Name = "Bloody Mary",
                    Price = 1750,
                    Quantity = 1,
                    ImageName = "taco_mondays"
                },
                new BasketItemModel()
                {
                    Name = "Cosmopolitans",
                    Price = 1885,
                    Quantity = 3
                },
                new BasketItemModel()
                {
                    Name = "Lemon Drops",
                    Price = 1600,
                    Quantity = 5
                },
                new BasketItemModel()
                {
                    Name = "Mill House Kold One",
                    Price = 950,
                    Quantity = 1
                },
                new BasketItemModel()
                {
                    Name = "Blonde Bottle",
                    Price = 950,
                    Quantity = 1
                },
                new BasketItemModel()
                {
                    Name = "Carlton Draft",
                    Price = 850,
                    Quantity = 2,
                    IsVoucherOffer = false,
                    ImageName = IconIds.ICON_VOUCHER_OFFER_TACO_MONDAYS
                },
                new BasketItemModel()
                {
                    Name = "Apples Red Victoria 1kg",
                    Price = 1000,
                    Quantity = 4,
                    IsVoucherOffer = true,
                    ImageName = IconIds.ICON_VOUCHER_OFFER_TACO_MONDAYS
                },
                new BasketItemModel()
                {
                    Name = "Nestles Dark Chocolate 300gms",
                    Price = 3000,
                    Quantity = 1,
                    IsVoucherOffer = true,
                    ImageName = IconIds.ICON_VOUCHER_OFFER_TACO_MONDAYS
                }
            };

            //var dialog4 = new CusViewBasketItemReviewDialog(string.Empty, null, baskets, false);
            //dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog4.Show(this);
        }

        void ShowCusViewSelectCharityDialog()
        {
            var charityItem1 = new CharityItem();
            charityItem1.PortraitPoster = "don_1_port_poster.png";
            charityItem1.Logo = "don_1_logo.png";
            charityItem1.LandcapsetPoster = "don_1_land_poster.png";
            charityItem1.Id = 1;
            charityItem1.Name = "Stop Violence";
            charityItem1.DonationType = new DonationType();
            charityItem1.DonationType.IsEnabled = true;
            charityItem1.DonationType.FixedAmount = 50;
            charityItem1.DonationType.FixedDonation = false;
            charityItem1.DonationType.FixedAmountStringIcon = "don_1_fixed_poster.png";
            charityItem1.DonationType.DonationPercentAmount = new DonationPercentAmount();
            charityItem1.DonationType.DonationPercentAmount.IsEnabled = true;
            charityItem1.DonationType.DonationPercentAmount.SuggestiveDonations = new SuggestiveDonations();
            charityItem1.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent1 = 5;
            charityItem1.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent2 = 10;
            charityItem1.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent3 = 20;
            charityItem1.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent4 = 30;
            charityItem1.DonationType.DonationPercentAmount.DonationExcludes.IsEnabled = true;

            charityItem1.DonationType.DonationAmounts = new DonationAmounts();
            charityItem1.DonationType.DonationAmounts.IsEnabled = true;
            charityItem1.DonationType.DonationAmounts.AmountOne = 50;
            charityItem1.DonationType.DonationAmounts.AmountTwo = 200;
            charityItem1.DonationType.DonationAmounts.AmountThree = 500;
            charityItem1.DonationType.DonationAmounts.AmountFour = 1000;

            charityItem1.DonationAmount = new DonationAmount();
            charityItem1.DonationAmount.Amount = 0;
            charityItem1.DonationAmount.SelectAmountStringIcon = IconIds.IMG_DON_1_SELECT;

            var charityItem2 = new CharityItem();
            charityItem2.Id = 2;
            charityItem2.Name = "Australian Red Cross";
            charityItem2.PortraitPoster = "don_2_port_poster.png";
            charityItem2.LandcapsetPoster = "don_2_land_poster.png";
            charityItem2.Logo = "don_2_logo.png";
            charityItem2.DonationType = new DonationType();
            charityItem2.DonationType.IsEnabled = true;
            charityItem2.DonationType.FixedAmount = 70;
            charityItem2.DonationType.FixedDonation = false;

            charityItem2.DonationType.DonationPercentAmount = new DonationPercentAmount();
            charityItem2.DonationType.FixedAmountStringIcon = "don_1_fixed_poster.png";
            charityItem2.DonationType.DonationPercentAmount.IsEnabled = false;
            charityItem2.DonationType.DonationPercentAmount.SuggestiveDonations = new SuggestiveDonations();
            charityItem2.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent1 = 10;
            charityItem2.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent2 = 20;
            charityItem2.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent3 = 30;
            charityItem2.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent4 = 40;
            charityItem2.DonationType.DonationPercentAmount.DonationExcludes.IsEnabled = true;

            charityItem2.DonationType.DonationAmounts = new DonationAmounts();
            charityItem2.DonationType.DonationAmounts.IsEnabled = true;
            charityItem2.DonationType.DonationAmounts.AmountOne = 100;
            charityItem2.DonationType.DonationAmounts.AmountTwo = 500;
            charityItem2.DonationType.DonationAmounts.AmountThree = 1000;
            charityItem2.DonationType.DonationAmounts.AmountFour = 2000;

            charityItem2.DonationAmount = new DonationAmount();
            charityItem2.DonationAmount.Amount = 0;
            charityItem2.DonationAmount.SelectAmountStringIcon = IconIds.IMG_DON_2_SELECT;

            var charityItem3 = new CharityItem();
            charityItem3.Id = 3;
            charityItem3.Name = "Breast Cancer";
            charityItem3.PortraitPoster = "don_3_port_poster.png";
            charityItem3.LandcapsetPoster = "don_3_land_poster.png";
            charityItem3.Logo = "don_3_logo.png";
            charityItem3.DonationType = new DonationType();
            charityItem3.DonationType.IsEnabled = true;
            charityItem3.DonationType.FixedAmount = 70;
            charityItem3.DonationType.FixedDonation = false;
            charityItem3.DonationType.DonationPercentAmount = new DonationPercentAmount();
            charityItem3.DonationType.FixedAmountStringIcon = "don_1_fixed_poster.png";
            charityItem3.DonationType.DonationPercentAmount.IsEnabled = false;
            charityItem3.DonationType.DonationPercentAmount.SuggestiveDonations = new SuggestiveDonations();
            charityItem3.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent1 = 10;
            charityItem3.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent2 = 20;
            charityItem3.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent3 = 30;
            charityItem3.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent4 = 40;
            charityItem3.DonationType.DonationPercentAmount.DonationExcludes.IsEnabled = true;
            charityItem3.DonationType.DonationAmounts = new DonationAmounts();
            charityItem3.DonationType.DonationAmounts.IsEnabled = true;
            charityItem3.DonationType.DonationAmounts.AmountOne = 200;
            charityItem3.DonationType.DonationAmounts.AmountTwo = 400;
            charityItem3.DonationType.DonationAmounts.AmountThree = 7000;
            charityItem3.DonationType.DonationAmounts.AmountFour = 9000;
            charityItem3.DonationAmount = new DonationAmount();
            charityItem3.DonationAmount.Amount = 0;
            charityItem3.DonationAmount.SelectAmountStringIcon = IconIds.IMG_DON_2_SELECT;

            var charityItem4 = new CharityItem();
            charityItem4.Id = 4;
            charityItem4.Name = "Qatar Charity";
            charityItem4.PortraitPoster = "don_4_port_poster.png";
            charityItem4.LandcapsetPoster = "don_4_land_poster.png";
            charityItem4.Logo = "don_4_logo.png";
            charityItem4.DonationType = new DonationType();
            charityItem4.DonationType.IsEnabled = true;
            charityItem4.DonationType.FixedAmount = 70;
            charityItem4.DonationType.FixedDonation = false;
            charityItem4.DonationType.DonationPercentAmount = new DonationPercentAmount();
            charityItem4.DonationType.FixedAmountStringIcon = "don_1_fixed_poster.png";
            charityItem4.DonationType.DonationPercentAmount.IsEnabled = false;
            charityItem4.DonationType.DonationPercentAmount.SuggestiveDonations = new SuggestiveDonations();
            charityItem4.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent1 = 10;
            charityItem4.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent2 = 20;
            charityItem4.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent3 = 30;
            charityItem4.DonationType.DonationPercentAmount.SuggestiveDonations.DonationPercent4 = 40;
            charityItem4.DonationType.DonationPercentAmount.DonationExcludes.IsEnabled = true;
            charityItem4.DonationType.DonationAmounts = new DonationAmounts();
            charityItem4.DonationType.DonationAmounts.IsEnabled = true;
            charityItem4.DonationType.DonationAmounts.AmountOne = 150;
            charityItem4.DonationType.DonationAmounts.AmountTwo = 550;
            charityItem4.DonationType.DonationAmounts.AmountThree = 1350;
            charityItem4.DonationType.DonationAmounts.AmountFour = 2350;
            charityItem4.DonationAmount = new DonationAmount();
            charityItem4.DonationAmount.Amount = 0;
            charityItem4.DonationAmount.SelectAmountStringIcon = IconIds.IMG_DON_2_SELECT;

            MerchantCharity merchantCharity = new MerchantCharity();
            merchantCharity.IdMerchant = 1;

            merchantCharity.MultipleItemsList = new List<CharityItem>();
            merchantCharity.MultipleItemsList.Add(charityItem1);
            merchantCharity.MultipleItemsList.Add(charityItem2);
            merchantCharity.MultipleItemsList.Add(charityItem3);
            merchantCharity.MultipleItemsList.Add(charityItem4);

            var dlgData = new SelectCharityDlgData()
            {
                //Items = merchantCharity.MultipleItemsList
            };

            long amount = 13800;

            //var dialog4 = new CusViewSelectCharitiyDialog(StringIds.STRING_DONATION, null, dlgData, amount);
            //dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog4.Show(this);
        }

        void ShowCusViewReceiptOptionDialog()
        {
            var data = new ReceiptOptionsDlgData();

            data.QRReceiptResult = "ReceiptOptionsDialog";
            data.fShowQrCode = true;

            data.FunctionButtons = new List<SelectButton>();

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_EMAIL_RECEIPT_LOWCASE,
                idImage = IconIds.VECTOR_EMAIL_RECEIPT,
                IdProcessor = 0,
                IsVectorDrawble = true
            });

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_TEXT_RECEIPT,
                idImage = IconIds.VECTOR_TEXT_RECEIPT,
                IdProcessor = 0,
                IsVectorDrawble = true
            });

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_PRINT_CUSTOMER,
                idImage = IconIds.VECTOR_PRINT_RECEIPT,
                IdProcessor = 0,
                IsVectorDrawble = true
            });

            //var dialog4 = new CusViewReceiptOptionsDialog(StringIds.STRING_RECEIPT_OPTIONS, null, data);
            //dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog4.Show(this);
        }

        void ShowCusViewListPaymentRecordDialog()
        {
            //Hardcode for UI testing 
            var item1 = new TransactionInfoModel();
            item1.Amount = 10000;
            item1.AuthNumber = "87654";
            item1.CardType = "Visa Debit";
            item1.CardInfo = "****7654";
            var date = new XDateTime();
            date.Year = 2023;
            date.Month = 6;
            date.Day = 30;
            date.Hours = 11;
            date.Minutes = 30;
            item1.CreatedDate = date;
            item1.CustomerName = "David";
            item1.ReferenceTypeStringIds = "Table 10";
            var item2 = new TransactionInfoModel();
            var item3 = new TransactionInfoModel();
            item2 = item1;
            item3 = item1;
            item1.IsSelected = true;
            item2.IsSelected = false;
            item3.IsSelected = false;
            var data = new ListPaymentRecordsDlgData();
            data.Items.Add(item1);
            data.Items.Add(item2);
            data.Items.Add(item3);
            //DialogBuilder.Show(IPayDialog.CUS_VIEW_LIST_ITEM_RECORDS_DIALOG, StringIds.STRING_REFUND_PURCHASE, null, true, false, data);
        }
    }
}