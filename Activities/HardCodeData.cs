using Android.App;
using CloudBanking.BaseControl;
using CloudBanking.BaseHardware;
using CloudBanking.Common;
using CloudBanking.Entities;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
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
using static CloudBanking.Entities.Database;
using static CloudBanking.Entities.RefundReasonDlgData;
using static CloudBanking.Utilities.UtilEnum;
using static Java.Util.Jar.Attributes;

namespace CloudBanking.UITestApp
{
    public partial class TestActivity : BaseActivity
    {
        private void ShowApprovalDialog(CaseDialog caseDialog)
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
                    DlgData.lpszSecondaryResult = "Signature didn't match";
                    break;
                case CaseDialog.CASE2:
                    DlgData.lszMainString = Localize.GetString(StringIds.STRING_EMVSTD_APPROVED);
                    DlgData.fApproved = true;
                    DlgData.lpszResult = "CONNECTION SUCCESSFUL";
                    DlgData.lpszSecondaryResult = "Access code has been validated";
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
                case FunctionType.IncrementalAdjust: lszTitle = StringIds.STRING_FUNCTIONTYPES_INCREMENTALADJUST; break;
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

            RequestDlgData.fNoPresentCard = false;
            RequestDlgData.fOtherPay = true;
            RequestDlgData.pInitProcessData = pInitProcessData;
            RequestDlgData.fMultiplePayments = false;
            RequestDlgData.fCanCancel = true;
            RequestDlgData.lTotal = pInitProcessData.lAmount
                                    + pInitProcessData.lTipAmount
                                    + pInitProcessData.lCashOut
                                    + (pInitProcessData.PaymentDonations != null ? pInitProcessData.PaymentDonations.lTotalDonations : 0)
                                    + (pInitProcessData.PaymentVouchers != null ? pInitProcessData.PaymentVouchers.lTotalVouchers : 0);

            RequestDlgData.PresentCardTitleId = StringIds.STRING_PRESENTCARD_TITLE;
            RequestDlgData.fAliPay = true;
            RequestDlgData.fWePay = true;
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
            //RequestDlgData.PresentCardAnimFileName = GlobalConstants.PRESENT_CARD_LOTTIE_ARROW_UP;

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
                    RequestDlgData.fRfid = true;
                    RequestDlgData.fManualPay = true;

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
                    RequestDlgData.fManualPay = false;

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
                    RequestDlgData.fManualPay = false;

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
                    RequestDlgData.fManualPay = false;
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

            var requestCardDialog = new RequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            {
            }, RequestDlgData);

            //comment out
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

