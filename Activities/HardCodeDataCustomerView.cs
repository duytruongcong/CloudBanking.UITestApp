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
            RequestDlgData.fShowCardFees = true;
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

            var requestCardDialog = new RequestCardDialog(StringIds.STRING_PAYMENT_METHODS, (iResult, args) =>
            {
            }, RequestDlgData);

            requestCardDialog.DialogStyle = DialogStyle.FULLSCREEN;
            requestCardDialog.Show(this);
        }
    }
}