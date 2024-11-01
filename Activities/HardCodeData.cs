using CloudBanking.BaseControl;
using CloudBanking.Common;
using CloudBanking.Entities;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
using CloudBanking.Repositories;
using CloudBanking.ShellContainers;
using CloudBanking.ShellModels;
using CloudBanking.ShellUI;
using CloudBanking.UI;
using CloudBanking.Utilities;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using static Android.Content.ClipData;
using System.Windows.Input;
using static Android.Icu.Text.CaseMap;
using static CloudBanking.Entities.RefundReasonDlgData;
using static CloudBanking.Utilities.UtilEnum;
using static CloudBanking.Entities.Database;

namespace CloudBanking.UITestApp
{
    public partial class TestActivity : BaseActivity
    {
        private void ShowApprovalDialog(CaseDialog caseDialog)
        {
            string lpszEntryModeString = "";

            ApprovalDlgData DlgData = new ApprovalDlgData();

            DlgData.lBalance = 12000;

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
                    DlgData.lpszSecondaryResult = "SIGNATURE DIDN'T MATCH";
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
                    DlgData.lpszSecondaryResult = "SIGNATURE DIDN'T MATCH";
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
                    DlgData.lpszSecondaryResult = "SIGNATURE DIDN'T MATCH";
                    DlgData.IdBitmap = GlobalResource.MB_ICONDECLINED_BMP;
                    DlgData.FunctionType = FunctionType.CardStatusCheck;
                    DlgData.fCustomerDisplay = true;

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
                    DlgData.fCustomerDisplay = false;

                    break;
            }

            DlgData.TransactionTypeStringId = GetStringId(DlgData.FunctionType);
            DlgData.AuthCode = "2895647";

            var approvalDialog = new ShellUI.ApprovalDialog(StringIds.STRING_TRANSACTION, null, DlgData);
            approvalDialog.OnResult += (iResult, args) =>
            {
                approvalDialog.Dismiss();
            };
            approvalDialog.DialogStyle = DialogStyle.FULLSCREEN;
            approvalDialog.Show(this);
        }

