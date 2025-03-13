using CloudBanking.BaseControl;
using CloudBanking.Common;
using CloudBanking.Entities;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
using CloudBanking.ShellContainers;
using CloudBanking.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using static Java.Util.Concurrent.Flow;
using Command = CloudBanking.Flow.Base.Command;

namespace CloudBanking.UITestApp
{
    public partial class TestActivity : BaseActivity
    {
        void ShowMessageDialogInvalidAmount(CaseDialog caseDialog)
        {
            //MessageDialog
#if false
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
#if false
            var mainResult = StringIds.STRING_APPROVED.GetUpperCaseString();
            var fCustomerPrint = true;

            var secondaryRes = Localize.GetString(fCustomerPrint ? StringIds.STRING_PRINTCUSTOMERRECEIPT : StringIds.STRING_PRINTMERCHANTRECEIPT).ToUpperInvariant();

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

        void ShowMiniDonationEnterAmountDialog()
        {
#if false
            DialogBuilder.Show(IPayDialog.MINI_DONATION_ENTER_AMOUNT_DIALOG, null, (iDialogResult, args) =>
            {
                
            }, true, false,  null);
#endif
        }

        void ShowEnterPlanTextPinDialog()
        {
#if true
            long lamount = 13800;

            DialogBuilder.Show(IPayDialog.PLAIN_TEXT_ENTER_PIN_DIALOG, StringIds.STRING_GIFT_CARD_PIN, (result, args) =>
            {
                //EnterPlanTextPinDialog
            }, true, false, lamount);
#endif
        }

        void ShowSoftKeyboardInputDialog()
        {
#if true
            DialogBuilder.Show(IPayDialog.SOFT_KEYBOARD_INPUT_DIALOG, StringIds.STRING_WALLETID, (result, args) =>
            {
                //SoftKeyboardInputDialog
            }, true, false, StringIds.STRING_CARD_DATA);
#endif
        }

        void ShowManualEntryCardNumberDialog()
        {
#if true
            var entryDlgData = new EntryCardNumberDlgData();

            entryDlgData.fShowExpiry = true;

            entryDlgData.fNonCreditCards = false;

            entryDlgData.fOffline = false;

            entryDlgData.fShowTokenFlag = false;

            entryDlgData.szPAN = "1234";

            entryDlgData.fShowBackButton = true;

            entryDlgData.fAnyProcessorsUsingTokens = false;

            entryDlgData.fAlphaNumKeyboard = false;

            entryDlgData.fValidating = false;

            entryDlgData.iMinLength = 11;

            DialogBuilder.Show(IShellDialog.MANUAL_PAY_ENTRY_CARD_NUMBER_DIALOG, StringIds.STRING_CARD_MANUAL_ENTRY, (iResult, args) =>
            {
                //EntryCardNumberDialog
            }, true, false, entryDlgData);
#endif
        }

        void ShowEzipayTransactions()
        {
#if true
            var selectFuncDialogDta = new SelFncDlgData()
            {
                iPage = 0,
                iMaxPage = 4,
                iMinPage = 1,
                pIdProcessor = 0,
                fShowLogout = false,
                fGrid = false,
                fModeDisplay = true,
                strListTitle = GlobalConstants.EZIPAY_PROCESSOR_NAME,
                FunctionButtons = new List<SelectButton>()
                {
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_GIFT_CARD_SALE,
                        idImage = IconIds.VECTOR_GIFT_CARD,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        Value = Command.FLOWCOMMAND_GIFT_CARD_SALE,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_VOUCHER_SALE,
                        idImage = IconIds.VECTOR_GIFT_CARD,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        Value = Command.FLOWCOMMAND_GIFT_VOUCHER_SALE,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_GIFT_CARD_BALANCE_ENQUIRY,
                        idImage = IconIds.VECTOR_GIFT_CARD,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        Value = Command.FLOWCOMMAND_GIFT_CARD_BALANCE_ENQUIRY,
                    },
                    new SelectButton()
                    {
                        iCommandLang =   StringIds.STRING_GS_WALLET_DIGITAL_PURCHASE,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        idImage = IconIds.VECTOR_GIFT_CARD,
                        Value = Command.FLOWCOMMAND_GIFT_DIGITAL_PURCHASE,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_RESERVE_FUNDS,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        idImage = IconIds.ICON_RESERVE_FUNDS,
                        Value = Command.FLOWCOMMAND_GIFT_PREAUTH,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_REDEEM_FUNDS,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        idImage = IconIds.ICON_REDEEM_FUNDS,
                        Value = Command.FLOWCOMMAND_EZIPAY_PREAUTH_COMPLETE,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_MANAGER,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        idImage = IconIds.ICON_EPAY_MANAGER,
                        Value = Command.FLOWCOMMAND_EZIPAY_MANAGER,
                    },
                }
            };

            DialogBuilder.Show(IPayDialog.MENU_DIALOG, StringIds.STRING_TRANSACTION, (iResult, args) =>
            {


            }, true, false, selectFuncDialogDta);
#endif
        }

