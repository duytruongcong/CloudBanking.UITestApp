using CloudBanking.BaseControl;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
using CloudBanking.Utilities;

namespace CloudBanking.UITestApp
{
    public partial class TestActivity : BaseActivity
    {
        void ShowMessageDialogInvalidAmount(CaseDialog caseDialog)
        {
            //MessageDialog
#if true
            var lMinAmount = 2000;
            var lMaxAmount = 100000;
            var msg = $"{Localize.GetString(StringIds.STRING_REQUIRED_AMOUNT_RANGE_COLON).ToUpperInvariant()}\n {lMinAmount.ToFormatLocalCurrencyAmount()} - {lMaxAmount.ToFormatLocalCurrencyAmount()}";

            ApplicationBaseFlow applicationBaseFlow = new ApplicationBaseFlow();

            switch (caseDialog)
            {
                case CaseDialog.CASE1:
                    applicationBaseFlow.ErrorMessage(StringIds.STRING_ERROR, Localize.GetString(StringIds.STRING_INVALID_AMOUNT), msg, GlobalResource.MB_RETRYCANCEL);
                    break;
            }
#endif
        }

        void ShowMessageDialogVoidTransaction()
        {
#if true
            var mainResult = StringIds.STRING_APPROVED.GetUpperCaseString();
            var fCustomerPrint = true;

            var secondaryRes = Localize.GetString( fCustomerPrint ? StringIds.STRING_PRINTCUSTOMERRECEIPT : StringIds.STRING_PRINTMERCHANTRECEIPT).ToUpperInvariant();

            ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_VOID_TRANSACTION, mainResult, false,
                GlobalResource.MB_OKCANCEL, GlobalResource.MB_ICONAPPROVAL_BMP,
                subMsg: secondaryRes,
                strSubMessageColor: GlobalConstants.STRING_APPROVAL_COLOR,
                aboveMsg: StringIds.STRING_VOIDTRANSACTION,
                isShowBackBtn: false,
                //subMsg: lamount > 0 ? lamount.ToFormatLocalCurrencyAmount() : string.Empty,
                textLeftButton: StringIds.STRING_NO_RECEIPT,
                textRightButton: StringIds.STRING_PRINT_RECEIPT);
#endif
        }

        void ShowMessageDialogGiftCardBalance()
        {
#if true

            var mainTitle = StringIds.STRING_BALANCE_ENQUIRY;
            var mainResult = string.Empty;
            mainResult = StringIds.STRING_APPROVED.GetUpperCaseString();
            var lBalance = 38000;
            var fCustomerPrint = true;

            var secondaryRes = fCustomerPrint ? StringIds.STRING_PRINTCUSTOMERRECEIPT : StringIds.STRING_PRINTMERCHANTRECEIPT;

            ApplicationBaseFlow.CustomStringMessageBox(true, mainTitle, mainResult, false,
                GlobalResource.MB_OKCANCEL, GlobalResource.MB_ICONAPPROVAL_BMP,
                aboveMsg: mainTitle,
                subMsg: Localize.GetString(secondaryRes).ToUpperInvariant(), fSubActualText: false,
                strSubMessageColor: GlobalConstants.STRING_APPROVAL_COLOR,
                thirdbMsg: lBalance > 0 ? lBalance.ToFormatLocalCurrencyAmount() : string.Empty, fThirdActualText: false,
                thirdbMsgColor: GlobalConstants.STRING_APPROVAL_COLOR,
                iThirdMessageTextSize: 100,
                textLeftButton: StringIds.STRING_NO_RECEIPT,
                textRightButton: StringIds.STRING_PRINT_RECEIPT);
#endif
        }

        void ShowMessageDialogGiftCardSale()
        {
#if true

            var mainResult = StringIds.STRING_APPROVED.GetUpperCaseString();
            var fCustomerPrint = true;
            var mainTitle = StringIds.STRING_GIFT_CARD_SALE;
            var lamount = 38000;

            var secondaryRes = fCustomerPrint ? StringIds.STRING_PRINTCUSTOMERRECEIPT : StringIds.STRING_PRINTMERCHANTRECEIPT;

            ApplicationBaseFlow.CustomStringMessageBox(true, mainTitle, mainResult, false,
                GlobalResource.MB_OKCANCEL, GlobalResource.MB_ICONAPPROVAL_BMP,
                aboveMsg: mainTitle,
                subMsg: Localize.GetString(secondaryRes).ToUpperInvariant(), fSubActualText: false,
                strSubMessageColor: GlobalConstants.STRING_APPROVAL_COLOR,
                thirdbMsg: lamount > 0 ? lamount.ToFormatLocalCurrencyAmount() : string.Empty, fThirdActualText: false,
                thirdbMsgColor: GlobalConstants.STRING_APPROVAL_COLOR,
                iThirdMessageTextSize: 100,
                isShowBackBtn: false,
                textLeftButton: StringIds.STRING_NO_RECEIPT,
                textRightButton: StringIds.STRING_PRINT_RECEIPT);
#endif
        }

        void ShowMessageDialogGiftRedeem()
        {
#if true

            var lTotalAmount = 38000;
            var fCustomerPrint = true;
            var mainResult = StringIds.STRING_APPROVED.GetUpperCaseString();

            var secondaryRes = fCustomerPrint ? StringIds.STRING_PRINTCUSTOMERRECEIPT : StringIds.STRING_PRINTMERCHANTRECEIPT;

            ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_GIFT_REDEEM, mainResult, false,
                GlobalResource.MB_OKCANCEL, GlobalResource.MB_ICONAPPROVAL_BMP,
                aboveMsg: StringIds.STRING_GIFT_REDEEM,
                subMsg: Localize.GetString(secondaryRes).ToUpperInvariant(), fSubActualText: false,
                strSubMessageColor: GlobalConstants.STRING_APPROVAL_COLOR,
                thirdbMsg: lTotalAmount > 0 ? lTotalAmount.ToFormatLocalCurrencyAmount() : string.Empty, fThirdActualText: false,
                thirdbMsgColor: GlobalConstants.STRING_APPROVAL_COLOR,
                iThirdMessageTextSize: 100,
                isShowBackBtn: false,
                textLeftButton: StringIds.STRING_NO_RECEIPT,
                textRightButton: StringIds.STRING_PRINT_RECEIPT);
#endif
        }
    }
}