        private void ShowMessageDialog(CaseDialog caseDialog)
        {
            IBaseDialog dialog = null;

            switch (caseDialog)
            {
                case CaseDialog.CASE1:

                    CustomStringMessageBox(false, StringIds.STRING_EMV_REMOVECARD, StringIds.STRING_EMV_REMOVECARD, false, GlobalResource.MB_NONE, GlobalResource.ICON_ICC_CARD_REMOVED, fAutoDismiss: true);
                    break;

                case CaseDialog.CASE2:

                    CustomStringMessageBox(true, StringIds.STRING_EMV_REMOVECARD, StringIds.STRING_CARD_REMOVE_TOO_SOON, false, GlobalResource.MB_RETRYCANCEL, GlobalResource.ICON_ICC_CARD_REMOVED);
                    break;

                case CaseDialog.CASE3:

                    ErrorMessage(string.Empty, Localize.GetString(StringIds.STRING_TRANSACTIONCANCELLED), false, string.Empty, false, Localize.GetString(StringIds.STRING_PLEASEREMOVECARD), GlobalResource.MB_NONE);
                    break;

                case CaseDialog.CASE4:

                    CustomStringMessageBox(true, StringIds.STRING_CONFIRM_SIGNATURE, StringIds.STRING_CONFIRM_SIGNATURE_IS_CORRECT_UPCASE, false, GlobalResource.MB_YESNO, ref dialog, GlobalResource.MB_ICON_SIGNATURE_RESULT, aboveMsg: StringIds.STRING_SIGNATURE_REQUIRED, fAboveMsgActualText: false);
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

        private void ErrorMessage(string IdDlgTitle, string lpszMainResult, bool fActualText, string lpszSecondaryResult, bool fSubActualText, string bottomWarningId, int uType)
        {
            CustomStringMessageBox(true, IdDlgTitle, lpszMainResult, fActualText, uType, GlobalResource.MB_ICONDECLINED_BMP, subMsg: lpszSecondaryResult, fSubActualText: fSubActualText, bottomWarningId: bottomWarningId);
        }

        private void CustomStringMessageBox(bool BlockUI, string IdDlgTitleText, string strContent, bool fActualText, int uType, int idImg = 0, EvtMessage Evt = null, string subMsg = "", bool fSubActualText = false, string bottomWarningId = "",
                string strSubMessageColor = "", int iSubMessageTextSize = 0, string thirdbMsg = "", bool fThirdActualText = false, bool isShowBackBtn = false, string aboveMsg = "", bool fAboveMsgActualText = false, bool fAutoDismiss = false, string thirdbMsgColor = "")
        {
            IBaseDialog dialog = null;

            CustomStringMessageBox(BlockUI, IdDlgTitleText, strContent, fActualText, uType, ref dialog, idImg, Evt, subMsg, fSubActualText, bottomWarningId,
              strSubMessageColor, iSubMessageTextSize, thirdbMsg, fThirdActualText, isShowBackBtn, aboveMsg, fAboveMsgActualText, fAutoDismiss, thirdbMsgColor);
        }

        private void CustomStringMessageBox(bool BlockUI, string IdDlgTitleText, string strContent, bool fActualText, int uType, ref IBaseDialog dialog, int idImg = 0, EvtMessage Evt = null, string subMsg = "", bool fSubActualText = false, string bottomWarningId = "",
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

            var messageDialog = new MessageDialog(IdDlgTitleText, null, data);
            messageDialog.DialogStyle = DialogStyle.FULLSCREEN;
            messageDialog.Show(this);
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

        private void ShowPreAuthEnterAmountDialog()
        {
            long amount = 10000;

            var dialog = new PreAuthEnterAmountDialog(StringIds.STRING_PRE_AUTH, null, amount, true);
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

        private void PreAuthItemGetNewAmount()
        {
            RecordViewModel selectedPayment = new RecordViewModel();
            selectedPayment.iCardType = CARDTYPE.CARD_AMEX;
            selectedPayment.iAccountTypeCode = AccountType.ACCOUNT_TYPE_SAVINGS;
            selectedPayment.szApprovalCode = "123456";
            selectedPayment.lAmount = 10000;
            selectedPayment.lszEndCardNumber = "7654";
            selectedPayment.CustomerReferenceType = ReferenceType.Customer;
            selectedPayment.lszCustomerReference = "6789";
            selectedPayment.AuthorizationExpiryDate = DateTime.Now;

            FunctionType functionType = FunctionType.PreAuthPartial;

            var data = new PreAuthCompletionDlgData()
            {
                CustomAmount = 38000,
                Amount = selectedPayment.lAmount,
                OriginalCardType = string.Format("{0} {1}", Localize.GetString(StringIds.STRING_CARDTYPE_AMEX), Localize.GetString(StringIds.STRING_ACCOUNTTYPESAVINGS)),
                LastFourDigitCardNumber = $"**** **** **** {selectedPayment.lszEndCardNumber}",
                Reference = selectedPayment.CustomerReferenceType,
                ReferenceNumber = selectedPayment?.lszCustomerReference,
                ExpireTime = selectedPayment?.AuthorizationExpiryDate,
                CustomerName = "David",
                PaymentStatus = Localize.GetString(StringIds.STRING_APPROVED).ToUpper(),
                AuthCode = selectedPayment?.szApprovalCode,
                CardBrandIconResName = CARDTYPE.CARD_AMEX.GetIconDrawable()
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

        void ShowEmailReceiptSendResultDialogSuccess()
        {
            var selectedEmail = "d.timms@yahoo.com";

            DialogBuilder.Show(IPayDialog.RECEIPT_RESULT_DIALOG, StringIds.STRING_EMAIL_RECEIPT_LOWCASE, (iResult, args) =>
            {
            }, true, false, new ReceiptResultDlgData(selectedEmail, UtilEnum.ReceiptType.Email, true));
        }

        void ShowEmailReceiptSendResultDialogFail()
        {
            var selectedEmail = "d.timms@yahoo.com";

            DialogBuilder.Show(IPayDialog.RECEIPT_RESULT_DIALOG, StringIds.STRING_EMAIL_RECEIPT_LOWCASE, (iResult, args) =>
            {
            }, true, false, new ReceiptResultDlgData(selectedEmail, UtilEnum.ReceiptType.Email, false));
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
            long amount = 10000;
            var keyValue = new KeyValuePair<double, long>(x, y);
            var data = new List<KeyValuePair<double, long>>() { keyValue, keyValue, keyValue, keyValue };

            var dialog = new SelectTipDialog(StringIds.STRING_SELECT_TIP_AMOUNT, null, data, true, amount);
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
            initData.iFunctionButton = FunctionType.Purchase;
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

        void ShowGetAmountDialog()
        {
            var data = new GetAmountDlgData();

            data.lszPayButtonText = StringIds.STRING_OK_UPCASE;
            data.EntryAmountTitleId = StringIds.STRING_PURCHASE;
            data.fShowReference = true;
            data.plszReference = "123456";
            data.ReferenceTypeTitleId = DataHelper.GetRefName(ReferenceType.Invoice);
            //data.isEnabledEntryAmount = false;

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
            selectedPayment.CustomerReferenceType = ReferenceType.Customer;
            selectedPayment.lszCustomerReference = "1234";
            selectedPayment.szSTAN = "2345";
            selectedPayment.szReferenceNumber = "1234";
            selectedPayment.AuthorizationExpiryDate = DateTime.Now;

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
                ExpireTime = selectedPayment.AuthorizationExpiryDate,
                CustomerName = string.Empty,  //GetCustomerName.OriginalPayment,TODO disabled on certification
            };

            string titleDialog = StringIds.STRING_FINAL_COMPLETION;

            FunctionType functionType = FunctionType.PreAuthPartial;

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

        void ShowGetAmountCashOutDialog()
        {
            var data = new GetAmountDlgData();
            data.lszPayButtonText = StringIds.STRING_OK_UPCASE;
            data.fInstoreCashoutFeeEnable = true;
            data.fInstoreCashoutFeePercent = true;
            data.lInstoreCashoutFeeAmount = 1000;
            data.plAmount = 2800;

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
            var data = new CusDisplaySelectDonationDlgData();

            //landscape
            //data.DonationImgName = "donation_port_demo_02";

            //portrait
            data.DonationImgName = "donation_small";

            var listValue = new List<long>() { 1, 2, 5, 10, 50 };
            data.DonationValues = listValue;

            var dialog = new SelectDonationDialog(StringIds.STRING_DONATION, null, data);
            dialog.DialogStyle = DialogStyle.FULLSCREEN;
            dialog.Show(this);
        }

        void ShowAdjustDonationDialog()
        {
            long amount = 38000;

            List<long> donation = new List<long> { 50, 200, 500, 1000 };

            //landscape
            //string iconId = "donation_port_demo_02.png";

            //portrait
            string iconId = "donation_small.png";

            string noBtnTitleId = "STRING_NO_DONATION";
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
                case FunctionType.IncrementalAdjust: titleId = StringIds.STRING_INCREMENTAL_ADJUST; break;
            }
            dlgData.szTotalTitle = titleId;

            dlgData.CurrencySymbol = "$";

            DialogBuilder.Show(IPayDialog.REQUEST_ALIPAY_WECHAT_DIALOG, StringIds.STRING_QR_PAYMENTS, (iResult, args) =>
            {
                //RequestAliPayWechatDialog
            }, blockUI, false, dlgData);

            //hardcode in dialog to show all case
            //_data_PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(_data.status)));
        }

        void ShowViewLeftIconRightQuadrupleTextOverlayDialog()
        {

            List<ViewFourthLineModel> dlgCardFeeData = new List<ViewFourthLineModel>();

            Action<ViewFourthLineModel> addCardFee = model => { if (model != null) dlgCardFeeData.Add(model); };

            addCardFee(SetDataCardFees(MerchantCardType.Visa));
            addCardFee(SetDataCardFees(MerchantCardType.MasterCard));
            addCardFee(SetDataCardFees(MerchantCardType.UnionPay));
            addCardFee(SetDataCardFees(MerchantCardType.Amex));
            addCardFee(SetDataCardFees(MerchantCardType.JCB));
            addCardFee(SetDataCardFees(MerchantCardType.Discover));
            addCardFee(SetDataCardFees(MerchantCardType.Diners));
            addCardFee(SetDataCardFees(MerchantCardType.Troy));

            var dialogCardFees = new ViewLeftIconRightQuadrupleTextOverlayDialog(StringIds.STRING_SURCHARGE_AND_SERVICE_FEES_UPCASE, dlgCardFeeData);

            dialogCardFees.OnLoadedEvt += delegate
            {
            };

            dialogCardFees?.Show(CrossCurrentActivity.Current.Activity);
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
            POSTicketInfo searchData = new POSTicketInfo();
            var pullSetupDlgData = new StandardSetupDialogModel()
            {
                OKBtnCommandId = GlobalResource.SEARCH_BUTTON,
                OkBtnTitleId = StringIds.STRING_SEARCH,
            };

            searchData.TicketNumber = "";
            searchData.EmployeeId = 1;
            searchData.Reference = string.Empty;
            searchData.TableNumber = 0;
            searchData.GuestName = string.Empty;

            pullSetupDlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
            {
                PropertyName = nameof(searchData.TableNumber),
                TitleId = StringIds.STRING_TABLENUMBER,
                HeaderTitleId = StringIds.STRING_TABLENUMBER,
                FieldTitleId = StringIds.STRING_TABLENUMBER,
                Value = searchData.TableNumber
            });

            pullSetupDlgData.Items.Add(new InputNumberFixedKeyboardEditModel()
            {
                PropertyName = nameof(searchData.TicketNumber),
                TitleId = StringIds.STRING_TICKETNO,
                HeaderTitleId = StringIds.STRING_TICKETNO,
                FieldTitleId = StringIds.STRING_TICKETNO,
                Value = searchData.TicketNumber
            });

            pullSetupDlgData.Items.Add(new InputTextEditModel()
            {
                PropertyName = nameof(searchData.GuestName),
                TitleId = StringIds.STRING_GUEST_NAME,
                HeaderTitleId = StringIds.STRING_GUEST_NAME,
                FieldTitleId = StringIds.STRING_GUEST_NAME,
                Value = searchData.GuestName
            });

            pullSetupDlgData.Items.Add(new InputTextEditModel()
            {
                PropertyName = nameof(searchData.Reference),
                TitleId = StringIds.STRING_REFERENCE,
                HeaderTitleId = StringIds.STRING_REFERENCE,
                FieldTitleId = StringIds.STRING_REFERENCE,
                Value = searchData.Reference
            });

            DialogBuilder.Show(IPayDialog.TICKET_SEARCH_OPTIONS_DIALOG, StringIds.STRING_TABLE_PAY_TICKET, (iResult, args) =>
            {
                //TicketSearchOptionsDialog
            }, true, false, pullSetupDlgData);
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
            var viewModel = new StandardSetupDialogModel()
            {
                OKBtnCommandId = GlobalResource.SAVE_BUTTON,
                OkBtnTitleId = StringIds.STRING_SAVE,
                CancelBtnCommandId = GlobalResource.CANCEL_SUB_FLOW,
                CancelTitleId = StringIds.STRING_CANCEL
            };

            var data = new POSTicketEditModel()
            {
                OriginalTicketNumber = "1234567",
                NewData = new POSTicketInfo()
                {
                    TicketNumber = "1234567",
                    GuestName = "Smith",
                    Reference = "Table 1",
                    TableNumber = 1
                }
            };

            viewModel.Items.Add(new InputNumberFixedKeyboardEditModel()
            {
                PropertyName = nameof(data.NewData.TableNumber),
                TitleId = StringIds.STRING_TABLE,
                HeaderTitleId = StringIds.STRING_TABLE,
                FieldTitleId = StringIds.STRING_TABLE,
                Value = data.NewData.TableNumber
            });

            viewModel.Items.Add(new InputNumberFixedKeyboardEditModel()
            {
                PropertyName = nameof(data.NewData.TicketNumber),
                TitleId = StringIds.STRING_TICKETNO,
                HeaderTitleId = StringIds.STRING_TICKETNO,
                FieldTitleId = StringIds.STRING_TICKETNO,
                Value = data.NewData.TicketNumber
            });

            viewModel.Items.Add(new InputTextEditModel()
            {
                PropertyName = nameof(data.NewData.GuestName),
                TitleId = StringIds.STRING_GUEST_NAME,
                HeaderTitleId = StringIds.STRING_GUEST_NAME,
                FieldTitleId = StringIds.STRING_GUEST_NAME,
                Value = data.NewData.GuestName
            });

            viewModel.Items.Add(new InputTextEditModel()
            {
                PropertyName = nameof(data.NewData.Reference),
                TitleId = StringIds.STRING_REFERENCE,
                HeaderTitleId = StringIds.STRING_REFERENCE,
                FieldTitleId = StringIds.STRING_REFERENCE,
                Value = data.NewData.Reference
            });

            DialogBuilder.Show(IPayDialog.STANDARD_SETUP_DIALOG, StringIds.STRING_EDIT_DETAILS, (iResult, args) =>
            {
                //StandardSetupDialog
            }, true, false, viewModel);
        }

        void ShowConfirmClosingTableDialog()
        {
            TicketSearchItemModel selectedTicket = new TicketSearchItemModel()
            {
                Balance = 35000,
                EmployeeId = 1,
                TableNumber = 4,
                TicketNumber = "1234567",
                GuestName = "John",
                Reference = "Jack Welsh",
                GuestAccCount = 5,
                PurchaseTotal = 25000,
                Stage = TicketStage.Opened
            };

            DialogBuilder.Show(IPayDialog.CONFIRM_CLOSING_TABLE_DIALOG, StringIds.STRING_NOTIFICATION, (iResult, args) =>
            {
                //ConfirmClosingTableDialog
            }, true, false, selectedTicket);
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
                OriginalCreatedBy = DateTimeOffset.Now,
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
                    AuthorizationExpiryDate = DateTime.Now,
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
                        IsActualTitle = true,
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
                //GuideTitleId = StringIds.STRING_PLACE_QR_CODE_INSIDE_THE_SCAN_AREA,
                ScanTitleId = StringIds.STRING_SCAN_QR_CODE,
            };

            IdDlgTitle = StringIds.STRING_REFUND_PURCHASE;

            dlgData.AboveScanViewTitleId = StringIds.STRING_FIND_PURCHASE;
            //dlgData.ManualIconId = 

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
    }
}