        void ShowEzipayManagers()
        {
#if true
            var selectFuncDialogDta = new SelFncDlgData()
            {
                iPage = 0,
                iMaxPage = 4,
                iMinPage = 1,
                pIdProcessor = 0,
                fShowLogout = false,
                fGrid = false,
                fModeDisplay = true,
                strListTitle = StringIds.STRING_MANAGER_OPTIONS,
                FunctionButtons = new List<SelectButton>()
                {
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_LOGON_RESET,
                        idImage = IconIds.VECTOR_LOGON,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        Value = Command.FLOWCOMMAND_GIFT_LOGON,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_SETTLEMENT_INQUIRY,
                        idImage = IconIds.VECTOR_SETTLEMENT,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        Value = Command.FLOWCOMMAND_GIFT_SETTLEMENT,
                    },
                    new SelectButton()
                    {
                        iCommandLang = StringIds.STRING_VOID_TRANSACTION,
                        idImage = IconIds.ICON_VOID_TRANSACTION,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        Value = Command.FLOWCOMMAND_GIFT_MANUAL_CANCEL,
                    },
                    new SelectButton()
                    {
                        iCommandLang =   StringIds.STRING_SETUP,
                        iCommand = GlobalResource.SELECT_BUTTON,
                        idImage = IconIds.VECTOR_SETUP,
                        Value = Command.FLOWCOMMAND_GIFT_ADMIN_SETUP,
                    }
                }
            };

            DialogBuilder.Show(IPayDialog.MENU_DIALOG, StringIds.STRING_TRANSACTION, (iResult, args) =>
            {
              

            }, true, false, selectFuncDialogDta);
#endif
        }

        void ShowSelectProcessorDialog()
        {
            List<GenericType> _processorIdList = new List<GenericType>();

            _processorIdList.Add(new GenericType
            {
                Id = 1,
                iType = 1,
                lszText = "Payplus Processor",
                Icon = IconIds.VECTOR_PROCESSOR
            });

            _processorIdList.Add(new GenericType
            {
                Id = 1,
                iType = 1,
                lszText = "Payplus Processor",
                Icon = IconIds.VECTOR_PROCESSOR
            });

            _processorIdList.Add(new GenericType
            {
                Id = 1,
                iType = 1,
                lszText = "Payplus Processor",
                Icon = IconIds.VECTOR_PROCESSOR
            });

            _processorIdList.Add(new GenericType
            {
                Id = 1,
                iType = 1,
                lszText = "Payplus Processor",
                Icon = IconIds.VECTOR_PROCESSOR
            });

            var result = DialogBuilder.Show(IShellDialog.SELECT_PROCESSOR_DIALOG, StringIds.STRING_PROCESSOR_OPTIONS, (iResult, args) =>
            {

                //SelectProcessorDialog
            }, true, false, _processorIdList);
        }
    }
}