        private void ShowSettlementApprovalDialog(CaseDialog caseDialog)
        {
            SettlementApprovalDlgData DlgData = new SettlementApprovalDlgData();

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_DECLINED);
                    DlgData.fApproved = false;
                    DlgData.lpszResult = "CANNOT COMPLETE";
                    DlgData.lpszSecondaryResult = "SIGNATURE DIDN'T MATCH";
                    break;
                case CaseDialog.CASE2:
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);
                    DlgData.fApproved = true;
                    DlgData.lpszResult = "CONNECTION SUCCESSFUL";
                    DlgData.lpszSecondaryResult = "ACCESS CODE HAS BEEN VALIDATED";
                    break;
            }

            var approvalDialog = new ShellUI.SettlementApprovalDialog(StringIds.STRING_TRANSACTION, null, DlgData);
            approvalDialog.OnResult += (iResult, args) =>
            {
                approvalDialog.Dismiss();
            };
            approvalDialog.DialogStyle = DialogStyle.FULLSCREEN;
            approvalDialog.Show(this);
        }

        public static string GetStringId(FunctionType functionType)
        {
            var lszTitle = "";

            switch (functionType)
            {
                case FunctionType.Refund: lszTitle = StringIds.STRING_FUNCTIONTYPES_REFUND; break;
                case FunctionType.Purchase: lszTitle = StringIds.STRING_PURCHASE; break;
                case FunctionType.PurchaseCash: lszTitle = StringIds.STRING_PURCHASE_AND_CASH; break;
                case FunctionType.Deposit: lszTitle = StringIds.STRING_FUNCTIONTYPES_DEPOSIT; break;
                case FunctionType.CardAuthentication: lszTitle = StringIds.STRING_FUNCTIONTYPES_CARDAUTHENTICATION; break;
                case FunctionType.TestComm: lszTitle = StringIds.STRING_FUNCTIONTYPES_TESTCOMM; break;
                case FunctionType.CardStatusCheck: lszTitle = StringIds.STRING_FUNCTIONTYPES_CARDSTATUSCHECK; break;
                case FunctionType.PreAuth: lszTitle = StringIds.STRING_FUNCTIONTYPES_PREAUTH; break;
                case FunctionType.CashOut: lszTitle = StringIds.STRING_CASHOUT; break;
                case FunctionType.Void: lszTitle = StringIds.STRING_FUNCTIONTYPES_VOID; break;
                case FunctionType.Adjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_ADJUST; break;
                //case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; break;
            }

            return lszTitle;
        }

        void ShowSwipeMerchantCardDialog()
        {
            var data = new SwipeMerchantCardDlgData()
            {
                lTotal = 2000,
                IsEmulator = false,
                szTotalTitle = StringIds.STRING_REFUND_AMOUNT,
                EventCommand = (ret, args) =>
                {

                }
            };
            var dlg = new MerchantSwipeCardDialog(StringIds.STRING_SWIPEMERCHANTCARD, (iResult, args) =>
            {

            }, data);

            dlg.DialogStyle = DialogStyle.FULLSCREEN;
            dlg.Show(this);
        }

        private void ShowRequestCardDialog(CaseDialog caseDialog)
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

            RequestDlgData.fNoPresentCard = true;// change to show difference case
            RequestDlgData.pInitProcessData = pInitProcessData;
            RequestDlgData.fMultiplePayments = false;
            RequestDlgData.fCanCancel = true;
            RequestDlgData.lTotal = pInitProcessData.lAmount
                                    + pInitProcessData.lTipAmount
                                    + pInitProcessData.lCashOut
                                    + (pInitProcessData.PaymentDonations != null ? pInitProcessData.PaymentDonations.lTotalDonations : 0)
                                    + (pInitProcessData.PaymentVouchers != null ? pInitProcessData.PaymentVouchers.lTotalVouchers : 0);

            RequestDlgData.PresentCardTitleId = StringIds.STRING_PRESENTCARD_TITLE;
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
            RequestDlgData.lszPreSurcharge = StringIds.STRING_SURCHARGE_CREDIT___DEBIT_FEES_APPLY;
            //RequestDlgData.fAlipayWechatLogo = true;
            RequestDlgData.PresentCardAnimFileName = GlobalConstants.PRESENT_CARD_LOTTIE_INSERT_SWIPE_TAP;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    RequestDlgData.iFunctionButton = FunctionType.Purchase;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE2:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE3:

                    RequestDlgData.iFunctionButton = FunctionType.Cash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE4:

                    RequestDlgData.iFunctionButton = FunctionType.Refund;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE5:

                    RequestDlgData.iFunctionButton = FunctionType.PreAuth;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = false;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = false;

                    RequestDlgData.PresentCardSubTitleId = StringIds.STRING_OPEN_PREAUTH_UPCASE;

                    break;

                case CaseDialog.CASE6:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = false;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE7:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE8:

                    RequestDlgData.iFunctionButton = FunctionType.CardStatusCheck;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = true;

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
                //case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; totalTitleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
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

            var requestCardDialog = new RequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            {
            }, RequestDlgData);

            ////comment out
            //var requestCardDialog = new AnimatedRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            //{
            //}, RequestDlgData);

            requestCardDialog.DialogStyle = DialogStyle.FULLSCREEN;
            requestCardDialog.Show(this);
        }

        private void ShowCusDisplayRequestCardDialog(CaseDialog caseDialog)
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
            //RequestDlgData.fShowCardFees = false;
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
            RequestDlgData.IsEmulator = false;
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
                //case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; totalTitleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
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

            var requestCardDialog = new CustomerDisplayRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            {
            }, RequestDlgData);

            requestCardDialog.DialogStyle = DialogStyle.FULLSCREEN;
            requestCardDialog.Show(this);
        }

        private void ShowDynamicOptionDialog(CaseDialog caseDialog)
        {
            var generalType = new List<GenericType>();

            var item1 = new GenericType();
            var item2 = new GenericType();
            var item3 = new GenericType();

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    item1.Id = AccountType.ACCOUNT_TYPE_CHEQUE;
                    item1.Icon = IconIds.VECTOR_CHEQUE;
                    item1.lszText = StringIds.STRING_ACCOUNTTYPECHEQUE;
                    generalType.Add(item1);

                    item2.Id = AccountType.ACCOUNT_TYPE_SAVINGS;
                    item2.Icon = IconIds.VECTOR_SAVINGS;
                    item2.lszText = StringIds.STRING_ACCOUNTTYPESAVINGS;
                    generalType.Add(item2);

                    item3.Id = AccountType.ACCOUNT_TYPE_CREDIT;
                    item3.Icon = IconIds.VECTOR_CREDIT;
                    item3.lszText = StringIds.STRING_ACCOUNTTYPECREDITCARD;
                    generalType.Add(item3);
                    break;
                case CaseDialog.CASE2:

                    item1.Id = AccountType.ACCOUNT_TYPE_CHEQUE;
                    item1.Icon = IconIds.VECTOR_CHEQUE;
                    item1.lszText = StringIds.STRING_ACCOUNTTYPECHEQUE;
                    generalType.Add(item1);

                    item2.Id = AccountType.ACCOUNT_TYPE_SAVINGS;
                    item2.Icon = IconIds.VECTOR_SAVINGS;
                    item2.lszText = StringIds.STRING_ACCOUNTTYPESAVINGS;
                    generalType.Add(item2);
                    break;

                case CaseDialog.CASE3:

                    item1.Id = AccountType.ACCOUNT_TYPE_CHEQUE;
                    item1.Icon = IconIds.VECTOR_CHEQUE;
                    item1.lszText = StringIds.STRING_ACCOUNTTYPECHEQUE;
                    generalType.Add(item1);
                    break;

                default:
                    break;
            }

            //ACCOUNT SELECTION CHQ / SAV / CRD is the normal account selection order in NZ
            var sortedGeneralType = generalType.OrderBy(p => p.Id == AccountType.ACCOUNT_TYPE_CHEQUE ? 0 : p.Id == AccountType.ACCOUNT_TYPE_SAVINGS ? 1 : 2).ToList();

            AccountType temp = null;

            var dynamicOptionDialog = new ShellUI.DynamicOptionDialog(StringIds.STRING_ACCOUNT_TYPES, null, sortedGeneralType, StringIds.STRING_ACCOUNT_SELECT_TYPE);
            dynamicOptionDialog.DialogStyle = DialogStyle.FULLSCREEN;
            dynamicOptionDialog.Show(this);
        }

        private void ShowMessageDialogShell(CaseDialog caseDialog)
        {
            MessageType messageData = new MessageType();

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    messageData.idImg = GlobalResource.MB_ICONDECLINED_BMP;
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_PRINT_PENDING_HOSPITALITY, StringIds.STRING_REPORT_PRINTED_FAILED, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE2:
                    messageData.idImg = GlobalResource.MB_ICONAPPROVAL_BMP;
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_PRINT_PENDING_HOSPITALITY, StringIds.STRING_REPORT_PRINTED_SUCCESSFULLY, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE3:
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_WARNING_TITLE, StringIds.STRING_PROCESSORNOTFOUND, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE4:
                    messageData.idImg = GlobalResource.MB_ICONAPPROVAL_BMP;
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_PRINTPENDING, StringIds.STRING_REPORT_PRINTED_SUCCESSFULLY, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE5:
                    messageData.idImg = GlobalResource.MB_ICONDECLINED_BMP;
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR_TITLE, StringIds.STRING_CARDREADERROR, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE6:
                    messageData.idImg = GlobalResource.MB_ICONDECLINED_BMP;
                    messageData.SubMessage = StringIds.STRING_PLEASE_TRY_AGAIN;
                    messageData.IsSubActualText = false;
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_RKI, StringIds.STRING_RKI_INITIALISATION_FAILED, false, GlobalResource.MB_RETRYCANCEL, ref messageData);
                    break;

                case CaseDialog.CASE7:
                    messageData.idImg = GlobalResource.MB_ICONAPPROVAL_BMP;
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_LOGON, StringIds.STRING_LOGON_SUCCESSFUL_UPCASE, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE8:
                    ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_WARNING_TITLE, StringIds.STRING_PROCESSORNOTFOUND, false, GlobalResource.MB_OK, ref messageData);
                    break;
            }
        }

        private void ShowMessageDialogDroid(CaseDialog caseDialog)
        {
            MessageType messageData = new MessageType();

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    messageData.idImg = GlobalResource.MB_ICONDECLINED_BMP;
                    messageData.BottomWarningId = StringIds.STRING_PRINT_MERCHANT_COPY;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ERROR, StringIds.STRING_ERRORDATA, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE2:
                    messageData.idImg = GlobalResource.MB_ICON_SIGNATURE_RESULT;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_MESSAGE, StringIds.STRING_SIGNATUREACCEPTED, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE3:
                    messageData.idImg = GlobalResource.ICON_SHIFT_STARTED;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_SHIFT, StringIds.STRING_SHIFTHASALREADYSTARTED, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE4:
                    messageData = new MessageType();
                    messageData.idImg = GlobalResource.MB_ICONDECLINED_BMP;
                    messageData.IsShowBackBtn = true;
                    messageData.IsShowCancelBtn = false;
                    messageData.SubMessage = StringIds.STRING_NO_TICKET_FOUND;
                    messageData.IsSubActualText = false;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_LINK_OPEN_TAB, StringIds.STRING_NO_OPEN_TAB_FOUND_UPCASE, false, GlobalResource.MB_RETRYCANCEL, ref messageData);
                    break;

                case CaseDialog.CASE5:
                    messageData = new MessageType();
                    messageData.idImg = GlobalResource.MB_ICONAPPROVAL_BMP;
                    messageData.SubMessage = StringIds.STRING_ACCESS_CODE_HAS_BEEN_VALIDATED;
                    messageData.strSubMessageColor = GlobalConstants.STRING_PRIMARY_COLOR;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_ACCESSCODE, StringIds.STRING_CONNECTION_SUCCESSFULL, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE6:
                    messageData.idImg = GlobalResource.MB_ICONAPPROVAL_BMP;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_PRINT_PENDING_HOSPITALITY, StringIds.STRING_REPORT_PRINTED_SUCCESSFULLY, false, GlobalResource.MB_OK, ref messageData);
                    break;

                case CaseDialog.CASE7:
                    messageData = new MessageType();
                    messageData.idImg = GlobalResource.MB_ICONDECLINED_BMP;
                    messageData.IsShowBackBtn = true;
                    messageData.IsShowCancelBtn = false;
                    messageData.SubMessage = StringIds.STRING_NO_TICKET_FOUND;
                    messageData.IsSubActualText = false;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_LINK_OPEN_TAB, StringIds.STRING_NO_OPEN_TAB_FOUND_UPCASE, false, GlobalResource.MB_RETRYCANCEL, ref messageData);
                    break;

                case CaseDialog.CASE8:
                    messageData = new MessageType();
                    messageData.idImg = GlobalResource.ICON_SHIFT_STARTED;
                    ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_SHIFT, StringIds.STRING_SHIFTHASALREADYSTARTED, false, GlobalResource.MB_OK, ref messageData);
                    break;
            }
        }


        private void ShowEOVProcessingDialog()
        {
            var pProcessingData = new ProcessingData();

            pProcessingData.Processed = 3;
            pProcessingData.cPending = 17;

            var enterPinDialog = new EOVProcessingDialog("", null, pProcessingData);
            enterPinDialog.DialogStyle = DialogStyle.FULLSCREEN;
            enterPinDialog.Show(this);
        }

        private void ShowPresentCardErrorDlg(CaseDialog caseDialog)
        {
            PresentCardErrorDlgData DlgData = new PresentCardErrorDlgData();

            string IdSecondText = string.Empty;

            DlgData.plszTitle = StringIds.STRING_PRESENTCARD_TITLE;

            DlgData.fCardInserted = false;

            DlgData.fShowRetry = false;

            bool fShowRetry = DlgData.fShowRetry;

            bool fShowError = true;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    DlgData.szMessageId = StringIds.STRING_CANNOTREADCARD;
                    fShowRetry = true;
                    break;

                case CaseDialog.CASE2:

                    DlgData.szMessageId = StringIds.STRING_CANNOTREADCARD;
                    IdSecondText = StringIds.STRING_PLEASEREMOVECARD;
                    fShowRetry = false;
                    break;
            }

            DlgData.szSecondMessageId = IdSecondText;

            DlgData.fShowRetry = fShowRetry;

            int result = 0;

            if (DlgData.fCardInserted && IdSecondText == string.Empty)
            {
                IdSecondText = StringIds.STRING_PLEASEREMOVECARD;

                DlgData.fHide = true;
            }

            var messageDialog = new PresentCardErrorDialog(StringIds.STRING_TRANSACTION, null, DlgData);
            messageDialog.DialogStyle = DialogStyle.FULLSCREEN;
            messageDialog.Show(this);
        }

        private void ShowEnterPinDialog(CaseDialog caseDialog)
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

            var enterPinDialog = new EnterPinDialog(StringIds.STRING_ENTERPIN, null, data);
            enterPinDialog.DialogStyle = DialogStyle.FULLSCREEN;
            enterPinDialog.Show(this);
        }

        private void ShowProcessMessageDialog(CaseDialog caseDialog)
        {
            var pProcessingData = new ProcessingData();

            pProcessingData.fAutoClose = true;
            string cancelBtnTitleId = "";

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    pProcessingData.hTextTitle = "";
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = "";
                    pProcessingData.hTextThree = "";
                    break;

                case CaseDialog.CASE2:

                    pProcessingData.hTextTitle = Localize.GetString(StringIds.STRING_EMAIL_RECEIPT);
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = "";
                    pProcessingData.hTextThree = Localize.GetString(StringIds.STRING_EMV_LEAVECARDINSERTED);
                    break;

                case CaseDialog.CASE3:

                    pProcessingData.hTextTitle = Localize.GetString(StringIds.STRING_EMAIL_RECEIPT);
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = Localize.GetString(StringIds.STRING_EMV_TERMINALACTIONANALYSIS);
                    pProcessingData.hTextThree = Localize.GetString(StringIds.STRING_EMV_LEAVECARDINSERTED);
                    break;

                case CaseDialog.CASE4:

                    pProcessingData.hTextTitle = Localize.GetString(StringIds.STRING_EMAIL_RECEIPT);
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = Localize.GetString(StringIds.STRING_EMV_TERMINALACTIONANALYSIS);
                    pProcessingData.hTextThree = Localize.GetString(StringIds.STRING_EMV_LEAVECARDINSERTED);
                    cancelBtnTitleId = StringIds.STRING_TEMPORARILY_HALT_TRANSMISSION;
                    break;
            }

            var enterPinDialog = new ProcessMessageDialog(StringIds.STRING_PROCESSING_TITLE, null, pProcessingData, cancelBtnTitleId);
            enterPinDialog.DialogStyle = DialogStyle.FULLSCREEN;
            enterPinDialog.Show(this);
        }

        private void ShowEntryCardNumberDialog()
        {
            var entryDlgData = new EntryCardNumberDlgData();

            entryDlgData.fShowExpiry = true;

            entryDlgData.fNonCreditCards = false;

            entryDlgData.fOffline = false;

            entryDlgData.fShowTokenFlag = false;

            entryDlgData.szPAN = "";

            entryDlgData.fAnyProcessorsUsingTokens = false;

            entryDlgData.fShowBackButton = true;

            var entryCardNumberDialog = new ShellUI.EntryCardNumberDialog(StringIds.STRING_MANUAL_PAY, null, entryDlgData);
            entryCardNumberDialog.DialogStyle = DialogStyle.FULLSCREEN;
            entryCardNumberDialog.Show(this);
        }

        private void ShowEntryExpiryDateDialog()
        {
            var entryDlgData = new EntryCardNumberDlgData();

            entryDlgData.fShowExpiry = true;

            entryDlgData.fNonCreditCards = false;

            entryDlgData.fOffline = false;

            entryDlgData.fShowTokenFlag = false;

            entryDlgData.szPAN = "";

            entryDlgData.fAnyProcessorsUsingTokens = false;

            entryDlgData.fShowBackButton = true;

            entryDlgData.szExpiry = "";

            var entryExpiryDateDialog = new ShellUI.EntryExpiryDateDialog(StringIds.STRING_EXPIRYDATE, null, entryDlgData);
            entryExpiryDateDialog.DialogStyle = DialogStyle.FULLSCREEN;
            entryExpiryDateDialog.Show(this);
        }

        private void ShowEntryCVVDialog()
        {
            var entrySecurityData = new EntryCVVDlgData();

            var entryCVVDialog = new ShellUI.EntryCVVDialog(StringIds.STRING_CSC_CODE, null, entrySecurityData);
            entryCVVDialog.DialogStyle = DialogStyle.FULLSCREEN;
            entryCVVDialog.Show(this);
        }

        private void ShowStandardSetupDialog()
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

            var standardSetupDialog = new ShellUI.StandardSetupDialog(StringIds.STRING_NO_CVV, null, noCVVStatusDlgData);
            standardSetupDialog.DialogStyle = DialogStyle.FULLSCREEN;
            standardSetupDialog.Show(this);
        }

        private void ShowCancelPreAuthConfirmDialog()
        {
            var data = new CancelPreAuthComfirmDlgData()
            {
                lAmount = 488,
                AuthCode = "287635",
                CardInfo = string.Format("{0} {1} *{2}", Localize.GetString(StringIds.STRING_CARDTYPE_VISA), Localize.GetString(StringIds.STRING_CREDIT), "8770"),
            };

            var cancelPreAuthConfirmDialog = new CancelPreAuthConfirmDialog(StringIds.STRING_CANCEL_PRE_AUTH, null, data);
            cancelPreAuthConfirmDialog.DialogStyle = DialogStyle.FULLSCREEN;
            cancelPreAuthConfirmDialog.Show(this);
        }

        private void ShowMainDialog()
        {
            var dialog = new MainDialog(StringIds.STRING_STARTAPP, null, 0);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        private void ShowSelectMoto()
        {
            //hardcode dialog here
            var generalType = new List<GenericType>()
            {
                new GenericType()
                {
                        Icon = IconIds.VECTOR_CURRENT_SETTLEMENT_DATE,
                        lszText =  StringIds.STRING_CURRENT_SETTLEMENT_DATE,
                        Id = GlobalResource.CURRENT_BUTTON,
                },
                new GenericType()
                {
                        Icon = IconIds.VECTOR_OTHER_DATE,
                        lszText =  StringIds.STRING_OTHER_DATE,
                        Id = GlobalResource.OTHER_BUTTON,
                },
            };

            var dialog3 = new UI.DynamicOptionDialog(StringIds.STRING_MOTO_TRANSACTIONS, null, generalType, StringIds.STRING_SELECTT_MOTO);
            dialog3.DialogStyle = DialogStyle.FULLSCREEN;
            dialog3.Show(this);
        }

        private void ShowSelectDate()
        {
            ////hardcode dialog here
            var generalType = new List<GenericType>()
            {
                new GenericType()
                {
                        Icon = IconIds.VECTOR_CURRENT_SETTLEMENT_DATE,
                        lszText =  StringIds.STRING_CURRENT_SETTLEMENT_DATE,
                        Id = GlobalResource.CURRENT_BUTTON,
                },
                new GenericType()
                {
                        Icon = IconIds.VECTOR_OTHER_DATE,
                        lszText =  StringIds.STRING_OTHER_DATE,
                        Id = GlobalResource.OTHER_BUTTON,
                },
            };

            var dialog3 = new UI.DynamicOptionDialog(StringIds.STRING_SETTLEMENT_INQUIRY, null, generalType, StringIds.STRING_SELECT_DATE, GlobalResource.CANCEL_SUB_FLOW, StringIds.STRING_CANCEL);
            dialog3.DialogStyle = DialogStyle.FULLSCREEN;
            dialog3.Show(this);
        }


        private void ShowSettlementOptions()
        {
            var generalType = new List<GenericType>()
            {
                new GenericType()
                {
                        Icon = IconIds.VECTOR_SETTLEMENT_INQUIRY,
                        lszText =  StringIds.STRING_SETTLEMENT_INQUIRY,
                        Id = GlobalResource.SETTLEMENT_ENQUIRY_BUTTON,
                },
                new GenericType()
                {
                        Icon = IconIds.VECTOR_SETTLEMENT_CUTOVER,
                        lszText =  StringIds.STRING_SETTLEMENT_CUTOVER,
                        Id = GlobalResource.SETTLEMENT_CUTOVER_BUTTON,
                },
            };

            var dialog3 = new UI.DynamicOptionDialog(StringIds.STRING_SETTLEMENT_INQUIRY, null, generalType, StringIds.STRING_SETTLEMENT_OPTIONS);
            dialog3.DialogStyle = DialogStyle.FULLSCREEN;
            dialog3.Show(this);
        }

        private void ShowReprintOptions()
        {
            var generalType = new List<GenericType>()
            {
                new GenericType()
                {
                        Icon = IconIds.VECTOR_LAST_RECEIPT,
                        lszText =  StringIds.STRING_LAST_RECEIPT,
                        Id = GlobalResource.REPRINTLAST_BUTTON,
                },
                new GenericType()
                {
                        Icon = IconIds.VECTOR_FIND_RECEIPT,
                        lszText =  StringIds.STRING_FIND_RECEIPT,
                        Id = GlobalResource.REPRINTFIND_BUTTON,
                },
            };

            var dialog3 = new UI.DynamicOptionDialog(StringIds.STRING_REPRINT_TITLE, null, generalType, StringIds.STRING_CUSTOMER_REPRINT_OPTION);
            dialog3.DialogStyle = DialogStyle.FULLSCREEN;
            dialog3.Show(this);
        }

        private void ShowPreAuthEnterAmountDialog(CaseDialog caseDialog)
        {
            long amount = 10000;

            bool fReferenceEnable = false;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    fReferenceEnable = true;
                    break;
                case CaseDialog.CASE2:
                    fReferenceEnable = false;
                    break;
            }

            var dialog = new PreAuthEnterAmountDialog(StringIds.STRING_PRE_AUTH, null, amount, fReferenceEnable);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        private void ShowSettlementGetDateDialog()
        {
            InitProcessData pInitProcessData = new InitProcessData();
            var dateFormat = GlobalConstants.FORMAT_DATE_DD_MM_YYYY;

            var dialog = new SettlementGetDateDialog(StringIds.STRING_ENTER_SETTLEMENT_DATE, null, pInitProcessData, dateFormat);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }


        private void ShowSearchOptions()
        {
            FunctionType functionType = FunctionType.PreAuthComplete;
            bool isReferenceDisabled = false;
            string IdDlgTitle = string.Empty;

            TransactionFindModel findModel = new TransactionFindModel()
            {
                Amount = 1000,
                IsExactAmount = false,
                Last4Digit = "1234",
                AuthCode = "5678",
                STAN = "224466"
            };

            FindPurchaseOptionDlgData dlgData = new FindPurchaseOptionDlgData();

            dlgData.Items.Add(new InputAmountEditModel()
            {
                TitleId = StringIds.STRING_AMOUNT,
                PropertyName = nameof(TransactionFindModel.Amount),
                Value = findModel.Amount,
                SpanSize = 1
            });

            dlgData.Items.Add(new SwitcherEditModel()
            {
                TitleId = StringIds.STRING_EXACT_AMOUNT,
                PropertyName = nameof(TransactionFindModel.IsExactAmount),
                Value = findModel.IsExactAmount,
                SpanSize = 1
            });


            var endCardNumb = new InputNumberFixedKeyboardEditModel()
            {
                TitleId = StringIds.STRING_LAST_FOUR_DIGITS,
                Value = findModel.Last4Digit,
                MaxLength = 4,
                PropertyName = nameof(TransactionFindModel.Last4Digit),
                SpanSize = 1
            };

            var authCodeModel = new InputNumberAndTextFixedKeyboardEditModel()
            {
                TitleId = StringIds.STRING_AUTHCODE,
                Value = findModel.AuthCode,
                IsEnabled = true,
                PropertyName = nameof(TransactionFindModel.AuthCode),
                SpanSize = 1
            };

            var stanModel = new InputNumberFixedKeyboardEditModel()
            {
                TitleId = StringIds.STRING_STAN,
                Value = findModel.STAN,
                IsEnabled = true,
                PropertyName = nameof(TransactionFindModel.STAN),
                SpanSize = 1
            };

            dlgData.Items.Add(endCardNumb);
            dlgData.Items.Add(authCodeModel);
            dlgData.Items.Add(stanModel);

            switch (functionType)
            {
                case FunctionType.Refund:

                    IdDlgTitle = StringIds.STRING_REFUND_PURCHASE;

                    dlgData.TopTitleId = StringIds.STRING_FIND_PURCHASE_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PURCHASE;

                    break;

                case FunctionType.PreAuthComplete:
                case FunctionType.PreAuthIncrement:
                case FunctionType.PreAuthPartial:
                case FunctionType.PreAuthCancel:
                case FunctionType.PreAuthDelayedCompletion:

                    IdDlgTitle = StringIds.STRING_FINAL_COMPLETION;

                    dlgData.TopTitleId = StringIds.STRING_FIND_PREAUTH_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.OpenTab:

                case FunctionType.CloseTab:

                    IdDlgTitle = functionType == FunctionType.OpenTab ? StringIds.STRING_OPEN_TAB : StringIds.STRING_CLOSE_TAB;

                    dlgData.TopTitleId = StringIds.STRING_FIND_OPEN_TAB;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_OPEN_TAB;

                    break;

                case FunctionType.ReprintPreAuth:

                    IdDlgTitle = StringIds.STRING_REPRINT_RECEIPT;

                    dlgData.TopTitleId = StringIds.STRING_FIND_PREAUTH_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.Reprint:

                    IdDlgTitle = StringIds.STRING_REPRINT_RECEIPT;

                    dlgData.TopTitleId = StringIds.STRING_SEARCH_PAYMENTS;

                    dlgData.RightBtnTitleId = StringIds.STRING_SEARCH_PAYMENTS;

                    break;

                default:

                    break;
            }

            var dialog = new FindPurchaseOptionDialog(IdDlgTitle, null, dlgData, findModel);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        private void PreAuthItemGetNewAmount(CaseDialog caseDialog)
        {
            RecordViewModel selectedPayment = new RecordViewModel();
            selectedPayment.iCardType = CARDTYPE.CARD_AMEX;
            selectedPayment.iAccountTypeCode = AccountType.ACCOUNT_TYPE_SAVINGS;
            selectedPayment.szApprovalCode = "123456";
            selectedPayment.lAmount = 10000;
            selectedPayment.lszEndCardNumber = "7654";
            selectedPayment.CustomerReferenceType = ReferenceType.Room;
            selectedPayment.lszCustomerReference = "6789";
            //selectedPayment.AuthorizationExpiryDate = DateTime.Now;

            FunctionType functionType;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    functionType = FunctionType.PreAuthPartial;
                    break;
                case CaseDialog.CASE2:
                    functionType = FunctionType.PreAuthComplete;
                    break;
                case CaseDialog.CASE3:
                    functionType = FunctionType.PreAuthIncrement;
                    break;
                default:
                    functionType = FunctionType.PreAuthPartial;
                    break;
            }


            var data = new PreAuthCompletionDlgData()
            {
                CustomAmount = 38000,
                Amount = selectedPayment.lAmount,
                OriginalCardType = string.Format("{0} {1}", Localize.GetString(StringIds.STRING_CARDTYPE_AMEX), Localize.GetString(StringIds.STRING_ACCOUNTTYPESAVINGS)),
                LastFourDigitCardNumber = $"**** **** **** {selectedPayment.lszEndCardNumber}",
                Reference = selectedPayment.CustomerReferenceType,
                ReferenceNumber = selectedPayment?.lszCustomerReference,
                //ExpireTime = selectedPayment?.AuthorizationExpiryDate,
                CustomerName = "David",
                PaymentStatus = Localize.GetString(StringIds.STRING_APPROVED).ToUpper(),
                AuthCode = selectedPayment?.szApprovalCode,
                CardBrandIconResName = CARDTYPE.CARD_AMEX.GetIconDrawable(),
                FunctionType = functionType
            };

            switch (functionType)
            {
                case FunctionType.PreAuthPartial:

                    data.RightBottomButtonTitle = StringIds.STRING_PAY;
                    data.TopTitle = StringIds.STRING_ENTER_PARTIAL_AMOUNT;

                    break;

                case FunctionType.PreAuthDelayedCompletion:

                    data.Amount = 0;
                    data.RightBottomButtonTitle = StringIds.STRING_COMPLETE_PRE_AUTH;
                    data.TopTitle = StringIds.STRING_DELAYED_PAYMENT_AMOUNT;

                    break;

                case FunctionType.PreAuthIncrement:

                    data.RightBottomButtonTitle = StringIds.STRING_TOP_UP;
                    data.TopTitle = StringIds.STRING_ENTER_TOP_UP_AMOUNT;

                    break;

                case FunctionType.PreAuthComplete:

                    data.RightBottomButtonTitle = StringIds.STRING_COMPLETE_PRE_AUTH;
                    data.TopTitle = StringIds.STRING_ENTER_FINAL_AMOUNT;

                    break;
            }

            var ret = DialogBuilder.Show(IPayDialog.PREAUTH_COMPLE_GET_NEW_AMOUNT_DIALOG, functionType.ToStringId(), (iResult, args) =>
            {
                //PreAuthCompleGetNewAmountDialog
            }, true, false, data);
        }

        private void ShowListPaymentDialog()
        {
            var item1 = new TransactionInfoModel();
            item1.Amount = 10000;
            item1.AuthNumber = "87654";
            item1.CardType = "Visa Debit";
            item1.CardInfo = "****6785";
            var date = new XDateTime();
            date.Year = 2023;
            date.Month = 6;
            date.Day = 30;
            date.Hours = 11;
            date.Minutes = 30;
            item1.CreatedDate = date;
            item1.CustomerName = "David";
            item1.ReferenceTypeStringIds = "Table 10";
            item1.Value = "4";
            item1.FunctionType = "Refund";
            item1.TransactionNumber = "4200000027201709294868542706";
            item1.FunctionType = Localize.GetString(FunctionType.Wechat.ToStringId());
            item1.ReferenceTypeStringIds = ReferenceType.Invoice.ToStringId();
            item1.ReferenceNumber = "123";
            item1.IconId = IconIds.VECTOR_REFUNDED;
            item1.Status = PaymentStatus.Refunded;


            var item2 = new TransactionInfoModel();
            item2.Amount = 10000;
            item2.AuthNumber = "87654";
            item2.CardType = "Visa Debit";
            item2.CardInfo = "****6785";
            date.Year = 2023;
            date.Month = 6;
            date.Day = 30;
            date.Hours = 11;
            date.Minutes = 30;
            item2.CreatedDate = date;
            item2.CustomerName = "David";
            item2.ReferenceTypeStringIds = "Table 10";
            item2.Value = "4";
            item2.FunctionType = "Refund";
            item2.TransactionNumber = "4200000027201709294868542706";
            item2.FunctionType = Localize.GetString(FunctionType.Wechat.ToStringId());
            item2.ReferenceTypeStringIds = ReferenceType.Invoice.ToStringId();
            item2.ReferenceNumber = "123";
            item2.IconId = IconIds.VECTOR_REFUNDED;
            item2.Status = PaymentStatus.Refunded;

            var item3 = new TransactionInfoModel();
            item3.Amount = 10000;
            item3.AuthNumber = "87654";
            item3.CardType = "Visa Debit";
            item3.CardInfo = "****6785";
            date.Year = 2023;
            date.Month = 6;
            date.Day = 30;
            date.Hours = 11;
            date.Minutes = 30;
            item3.CreatedDate = date;
            item3.CustomerName = "David";
            item3.ReferenceTypeStringIds = "Table 10";
            item3.Value = "4";
            item3.FunctionType = "Refund";
            item3.TransactionNumber = "4200000027201709294868542706";
            item3.FunctionType = Localize.GetString(FunctionType.Wechat.ToStringId());
            item3.ReferenceTypeStringIds = ReferenceType.Invoice.ToStringId();
            item3.ReferenceNumber = "123";
            item3.IconId = IconIds.VECTOR_REFUNDED;
            item3.Status = PaymentStatus.Refunded;

            var data = new ListPaymentRecordsDlgData();
            data.Items.Add(item1);
            data.Items.Add(item2);
            data.Items.Add(item3);
            DialogBuilder.Show(IPayDialog.LIST_ITEM_RECORDS_DIALOG, StringIds.STRING_REFUND_PURCHASE, null, true, false, data, FunctionType.WechatRefund);
            //ListPaymentDialog
        }

        void BasketItemSelectOffer()
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

            DialogBuilder.Show(IPayDialog.CUS_DISPLAY_BASKET_ITEM_SELECT_OFFER_DIALOG, string.Empty, (result, args) =>
            {

            }, true, false, baskets);
        }

        void SignOrPin()
        {
            DialogBuilder.Show(IShellDialog.SIGN_OR_PIN_DIALOG, StringIds.STRING_SIGNATURE_AND_PIN, (iResult, args) =>
            {

            }, true, false);
        }

        void DigitalSignature()
        {
            DigitalSignatureDlgData data = new DigitalSignatureDlgData();

            //DialogBuilder.MerchantName = "Dr Michael PATterson";
            //DialogBuilder.IsShowMerchantName = true;
            var ret = DialogBuilder.Show(IShellDialog.DIGITAL_SIGNATURE_DIALOG, StringIds.STRING_DIGITAL_SIGNATURE, (iResult, args) =>
            {

            }, true, false, data);
        }

        int CustomerAuthenticateOptions()
        {
            var ret = DialogBuilder.Show(IPayDialog.CUS_DISPLAY_AUTHENTICATE_OPTIONS_DIALOG, StringIds.STRING_ICUSTOMER_AUTHENTICATE, (result, args) =>
            {

            }, true, false);

            return ret;
        }

        void ShowBasketItemReviewDialog()
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

            DialogBuilder.Show(IPayDialog.CUS_DISPLAY_BASKET_ITEM_REVIEW_DIALOG, string.Empty, (result, args) =>
            {

            }, true, false, baskets, false);
        }

        void ShowConfirmServiceDialog()
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

            DialogBuilder.Show(IPayDialog.CONFIRM_SERVICE_DIALOG, StringIds.STRING_CUSTOMER_RATING, (iResult, args) =>
            {

            }, true, false, data, true);
        }

        void ShowConfirmSurveyDialog()
        {
            var item1 = new SurveyItem() { Title = "Service Quality", Icon = "vector_service_quality", StarMaxRating = 5 };
            var item2 = new SurveyItem() { Title = "Employee Efficiency", Icon = "vector_employee_efficiency", StarMaxRating = 5 };
            var item3 = new SurveyItem() { Title = "Product Quality", Icon = "vector_product_quality", StarMaxRating = 5 };
            var item4 = new SurveyItem() { Title = "Return Likelihood", Icon = "vector_return_likelihood", StarMaxRating = 5 };
            var item5 = new SurveyItem() { Title = "Rated to Competitors", Icon = "vector_rated_to_competitors", StarMaxRating = 5 };
            var data = new List<SurveyItem>() { item1, item2, item3, item4, item5 };

            DialogBuilder.Show(IPayDialog.CONFIRM_SURVEY_DIALOG, StringIds.STRING_CUSTOMER_SURVEY, (iResult, args) =>
            {
            }, true, false, data);
        }

        void AdvancedSearch()
        {
            FunctionType iFunctionButton = FunctionType.PreAuthComplete;
            TransactionFindModel localModel = new TransactionFindModel();

            string RightButtonTextId = string.Empty;

            switch (iFunctionButton)
            {
                case FunctionType.Refund:

                    RightButtonTextId = StringIds.STRING_FIND_PURCHASE;

                    break;

                case FunctionType.PreAuthComplete:
                case FunctionType.PreAuthIncrement:
                case FunctionType.PreAuthPartial:
                case FunctionType.PreAuthCancel:
                case FunctionType.PreAuthDelayedCompletion:

                    RightButtonTextId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.Reprint:

                case FunctionType.ReprintPreAuth:

                    RightButtonTextId = StringIds.STRING_FIND_RECEIPT;

                    break;
            }

            var result = DialogBuilder.Show(IPayDialog.ADVANCED_SEARCH_DIALOG, StringIds.STRING_ADVANCE_SEARCH, (iResult, args) =>
            {

            }, true, false, localModel, RightButtonTextId);

        }

        void ShowEmailReceiptSendResultDialog(CaseDialog caseDialog)
        {
            var selectedEmail = "d.timms@yahoo.com";
            var selectedCellNumber = "+61 404 033 099";

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    DialogBuilder.Show(IPayDialog.RECEIPT_RESULT_DIALOG, StringIds.STRING_EMAIL_RECEIPT_LOWCASE, (iResult, args) =>
                    {
                        //EmailReceiptSendResultDialog
                    }, true, false, new ReceiptResultDlgData(selectedEmail, UtilEnum.ReceiptType.Email, true));
                    break;

                case CaseDialog.CASE2:

                    DialogBuilder.Show(IPayDialog.RECEIPT_RESULT_DIALOG, StringIds.STRING_EMAIL_RECEIPT_LOWCASE, (iResult, args) =>
                    {
                        //EmailReceiptSendResultDialog
                    }, true, false, new ReceiptResultDlgData(selectedEmail, UtilEnum.ReceiptType.Email, false));
                    break;

                case CaseDialog.CASE3:
                    DialogBuilder.Show(IPayDialog.RECEIPT_RESULT_DIALOG, StringIds.STRING_TEXT_RECEIPT, (iResult, args) =>
                    {
                        //EmailReceiptSendResultDialog
                    }, true, false, new ReceiptResultDlgData(selectedCellNumber, UtilEnum.ReceiptType.Text, true));
                    break;

                case CaseDialog.CASE4:
                    DialogBuilder.Show(IPayDialog.RECEIPT_RESULT_DIALOG, StringIds.STRING_TEXT_RECEIPT, (iResult, args) =>
                    {
                        //EmailReceiptSendResultDialog
                    }, true, false, new ReceiptResultDlgData(selectedCellNumber, UtilEnum.ReceiptType.Text, false));
                    break;

            }


        }

        void ListPaymentRecordDialog()
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
            DialogBuilder.Show(IPayDialog.LIST_ITEM_RECORDS_DIALOG, StringIds.STRING_REFUND_PURCHASE, null, true, false, data);
        }

        void ShowSelectTipDialog()
        {
            double x = 5.00;
            long y = 1000;
            long originalAmount = 90000;
            long amount = 10000;
            var keyValue = new KeyValuePair<double, long>(x, y);
            var data = new List<KeyValuePair<double, long>>() { keyValue, keyValue, keyValue, keyValue };

            var dialog = new SelectTipDialog(StringIds.STRING_SELECT_TIP_AMOUNT, null, data, true, amount, originalAmount);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowEnterTipAmountDialog()
        {
            object[] data = new object[2];
            data[0] = false;
            data[1] = "Enter Tip Amount";
            var dialog = new GetTipAmountDialog("Enter Tip", null, data);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowSurchargeConfirmDialog()
        {
            var data = new SurchargeConfirmationDlgData();
            data.iCardType = CARDTYPE.CARD_VISA;
            data.szCardNumber = "**** **** **** 7654";
            data.szCardHolderName = "David Smith";
            data.fRemoveSurchargeFee = false;
            data.wszCurrencyCode = "NZD Currency";

            var initData = new ShellInitProcessData();
            initData.lAmount = 13018;
            initData.lCashOut = 1000;
            initData.lAccountSurChargeFee = 25;
            initData.lServiceFee = 2000;
            initData.lTotalAmount = 10000;
            initData.lTotalAmountPaid = 5000;
            initData.lSurChargeFee = 25;
            initData.lSurChargePercent = 10;
            initData.lAccountSurChargePercent = 5;
            initData.iFunctionButton = FunctionType.PreAuthComplete;
            initData.ExpandedPayment = new Payment()
            {
                lAmount = 5000
            };
            data.pInitProcessData = initData;

            var dialog4 = new SurchargeConfirmDialog(StringIds.STRING_SURCHARGE_CONFIRMATION, null, data);
            dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            dialog4.Show(this);
        }

        void ShowAuthenticatingDialog()
        {
            CustomerDisplayAuthType authType = CustomerDisplayAuthType.FingerPrintID;

            DialogBuilder.Show(IPayDialog.CUS_DISPLAY_AUTHENTICATING_DIALOG, StringIds.STRING_AUTHENTICATING_CUSTOMER, (iResult, args) =>
            {
            }, true, false, authType, false);
        }

        void ShowGetAmountDialog(CaseDialog caseDialog)
        {
            var data = new GetAmountDlgData();

            data.lszPayButtonText = StringIds.STRING_OK_UPCASE;
            data.EntryAmountTitleId = StringIds.STRING_PURCHASE;
            data.plszReference = "123456";
            data.ReferenceTypeTitleId = DataHelper.GetRefName(ReferenceType.Invoice);
            //data.isEnabledEntryAmount = false;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    data.fShowReference = false;
                    //data.isEnabledEntryAmount = true;

                    break;
                case CaseDialog.CASE2:
                    data.fShowReference = true;
                    //data.isEnabledEntryAmount = false;
                    break;

                default:
                    data.fShowReference = true;
                    //data.isEnabledEntryAmount = true;
                    break;
            }

            var dialog = new GetAmountDialog(StringIds.STRING_PURCHASE_UPCASE, null, data);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowCustomerDisplayGetCashOutDialog()
        {
            GetAmountDlgData dlgData = new GetAmountDlgData();

            dlgData.plszReference = "7654";
            dlgData.ReferenceTypeTitleId = DataHelper.GetRefName(ReferenceType.Employee);
            dlgData.plAmount = 38000;
            dlgData.fCalculator = false;
            dlgData.fShowReference = true;
            dlgData.EntryAmountTitleId = StringIds.STRING_REFERENCE;
            dlgData.fInstoreCashoutFeeEnable = true;
            dlgData.fInstoreCashoutFeePercent = true;
            dlgData.lInstoreCashoutFeeAmount = 10000;

            var ret = DialogBuilder.Show(IPayDialog.CUS_DISPLAY_GET_CASH_OUT_DIALOG, string.Empty, (result, args) =>
            {


            }, true, false, 68000, dlgData);
        }

        void ShowPreAuthCompletePreAuthInfoDialog()
        {
            RecordViewModel selectedPayment = new RecordViewModel();
            selectedPayment.iCardType = CARDTYPE.CARD_AMEX;
            selectedPayment.iAccountTypeCode = AccountType.ACCOUNT_TYPE_SAVINGS;
            selectedPayment.szApprovalCode = "123456";
            selectedPayment.lAmount = 10000;
            selectedPayment.DateTime = DateTime.Now.ToXDateTime();
            selectedPayment.lszEndCardNumber = "7654";
            selectedPayment.CustomerReferenceType = ReferenceType.Room;
            selectedPayment.lszCustomerReference = "123";
            selectedPayment.szSTAN = "2345";
            selectedPayment.szReferenceNumber = "1234";
            //selectedPayment.AuthorizationExpiryDate = DateTime.Now;

            var data = new PreAuthCompletePreAuthDetailsDlgData()
            {
                Status = UtilEnum.PaymentStatus.Approved,
                CreatedBy = selectedPayment.DateTime,
                Amount = selectedPayment.lAmount,
                AuthCode = selectedPayment.szApprovalCode,
                OriginalCardType = string.Format("{0} {1}", Localize.GetString(selectedPayment.iCardType.GetNameStringId()), Localize.GetString(StringIds.STRING_CREDIT)),
                OriginalLastFourDigitCardNumber = "****" + selectedPayment.lszEndCardNumber,
                LastFourDigitCardNumber = $"****{selectedPayment.lszEndCardNumber}",
                Reference = selectedPayment.CustomerReferenceType,
                ReferenceNumber = selectedPayment.lszCustomerReference,
                STAN = selectedPayment.szSTAN,
                RRNNumber = selectedPayment.szReferenceNumber,
                TransactionId = selectedPayment.Id.ToString(),
                //ExpireTime = selectedPayment.AuthorizationExpiryDate,
                CustomerName = "TRUONG VINH LOI",  //GetCustomerName.OriginalPayment,TODO disabled on certification
            };

            string titleDialog = StringIds.STRING_FINAL_COMPLETION;

            FunctionType functionType = FunctionType.PreAuthComplete;

            switch (functionType)
            {
                case FunctionType.PreAuthComplete:

                    data.BottomRightBtnTitleId = StringIds.STRING_COMPLETE_PRE_AUTH;

                    titleDialog = StringIds.STRING_FINAL_COMPLETION;

                    break;

                case FunctionType.PreAuthCancel:

                    data.BottomRightBtnTitleId = StringIds.STRING_CANCEL_PRE_AUTH;

                    titleDialog = StringIds.STRING_CANCEL_PRE_AUTH;

                    break;

                case FunctionType.PreAuthIncrement:

                    data.BottomRightBtnTitleId = StringIds.STRING_TOP_UP;

                    titleDialog = StringIds.STRING_TOP_UP;

                    break;

                case FunctionType.PreAuthDelayedCompletion:

                    data.Amount = 0;

                    data.BottomRightBtnTitleId = StringIds.STRING_DELAYED_COMPLETION;

                    titleDialog = StringIds.STRING_DELAYED_COMPLETION;

                    break;

                case FunctionType.PreAuthPartial:

                    data.BottomRightBtnTitleId = StringIds.STRING_PARTIAL_COMPLETION;

                    titleDialog = StringIds.STRING_PARTIAL_COMPLETION;

                    break;

                case FunctionType.ReprintPreAuthPending:

                    if (selectedPayment.iFunctionType == FunctionType.PreAuthComplete)
                        data.Amount = 0;

                    data.BottomRightBtnTitleId = StringIds.STRING_REPRINT_RECEIPT;

                    titleDialog = StringIds.STRING_PRINT_PENDING_HOSPITALITY;

                    break;
            }

            DialogBuilder.Show(IPayDialog.PREAUTH_COMPLETE_PREAUTH_INFO_DIALOG, titleDialog, (iResult, args) =>
            {
                //PreAuthCompletePreAuthInfoDialog
            }, true, false, data);
        }

        void ShowLogonDialogCase01()
        {
            object[] param = new object[3];
            var data = new LogonDlgData();
            data.InitData = new InitLogonModel();
            data.InitData.IdUserMustMatch = 1;
            param[0] = data;
            param[1] = true;
            var dialog = new LogonDialog(StringIds.STRING_LOGON, null, data, true);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowLogonDialogCase02()
        {
            object[] param = new object[3];
            var data = new LogonDlgData();
            data.InitData = new InitLogonModel();
            param[0] = data;
            param[1] = true;
            var dialog = new LogonDialog(StringIds.STRING_LOGON, null, data, true);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowSingleUserLoginDialog()
        {
            var userId = string.Empty;
            var passcode = string.Empty;
            var pLogonData = new LogonDlgData()
            {
                InitData = new InitLogonModel()
                {
                    fLogonPasscode = true,
                    fAutoLogin = false,
                }
            };

            DialogBuilder.Show(IPayDialog.SINGLE_USER_LOGIN_DIALOG, StringIds.STRING_LOGON, (iResult, args) =>
            {

            }, true, false, pLogonData, false);
        }

        void ShowGetAmountCashOutDialog(CaseDialog caseDialog)
        {
            var data = new GetAmountDlgData();
            data.lszPayButtonText = StringIds.STRING_OK_UPCASE;
            data.lInstoreCashoutFeeAmount = 1000;
            data.plAmount = 2800;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    data.fInstoreCashoutFeeEnable = true;
                    data.fInstoreCashoutFeePercent = true;
                    break;

                case CaseDialog.CASE2:
                    data.fInstoreCashoutFeeEnable = false;
                    data.fInstoreCashoutFeePercent = false;
                    break;

                default:
                    data.fInstoreCashoutFeeEnable = true;
                    data.fInstoreCashoutFeePercent = false;
                    break;
            }

            DialogBuilder.Show(IPayDialog.GET_AMOUNT_CASH_OUT_DIALOG, StringIds.STRING_CASH, (iResult, args) =>
            {
            }, true, false, data);
        }

        void ShowSelectMerchantDialog()
        {
            IList<Merchant> merchants = new List<Merchant>();
            Merchant madison = new Merchant();

            madison.lszMerchantName = "Madison";
            madison.Id = 1;
            madison.lszLogo = "icon_default_merchant";
            madison.lszPicture = "icon_default_merchant";
            madison.lszTerminalIDNumber = "49500096";
            madison.Setup.fPromptCustomerRef = false;
            madison.Setup.fRefundMerchantCard = true;
            madison.Setup.fRefundAccessCode = false;
            madison.Setup.lszRefundAccessCode = "1111";
            madison.Setup.fPrintQRClaiming = true;
            madison.Setup.fAdvertising = false;
            madison.Setup.fInstoreCashoutFeeEnable = false;
            madison.Setup.fInstoreCashoutFeePercent = false;
            madison.Setup.lInstoreCashoutFeeAmount = 0;
            merchants.Add(madison);

            Merchant david = new Merchant();

            david.lszMerchantName = "David";
            david.Id = 1;
            david.lszLogo = "icon_default_merchant";
            david.lszPicture = "icon_default_merchant";
            david.lszTerminalIDNumber = "49500096";
            david.Setup.fPromptCustomerRef = false;
            david.Setup.fRefundMerchantCard = true;
            david.Setup.fRefundAccessCode = false;
            david.Setup.lszRefundAccessCode = "1111";
            david.Setup.fPrintQRClaiming = true;
            david.Setup.fAdvertising = false;
            david.Setup.fInstoreCashoutFeeEnable = false;
            david.Setup.fInstoreCashoutFeePercent = false;
            david.Setup.lInstoreCashoutFeeAmount = 0;
            merchants.Add(david);

            Merchant John = new Merchant();

            John.lszMerchantName = "John Smith";
            John.Id = 1;
            John.lszLogo = "icon_default_merchant";
            John.lszPicture = "icon_default_merchant";
            John.lszTerminalIDNumber = "49500096";
            John.Setup.fPromptCustomerRef = false;
            John.Setup.fRefundMerchantCard = true;
            John.Setup.fRefundAccessCode = false;
            John.Setup.lszRefundAccessCode = "1111";
            John.Setup.fPrintQRClaiming = true;
            John.Setup.fAdvertising = false;
            John.Setup.fInstoreCashoutFeeEnable = false;
            John.Setup.fInstoreCashoutFeePercent = false;
            John.Setup.lInstoreCashoutFeeAmount = 0;

            merchants.Add(John);

            Merchant luke = new Merchant();

            luke.lszMerchantName = "Dr Luke";
            luke.Id = 4;
            luke.lszLogo = "icon_default_merchant";
            luke.lszPicture = "icon_default_merchant";
            luke.lszTerminalIDNumber = "49500096";
            luke.Setup.fPromptCustomerRef = false;
            luke.Setup.fRefundMerchantCard = true;
            luke.Setup.fRefundAccessCode = false;
            luke.Setup.lszRefundAccessCode = "1111";
            luke.Setup.fPrintQRClaiming = true;
            luke.Setup.fAdvertising = false;
            luke.Setup.fInstoreCashoutFeeEnable = false;
            luke.Setup.fInstoreCashoutFeePercent = false;
            luke.Setup.lInstoreCashoutFeeAmount = 0;

            merchants.Add(luke);

            DialogBuilder.Show(IPayDialog.SELECT_MERCHANT_DIALOG, StringIds.STRING_MERCHANT, (int iResult, object[] args) =>
            {

            }, true, false, merchants);
        }

        void ShowSelectDonationDialog()
        {
            //var data = new CusDisplaySelectDonationDlgData();

            ////landscape
            ////data.DonationImgName = "donation_port_demo_02";

            ////portrait
            //data.DonationImgName = "donation_small";

            //var listValue = new List<long>() { 1, 2, 5, 10, 50 };
            //data.DonationValues = listValue;

            //DialogBuilder.Show(IPayDialog.SELECT_DONATION_AMOUNT_DIALOG, StringIds.STRING_DONATION, (iResult, args) =>
            //{

            //}, true, false, data);
        }

        void ShowAdjustDonationDialog()
        {
            long amount = 38000;

            List<long> donation = new List<long> { 50, 200, 500, 1000 };

            //landscape
            //string iconId = "donation_port_demo_02.png";

            //portrait
            string iconId = "donation_small.png";

            string noBtnTitleId = "STRING_DONT_WANT_TO_DONATE";
            int noBtnCommand = 5016;

            var dialog = new AdjustDonationDialog(StringIds.STRING_DONATION, null, amount, donation, iconId, noBtnTitleId, noBtnCommand);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowAdvertisingDialog()
        {
            IList<string> imagePaths = new List<string>();

            //portrait
            imagePaths.Add("ads_1.png");
            imagePaths.Add("ads_2.png");

            //landspace
            //imagePaths.Add("land_avertising_1.png");
            //imagePaths.Add("ads_1_land.png");
            //imagePaths.Add("ads_3_land.png");

            int ellapseSeconds = 2;

            var dialog = new AdvertisingDialog(StringIds.STRING_ANDROID_PAYMENTS, null, imagePaths, ellapseSeconds, GlobalResource.NEXT_BUTTON, true);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowReceiptOptionDialog()
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
                iCommand = GlobalResource.BUTTON_EMAIL_RECEIPT,
            });

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_TEXT_RECEIPT,
                idImage = IconIds.VECTOR_TEXT_RECEIPT,
                IdProcessor = 0,
                iCommand = GlobalResource.BUTTON_TEXT_RECEIPT,
            });

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_PRINT_CUSTOMER,
                idImage = IconIds.VECTOR_PRINT_RECEIPT,
                IdProcessor = 0,
                iCommand = GlobalResource.BUTTON_PRINT_RECEIPT,
            });

            var dialog4 = new ReceiptOptionsDialog(StringIds.STRING_RECEIPT_OPTIONS, null, data);
            dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            dialog4.Show(this);
        }

        void ShowRequestAliPayWechat(string szQRCode, bool fAliPay = true, bool blockUI = false, ResultStatus status = ResultStatus.None)
        {
            QrCodeAlipayWeChatDlgData dlgData = new QrCodeAlipayWeChatDlgData();

            FunctionType iFunctionButton;

            iFunctionButton = FunctionType.Purchase;

            var titleId = string.Empty;

            dlgData.lAmount = 13800;

            dlgData.fAliPay = fAliPay;

            dlgData.szQRCode = szQRCode;

            dlgData.status = status;

            dlgData.lSurcharge = 8000;

            switch (iFunctionButton)
            {
                case FunctionType.Refund: titleId = StringIds.STRING_FUNCTIONTYPES_REFUND; break;
                case FunctionType.Purchase: titleId = StringIds.STRING_PURCHASE; break;
                case FunctionType.PurchaseCash: titleId = StringIds.STRING_PURCHASE_AND_CASH; break;
                case FunctionType.Deposit: titleId = StringIds.STRING_DEPOSIT; break;
                case FunctionType.CardAuthentication: titleId = StringIds.STRING_CARD_AUTH; break;
                case FunctionType.TestComm: titleId = StringIds.STRING_TESTCOMM; break;
                case FunctionType.CardStatusCheck: titleId = StringIds.STRING_CARD_CHECK_OPTIONS; break;
                case FunctionType.PreAuth: titleId = StringIds.STRING_PRE_AUTH; break;
                case FunctionType.CashOut: titleId = StringIds.STRING_CASHOUT; break;
                case FunctionType.Void: titleId = StringIds.STRING_VOID; break;
                case FunctionType.Adjust: titleId = StringIds.STRING_ADJUST; break;
                //case FunctionType.IncrementalAdjust: titleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
            }
            dlgData.szTotalTitle = titleId;

            dlgData.CurrencySymbol = "$";
            dlgData.szContent = StringIds.STRING_TRANSMISSIONERROR;

            DialogBuilder.Show(IPayDialog.REQUEST_ALIPAY_WECHAT_DIALOG, StringIds.STRING_QR_PAYMENTS, (iResult, args) =>
            {
                //RequestAliPayWechatDialog
            }, blockUI, false, dlgData);

            //hardcode in dialog to show all case
            //_data_PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(_data.status)));
        }

        void ShowViewLeftIconRightQuadrupleTextOverlayDialog()
        {

            //List<ViewFourthLineModel> dlgCardFeeData = new List<ViewFourthLineModel>();

            //Action<ViewFourthLineModel> addCardFee = model => { if (model != null) dlgCardFeeData.Add(model); };

            //addCardFee(SetDataCardFees(MerchantCardType.Visa));
            //addCardFee(SetDataCardFees(MerchantCardType.MasterCard));
            //addCardFee(SetDataCardFees(MerchantCardType.UnionPay));
            //addCardFee(SetDataCardFees(MerchantCardType.Amex));
            //addCardFee(SetDataCardFees(MerchantCardType.JCB));
            //addCardFee(SetDataCardFees(MerchantCardType.Discover));
            //addCardFee(SetDataCardFees(MerchantCardType.Diners));
            //addCardFee(SetDataCardFees(MerchantCardType.Troy));

            //var dialogCardFees = new ViewLeftIconRightQuadrupleTextOverlayDialog(StringIds.STRING_SURCHARGE_AND_SERVICE_FEES_UPCASE, dlgCardFeeData);

            //dialogCardFees.OnLoadedEvt += delegate
            //{
            //};

            //dialogCardFees?.Show(CrossCurrentActivity.Current.Activity);
        }

        void ShowListCardBrandDialog()
        {

            //List<int> listCardBrandImgId = new List<int>();

            //listCardBrandImgId.Add(Resource.Drawable.vector_visa);
            //listCardBrandImgId.Add(Resource.Drawable.vector_master_card_text);
            //listCardBrandImgId.Add(Resource.Drawable.vector_american_express);
            //listCardBrandImgId.Add(Resource.Drawable.vector_union_pay_bg);
            //listCardBrandImgId.Add(Resource.Drawable.vector_discover_network);
            //listCardBrandImgId.Add(Resource.Drawable.vector_jcb_bg);
            //listCardBrandImgId.Add(Resource.Drawable.vector_diners_bg);
            //listCardBrandImgId.Add(Resource.Drawable.vector_eft_pos);
            //listCardBrandImgId.Add(Resource.Drawable.wechatpay_logo);
            //listCardBrandImgId.Add(Resource.Drawable.alipay_logo);
            //listCardBrandImgId.Add(Resource.Drawable.vector_epay);
            //listCardBrandImgId.Add(Resource.Drawable.vector_centra_pay);
            //listCardBrandImgId.Add(Resource.Drawable.vector_bnpl);
            //listCardBrandImgId.Add(Resource.Drawable.vector_crypto);

            List<CardBrandViewModel> listCardBrandModel = new List<CardBrandViewModel>();

            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_VISA, CardBrandImg = "vector_visa" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_MASTER, CardBrandImg = "vector_master_card_text" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_AMEX, CardBrandImg = "vector_american_express" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_UNIONPAY, CardBrandImg = "vector_union_pay_bg" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_DISCOVER, CardBrandImg = "vector_discover_network" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_JCB, CardBrandImg = "vector_jcb_bg" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_DINERS, CardBrandImg = "vector_diners_bg" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_EFTPOS, CardBrandImg = "vector_eft_pos" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_WESTFLD, CardBrandImg = "wechatpay_logo" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_ALLIEDP, CardBrandImg = "alipay_logo" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_ALLIEDP, CardBrandImg = "vector_epay" });
            listCardBrandModel.Add(new CardBrandViewModel() { iCardType = CARDTYPE.CARD_EFTPOSDEBIT, CardBrandImg = "vector_centra_pay" });

            DialogBuilder.Show(IShellDialog.SELECT_CARD_BRAND_DIALOG, StringIds.STRING_MERCHANT_FEES, (iResult, args) =>
            {

                //ListCardBrandDialog
            }, true, false, listCardBrandModel);
        }

        void ShowSurchargeFeeDetailDialog()
        {
            SurchargeFeeDetailDlgData data = new SurchargeFeeDetailDlgData()
            {
                CardResId = "vector_visa",
                lAmount = 38000,
                iCardType = CARDTYPE.CARD_AMEX,
                Rules = new List<SurchargeRule>()
                {
                    new SurchargeRule()
                    {
                        Surcharge = new MerchantCardSurcharge()
                        {
                            lFeeChange = 1000,
                            usFeeHigh = 40,
                            usFeeLow = 20,
                            fPercent = true
                        },
                        EFTPaymentMethods = new List<ENTRYMODE>()
                        {
                            ENTRYMODE.EM_SMC,
                            ENTRYMODE.EM_RFID,
                            ENTRYMODE.EM_SWIPED,
                            ENTRYMODE.EM_MOTO,
                            ENTRYMODE.EM_MANUAL,
                        },
                        CardLocation = CardLocation.All
                    },
                    new SurchargeRule()
                    {
                        Surcharge = new MerchantCardSurcharge()
                        {
                            lFeeChange = 1000,
                            usFeeHigh = 40,
                            usFeeLow = 20,
                            fPercent = true

                        },
                        EFTPaymentMethods = new List<ENTRYMODE>()
                        {
                            ENTRYMODE.EM_SMC,
                            ENTRYMODE.EM_RFID,
                            ENTRYMODE.EM_SWIPED,
                            ENTRYMODE.EM_MOTO,
                            ENTRYMODE.EM_MANUAL,
                        },
                        CardLocation = CardLocation.All

                    },new SurchargeRule()
                    {
                        Surcharge = new MerchantCardSurcharge()
                        {
                            lFeeChange = 1000,
                            usFeeHigh = 40,
                            usFeeLow = 20,
                            fPercent = true

                        },
                        EFTPaymentMethods = new List<ENTRYMODE>()
                        {
                            ENTRYMODE.EM_SMC,
                            ENTRYMODE.EM_RFID,
                            ENTRYMODE.EM_SWIPED,
                            ENTRYMODE.EM_MOTO,
                            ENTRYMODE.EM_MANUAL
                        },
                        CardLocation = CardLocation.All

                    },new SurchargeRule()
                    {
                        Surcharge = new MerchantCardSurcharge()
                        {
                            lFeeChange = 1000,
                            usFeeHigh = 40,
                            usFeeLow = 20,
                            fPercent = true

                        },
                        EFTPaymentMethods = new List<ENTRYMODE>()
                        {
                            ENTRYMODE.EM_SMC,
                            ENTRYMODE.EM_RFID,
                            ENTRYMODE.EM_SWIPED,
                            ENTRYMODE.EM_MOTO,
                            ENTRYMODE.EM_MANUAL
                        },
                        CardLocation = CardLocation.All

                    },
                }

            };
            var dialog = new SurchargeFeeDetailDialog(StringIds.STRING_SURCHARGE_AND_SERVICE_FEES, null, data);

            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog?.Show(CrossCurrentActivity.Current.Activity);
        }

        private ViewFourthLineModel SetDataCardFees(MerchantCardType merchantCardType)
        {
            ViewFourthLineModel data = null;

            MerchantCreditCardsAccepted creditCardAccepted = new MerchantCreditCardsAccepted();

            MerchantDebitCardsAccepted debitCardAccepted = new MerchantDebitCardsAccepted();

            long lSurcharge = 0;

            long lCreditAccountSurcharge = 0;

            long lSavingsAccountSurcharge = 0;

            long lChequeAccountSurcharge = 0;

            string cardName = string.Empty;

            switch (merchantCardType)
            {
                case MerchantCardType.Debit:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_DEBIT);

                    data = new ViewFourthLineModel()
                    {
                        //LeftIconImgResId = Resource.Drawable.vector_debit_logo
                    };

                    break;

                case MerchantCardType.Visa:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_VISA);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_visa
                    };

                    break;

                case MerchantCardType.MasterCard:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_MASTERCARD);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_master_card
                    };

                    break;

                case MerchantCardType.Amex:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_AMEX);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_american_express
                    };

                    break;

                case MerchantCardType.Diners:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_DINERS);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_diners
                    };

                    break;

                case MerchantCardType.JCB:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_JCB);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_jcb
                    };

                    break;

                case MerchantCardType.Discover:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_DISCOVER);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_discover_network
                    };

                    break;

                case MerchantCardType.UnionPay:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_UNIONPAY);

                    data = new ViewFourthLineModel()
                    {
                        LeftIconImgResId = Resource.Drawable.vector_union_pay
                    };

                    break;

                case MerchantCardType.Interact:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_INTERACT);

                    data = new ViewFourthLineModel()
                    {
                        //LeftIconImgResId = Resource.Drawable.vector_interac_logo
                    };

                    break;

                case MerchantCardType.Maestro:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_MAESTRO);

                    data = new ViewFourthLineModel()
                    {
                        //LeftIconImgResId = Resource.Drawable.vector_maestro_logo
                    };

                    break;

                case MerchantCardType.Troy:

                    cardName = Localize.GetString(StringIds.STRING_CARDTYPE_TROY);

                    data = new ViewFourthLineModel()
                    {
                        //LeftIconImgResId = Resource.Drawable.vector_troy_logo
                    };

                    break;
            }

            double dPercent = 0;
            bool fAdd = false;

            if (data != null && (creditCardAccepted != null || debitCardAccepted != null))
            {
                data.IsLeftIconVector = true;

                if (creditCardAccepted != null)
                {
                    lSurcharge = 10000;

                    if (lSurcharge != 0)
                    {
                        fAdd = true;

                        data.FirstLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_SURCHARGE);

                        if (dPercent > 0)
                            data.FirstLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.FirstLine_Value = lSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    lChequeAccountSurcharge = 1000;

                    if (lChequeAccountSurcharge != 0)
                    {
                        fAdd = true;

                        data.SecondLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_CHEQUE);

                        if (dPercent > 0)
                            data.SecondLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.SecondLine_Value = lChequeAccountSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    lSavingsAccountSurcharge = 1000;

                    if (lSavingsAccountSurcharge != 0)
                    {
                        fAdd = true;

                        data.ThirdLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_SAVINGS);

                        if (dPercent > 0)
                            data.ThirdLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.ThirdLine_Value = lSavingsAccountSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    lCreditAccountSurcharge = 2000;

                    if (lCreditAccountSurcharge != 0)
                    {
                        fAdd = true;

                        data.FourthLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_CREDIT);

                        if (dPercent > 0)
                            data.FourthLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.FourthLine_Value = lCreditAccountSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    if (!fAdd)
                        return null;
                }
                else if (debitCardAccepted != null)
                {
                    lSurcharge = 1000;

                    if (lSurcharge != 0)
                    {
                        fAdd = true;

                        data.FirstLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_SURCHARGE);

                        if (dPercent > 0)
                            data.FirstLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.FirstLine_Value = lSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    lChequeAccountSurcharge = 2000;

                    if (lChequeAccountSurcharge != 0)
                    {
                        fAdd = true;

                        data.SecondLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_CHEQUE);

                        if (dPercent > 0)
                            data.SecondLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.SecondLine_Value = lChequeAccountSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    lSavingsAccountSurcharge = 3000;

                    if (lSavingsAccountSurcharge != 0)
                    {
                        fAdd = true;

                        data.ThirdLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_SAVINGS);

                        if (dPercent > 0)
                            data.ThirdLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.ThirdLine_Value = lSavingsAccountSurcharge.ToFormatLocalCurrencyAmount();
                    }

                    lCreditAccountSurcharge = 4000;

                    if (lCreditAccountSurcharge != 0)
                    {
                        fAdd = true;

                        data.FourthLine_Title = cardName + " " + Localize.GetString(StringIds.STRING_CREDIT);

                        if (dPercent > 0)
                            data.FourthLine_Title += $" {(dPercent / 100).ToString("F2")}%";

                        data.FourthLine_Value = lCreditAccountSurcharge.ToFormatLocalCurrencyAmount();
                    }
                }
            }

            if (fAdd)
                return data;
            else
                return null;
        }


        void ShowReceiptEmailAddressDialog()
        {
            DialogBuilder.Show(IPayDialog.RECEIPT_EMAIL_ADDRESS_DIALOG, StringIds.STRING_EMAIL_RECEIPT_LOWCASE, (iResult, args) =>
            {

            }, true, false);
        }

        void ShowEnterCellNumberDialog()
        {
            DialogBuilder.Show(IPayDialog.RECEIPT_TEXT_CELL_NUMBER_DIALOG, StringIds.STRING_CELL_NUMBER, (iResult, args) =>
            {

            }, true, false, new EnterCellNumberDlgData(string.Empty, string.Empty, StringIds.STRING_CANCEL, StringIds.STRING_SEND));
        }

        void ShowSplitReviewPaymentsDialog()
        {
            var data = new ReviewPaymentDialogModel();
            data.TotalBalance = 10732;
            data.TotalAmount = 53512;
            data.GuestName = "Table 21";
            data.TitlePaymentId = StringIds.STRING_PURCHASE;

            data.SplitItems = new List<POSSplitItem>();

            var posSplitItem1 = new POSSplitItem() { Id = 1 };
            posSplitItem1.TenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Jack Welsh" });
            data.SplitItems.Add(posSplitItem1);

            var posSplitItem2 = new POSSplitItem() { Id = 2 };
            posSplitItem2.TenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Alexie Smith" });
            data.SplitItems.Add(posSplitItem2);

            var posSplitItem3 = new POSSplitItem() { Id = 3 };
            posSplitItem3.TenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Paul" });
            data.SplitItems.Add(posSplitItem3);

            var posSplitItem4 = new POSSplitItem() { Id = 4 };
            posSplitItem4.TenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Thomas" });
            data.SplitItems.Add(posSplitItem4);

            var posSplitItem5 = new POSSplitItem() { Id = 4 };
            posSplitItem5.TenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Hilary" });
            data.SplitItems.Add(posSplitItem5);

            //var data = new ReviewPaymentDialogModel();
            //data.TotalBalance = 10732;
            //data.TotalAmount = 53512;
            //data.TableName = "Table 21";
            //data.TitlePaymentId = StringIds.STRING_PURCHASE;

            //data.SplitItems = new List<SplitItem>();
            //data.SplitItems.Add(new SplitItem() { Index = 1, GuestName = "Jack Welsh", Amount = 10502, IsMultipleTender = true });
            //data.SplitItems.Add(new SplitItem() { Index = 2, GuestName = "Alexie Smith", Amount = 10502, IsMultipleTender = false });
            //data.SplitItems.Add(new SplitItem() { Index = 3, GuestName = "Paul", Amount = 10502, IsMultipleTender = false });
            //data.SplitItems.Add(new SplitItem() { Index = 4, GuestName = "Thomas", Amount = 10502, IsMultipleTender = false });
            //data.SplitItems.Add(new SplitItem() { Index = 5, GuestName = "Hilary", Amount = 10502, IsMultipleTender = true });

            //TABLE_PAY_GUEST_SPLIT_PAYMENT_REVIEW_DIALOG
            var dialog = new SplitReviewPaymentsDialog(StringIds.STRING_REVIEW_PAYMENTS, null, data);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowTicketSearchOptionsDialog()
        {
            //POSTicketInfo searchData = new POSTicketInfo();
            //var pullSetupDlgData = new StandardSetupDialogModel()
            //{
            //    OKBtnCommandId = GlobalResource.SEARCH_BUTTON,
            //    OkBtnTitleId = StringIds.STRING_SEARCH,
            //};

            //searchData.TicketNumber = "";
            //searchData.EmployeeId = 1;
            //searchData.Reference = string.Empty;
            //searchData.TableNumber = 0;
            //searchData.GuestName = string.Empty;

            //pullSetupDlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
            //{
            //    PropertyName = nameof(searchData.TableNumber),
            //    TitleId = StringIds.STRING_TABLENUMBER,
            //    HeaderTitleId = StringIds.STRING_TABLENUMBER,
            //    FieldTitleId = StringIds.STRING_TABLENUMBER,
            //    Value = searchData.TableNumber
            //});

            //pullSetupDlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
            //{
            //    PropertyName = nameof(searchData.TicketNumber),
            //    TitleId = StringIds.STRING_TICKETNO,
            //    HeaderTitleId = StringIds.STRING_TICKETNO,
            //    FieldTitleId = StringIds.STRING_TICKETNO,
            //    Value = searchData.TicketNumber
            //});

            //pullSetupDlgData.Items.Add(new InputTextEditModel()
            //{
            //    PropertyName = nameof(searchData.GuestName),
            //    TitleId = StringIds.STRING_GUEST_NAME,
            //    HeaderTitleId = StringIds.STRING_GUEST_NAME,
            //    FieldTitleId = StringIds.STRING_GUEST_NAME,
            //    Value = searchData.GuestName
            //});

            //pullSetupDlgData.Items.Add(new InputTextEditModel()
            //{
            //    PropertyName = nameof(searchData.Reference),
            //    TitleId = StringIds.STRING_REFERENCE,
            //    HeaderTitleId = StringIds.STRING_REFERENCE,
            //    FieldTitleId = StringIds.STRING_REFERENCE,
            //    Value = searchData.Reference
            //});

            //DialogBuilder.Show(IPayDialog.TICKET_SEARCH_OPTIONS_DIALOG, StringIds.STRING_TABLE_PAY_TICKET, (iResult, args) =>
            //{
            //    //TicketSearchOptionsDialog
            //}, true, false, pullSetupDlgData);
        }

        void ShowIncreaseSplitDialog()
        {
            IncreaseSplitDialog adjustDialog = null;

            adjustDialog = new IncreaseSplitDialog(StringIds.STRING_INCREASE_MY_SPLIT_AMOUNT, (iResult, args) =>
            {
                adjustDialog.Dismiss();

            }, new AdjustSplitDlgData()
            {
                Reference = "Smith",
                Amount = 4000,
                MaxValue = 15000,
                AmountTitleId = StringIds.STRING_ADJUST_AMOUNT,
                MainBtnTitleId = StringIds.STRING_OK
            });

            adjustDialog.DialogStyle = DialogStyle.FULLSCREEN;

            adjustDialog.Show(CrossCurrentActivity.Current.Activity);
        }

        void ShowSplitPayDialog()
        {
            var splitItem = new SplitModel();

            splitItem.Items.Add(new SplitItem()
            {
                Amount = 1000,
                Reference = "LOI",
                Id = 1,
                IsPaid = false
            });

            splitItem.Items.Add(new SplitItem()
            {
                Amount = 2000,
                Reference = "KHA",
                Id = 2,
                IsPaid = true
            });

            splitItem.Items.Add(new SplitItem()
            {
                Amount = 3000,
                Reference = "DUY",
                Id = 3,
                IsPaid = true
            });

            splitItem.Items.Add(new SplitItem()
            {
                Amount = 4000,
                Reference = "LIEN",
                Id = 4,
                IsPaid = true
            });

            var data = new POSGuestBalanceDlgData()
            {
                TableId = 1,
                BalanceTotal = 150000,
                GuestName = "MARTIN",
                SplitItem = splitItem
            };

            DialogBuilder.Show(IPayDialog.SPLIT_PAY_DIALOG, StringIds.STRING_SPLIT_PAY, (iResult, args) =>
            {
                //SplitPayDialog
            }, true, false, data);
        }

        void ShowReviewPaymentDialog()
        {
            var tenderItems = new List<POSTenderItem>();
            tenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Wechat" });
            tenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Visa Credit" });
            tenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Alipay" });
            tenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Alipay" });
            tenderItems.Add(new POSTenderItem() { Amount = 10502, ApprovedTime = DateTime.Now, TenderName = "Visa Debit" });

            var data = new POSReviewPaymentsDlgData()
            {
                TenderItems = tenderItems,
                PurchaseTotal = 40000,
                TotalBalance = 50000,
                SubTitle = StringIds.STRING_ALL_GUESTS.GetString(),
                RightSubTitleId = StringIds.STRING_BALANCE
            };

            DialogBuilder.Show(IPayDialog.REVIEW_PAYMENTS_DIALOG, StringIds.STRING_REVIEW_PAYMENTS, (iResult, args) =>
            {
                //ReviewPaymentDialog
            }, true, false, data);
        }

        void ShowSelectOptionDialog()
        {
            var items = new List<SelectOptionItem>()
            {
                new SelectOptionItem()
                {
                    Command = GlobalResource.REVIEW_PAYMENTS_BUTTON,
                    TitleId = StringIds.STRING_REVIEW_PAYMENTS,
                    VectorIconResName = IconIds.VECTOR_SHOW_LIGHT,
                },
                new SelectOptionItem()
                {
                    Command = GlobalResource.EDIT_BUTTON,
                    TitleId = StringIds.STRING_EDIT_DETAILS,
                    VectorIconResName = IconIds.VECTOR_EDIT_GRAY_NO_BACKGROUND,
                },
            };

            var data = new SelectOptionDlgData();

            data.MenuItems = items;

            DialogBuilder.Show(IPayDialog.ACCESS_MENU_DIALOG, StringIds.STRING_REFERENCE_TYPE, (iResult, args) =>
            {
                //SelectOptionDialog
            }, true, true, data);
        }

        void ShowGuestBalanceDialog()
        {
            var data = new POSGuestBalanceDlgData()
            {
                BalanceTotal = 25000,
                GuestName = "Smith",
                TableId = 1,
            };

            DialogBuilder.Show(IPayDialog.GUEST_VIEW_BALANCE_DIALOG, StringIds.STRING_PAYMENT_AMOUNT, (iResult, args) =>
            {
                //GuestBalanceDialog
            }, true, false, data);
        }

        void ShowSelectGuestAccountDialog()
        {
            var data = new SelectGuestAccountDlgData()
            {
                TicketNumber = "12345",
                RefNumber = "12",
                RefTypeStringIds = StringIds.STRING_TABLE,
                TotalAmount = 25000,
                GuestItems = new List<GuestAccItemViewModel>()
            };


            data.GuestItems.Add(new GuestAccItemViewModel()
            {
                AvtUrl = "https://creazilla-store.fra1.digitaloceanspaces.com/icons/7912990/avatar-icon-md.png",
                Name = "Loi Truong",
                Id = 1,
                Number = "1",
                TotalAmount = 2000
            });

            data.GuestItems.Add(new GuestAccItemViewModel()
            {
                AvtUrl = "https://i.pngimg.me/thumb/f/720/comvecteezy420553.jpg",
                Name = "Duy Ha",
                Id = 2,
                Number = "2",
                TotalAmount = 2000
            });

            data.GuestItems.Add(new GuestAccItemViewModel()
            {
                AvtUrl = "https://cdn.iconscout.com/icon/free/png-512/free-avatar-369-456321.png?f=webp&w=256",
                Name = "Duy Ha",
                Id = 3,
                Number = "3",
                TotalAmount = 2000
            });

            data.GuestItems.Add(new GuestAccItemViewModel()
            {
                AvtUrl = "https://cdn.iconscout.com/icon/free/png-512/free-avatar-369-456321.png?f=webp&w=256",
                Name = "Huy Le",
                Id = 4,
                Number = "4",
                TotalAmount = 2000
            });

            DialogBuilder.Show(IPayDialog.SELECT_GUEST_ACCOUNT_DIALOG, StringIds.STRING_GUEST_ACCOUNT, (iResult, args) =>
            {
                //SelectGuestAccountDialog
            }, true, false, data);
        }

        void ShowSelectTablePayTicketsDialog()
        {
            var data = new List<ListTablePayItemViewModel>();

            data.Add(new ListTablePayItemViewModel()
            {
                Reference = "Jack Welsh",
                GuestAccCount = 1,
                Amount = 274140,
                TableNumber = 21,
                TicketNumber = "765323",
            });

            data.Add(new ListTablePayItemViewModel()
            {
                Reference = "Jack Welsh",
                GuestAccCount = 1,
                Amount = 274140,
                TableNumber = 21,
                TicketNumber = "765323",
            });

            data.Add(new ListTablePayItemViewModel()
            {
                Reference = "Jack Welsh",
                GuestAccCount = 1,
                Amount = 274140,
                TableNumber = 21,
                TicketNumber = "765323",
            });

            data.Add(new ListTablePayItemViewModel()
            {
                Reference = "Jack Welsh",
                GuestAccCount = 1,
                Amount = 274140,
                TableNumber = 21,
                TicketNumber = "765323",
            });

            var localSelectedTicketNumber = string.Empty;

            DialogBuilder.Show(IPayDialog.SELECT_TABLE_PAY_TICKET_DIALOG, StringIds.STRING_ALL_TICKETS, (iResult, args) =>
            {
                //SelectTablePayTicketsDialog

            }, true, false, data);
        }

        void ShowSelectTenderExtraAmountDialog()
        {
            var dlgData = new SelectTenderExtraAmountDlgData();

            dlgData.TenderNumber = 5;

            dlgData.lAmount = 36000;

            dlgData.lCashout = 5000;

            //dlgData.lPreCashout = 4000;

            dlgData.lTip = 1000;

            dlgData.Donations = new PaymentDonations() { lTotalDonations = 1000 };

            //dlgData.PaymentVouchers = new PaymentVouchers() { lTotalVouchers = 1000 };

            DialogBuilder.Show(IPayDialog.SELECT_TENDER_EXTRA_AMOUNT_DIALOG, StringIds.STRING_MULTI_TENDER, (int iResult, object[] args) =>
            {
                //SelectTenderExtraAmountDialog
            }, true, false, dlgData);
        }

        void ShowGetAmountRefundAlipayWeChatDialog_01()
        {
            RefundAlipayWechatDlgData data = new RefundAlipayWechatDlgData()
            {
                szTransactionId = "4200000027201709294868542706",
                lAmount = 20000,
                lOrginalAmount = 40000
            };

            var fAlipay = true;
            var fManual = true;

            GetAmountRefundAlipayWeChatDialog dialog = new GetAmountRefundAlipayWeChatDialog(StringIds.STRING_WECHAT_REFUND_ONLY, null, data, fAlipay, fManual);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowGetAmountRefundAlipayWeChatDialog_02()
        {
            var data = new RefundAlipayWechatDlgData()
            {
                szTransactionId = "4200000027201709294868542706",
                lAmount = 20000,
                lOrginalAmount = 40000
            };

            var fAlipay = true;
            var fManual = false;

            var dialog = new GetAmountRefundAlipayWeChatDialog(StringIds.STRING_WECHAT_REFUND_ONLY, null, data, fAlipay, fManual);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void EditTicket()
        {
            //var viewModel = new StandardSetupDialogModel()
            //{
            //    OKBtnCommandId = GlobalResource.SAVE_BUTTON,
            //    OkBtnTitleId = StringIds.STRING_SAVE,
            //    CancelBtnCommandId = GlobalResource.CANCEL_SUB_FLOW,
            //    CancelTitleId = StringIds.STRING_CANCEL
            //};

            //var data = new POSTicketEditModel()
            //{
            //    OriginalTicketNumber = "1234567",
            //    NewData = new POSTicketInfo()
            //    {
            //        TicketNumber = "1234567",
            //        GuestName = "Smith",
            //        Reference = "Table 1",
            //        TableNumber = 1
            //    }
            //};

            //viewModel.Items.Add(new InputNumberFixedKeyboardEditModel()
            //{
            //    PropertyName = nameof(data.NewData.TableNumber),
            //    TitleId = StringIds.STRING_TABLE,
            //    HeaderTitleId = StringIds.STRING_TABLE,
            //    FieldTitleId = StringIds.STRING_TABLE,
            //    Value = data.NewData.TableNumber
            //});

            //viewModel.Items.Add(new InputNumberFixedKeyboardEditModel()
            //{
            //    PropertyName = nameof(data.NewData.TicketNumber),
            //    TitleId = StringIds.STRING_TICKETNO,
            //    HeaderTitleId = StringIds.STRING_TICKETNO,
            //    FieldTitleId = StringIds.STRING_TICKETNO,
            //    Value = data.NewData.TicketNumber
            //});

            //viewModel.Items.Add(new InputTextEditModel()
            //{
            //    PropertyName = nameof(data.NewData.GuestName),
            //    TitleId = StringIds.STRING_GUEST_NAME,
            //    HeaderTitleId = StringIds.STRING_GUEST_NAME,
            //    FieldTitleId = StringIds.STRING_GUEST_NAME,
            //    Value = data.NewData.GuestName
            //});

            //viewModel.Items.Add(new InputTextEditModel()
            //{
            //    PropertyName = nameof(data.NewData.Reference),
            //    TitleId = StringIds.STRING_REFERENCE,
            //    HeaderTitleId = StringIds.STRING_REFERENCE,
            //    FieldTitleId = StringIds.STRING_REFERENCE,
            //    Value = data.NewData.Reference
            //});

            //DialogBuilder.Show(IPayDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_EDIT_DETAILS, (iResult, args) =>
            //{
            //    //StandardSetupDialog
            //}, true, false, viewModel);
        }

        void ShowConfirmClosingTableDialog()
        {
            //TicketSearchItemModel selectedTicket = new TicketSearchItemModel()
            //{
            //    Balance = 35000,
            //    EmployeeId = 1,
            //    TableNumber = 4,
            //    TicketNumber = "1234567",
            //    GuestName = "John",
            //    Reference = "Jack Welsh",
            //    GuestAccCount = 5,
            //    PurchaseTotal = 25000,
            //    Stage = TicketStage.Opened
            //};

            //DialogBuilder.Show(IPayDialog.CONFIRM_CLOSING_TABLE_DIALOG, StringIds.STRING_NOTIFICATION, (iResult, args) =>
            //{
            //    //ConfirmClosingTableDialog
            //}, true, false, selectedTicket);
        }

        void ShowGetTenderBalanceDialog()
        {
            GetTenderBalanceDlgData dlgData = new GetTenderBalanceDlgData();
            dlgData.PaidAmount = 35000;
            dlgData.PurchaseAmount = 50000;
            dlgData.index = 2;

            var iResult = DialogBuilder.Show(IPayDialog.GET_TENDER_BALANCE_DIALOG, StringIds.STRING_MULTI_TENDER, (int iResult, object[] args) =>
            {
                //GetTenderBalanceDialog
            }, true, false, dlgData);
        }

        void RefundOptions()
        {
            SelRefndOptDlgData data = new SelRefndOptDlgData();

            data.fShowLogout = false;
            data.iExitButton = 0;
            data.pIdSecurityUser = 1;
            data.FunctionButtons = new List<SelectButton>();

            data.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_EFT_REFUND,
                Title = StringIds.STRING_EFT_REFUND,
                idImage = IconIds.VECTOR_REFUND_MERCHANT_CARD,
                IdProcessor = 0,
                iCommand = GlobalResource.REFUND_EFT_REFUND_BUTTON
            });

            // NOTE: Not supported
            //data.FunctionButtons.Add(new SelectButton()
            //{
            //    iCommandLang = StringIds.STRING_VOUCHER,
            //    Title = StringIds.STRING_VOUCHER,
            //    idImage = IconIds.VECTOR_VOUCHER,
            //    IdProcessor = 0,
            //    iCommand = GlobalResource.REFUND_VOUCHER_BUTTON
            //});

            var fAliPay = true;

            if (fAliPay)
                data.FunctionButtons.Add(new SelectButton()
                {
                    iCommandLang = StringIds.STRING_ALIPAY,
                    Title = StringIds.STRING_ALIPAY,
                    idImage = IconIds.VECTOR_ALI_PAY,
                    IdProcessor = 0,
                    iCommand = GlobalResource.ALI_PAY_BUTTON
                });

            var fWePay = true;

            //Unsupported in this version.
            //if (DialogBuilder.Orientation == ServiceLocators.ScreenOrientation.Landscape)
            //    fWePay = false;

            if (fWePay)
                data.FunctionButtons.Add(new SelectButton()
                {
                    iCommandLang = StringIds.STRING_WEPAY,
                    Title = StringIds.STRING_WEPAY,
                    idImage = IconIds.VECTOR_WECHAT,
                    IdProcessor = 0,
                    iCommand = GlobalResource.WECHAT_PAY_BUTTON
                });

            // NOTE: Not supported
            //data.FunctionButtons.Add(new SelectButton()
            //{
            //    iCommandLang = StringIds.STRING_REWARDS,
            //    Title = StringIds.STRING_REWARDS,
            //    idImage = IconIds.VECTOR_REWARDS,
            //    IdProcessor = 0,
            //    iCommand = GlobalResource.FNC_REWARD_BUTTON
            //});

            //data.FunctionButtons.Add(new SelectButton()
            //{
            //    iCommandLang = StringIds.STRING_TOP_UP,
            //    Title = StringIds.STRING_TOP_UP,
            //    idImage = IconIds.VECTOR_TOP_UP,
            //    IdProcessor = 0,
            //    iCommand = GlobalResource.REFUND_TOP_UP_BUTTON
            //});

            DialogBuilder.Show(IPayDialog.REFUND_OPTION_DIALOG, StringIds.STRING_REFUND_OPTIONS, (iResult, args) =>
            {
                //RefundOptionsDialog
            }, true, false, data);
        }

        void GetRefundAccessCode()
        {
            DialogBuilder.Show(IPayDialog.REFUND_ACCESS_DIALOG, StringIds.STRING_ACCESSCODE, (iResult, args) =>
            {
                //AccessCodeEnterDialog
            }, true, false);
        }

        void ShowRefundSeachResultDialog()
        {
            var results = new List<ResultViewModel>();

            long amount = 10000;

            results.Add(new ResultViewModel()
            {
                DataValueString = "abc",
                Id = 1,
                IsActualTitle = true,
                IsRightTextBold = true,
                IsSpecial = true,
                RightIconId = Resource.Drawable.vector_info_gray,
                SubValue = "subvalue",
                Title = StringIds.STRING_TRANSACTIONID,
                Value = amount.ToFormatLocalCurrencyAmount(),
            });

            results.Add(new ResultViewModel()
            {
                DataValueString = "abc",
                Id = 2,
                IsActualTitle = true,
                IsRightTextBold = true,
                IsSpecial = true,
                RightIconId = Resource.Drawable.vector_info_gray,
                SubValue = "subvalue",
                Title = StringIds.STRING_TRANSACTIONID,
                Value = amount.ToFormatLocalCurrencyAmount(),
            });

            results.Add(new ResultViewModel()
            {
                DataValueString = "abc",
                Id = 3,
                IsActualTitle = true,
                IsRightTextBold = true,
                IsSpecial = true,
                RightIconId = Resource.Drawable.vector_info_gray,
                SubValue = "subvalue",
                Title = StringIds.STRING_TRANSACTIONID,
                Value = amount.ToFormatLocalCurrencyAmount(),
            });

            results.Add(new ResultViewModel()
            {
                DataValueString = "abc",
                Id = 4,
                IsActualTitle = true,
                IsRightTextBold = true,
                IsSpecial = true,
                RightIconId = Resource.Drawable.vector_info_gray,
                SubValue = "subvalue",
                Title = StringIds.STRING_TRANSACTIONID,
                Value = amount.ToFormatLocalCurrencyAmount(),
            });

            DialogBuilder.Show(IPayDialog.REFUND_SEARCH_RESULT_DIALOG, StringIds.STRING_TRANSACTION_LIST, (iResult, args) =>
            {
                //RefundSeachResultDialog
            }, true, false, results);
        }

        void ShowRefundNFCDialog()
        {
            var dialogData = new RefundNFCDlgData()
            {
                fAdvaceSearch = true,
                BottomTitleId = StringIds.STRING_NFC,
                TopTitleId = StringIds.STRING_NFC
            };

            DialogBuilder.Show(IPayDialog.REFUND_NFC_DIALOG, StringIds.STRING_NFC, (iResult, args) =>
            {
                //RefundNFCDialog
            }, true, false, dialogData);
        }

        void ShowRefundSearchDetailDialog()
        {
            RefundPurchaseDetailDlgData dlgData = new RefundPurchaseDetailDlgData()
            {
                CustomerName = "Smith",
                OriginalAmount = 10000,
                OriginalAuthCode = "1234",
                OriginalCardNumber = "1234 5678 9876",
                OriginalCardType = "Debit",
                //OriginalCreatedBy = DateTimeOffset.Now,
                OriginalLastFourDigitCardNumber = "5678",
                OriginalRRNNumber = "1234",
                OriginalSTAN = "1234",
                OriginalTransactionId = "4200000027201709294868542706",
                Reference = ReferenceType.Invoice,
                ReferenceNumber = "1234",
                RefundedAmount = 2000,
                RefundedAuthCode = "3344",
                RefundedCardNumber = "8877 6655 8877",
                RefundedCardType = "Debit",
                RefundedCreatedBy = DateTimeOffset.Now,
                RefundedLastFourDigitCardNumber = "1234",
                RefundedRRNNumber = "123456",
                RefundedSTAN = "1122",
                RefundedTransactionId = "123",
                RREF = "1234",
                Status = PaymentStatus.Refunded
            };

            FunctionType functionType = FunctionType.WechatRefund;

            DialogBuilder.Show(IPayDialog.REFUND_SEARCH_DETAILS_DIALOG, StringIds.STRING_REFUND_PURCHASE, (iResult, args) =>
            {
                //RefundSearchDetailDialog
            }, true, false, dlgData, functionType);
        }

        void ShowRefundListCardDialog()
        {
            var dlgData = new RefundListCardDlgData()
            {
                RefundAmount = 36000,
                payment = new Payment()
                {
                    DateTime = DateTime.Now,
                    //AuthorizationExpiryDate = DateTime.Now,
                    szReferenceNumber = "12354",
                    Id = 1,
                    IdProcessor = 1,
                    lAmount = (int)10000,
                    lCashOut = (int)5000,
                    lTipAmount = (int)1000,
                    iCardType = CARDTYPE.CARD_AMEX,
                    iAccountTypeCode = 2,
                    lszEndCardNumber = "1234",
                    szApprovalCode = "5544",
                    CustomerReferenceType = ReferenceType.Customer,
                    lszCustomerReference = "3456",
                    fRefunded = true,
                    szSTAN = "4455",
                    szCardHolderName = "David Smith",
                    iPaymentType = (ushort)1,
                    Donations = new PaymentDonations()
                    {
                        lTotalDonations = 1000,
                        Donations = new List<PaymentDonation>()
                        {
                            new PaymentDonation(){IdCharity = 1, lDonation=1000, Name="abc",StringIcon="ic_ads_1.png"},
                            new PaymentDonation(){IdCharity = 2, lDonation=1000, Name="abc",StringIcon="ic_ads_1.png"},
                            new PaymentDonation(){IdCharity = 3, lDonation=1000, Name="abc",StringIcon="ic_ads_1.png"},
                        }
                    },
                    szAuthorizationResponseCode = "1234",
                    szTransactionId = "4567",
                },
                cardIconString = CARDTYPE.CARD_AMEX.GetIconDrawable(),
            };

            DialogBuilder.Show(IPayDialog.REFUND_LIST_CARD_DIALOG, StringIds.STRING_REFUND_PURCHASE, (iResult, args) =>
            {
                //RefundListCardDialog
            }, true, false, dlgData);
        }

        void ShowAdvancedSearchDialog()
        {
            TransactionFindModel model = new TransactionFindModel()
            {
                Reference = ReferenceType.Invoice,
                ReferenceNumber = "12345",
                Amount = 50000,
                IsExactAmount = false,
                TransactionId = "1"
            };

            string RightButtonTextId = string.Empty;

            string Title = StringIds.STRING_ADVANCE_SEARCH;

            FunctionType iFunctionButton = FunctionType.WechatRefund;

            switch (iFunctionButton)
            {
                case FunctionType.Refund:

                    RightButtonTextId = StringIds.STRING_FIND_PURCHASE;

                    break;
                case FunctionType.AlipayRefund:

                    Title = StringIds.STRING_ALIPAY_SEARCH;

                    RightButtonTextId = StringIds.STRING_FIND_PURCHASE;
                    break;

                case FunctionType.WechatRefund:

                    Title = StringIds.STRING_WECHAT_SEARCH;

                    RightButtonTextId = StringIds.STRING_FIND_PURCHASE;

                    break;

                case FunctionType.PreAuthComplete:
                case FunctionType.PreAuthIncrement:
                case FunctionType.PreAuthPartial:
                case FunctionType.PreAuthCancel:
                case FunctionType.PreAuthDelayedCompletion:

                    RightButtonTextId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.Reprint:

                case FunctionType.ReprintPreAuth:

                    RightButtonTextId = StringIds.STRING_FIND_RECEIPT;

                    break;
            }

            DialogBuilder.Show(IPayDialog.ADVANCED_SEARCH_DIALOG, Title, (iResult, args) =>
            {
                //AdvancedSearchDialog
            }, true, false, model, RightButtonTextId);
        }

        protected int ShowRefundReasonDialog()
        {
            RefundReasonDlgData dlgData = new RefundReasonDlgData()
            {
                Comment = "abcdef",
                RefundValue = 50000,
                SelectedCommand = 1,
                MenuItems = new List<AccessMenuItem>()
                {
                    new AccessMenuItem()
                    {
                        Command = (int)UtilEnum.RefundReason.None,
                        TitleId = StringIds.STRING_NONE,
                    },
                    new AccessMenuItem()
                    {
                        Command = (int)UtilEnum.RefundReason.ReturnedGoods,
                        TitleId = StringIds.STRING_RETURNED_GOODS,
                    },
                    new AccessMenuItem()
                    {
                        Command = (int)UtilEnum.RefundReason.CancelledPurchase,
                        TitleId = StringIds.STRING_CANCELLED_PURCHASE
                    },
                    new AccessMenuItem()
                    {
                        Command = (int)UtilEnum.RefundReason.CustomerService,
                        TitleId = StringIds.STRING_CUSTOMER_SERVICE
                    },
                    new AccessMenuItem()
                    {
                        Command = (int)UtilEnum.RefundReason.EmployeeError,
                        TitleId = StringIds.STRING_EMPLOYEE_ERROR
                    },
                    new AccessMenuItem()
                    {
                        Command = (int)UtilEnum.RefundReason.Other,
                        TitleId = StringIds.STRING_OTHER
                    }
                }
            };
            return DialogBuilder.Show(IPayDialog.REFUND_REASON_DIALOG, StringIds.STRING_REFUND_REASON, (iResult, args) =>
            {
                //RefundReasonDialog
            }, true, true, dlgData);
        }

        void ShowRefundPurchaseListItemsDialog()
        {
            long amount = 10000;
            long lCashOut = 10000;
            long lDonation = 10000;
            long lTipAmount = 2000;
            RefundPurchaseListItemsDlgData dlgData = new RefundPurchaseListItemsDlgData()
            {
                Amount = 60000,
                ListPurchase = new List<LeftRightTextCheckIconModel>()
                {
                    new LeftRightTextCheckIconModel()
                    {
                        Id = 1,
                        Title = StringIds.STRING_PURCHASE,
                        Value = amount.ToFormatLocalCurrencyAmount(),
                        IsChecked = true,
                        IsTopRadiusBackground = true
                    },
                    new LeftRightTextCheckIconModel()
                    {
                        Id = 2,
                        Title = StringIds.STRING_CASHOUT,
                        Value = lCashOut.ToFormatCurrency(),
                        IsChecked = true
                    },
                    new LeftRightTextCheckIconModel()
                    {
                        Id = 3,
                        IsActualTitle = false,
                        Title = StringIds.STRING_DONATION,
                        Value = lDonation.ToFormatCurrency(),
                        IsChecked = true
                    },
                    new LeftRightTextCheckIconModel()
                    {
                        Id = 4,
                        Title = StringIds.STRING_TIP,
                        Value = lTipAmount.ToFormatCurrency(),
                        IsChecked = true
                    },
                    new LeftRightTextCheckIconModel()
                    {
                        IsActualTitle = true,
                        Title = Localize.GetString(StringIds.STRING_SALESSUMMARY_TOTALREFUND).ToUpper(),
                        Value = amount.ToFormatCurrency(),
                        HasCheckbox = false,
                        HasBottomLine = false,
                        IsBoldAll = true,
                        IsBottomRadiusBackground = true
                    }
                }
            };

            DialogBuilder.Show(IPayDialog.REFUND_PURCHASE_LIST_ITEMS, StringIds.STRING_REFUND_PURCHASE, (iResult, args) =>
            {
                //RefundPurchaseListItemsDialog
            }, true, false, dlgData);
        }

        void ShowRefundTypes()
        {

            var generalType = new List<GenericType>()
            {
                new GenericType()
                {
                     Icon = IconIds.VECTOR_REFUND_PURCHASE,
                     lszText =  StringIds.STRING_REFUND_PURCHASE,
                     Id = GlobalResource.FNC_REFUND_PURCHASE,
                },
                new GenericType()
                {
                     Icon = IconIds.VECTOR_REFUND_ONLY,
                     lszText =  StringIds.STRING_REFUND_ONLY,
                     Id = GlobalResource.FNC_REFUND_ONLY,
                },
            };

            DialogBuilder.Show(IPayDialog.REFUND_SELECT_TYPE_DIALOG, StringIds.STRING_REFUND_TITLE, (iResult, args) =>
            {
                //DynamicOptionDialog
            }, true, false, generalType);
        }

        void ShowManualScanQRCodeDialog()
        {
            string IdDlgTitle = string.Empty;

            ManualScanQRCodeDlgData dlgData = new ManualScanQRCodeDlgData()
            {
                GuideTitleId = StringIds.STRING_PLACE_SCAN_CODE_INSIDE_THE_SCAN_AREA,
                ScanTitleId = StringIds.STRING_SCAN_QR_CODE,
                AboveScanViewTitleId = StringIds.STRING_FIND_PURCHASE,
                ManualTitleId = StringIds.STRING_MANUAL_ENTER,
                //ManualCommand = GlobalResource.MANUAL_BUTTON,
                ManualIconId = IconIds.VECTOR_MANUAL_REFUND,
                fScanAlipayWechat = true
            };

            IdDlgTitle = StringIds.STRING_REFUND_PURCHASE;

            DialogBuilder.Show(IPayDialog.SCAN_QRCODE_DIALOG, IdDlgTitle, (iResult, args) =>
            {

                //ManualScanQRCodeDialog
            }, true, false, dlgData);
        }

        void ShowFindPurchaseOptionDialog(CaseDialog caseDialog)
        {
            string IdDlgTitle = string.Empty;

            FindPurchaseOptionDlgData dlgData = new FindPurchaseOptionDlgData();

            FunctionType functionType;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    functionType = FunctionType.Refund;
                    break;

                case CaseDialog.CASE2:
                    functionType = FunctionType.WechatRefund;
                    break;

                default:
                    functionType = FunctionType.Refund;
                    break;
            }


            var fPayplusRefund = functionType == FunctionType.WechatRefund || functionType == FunctionType.AlipayRefund;

            TransactionFindModel findModel = new TransactionFindModel()
            {
                Reference = ReferenceType.Invoice,
                ReferenceNumber = "1234",
                TransactionId = "12",
                Amount = 13000,
                IsExactAmount = false,
                Last4Digit = "5678",
                AuthCode = "1234",
                STAN = "7890",
                //fPayplusRefund = fPayplusRefund
            };

            if (fPayplusRefund)
            {
                var referenceTypeModel = new SingleSelectEditModel()
                {
                    TitleId = StringIds.STRING_REFERENCE_TYPE,
                    Value = findModel.Reference,
                    PropertyName = nameof(TransactionFindModel.Reference),
                };

                var referenceTypes = new List<RadioEditModel>();

                foreach (ReferenceType type in Enum.GetValues(typeof(ReferenceType)))
                {
                    referenceTypes.Add(new RadioEditModel()
                    {
                        GroupName = nameof(TransactionFindModel.Reference),
                        TitleId = DataHelper.GetRefName(type),
                        Identifier = type,
                    });
                }

                referenceTypeModel.SetItems(referenceTypes);

                dlgData.Items.Add(referenceTypeModel);

                dlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
                {
                    TitleId = StringIds.STRING_REFERENCE_CODE,
                    Value = findModel.ReferenceNumber,
                    IsEnabled = true,
                    PropertyName = nameof(TransactionFindModel.ReferenceNumber),
                    SpanSize = 1
                });

                //dlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
                //{
                //    TitleId = StringIds.STRING_TRANSACTIONID,
                //    Value = findModel.TransactionId,
                //    IsEnabled = true,
                //    PropertyName = nameof(TransactionFindModel.TransactionId),
                //    SpanSize = 1
                //});
            }
            else
            {
                dlgData.Items.Add(new InputAmountEditModel()
                {
                    TitleId = StringIds.STRING_AMOUNT,
                    PropertyName = nameof(TransactionFindModel.Amount),
                    Value = findModel.Amount,
                    SpanSize = 1
                });

                dlgData.Items.Add(new SwitcherEditModel()
                {
                    TitleId = StringIds.STRING_EXACT_AMOUNT,
                    PropertyName = nameof(TransactionFindModel.IsExactAmount),
                    Value = findModel.IsExactAmount,
                    SpanSize = 1
                });


                var endCardNumb = new InputNumberFixedKeyboardEditModel()
                {
                    TitleId = StringIds.STRING_LAST_FOUR_DIGITS,
                    Value = findModel.Last4Digit,
                    MaxLength = 4,
                    PropertyName = nameof(TransactionFindModel.Last4Digit),
                    SpanSize = 1
                };

                dlgData.Items.Add(endCardNumb);

                var authCodeModel = new InputNumberAndTextFixedKeyboardEditModel()
                {
                    TitleId = StringIds.STRING_AUTHCODE,
                    Value = findModel.AuthCode,
                    IsEnabled = true,
                    PropertyName = nameof(TransactionFindModel.AuthCode),
                    SpanSize = 1
                };

                dlgData.Items.Add(authCodeModel);

                var stanModel = new InputNumberFixedKeyboardEditModel()
                {
                    TitleId = StringIds.STRING_STAN,
                    Value = findModel.STAN,
                    IsEnabled = true,
                    PropertyName = nameof(TransactionFindModel.STAN),
                    SpanSize = 1
                };

                dlgData.Items.Add(stanModel);

                //var refModel = new InputTextEditModel()
                //{
                //    TitleId = StringIds.STRING_CUSTOMERREF,
                //    Value = findModel.ReferenceNumber,
                //    IsEnabled = true,
                //    PropertyName = nameof(TransactionFindModel.ReferenceNumber)
                //};

                //dlgData.Items.Add(refModel);
            }

            switch (functionType)
            {
                case FunctionType.Refund:

                    IdDlgTitle = StringIds.STRING_REFUND_PURCHASE;

                    dlgData.TopTitleId = StringIds.STRING_FIND_PURCHASE_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PURCHASE;

                    break;

                case FunctionType.PreAuthComplete:
                case FunctionType.PreAuthIncrement:
                case FunctionType.PreAuthPartial:
                case FunctionType.PreAuthCancel:
                case FunctionType.PreAuthDelayedCompletion:

                    IdDlgTitle = functionType.ToStringId();

                    dlgData.TopTitleId = StringIds.STRING_FIND_PREAUTH_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.OpenTab:

                case FunctionType.CloseTab:

                    IdDlgTitle = functionType == FunctionType.OpenTab ? StringIds.STRING_OPEN_TAB : StringIds.STRING_CLOSE_TAB;

                    dlgData.TopTitleId = StringIds.STRING_FIND_OPEN_TAB;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_OPEN_TAB;

                    break;

                case FunctionType.ReprintPreAuth:

                    IdDlgTitle = StringIds.STRING_REPRINT_RECEIPT;

                    dlgData.TopTitleId = StringIds.STRING_FIND_PREAUTH_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.ReprintPreAuthPending:

                    IdDlgTitle = StringIds.STRING_PRINT_PENDING_HOSPITALITY;

                    dlgData.TopTitleId = StringIds.STRING_FIND_PENDING_HOSPITALITY_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.Reprint:

                    IdDlgTitle = StringIds.STRING_REPRINT_RECEIPT;

                    dlgData.TopTitleId = StringIds.STRING_SEARCH_PAYMENTS;

                    dlgData.RightBtnTitleId = StringIds.STRING_SEARCH_PAYMENTS;

                    break;

                case FunctionType.AlipayRefund:

                    IdDlgTitle = StringIds.STRING_ALIPAY_REFUND_PURCHASE;

                    dlgData.TopTitleId = StringIds.STRING_SEARCH_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PURCHASE;

                    dlgData.fQrCode = dlgData.fAdvancedSearch = dlgData.fViewList = true;

                    break;

                case FunctionType.WechatRefund:

                    IdDlgTitle = StringIds.STRING_WECHAT_REFUND_PURCHASE;

                    dlgData.TopTitleId = StringIds.STRING_SEARCH_OPTIONS;

                    dlgData.RightBtnTitleId = StringIds.STRING_FIND_PURCHASE;

                    dlgData.fQrCode = dlgData.fAdvancedSearch = dlgData.fViewList = true;

                    break;

                default:

                    break;
            }

            DialogBuilder.Show(IPayDialog.SEARCH_OPTION_DIALOG, IdDlgTitle, (iResult, args) =>
            {
                //FindPurchaseOptionDialog
            }, true, false, dlgData, findModel);
        }

        void ShowSearchFilterOptionsDialog()
        {
            FunctionType functionType = FunctionType.AlipayRefund;

            TransactionFindModel dlgData = new TransactionFindModel();

            string RightButtonTextId = string.Empty;

            switch (functionType)
            {
                case FunctionType.Refund:
                case FunctionType.AlipayRefund:
                case FunctionType.WechatRefund:

                    RightButtonTextId = StringIds.STRING_FIND_PURCHASE;

                    break;

                case FunctionType.PreAuthComplete:
                case FunctionType.PreAuthIncrement:
                case FunctionType.PreAuthPartial:
                case FunctionType.PreAuthCancel:
                case FunctionType.PreAuthDelayedCompletion:

                    RightButtonTextId = StringIds.STRING_FIND_PRE_AUTH;

                    break;

                case FunctionType.CloseTab:
                case FunctionType.OpenTab:

                    RightButtonTextId = StringIds.STRING_FIND_OPEN_TAB;

                    break;

                case FunctionType.Reprint:

                case FunctionType.ReprintPreAuth:

                    RightButtonTextId = StringIds.STRING_FIND_RECEIPT;

                    break;

                default:

                    break;
            }

            DialogBuilder.Show(IPayDialog.SEARCH_FILTER_OPTIONS_DIALOG, StringIds.STRING_VIEW_LIST, (iResult, args) =>
            {
                //SearchFilterOptionsDialog
            }, true, false, dlgData, RightButtonTextId, functionType);

        }

        void SetupMenu()
        {
            bool fDumpDb = true;

            var menuItems = new SelFncDlgData()
            {
                iPage = 0,
                iMaxPage = 4,
                iMinPage = 1,
                pIdProcessor = 0,
                fShowLogout = false,
                fGrid = false,
                fModeDisplay = false,
                pIdSecurityUser = 0,
            };

            menuItems.FunctionButtons = new List<SelectButton>();

            menuItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_INFO,
                Title = StringIds.STRING_INFO,
                idImage = IconIds.VECTOR_INFO,
                IdProcessor = 0,
                iCommand = GlobalResource.INFO_BUTTON
            });

            menuItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_SETUP,
                Title = StringIds.STRING_SETUP,
                idImage = IconIds.VECTOR_SETUP,
                IdProcessor = 0,
                iCommand = GlobalResource.SETUP_BUTTON
            });

            //menuItems.FunctionButtons.Add(new SelectButton()
            //{
            //    iCommandLang = StringIds.STRING_ADVANCE_SETUP,
            //    Title = StringIds.STRING_ADVANCE_SETUP,
            //    idImage = IconIds.VECTOR_AUTO_SETUP,
            //    IdProcessor = 0,
            //    iCommand = GlobalResource.ADMIN_SETUP_BUTTON
            //});

            menuItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = fDumpDb ? StringIds.STRING_DUMP_LOGS_AND_DATABASES : StringIds.STRING_DUMP_LOGS,
                Title = fDumpDb ? StringIds.STRING_DUMP_LOGS_AND_DATABASES : StringIds.STRING_DUMP_LOGS,
                idImage = IconIds.VECTOR_TEM_UPDATE,
                IdProcessor = 0,
                iCommand = GlobalResource.UPLOAD_LOGS
            });

            menuItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = string.Empty,
                Title = "Close POS Intergrated Mode",
                idImage = IconIds.VECTOR_EXIT,
                IdProcessor = 0,
                iCommand = GlobalResource.EXIT_POS_INTEGRATE
            });

            menuItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_EXIT,
                Title = StringIds.STRING_EXIT,
                idImage = IconIds.VECTOR_EXIT,
                IdProcessor = 0,
                iCommand = GlobalResource.EXIT_BUTTON
            });

            DialogBuilder.Show(IShellDialog.MENU_DIALOG, StringIds.STRING_MENU, (iResult, args) =>
            {
                //MenuDialog
            }, true, false, menuItems);
        }

        void SetupInfo()
        {
            var infoItems = new SelFncDlgData()
            {
                iPage = 0,
                iMaxPage = 4,
                iMinPage = 1,
                pIdProcessor = 0,
                fShowLogout = false,
                fGrid = false,
                pIdSecurityUser = 0
            };

            infoItems.FunctionButtons = new List<SelectButton>();

            infoItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_SETUP_MERCHANTINFO,
                Title = StringIds.STRING_SETUP_MERCHANTINFO,
                idImage = IconIds.VECTOR_MERCHANT_INFO,
                IdProcessor = 0,
                iCommand = GlobalResource.OK_BUTTON
            });

            infoItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_SECURITY_INFO,
                Title = StringIds.STRING_SECURITY_INFO,
                idImage = IconIds.VECTOR_SECURITY_INFO,
                IdProcessor = 0,
                iCommand = GlobalResource.OK_BUTTON + 1000
            });

            infoItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_TERMINAL_INFO,
                Title = StringIds.STRING_TERMINAL_INFO,
                idImage = IconIds.VECTOR_TERMINAL_INFO,
                IdProcessor = 0,
                iCommand = GlobalResource.OK_BUTTON + 2000
            });

            infoItems.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_PRINT_TERMINAL_INFO,
                Title = StringIds.STRING_PRINT_TERMINAL_INFO,
                idImage = IconIds.VECTOR_PRINT_TERMINAL_INFO,
                IdProcessor = 0,
                iCommand = GlobalResource.OK_BUTTON + 3000
            });

            DialogBuilder.Show(IShellDialog.MENU_DIALOG, StringIds.STRING_INFO, (iResult, args) =>
            {
                //MenuDialog
            }, true, false, infoItems);
        }

        void MerchantInfo()
        {
            var dataMerchant = new StandardSetupDialogModel()
            {
                OKBtnCommandId = GlobalResource.OK_BUTTON,
                OkBtnTitleId = "",
                Items = new List<BaseEditModel>()
                {
                    new InputNumberEditModel()
                    {
                        TitleId = StringIds.STRING_TERMINAL_ID,
                        Value =  "123456",
                        IsEnabled = false
                    },
                    new InputNumberEditModel()
                    {
                        TitleId = StringIds.STRING_MERCHANT_ID,
                        Value = "123456",
                        IsEnabled = false
                    }
                }
            };

            DialogBuilder.Show(IPayDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_SETUP_MERCHANTINFO, (iResult, args) =>
            {
                //StandardSetupDialog
            }, true, false, dataMerchant);
        }

        void ShowSecurityInfo()
        {
            var dataSecurity = new StandardSetupDialogModel()
            {
                OKBtnCommandId = GlobalResource.OK_BUTTON,
                OkBtnTitleId = "",
                Items = new List<BaseEditModel>()
                {
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_KEK_1_KVC,
                        Value =  "ACD765",
                        IsEnabled = false
                    },
                    new InputNumberEditModel()
                    {
                        TitleId = StringIds.STRING_KEK_2_KVC,
                        Value =  "ACD765",
                        IsEnabled = false
                    },
                    new InputNumberEditModel()
                    {
                        TitleId = StringIds.STRING_TSEK_KVC,
                        Value = "ACD765",
                        IsEnabled = false
                    }
                }
            };

            DialogBuilder.Show(IPayDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_SECURITY_INFO, (iResult, args) =>
            {
                //StandardSetupDialog
            }, true, false, dataSecurity);
        }

        void ShowTerminalInfo()
        {
            var dataTerminal = new StandardSetupDialogModel()
            {
                OKBtnCommandId = GlobalResource.OK_BUTTON,
                OkBtnTitleId = "",
                Items = new List<BaseEditModel>()
                {
                    new InputNumberEditModel()
                    {
                        TitleId = StringIds.STRING_TERMINAL_SOFTWARE_VERSION,
                        Value = "PX78UIOI8976",
                        IsEnabled = false
                    },
                    new InputNumberEditModel()
                    {
                        TitleId = StringIds.STRING_TERMINAL_SERIAL_NUMBER,
                        Value = "PX78UIOI8976",
                        IsEnabled = false
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_PIN_PAD_SOFTWARE_VERSION,
                        Value = "PX78UIOI8976",
                        IsEnabled = false
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_PIN_PAD_SERIAL_NUMBER,
                        Value = "PX78UIOI8976",
                        IsEnabled = false
                    }
                }
            };

            DialogBuilder.Show(IPayDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_TERMINAL_INFO, (iResult, args) =>
            {
                //StandardSetupDialog
            }, true, false, dataTerminal);
        }

        void ShowPrintHeaderSetup()
        {
            var printerHeaderSetupDlgData = new StandardSetupDialogModel()
            {
                Items = new List<BaseEditModel>()
                {
                    new SwitcherEditModel()
                    {
                        TitleId = StringIds.STRING_ENABLED,
                        FieldTitleId = nameof(MerchantPrintMessages.fPrinterHeaderEnable),
                        Value = true
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_MESSAGES_LINE1,
                        HeaderTitleId = StringIds.STRING_PRINT_HEADER,
                        FieldTitleId = StringIds.STRING_MESSAGES_LINE1,
                        Value = "Thank you for shopping"
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_MESSAGES_LINE2,
                        HeaderTitleId = StringIds.STRING_PRINT_HEADER,
                        FieldTitleId = StringIds.STRING_MESSAGES_LINE2,
                        Value = "Thank you for shopping"
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_MESSAGES_LINE3,
                        HeaderTitleId = StringIds.STRING_PRINT_HEADER,
                        FieldTitleId = StringIds.STRING_MESSAGES_LINE3,
                        Value = "Thank you for shopping"
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_MESSAGES_LINE4,
                        HeaderTitleId = StringIds.STRING_PRINT_HEADER,
                        FieldTitleId = StringIds.STRING_MESSAGES_LINE4,
                        Value = "Thank you for shopping"
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_MESSAGES_LINE5,
                        HeaderTitleId = StringIds.STRING_PRINT_HEADER,
                        FieldTitleId = StringIds.STRING_MESSAGES_LINE5,
                        Value = "Thank you for shopping"
                    },
                    new InputTextEditModel()
                    {
                        TitleId = StringIds.STRING_MESSAGES_LINE6,
                        HeaderTitleId = StringIds.STRING_PRINT_HEADER,
                        FieldTitleId = StringIds.STRING_MESSAGES_LINE6,
                        Value = "Thank you for shopping"
                    }
                }
            };

            DialogBuilder.Show(IPayDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_PRINT_HEADER, (iResult, args) =>
            {
                //StandardSetupDialog
            }, true, false, printerHeaderSetupDlgData);
        }

        void ShowPullConnections()
        {
            var pullSetupDlgData = new StandardSetupDialogModel()
            {
                OKBtnCommandId = GlobalResource.OK_BUTTON,
                OkBtnTitleId = StringIds.STRING_SAVE
            };

            pullSetupDlgData.Items.Add(new InputIPEditModel()
            {
                TitleId = StringIds.STRING_IPADDRESS,
                HeaderTitleId = StringIds.STRING_IPADDRESS,
                FieldTitleId = StringIds.STRING_IPADDRESS,
                Value = "192.168.0.123",
                PropertyName = null
            });

            pullSetupDlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
            {
                TitleId = StringIds.STRING_PORT,
                HeaderTitleId = StringIds.STRING_PORT,
                FieldTitleId = StringIds.STRING_PORT,
                PropertyName = null,
                Value = "5555"
            });

            pullSetupDlgData.Items.Add(new SwitcherEditModel()
            {
                TitleId = StringIds.HTTPS_CONNECTION,
                FieldTitleId = "IsSSL",
                PropertyName = null,
                Value = true
            });

            DialogBuilder.Show(IShellDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_PULLCONNECTIONS, (iResult, args) =>
            {

            }, true, false, pullSetupDlgData);
        }

        void ShowShiftEnterDateRangeDialog()
        {
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;

            DialogBuilder.Show(IPayDialog.SHIFT_ENTER_DATE_RANGE_DIALOG, StringIds.STRING_HISTORY_SALES, (iResult, args) =>
            {
                //ShiftEnterDateRangeDialog
            }, true, false, startDate, endDate);
        }

        void ShowSelectPaymentMethod()
        {

            var selectFuncDialogDta = new SelFncDlgData()
            {
                iPage = 0,
                iMaxPage = 4,
                iMinPage = 1,
                pIdProcessor = 0,
                fShowLogout = false,
                fGrid = false,
                fModeDisplay = false,
            };

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_EFT_CARDS,
                Title = StringIds.STRING_EFT_CARDS,
                idImage = IconIds.VECTOR_EFT_CARD,
                IdProcessor = 0,
                iCommand = GlobalResource.REPORT_EFT_CARDS_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_ALIPAY,
                Title = StringIds.STRING_ALIPAY,
                idImage = IconIds.VECTOR_ALI_PAY,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_ALIPAY_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_WECHAT,
                Title = StringIds.STRING_WECHAT,
                idImage = IconIds.VECTOR_WECHAT,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_WEPAY_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_OTHERS,
                Title = StringIds.STRING_OTHERS,
                idImage = IconIds.VECTOR_OTHER,
                IdProcessor = 0,
                //iCommand = GlobalResource.OTHER_BUTTON,
            });

            DialogBuilder.Show(IPayDialog.REPORT_SELECT_PAYMENT_METHOD_DIALOG, StringIds.STRING_CURRENT_SALES, (iResult, args) =>
            {
                //MenuDialog
            }, true, false, selectFuncDialogDta);
        }

        void SettlementApproval()
        {
            FunctionType IdFunctionButton = FunctionType.SettleInquiry;

            string lpszResult = "TRANS. COMPLETE";

            string lpszSecondaryResult = null;

            SettlementApprovalDlgData DlgData = new SettlementApprovalDlgData();

            DlgData.fApproved = true;

            DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);

            DlgData.lpszResult = lpszResult;

            DlgData.lpszSecondaryResult = lpszSecondaryResult;

            DlgData.IdFunctionButton = IdFunctionButton;

            string IdTitleText = string.Empty;

            switch (IdFunctionButton)
            {
                case FunctionType.SettleCutOver:
                    IdTitleText = StringIds.STRING_SETTLEMENT_CUTOVER;
                    break;
                case FunctionType.SettleInquiry:
                    IdTitleText = StringIds.STRING_SETTLEMENT_INQUIRY;
                    break;
                case FunctionType.Purchase:
                    IdTitleText = StringIds.STRING_ADVICE_PROCESS;
                    break;
            }

            DialogBuilder.Show(IShellDialog.SETTLEMENTAPPROVAL_DIALOG, IdTitleText, (iResult, args) =>
            {
                //SettlementApprovalDialog
            }, false, false, DlgData);

        }

        void SelectDCCCurrency()
        {
            Currency currency;

            SelectDCCCurrencyData DlgData = new SelectDCCCurrencyData();

            DlgData.lTotal = 13800;

            DlgData.lLocalTotal = 10100;

            DlgData.lCardCurrencyTotal = 6420;

            DlgData.fSmartCard = true;

            DlgData.szConversionRate = "1.08234";

            DlgData.szMarginPercentage = "3.3162";

            currency = CurrencyRepository.Instance.GetByCurrencyCode(554);

            if (currency != null)
            {
                DlgData.LocalCurrency = currency.wszCurrencyCode;

                DlgData.LocalCountry = "NZD";

                DlgData.LocalImage = currency.iCurrencyCodeFlag;
            }

            currency = CurrencyRepository.Instance.GetByCurrencyCode(840);

            if (currency != null)
            {
                DlgData.HomeCurrency = currency.wszCurrencyCode;
                DlgData.HomeCountry = "USD";
                DlgData.HomeImage = currency.iCurrencyCodeFlag;
            }

            DialogBuilder.Show(IShellDialog.SELECT_DCCCURRENCY_DIALOG, StringIds.STRING_CURRENCY, (iResult, args) =>
            {
                //SelectDCCCurrencyDialog
            }, true, false, DlgData);
        }

        void DCCRateApproval()
        {
            long lTotal = 38000;

            DCCRateResponse DCCRateResponse = new DCCRateResponse();

            DCCRateResponse.szLocalCurrency = "AUD";
            DCCRateResponse.szCardCurrency = "NZD";
            DCCRateResponse.szConversionRate = "1.08234";
            DCCRateResponse.szForeignAmount = "12.34";

            DialogBuilder.Show(IShellDialog.DCC_RATEAPPROVAL_DIALOG, StringIds.STRING_FXRATEAPPROVAL, (iResult, args) =>
            {
                //DCCRateApprovalDialog
            }, true, false, DCCRateResponse, lTotal);
        }

        void DCCConfirmation()
        {
            var currency = CurrencyRepository.Instance.GetByCurrencyCode(840);
            DCCConfimationData data = new DCCConfimationData();

            if (currency == null)
                return;

            data.FlagImage = currency.iCurrencyCodeFlag;
            data.Currency = currency.wszCurrencyCode;
            data.lAmount = 6420;
            data.Content = "I declare i have been given a choice in payment currency and i agree to pay the above amount. ";

            DialogBuilder.Show(IShellDialog.DCC_CONFIRMATION_DIALOG, StringIds.STRING_CONFIRMATION, (iResult, args) =>
            {
                //DCCConfirmationDialog
            }, true, false, data);
        }

        void ShowConfirmPreauthAutoTopUpDialog()
        {
            ConfirmPreauthAutoTopUpData data = new ConfirmPreauthAutoTopUpData();

            data.TopTitleId = StringIds.STRING_AMOUNT_EXCEEDS;
            data.ResultTitleId = StringIds.STRING_APPROVAL;
            data.MainMessage = StringIds.STRING_DO_YOU_WANT_TO_PERFORM_A_PREAUTH_TOPUP;
            data.BtnRight = StringIds.STRING_YES;
            data.lAmount = 10000;
            data.lTipAmount = 1600;
            data.lAccountSurChargeFee = 116;
            data.lSurChargeFee = 100;
            data.TotalAmount = 11816;
            data.lAuthAmount = 10000;
            data.MainIcon = IconIds.VECTOR_TOP_UP;

            DialogBuilder.Show(IShellDialog.CONFIRM_PREAUTH_AUTO_TOPUP_DIALOG, StringIds.STRING_CONFIRMATION, (iResult, args) =>
            {
                //ConfirmPreauthAutoTopUpDialog
            }, true, false, data);
        }

        void ConfirmTopUpMessageBox(CaseDialog caseDialog)
        {
            long lTotalAmount = 40000;
            FunctionType function = FunctionType.Purchase;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    function = FunctionType.PreAuthPartial;
                    break;
                case CaseDialog.CASE2:
                    function = FunctionType.Purchase;
                    break;
            }

            MessageType messageData = new MessageType();
            long lPreviousAmount = 50000;
            FunctionType iFunctionButton = FunctionType.PreAuthPartial;

            string aboveMsg = function == FunctionType.PreAuthPartial ? Localize.GetString(StringIds.STRING_PARTIAL_COMPLETION_COULD_BE_DECLINED)
                              : Localize.GetString(StringIds.STRING_FINAL_COMPLETION_COULD_BE_DECLINED);
            string topMsg = function == FunctionType.PreAuthPartial ? Localize.GetString(StringIds.STRING_PREAUTH_PARTIAL_UPCASE) : Localize.GetString(StringIds.STRING_FINAL_COMPLETION_UPCASE);
            string mainMsg = string.Format(Localize.GetString(StringIds.STRING_AMOUNT_EXCEEDS_THE_APPROVED_PREAUTH_AMOUNT), lTotalAmount.ToFormatLocalCurrencyAmount());
            //string warningMsg = Localize.GetString(StringIds.STRING_YOU_WILL_NEED_TO_PERFORM_A_PREAUTH_TOPUP);
            string warningMsg = "";

            var buttonBottom = GlobalResource.MB_RESUMECANCEL;

            messageData.IsShowCancelBtn = true;
            messageData.IsShowBackBtn = false;
            messageData.IsAboveMsgActualText = false;
            messageData.IsActualText = false;
            messageData.IsSubActualText = false;
            messageData.IsThirdActualText = false;
            messageData.AboveMessage = aboveMsg;
            messageData.IsAboveMsgActualText = false;
            messageData.Message = mainMsg;
            messageData.SubMessage = "";
            messageData.ThirdMessage = "";
            messageData.AboveMessageTop = topMsg;
            messageData.TextRightButton = StringIds.STRING_TOP_UP;
            messageData.TextLeftButton = StringIds.STRING_ENTER_AMOUNT;
            messageData.iType = buttonBottom;
            messageData.idImg = 0;
            messageData.BottomWarningId = warningMsg;

            ApplicationFlow.CustomStringMessageBox(true, StringIds.STRING_DECLINED, mainMsg, false, buttonBottom, ref messageData);
        }

        void ShowDigitalSignatureConfirmDialog()
        {
            string digitalData = "qwkjhjhjhkhkhjhhklkkikk";
            EvtMessage evt = new EvtMessage();

            DialogBuilder.Show(IShellDialog.DIGITAL_SIGNATURE_CONFIRM_DIALOG, StringIds.STRING_CONFIRM_SIGNATURE, (iResult, args) =>
            {
                //DigitalSignatureConfirmDialog
            }, true, false, digitalData, evt);
        }

        void ShowSelectCharityDialog()
        {
            uint startId = 0;
            var charityItem1 = new CharityItem();
            charityItem1.PortraitPoster = "don_1_port_poster.png";
            charityItem1.Logo = "don_1_logo.png";
            charityItem1.LandcapsetPoster = "don_1_land_poster.png";
            charityItem1.Id = startId++;
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
            charityItem2.Id = startId++;
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
            charityItem3.Id = startId++;
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
            charityItem4.Id = startId++;
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

            ////
            var standaloneCharity01 = new StandaloneCharity();
            standaloneCharity01.Id = startId++;
            standaloneCharity01.Name = "Qatar Charity";
            standaloneCharity01.Banner = "don_1_land_poster.png";
            standaloneCharity01.AdvertisingImage = "don_1_port_poster.png";


            var dlgData = new SelectCharityDlgData()
            {
                Items = new List<CharityItem>() { charityItem1, charityItem2, charityItem3, charityItem4 },
                fHideCancelButton = true,
                //Items = new List<StandaloneCharity>() { standaloneCharity01 }
            };

            DialogBuilder.Show(IPayDialog.DONATION_SELECT_FIRST_CHARITY_DIALOG, StringIds.STRING_DONATION, (iResult, args) =>
            {

            }, true, false, dlgData);
        }

        void ShowEnterAccesscode()
        {
            ApplicationBaseFlow baseFlow = new ApplicationBaseFlow();
            baseFlow.Notification(StringIds.STRING_NOTIFICATION, IconIds.VECTOR_SECURITY_ACCESS, StringIds.STRING_USER_SECURITY_ACCESS_DENIED, StringIds.STRING_ENTERACCESSCODE, StringIds.STRING_OK);
        }

        void ShowDonationEnterAccessCodeDialog()
        {
            //var dlgData = new DonationEnterAccessCodeDlgData()
            //{
            //    AvatarRes = "dr_madison.png",
            //    MerchantName = "Dr Madison"
            //};

            //DialogBuilder.Show(IPayDialog.DONATION_ENTER_ACCESS_CODE_DIALOG, StringIds.STRING_ENTERACCESSCODE, (iResult, args) =>
            //{

            //}, true, false, dlgData);
        }

        void ShowDonationAdvertising()
        {
            //var dlgData = new DonationAdvertisingDlgData()
            //{
            //    AdvertisingImgRes = "donation_salvation_ads.png",
            //    AdvertisingBannerRes = "donation_salvation_banner.png"
            //};

            //DialogBuilder.Show(IPayDialog.DONATION_ADVERTISING_DIALOG, StringIds.STRING_ENTERACCESSCODE, (iResult, args) =>
            //{

            //}, true, false, dlgData);
        }

        void ShowDonationSelectAmountDialog()
        {
            var listValue = new List<long>() { 1, 2, 5, 10, 50 };

            DialogBuilder.Show(IPayDialog.DONATION_SELECT_AMOUNT_DIALOG, StringIds.STRING_DONATION, (iResult, args) =>
            {

            }, true, false, listValue);
        }

        void ShowDonationSelectOptionDialog()
        {
            //var items = new List<SelectOptionItem>()
            //{
            //    new SelectOptionItem()
            //    {
            //        Command = GlobalResource.EXIT_BUTTON,
            //        TitleId = StringIds.STRING_EXIT_CHARITY,
            //        VectorIconResName = IconIds.VECTOR_USER_LOGOUT,
            //    },
            //    new SelectOptionItem()
            //    {
            //        //Command = GlobalResource.ADVERTISING_SCREEN_OFF_BUTTON,
            //        TitleId = StringIds.STRING_ADVERTISING_SCREEN_OFF,
            //        VectorIconResName = IconIds.VECTOR_SCREEN_GRAY,
            //    },
            //     new SelectOptionItem()
            //    {
            //        Command = GlobalResource.SYNC_BUTTON,
            //        TitleId = StringIds.STRING_SYNC_SETTINGS,
            //        VectorIconResName = IconIds.VECTOR_SYNC,
            //    },
            //};

            //var data = new SelectOptionDlgData();

            //data.MenuItems = items;

            //DialogBuilder.Show(IPayDialog.ACCESS_MENU_DIALOG, StringIds.STRING_MENU, (iResult, args) =>
            //{
            //    //SelectOptionDialog
            //}, true, true, data);
        }

        void ShowDonationReceiptOptionsDialog()
        {
            //var data = new ReceiptOptionsDlgData();

            //data.QRReceiptResult = "ReceiptOptionsDialog";
            //data.fShowQrCode = true;

            //data.FunctionButtons = new List<SelectButton>();

            //data.FunctionButtons.Add(new SelectButton()
            //{
            //    Title = StringIds.STRING_EMAIL_RECEIPT_LOWCASE,
            //    idImage = IconIds.VECTOR_EMAIL_RECEIPT,
            //    IdProcessor = 0,
            //    IsVectorDrawble = true
            //});

            //data.FunctionButtons.Add(new SelectButton()
            //{
            //    Title = StringIds.STRING_TEXT_RECEIPT,
            //    idImage = IconIds.VECTOR_TEXT_RECEIPT,
            //    IdProcessor = 0,
            //    IsVectorDrawble = true
            //});

            //data.FunctionButtons.Add(new SelectButton()
            //{
            //    Title = StringIds.STRING_PRINT_CUSTOMER,
            //    idImage = IconIds.VECTOR_PRINT_RECEIPT,
            //    IdProcessor = 0,
            //    IsVectorDrawble = true
            //});

            //var dialog4 = new DonationReceiptOptionsDialog(StringIds.STRING_RECEIPT_OPTIONS, null, data);
            //dialog4.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog4.Show(this);
        }

        private void ShowDonationRequestCardDialog(CaseDialog caseDialog)
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
            RequestDlgData.pInitProcessData = pInitProcessData;
            RequestDlgData.fMultiplePayments = false;
            RequestDlgData.fCanCancel = true;
            RequestDlgData.lTotal = pInitProcessData.lAmount
                                    + pInitProcessData.lTipAmount
                                    + pInitProcessData.lCashOut
                                    + (pInitProcessData.PaymentDonations != null ? pInitProcessData.PaymentDonations.lTotalDonations : 0)
                                    + (pInitProcessData.PaymentVouchers != null ? pInitProcessData.PaymentVouchers.lTotalVouchers : 0);

            RequestDlgData.PresentCardTitleId = StringIds.STRING_PRESENTCARD_TITLE;
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
            RequestDlgData.lszPreSurcharge = StringIds.STRING_SURCHARGE_CREDIT___DEBIT_FEES_APPLY;
            //RequestDlgData.fAlipayWechatLogo = true;
            //RequestDlgData.PresentCardAnimFileName = GlobalConstants.PRESENT_CARD_LOTTIE_INSERT_SWIPE_TAP;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    RequestDlgData.iFunctionButton = FunctionType.Purchase;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE2:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE3:

                    RequestDlgData.iFunctionButton = FunctionType.Cash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE4:

                    RequestDlgData.iFunctionButton = FunctionType.Refund;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE5:

                    RequestDlgData.iFunctionButton = FunctionType.PreAuth;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = false;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = false;

                    RequestDlgData.PresentCardSubTitleId = StringIds.STRING_OPEN_PREAUTH_UPCASE;

                    break;

                case CaseDialog.CASE6:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = false;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE7:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE8:

                    RequestDlgData.iFunctionButton = FunctionType.CardStatusCheck;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = true;

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
                //case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; totalTitleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
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

            //var requestCardDialog = new DonationRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            //{
            //}, RequestDlgData);

            ////comment out
            ////var requestCardDialog = new AnimatedRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            ////{
            ////}, RequestDlgData);

            //requestCardDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //requestCardDialog.Show(this);
        }

        void ShowStandaloneSelectDonationAmountDialog()
        {
            //CusDisplaySelectDonationDlgData dlgData = new CusDisplaySelectDonationDlgData();

            //dlgData.DonationImgName = "donation_salvation_banner.png";
            //dlgData.DonationValues = new List<long>() { 200, 500, 1000, 2000, 3000, 5000, 10000 };

            //DialogBuilder.Show(IPayDialog.STANDALONE_SELECT_DONATION_AMOUNT_DIALOG, StringIds.STRING_DONATION, (iResult, args) =>
            //{
            //    //StandaloneSelectDonationAmountDialog
            //}, true, false, dlgData);
        }

        void ShowQrCodeReceiptClaimDialog()
        {
            string QRReceiptResult = "wrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioo";

            DialogBuilder.Show(IPayDialog.RECEIPT_QR_CODE_DIALOG, StringIds.STRING_RECEIPT_CLAIM, (iResult, args) =>
            {

            }, true, false, QRReceiptResult);
        }

        void ShowHelpDialog()
        {
            //HelpDlgData helpDlgData = new HelpDlgData();

            //helpDlgData.Title = StringIds.STRING_PURCHASE;

            //helpDlgData.SettingUrl = "wrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioo";

            //DialogBuilder.Show(IPayDialog.HELP_DIALOG, StringIds.STRING_HELP_AND_TRAINING, (iResult, args) =>
            //{
            //}, true, false, helpDlgData);
        }

        void ShowHelpQRDialog()
        {
            //HelpQRDialog helpQRDialog = null;

            //HelpDlgData _data = new HelpDlgData();

            //_data.SettingUrl = "wrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioowrtyuioo";

            //helpQRDialog = new HelpQRDialog(StringIds.STRING_HELP_QRCODE, (iResult3, args3) =>
            //{
            //    helpQRDialog.Dismiss();
            //    helpQRDialog = null;

            //}, _data);

            //helpQRDialog.Show(this);
        }

        void ShowSelectFunctionDialog()
        {
            var selectFuncDialogDta = new SelFncDlgData()
            {
                iPage = 1,
                iMaxPage = 4,
                iMinPage = 1,
                pIdProcessor = 0,
                fShowLogout = false,
                fGrid = true,
                pIdSecurityUser = 1,
                strListTitle = StringIds.STRING_SELECT_TRANSACTION_TYPE,
            };

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_PURCHASE,
                Title = StringIds.STRING_PURCHASE,
                idImage = IconIds.VECTOR_PURCHASE,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_PURCHASE_AND_CASH,
                Title = StringIds.STRING_PURCHASE_AND_CASH,
                idImage = IconIds.VECTOR_PURCHASE_CASH,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_CASH_ONLY,
                Title = StringIds.STRING_CASH_ONLY,
                idImage = IconIds.VECTOR_CASH_ONLY,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_MOTO_OPTIONS,
                Title = StringIds.STRING_MOTO_OPTIONS,
                idImage = IconIds.VECTOR_MOTO,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_FUNCTIONTYPES_REFUND,
                Title = StringIds.STRING_FUNCTIONTYPES_REFUND,
                idImage = IconIds.VECTOR_REFUND,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_FUNCTIONTYPES_PREAUTH,
                Title = StringIds.STRING_FUNCTIONTYPES_PREAUTH,
                idImage = IconIds.VECTOR_PRE_AUTH_RED,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_OPEN_TAB,
                Title = StringIds.STRING_OPEN_TAB,
                idImage = IconIds.VECTOR_OPEN_TAB,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_REPRINT,
                Title = StringIds.STRING_REPRINT,
                idImage = IconIds.VECTOR_REPRINT_RECEIPT,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_LOGON,
                Title = StringIds.STRING_LOGON,
                idImage = IconIds.VECTOR_LOGON,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_FUNCTIONTYPES_SETTLE,
                Title = StringIds.STRING_FUNCTIONTYPES_SETTLE,
                idImage = IconIds.VECTOR_SETTLEMENT,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_CARD_STATUS_CHECK,
                Title = StringIds.STRING_CARD_STATUS_CHECK,
                idImage = IconIds.VECTOR_AUTH_ONLY,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_FUNCTIONTYPES_PREAUTH,
                Title = StringIds.STRING_FUNCTIONTYPES_PREAUTH,
                idImage = IconIds.VECTOR_PRE_AUTH_RED,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_FINAL_COMPLETION,
                Title = StringIds.STRING_FINAL_COMPLETION,
                idImage = IconIds.VECTOR_PRE_AUTH_COMPLETE,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_OTHER_PRE_AUTHS,
                Title = StringIds.STRING_OTHER_PRE_AUTHS,
                idImage = IconIds.VECTOR_PRE_AUTH_INCREMENT,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_PARTIAL_COMPLETION,
                Title = StringIds.STRING_PARTIAL_COMPLETION,
                idImage = IconIds.VECTOR_PRE_AUTH_PARTIAL,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_DELAYED_COMPLETION,
                Title = StringIds.STRING_DELAYED_COMPLETION,
                idImage = IconIds.VECTOR_PRE_AUTH_DELAYED_COMPLETION,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_REPRINT_PREAUTH,
                Title = StringIds.STRING_REPRINT_PREAUTH,
                idImage = IconIds.VECTOR_TAB_AUTH_ONLY,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_CANCEL_PRE_AUTH,
                Title = StringIds.STRING_CANCEL_PRE_AUTH,
                idImage = IconIds.VECTOR_CANCEL_PRE_AUTH,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            selectFuncDialogDta.FunctionButtons.Add(new SelectButton()
            {
                iCommandLang = StringIds.STRING_PRINT_PENDING_HOSPITALITY,
                Title = StringIds.STRING_PRINT_PENDING_HOSPITALITY,
                idImage = IconIds.VECTOR_PRINT_PENDING,
                IdProcessor = 0,
                iCommand = GlobalResource.FNC_SALE_BUTTON,
            });

            DialogBuilder.Show(IPayDialog.SELECT_FUNCTION_DIALOG, StringIds.STRING_TRANSACTION, (iResult, args) =>
            {

            }, true, false, selectFuncDialogDta);
        }

        void ShowAttendedMenu()
        {
            //var generalType = new List<GenericType>()
            //{
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_UNATTENDED_USER,
            //        lszText =  StringIds.STRING_MANAGER,
            //        Id = GlobalResource.MANAGER_FUNCTION_BUTTON,
            //    },
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_UNATTENDED_USER_NO_TIE,
            //        lszText =  StringIds.STRING_UNATTENDED,
            //        Id = GlobalResource.UNATTENDED_BUTTON,
            //    },
            //};

            ////var dialog3 = new UI.UnattendedDynamicOptionDialog(string.Empty, null, generalType, string.Empty, GlobalResource.CANCEL_BUTTON, StringIds.STRING_CANCEL);
            ////dialog3.DialogStyle = DialogStyle.FULLSCREEN;
            ////dialog3.Show(this);

            //DialogBuilder.Show(IPayDialog.UNATTENDED_DYNAMIC_OPTION_DIALOG, string.Empty, (iResult, args) =>
            //{

            //}, true, false, generalType, string.Empty, GlobalResource.CANCEL_BUTTON, StringIds.STRING_CANCEL);
        }

        void ShowSelectUnattendedMode()
        {
            //var generalType = new List<GenericType>()
            //{
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_FUEL,
            //        lszText =  StringIds.STRING_FUEL,
            //        Id = GlobalResource.OK_BUTTON,
            //    },
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_VENDING,
            //        lszText =  StringIds.STRING_VENDING,
            //        Id = GlobalResource.OK_BUTTON + 1000,
            //    },
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_EV,
            //        lszText =  StringIds.STRING_EV,
            //        Id = GlobalResource.OK_BUTTON + 2000,
            //    },
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_STANDALONE_NO_BACKGROUND,
            //        lszText =  StringIds.STRING_STANDALONE,
            //        Id = GlobalResource.OK_BUTTON + 3000,
            //    },
            //};

            //DialogBuilder.Show(IPayDialog.UNATTENDED_DYNAMIC_OPTION_DIALOG, string.Empty, (iResult, args) =>
            //{

            //}, true, false, generalType, string.Empty, GlobalResource.CANCEL_BUTTON, StringIds.STRING_CANCEL);
        }

        void ShowUnattendedSingleUserLoginDialog()
        {
            //var userId = string.Empty;
            //var passcode = string.Empty;
            //var pLogonData = new LogonDlgData()
            //{
            //    InitData = new InitLogonModel()
            //    {
            //        fLogonPasscode = true,
            //        fAutoLogin = false,
            //    }
            //};

            //DialogBuilder.Show(IPayDialog.UNATTENDED_SINGLE_USER_LOGIN_DIALOG, StringIds.STRING_LOGON, (iResult, args) =>
            //{

            //}, true, false, pLogonData, false);
        }

        void ShowSelectManagerMenu()
        {
            //var generalType = new List<GenericType>()
            //{
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_UNATTENDED_SETUP,
            //        lszText =  StringIds.STRING_SETUP,
            //        Id = GlobalResource.SETUP_BUTTON,
            //    },
            //    new GenericType()
            //    {
            //        Icon = IconIds.VECTOR_UNATTENDED_TRANSACTION,
            //        lszText =  StringIds.STRING_TRANSACTIONS_TITLE,
            //        Id = GlobalResource.SELECT_FUNCTION_BUTTON,
            //    }
            //};

            //DialogBuilder.Show(IPayDialog.UNATTENDED_DYNAMIC_OPTION_DIALOG, string.Empty, (iResult, args) =>
            //{

            //}, true, false, generalType, string.Empty, GlobalResource.CANCEL_BUTTON, StringIds.STRING_CANCEL);
        }

        void ShowUnattendedGetAmountDialog(CaseDialog caseDialog)
        {
            //var data = new GetAmountDlgData();

            //data.lszPayButtonText = StringIds.STRING_OK_UPCASE;
            //data.EntryAmountTitleId = StringIds.STRING_AMOUNT_PREAPPROVED;
            //data.plszReference = "123456";
            //data.ReferenceTypeTitleId = DataHelper.GetRefName(ReferenceType.Invoice);
            ////data.isEnabledEntryAmount = false;

            //switch (caseDialog)
            //{
            //    case CaseDialog.CASE1:
            //        data.fShowReference = false;
            //        //data.isEnabledEntryAmount = true;

            //        break;
            //    case CaseDialog.CASE2:
            //        data.fShowReference = true;
            //        //data.isEnabledEntryAmount = false;
            //        break;

            //    default:
            //        data.fShowReference = true;
            //        //data.isEnabledEntryAmount = true;
            //        break;
            //}

            //var dialog = new UnattendedGetAmountDialog(StringIds.STRING_PURCHASE_UPCASE, null, data);
            //dialog.DialogStyle = DialogStyle.FULLSCREEN;
            //dialog.Show(this);
        }

        private void ShowUnattendedRequestCardDialog(CaseDialog caseDialog)
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
            RequestDlgData.pInitProcessData = pInitProcessData;
            RequestDlgData.fMultiplePayments = false;
            RequestDlgData.fCanCancel = true;
            RequestDlgData.lTotal = pInitProcessData.lAmount
                                    + pInitProcessData.lTipAmount
                                    + pInitProcessData.lCashOut
                                    + (pInitProcessData.PaymentDonations != null ? pInitProcessData.PaymentDonations.lTotalDonations : 0)
                                    + (pInitProcessData.PaymentVouchers != null ? pInitProcessData.PaymentVouchers.lTotalVouchers : 0);

            RequestDlgData.PresentCardTitleId = StringIds.STRING_PRESENTCARD_TITLE;
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
            RequestDlgData.lszPreSurcharge = StringIds.STRING_SURCHARGE_CREDIT___DEBIT_FEES_APPLY;
            //RequestDlgData.fAlipayWechatLogo = true;
            //RequestDlgData.PresentCardAnimFileName = GlobalConstants.PRESENT_CARD_LOTTIE_INSERT_SWIPE_TAP;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    RequestDlgData.iFunctionButton = FunctionType.Purchase;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;
                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE2:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE3:

                    RequestDlgData.iFunctionButton = FunctionType.Cash;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.ErrorMessageId = StringIds.STRING_CANNOTREADCARD;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE4:

                    RequestDlgData.iFunctionButton = FunctionType.Refund;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = false;
                    RequestDlgData.fOtherPay = false;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE5:

                    RequestDlgData.iFunctionButton = FunctionType.PreAuth;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = false;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = false;

                    RequestDlgData.PresentCardSubTitleId = StringIds.STRING_OPEN_PREAUTH_UPCASE;

                    break;

                case CaseDialog.CASE6:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = false;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = false;

                    break;

                case CaseDialog.CASE7:

                    RequestDlgData.iFunctionButton = FunctionType.PurchaseCash;
                    RequestDlgData.fMSR = false;
                    RequestDlgData.fSmart = false;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = false;
                    RequestDlgData.fWePay = true;

                    break;

                case CaseDialog.CASE8:

                    RequestDlgData.iFunctionButton = FunctionType.CardStatusCheck;
                    RequestDlgData.fMSR = true;
                    RequestDlgData.fSmart = true;
                    RequestDlgData.fRfid = true;

                    RequestDlgData.fManualPay = true;
                    RequestDlgData.fOtherPay = true;

                    RequestDlgData.fAliPay = true;
                    RequestDlgData.fWePay = true;

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
                //case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; totalTitleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
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

            //var requestCardDialog = new UnattendedRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            //{
            //}, RequestDlgData);

            ////comment out
            ////var requestCardDialog = new AnimatedRequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            ////{
            ////}, RequestDlgData);

            //requestCardDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //requestCardDialog.Show(this);
        }

        private void ShowUnattendedProcessMessageDialog(CaseDialog caseDialog)
        {
            var pProcessingData = new ProcessingData();

            pProcessingData.fAutoClose = true;
            string cancelBtnTitleId = "";

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    pProcessingData.hTextTitle = "";
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = "";
                    pProcessingData.hTextThree = "";
                    break;

                case CaseDialog.CASE2:

                    pProcessingData.hTextTitle = Localize.GetString(StringIds.STRING_EMAIL_RECEIPT);
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = "";
                    pProcessingData.hTextThree = Localize.GetString(StringIds.STRING_EMV_LEAVECARDINSERTED);
                    break;

                case CaseDialog.CASE3:

                    pProcessingData.hTextTitle = Localize.GetString(StringIds.STRING_EMAIL_RECEIPT);
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = Localize.GetString(StringIds.STRING_EMV_TERMINALACTIONANALYSIS);
                    pProcessingData.hTextThree = Localize.GetString(StringIds.STRING_EMV_LEAVECARDINSERTED);
                    break;

                case CaseDialog.CASE4:

                    pProcessingData.hTextTitle = Localize.GetString(StringIds.STRING_EMAIL_RECEIPT);
                    pProcessingData.hTextOne = Localize.GetString(StringIds.STRING_EMV_PROCESSINGNOW);
                    pProcessingData.hTextTwo = Localize.GetString(StringIds.STRING_EMV_TERMINALACTIONANALYSIS);
                    pProcessingData.hTextThree = Localize.GetString(StringIds.STRING_EMV_LEAVECARDINSERTED);
                    cancelBtnTitleId = StringIds.STRING_TEMPORARILY_HALT_TRANSMISSION;
                    break;
            }

            //var enterPinDialog = new UnattendedProcessMessageDialog(StringIds.STRING_PROCESSING_TITLE, null, pProcessingData, cancelBtnTitleId);
            //enterPinDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //enterPinDialog.Show(this);
        }

        void ShowUnattendedSelectDCCCurrency()
        {
            Currency currency;

            SelectDCCCurrencyData DlgData = new SelectDCCCurrencyData();

            DlgData.lTotal = 13800;

            DlgData.lLocalTotal = 10100;

            DlgData.lCardCurrencyTotal = 6420;

            DlgData.fSmartCard = true;

            DlgData.szConversionRate = "1.08234";

            DlgData.szMarginPercentage = "3.3162";

            currency = CurrencyRepository.Instance.GetByCurrencyCode(554);

            if (currency != null)
            {
                DlgData.LocalCurrency = currency.wszCurrencyCode;

                DlgData.LocalCountry = "NZD";

                DlgData.LocalImage = currency.iCurrencyCodeFlag;
            }

            currency = CurrencyRepository.Instance.GetByCurrencyCode(840);

            if (currency != null)
            {
                DlgData.HomeCurrency = currency.wszCurrencyCode;
                DlgData.HomeCountry = "USD";
                DlgData.HomeImage = currency.iCurrencyCodeFlag;
            }

            //DialogBuilder.Show(IShellDialog.UNATTENDED_SELECT_DCCCURRENCY_DIALOG, StringIds.STRING_CURRENCY, (iResult, args) =>
            //{
            //    //SelectDCCCurrencyDialog
            //}, true, false, DlgData);
        }

        void ShowUnattendedDCCConfirmation()
        {
            var currency = CurrencyRepository.Instance.GetByCurrencyCode(840);
            DCCConfimationData data = new DCCConfimationData();

            data.FlagImage = currency.iCurrencyCodeFlag;
            data.Currency = currency.wszCurrencyCode;
            data.lAmount = 6420;
            data.Content = "I declare i have been given a choice in payment currency and i agree to pay the above amount. ";

            //DialogBuilder.Show(IShellDialog.UNATTENDED_DCC_CONFIRMATION_DIALOG, StringIds.STRING_CONFIRMATION, (iResult, args) =>
            //{
            //    //UnattendedDCCConfirmation
            //}, true, false, data);
        }

        private void ShowUnattendedApprovalDialog()
        {
            ApprovalDlgData data = new ApprovalDlgData();
            data.lPurchaseApproval = 12000;
            data.PrintStage = PrintStage.PrintComplete;
            data.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);
            data.fApproved = true;
            data.lpszThirdResult = Localize.GetString(StringIds.STRING_PRINT_MERCHANT_COPY).ToUpper();
            data.fCustomerDisplay = false;
            data.IdBitmap = GlobalResource.MB_ICONAPPROVAL_BMP;
            data.FunctionType = FunctionType.Purchase;
            data.TransactionTypeStringId = GetStringId(data.FunctionType);

            //var approvalDialog = new ShellUI.UnattendedApprovalDialog(StringIds.STRING_TRANSACTION, null, data);
            //approvalDialog.OnResult += (iResult, args) =>
            //{
            //    approvalDialog.Dismiss();
            //};
            //approvalDialog.DialogStyle = DialogStyle.FULLSCREEN;
            //approvalDialog.Show(this);

        }

        void ShowUnattendedReceiptOptionDialog()
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
                iCommand = GlobalResource.BUTTON_EMAIL_RECEIPT,
            });

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_TEXT_RECEIPT,
                idImage = IconIds.VECTOR_TEXT_RECEIPT,
                IdProcessor = 0,
                iCommand = GlobalResource.BUTTON_TEXT_RECEIPT,
            });

            data.FunctionButtons.Add(new SelectButton()
            {
                Title = StringIds.STRING_PRINT_CUSTOMER,
                idImage = IconIds.VECTOR_PRINT_RECEIPT,
                IdProcessor = 0,
                iCommand = GlobalResource.BUTTON_PRINT_RECEIPT,
            });

            //DialogBuilder.Show(IPayDialog.UNATTENDED_RECEIPT_OPTIONS_DIALOG, StringIds.STRING_REVIEW, null, true, false, data);
        }

        void ShowUnattendedReviewTransDialog(CaseDialog caseDialog)
        {
            //string chargeTime = "02:00:00 HR";
            //string remainingTime = "00:30:00 HR";
            //float cost = 13.8f;
            //long lPreApproved = 10000;
            //string Curccency = "ZD";
            //float fFuelLitre = 48.2f;
            //long lLittreCost = 197;
            //string strFuelType = "95RON";
            //UnattendedReviewDlgData dlgDataReview = new UnattendedReviewDlgData();

            //switch (caseDialog)
            //{
            //    case CaseDialog.CASE1://fuel

            //        dlgDataReview = new UnattendedReviewDlgData()
            //        {
            //            lTotalAmount = 38000,
            //            TitleId = StringIds.STRING_TOTAL_PAYABLE,
            //            strIconResId = IconIds.VECTOR_FUEL
            //        };

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_FUEL_LITRES,
            //            Value = $"{fFuelLitre}L",
            //            IsRightTextBold = true,
            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_LITRE_COST,
            //            Value = lLittreCost.ToFormatLocalCurrencyAmount(),
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_FUEL_TYPE,
            //            Value = strFuelType,
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_PRE_APPROVED,
            //            Value = lPreApproved.ToFormatLocalCurrencyAmount(),
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Value = Curccency + Localize.GetString(StringIds.STRING_CURRENCY),
            //            IsSpecial = true
            //        });
            //        break;

            //    case CaseDialog.CASE2://hardcode vending electric

            //        dlgDataReview = new UnattendedReviewDlgData()
            //        {
            //            lTotalAmount = 38000,
            //            TitleId = StringIds.STRING_TOTAL_PAYABLE,
            //            strIconResId = IconIds.VECTOR_EV
            //        };

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_CHARGE_TIME,
            //            Value = chargeTime,
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_REMAINING_TIME,
            //            Value = remainingTime,
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_COST_PER_KWH,
            //            Value = $"{cost} CENT",
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Title = StringIds.STRING_PRE_APPROVED,
            //            Value = lPreApproved.ToFormatLocalCurrencyAmount(),
            //            IsRightTextBold = true,

            //        });

            //        dlgDataReview.listData.Add(new ResultViewModel()
            //        {
            //            Value = Curccency + Localize.GetString(StringIds.STRING_CURRENCY),
            //            IsSpecial = true
            //        });

            //        break;
            //}

            //DialogBuilder.Show(IPayDialog.UNATTENDED_REVIEW_TRANS_DIALOG, StringIds.STRING_REVIEW, null, true, false, dlgDataReview);
        }

        void ShowUnattendedVendingStoreDialog()
        {

        //    List<UnattendedVendingProductViewModel> listProductModel = new List<UnattendedVendingProductViewModel>();

        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_AMERICANO, ProductName = "Americano", Price = 440 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_BREWED_COFFEE, ProductName = "Brewed Coffee", Price = 480 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_CAFFE_LATTE, ProductName = "Caffe Latte", Price = 500 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_CAFFE_MOCHA, ProductName = "Caffe Mocha", Price = 550 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_CAPPUCCINO, ProductName = "Cappuccino", Price = 500 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_ESPRESSO, ProductName = "Flat White", Price = 500 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_FLAT_WHITE, ProductName = "Oleato Latte", Price = 550 });
        //    listProductModel.Add(new UnattendedVendingProductViewModel() { ProductImg = IconIds.IMG_UNATTENDED_OLEATO_LATTE, ProductName = "Espresso", Price = 400 });


        //    DialogBuilder.Show(IPayDialog.UNATTENDED_VENDING_STORE_DIALOG, string.Empty, (iResult, args) =>
        //    {

        //        //UnattendedVendingStoreDialog
        //    }, true, false, listProductModel);
        //}

        //void ShowUnattendAdvertisingDialog()
        //{

        //    string imagePath = IconIds.IMG_UNATTENDED_FUEL;
        //    //imagePaths = IconIds.IMG_UNATTENDED_FUEL;
        //    //imagePaths = IconIds.IMG_UNATTENDED_VENDING;
        //    //imagePaths = IconIds.IMG_UNATTENDED_EV;
            

        //    DialogBuilder.Show(IPayDialog.UNATTENDED_ADVERTISING_DIALOG, string.Empty, (Result, args) =>
        //    {

        //    }, true, false, imagePath);
        }

        void ShowSelectApplicationTypeDialog()
        {
            List<CandidateAid> adis = new List<CandidateAid>();

            adis.Add(new CandidateAid() { szAid = "123GJJK", szAppLabel = "LAS", szAppPreName = "BCONS" });
            adis.Add(new CandidateAid() { szAid = "123GJJK", szAppLabel = "LAS", szAppPreName = "BCONS" });
            adis.Add(new CandidateAid() { szAid = "123GJJK", szAppLabel = "LAS", szAppPreName = "BCONS" });
            adis.Add(new CandidateAid() { szAid = "123GJJK", szAppLabel = "LAS", szAppPreName = "BCONS" });
            adis.Add(new CandidateAid() { szAid = "123GJJK", szAppLabel = "LAS", szAppPreName = "BCONS" });
            adis.Add(new CandidateAid() { szAid = "123GJJK", szAppLabel = "LAS", szAppPreName = "BCONS" });

            DialogBuilder.Show(IShellDialog.SELECT_APPLICATIONTYPE_DIALOG, StringIds.STRING_EMV_SELECTAPPLICATION, (iResult, args) =>
            {
                //SelectApplicationTypeDialog
            }, true, false, adis);
        }

        void ShowNotificationDialog()
        {
            string strHeaderTitlteId = StringIds.STRING_NOTIFICATION;
            string strImageVector = IconIds.VECTOR_USER_ID_PASSCODE_INCORRECT;
            string strMainResultId = StringIds.STRING_USER_IS_NOT_ALLOW;
            //string strLeftButtonTextId = StringIds.STRING_CANCEL_UPCASE;
            string strLeftButtonTextId = null;
            string strRightButtonTextId = StringIds.STRING_OK_UPCASE;

            DialogBuilder.Show(IPayDialog.NOTIFICATION_DIALOG, strHeaderTitlteId, (iResult, args) =>
            {
                //NotificationDialog
            }, true, true, strMainResultId, strImageVector, strLeftButtonTextId, strRightButtonTextId);
        }

        void ShowSalesShiftDialog()
        {
            //SaleShiftDlgData data = new SaleShiftDlgData();
            //data.ReportCategory = ReportCategory.Sales;
            //data.IsShowBottomButton = true;
            //data.RightButtonTitleId = StringIds.STRING_DELETE_ALL;

            //data.Items.Add(new SaleShiftTitleModel()
            //{
            //    Date = DateTime.Now,
            //    IdList = new List<uint> { 1, 2, 3, 4, 5, 6 },
            //    IsShowLine = false
            //});

            //data.Items.Add(new SaleShiftTitleModel()
            //{
            //    Date = DateTime.Now,
            //    IdList = new List<uint> { 1, 2, 3, 4, 5, 6 }
            //});

            //data.Items.Add(new SaleShiftTitleModel()
            //{
            //    Date = DateTime.Now,
            //    IdList = new List<uint> { 1, 2, 3, 4, 5, 6 }
            //});

            //data.Items.Add(new SaleShiftTitleModel()
            //{
            //    Date = DateTime.Now,
            //    IdList = new List<uint> { 1, 2, 3, 4, 5, 6 }
            //});

            //DialogBuilder.Show(IPayDialog.SELECT_SALE_SHIFT_DIALOG, StringIds.STRING_SHIFT_LIST, (result, args) =>
            //{
            //    //SalesShiftDialog
            //}, true, false, data);
        }

        
    }
}