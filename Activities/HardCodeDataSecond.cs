using CloudBanking.BaseControl;
using CloudBanking.Common;
using CloudBanking.Entities;
using CloudBanking.Flow.Base;
using CloudBanking.Language;
using CloudBanking.ShellContainers;
using CloudBanking.Utilities;
using Java.Util;
using System;
using System.Collections.Generic;
using static CloudBanking.Utilities.UtilEnum;
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
            var mainResult = StringIds.STRING_APPROVED.GetString();
            var fCustomerPrint = true;

            var secondaryRes = Localize.GetString(fCustomerPrint ? StringIds.STRING_PRINTCUSTOMERRECEIPT : StringIds.STRING_PRINTMERCHANTRECEIPT);

            ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_VOID_TITLE, mainResult, false,
                GlobalResource.MB_OKCANCEL, GlobalResource.MB_ICONAPPROVAL_BMP,
                subMsg: secondaryRes,
                strSubMessageColor: GlobalConstants.STRING_PRIMARY_COLOR,
                aboveMsg: StringIds.STRING_VOIDTRANSACTION,
                isShowBackBtn: false,
                //subMsg: lamount > 0 ? lamount.ToFormatLocalCurrencyAmount() : string.Empty,
                textLeftButton: StringIds.STRING_NO_RECEIPT,
                textRightButton: StringIds.STRING_PRINT_RECEIPT);
#endif
        }

        void ShowMessageDialogGiftCardBalance()
        {
#if false

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
#if false

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
#if false

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
#if false
            long lamount = 13800;

            DialogBuilder.Show(IPayDialog.PLAIN_TEXT_ENTER_PIN_DIALOG, StringIds.STRING_GIFT_CARD_PIN, (result, args) =>
            {
                //EnterPlanTextPinDialog
            }, true, false, lamount);
#endif
        }

        void ShowSoftKeyboardInputDialog()
        {
#if false
            DialogBuilder.Show(IPayDialog.SOFT_KEYBOARD_INPUT_DIALOG, StringIds.STRING_WALLETID, (result, args) =>
            {
                //SoftKeyboardInputDialog
            }, true, false, StringIds.STRING_CARD_DATA);
#endif
        }

        void ShowManualEntryCardNumberDialog()
        {
#if false
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

            DialogBuilder.Show(IPayDialog.MANUAL_PAY_ENTRY_CARD_NUMBER_DIALOG, StringIds.STRING_CARD_MANUAL_ENTRY, (iResult, args) =>
            {
                //EntryCardNumberDialog
            }, true, false, entryDlgData);
#endif
        }

        void ShowEzipayTransactions()
        {
#if false
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
#if false
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
#if true
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

            var result = DialogBuilder.Show(IShellDialog.SELECT_PROCESSOR_DIALOG, StringIds.STRING_SELECTPROCESSOR, (iResult, args) =>
            {

                //SelectProcessorDialog
            }, true, false, _processorIdList);
#endif
        }

        void ShowRemoveSurchargeDialog()
        {
#if false
            SurchargeConfirmationDlgData dlgSurchargeConfirmData = new SurchargeConfirmationDlgData();

            dlgSurchargeConfirmData.pInitProcessData = new ShellInitProcessData() 
            { 
                lSurChargeFee = 6800,
                lAccountSurChargeFee = 3000,
                lCashOutFee = 7800
            };

            dlgSurchargeConfirmData.iCardType = CARDTYPE.CARD_AMEX;

            dlgSurchargeConfirmData.wszCurrencyCode = GlobalData.GlobalCurrency.wszCurrencyCode;

            dlgSurchargeConfirmData.iAccountTypeCode = AccountType.ACCOUNT_TYPE_UNKNOWN;

            dlgSurchargeConfirmData.szCardHolderName = "Mr. Smith";

            dlgSurchargeConfirmData.szCardNumber = "*****8765";

            dlgSurchargeConfirmData.fCTLS = false;

            var ret = DialogBuilder.Show(IShellDialog.REMOVE_SURCHARGE_DIALOG, StringIds.STRING_REMOVE_SURCHARGE, (iResult, args) =>
            {
              
            }, true, false, dlgSurchargeConfirmData);
#endif
        }

        void ShowAddCustomerDialog()
        {
#if false
            DialogBuilder.ShowSubFlowDialog(IPayDialog.ADD_CUSTOMER_DIALOG,
                                StringIds.STRING_NEW_CUSTOMER,
                                (result, args) =>
                                {

                                });
#endif
        }

        void ShowSelectCustomerDialog()
        {
#if false

            List<Customer> customerList = new List<Customer>();

            customerList.Add(new Customer()
            {
                FirstName = "Bob",
                LastName = "Smith"
            });

            customerList.Add(new Customer()
            {
                FirstName = "Bob",
                LastName = "Smith"
            });

            customerList.Add(new Customer()
            {
                FirstName = "Bob",
                LastName = "Smith"
            });

            customerList.Add(new Customer()
            {
                FirstName = "Bob",
                LastName = "Smith"
            });


            var dlgData = new SelectCustomerDlgData()
            {
                ListCustomers = customerList,
                strListTitle = StringIds.STRING_RECENTLY_ADDED
            };

            DialogBuilder.ShowSubFlowDialog(IPayDialog.SELECT_CUSTOMER_DIALOG,
                StringIds.STRING_CUSTOMER,
                (result, args) =>
                {

                }, dlgData);
#endif

        }

        void ShowReviewCustomerDialog()
        {
#if false
            var customer = new Customer();
            customer.FirstName = "Bob";
            customer.LastName = "Smith";
            customer.Email = "bob@gmail.com";
            customer.PhoneNumber = "1234567890";
            customer.Address = "456 Landy street";
            customer.Birthday = DateTime.Now;
            customer.CreatedAt = DateTime.Now;

            DialogBuilder.ShowSubFlowDialog(IPayDialog.REVIEW_CUSTOMER_DIALOG,
                                    StringIds.STRING_CUSTOMER,
                                    (result, args) =>
                                    {

                                    }, customer);
#endif
        }

        void ShowAddNoteDialog()
        {
#if false

            var addNoteDlgData = new AddNoteDlgData()
            {
                CurrentNote = "Please note here!",
                Amount = 13800
            };

            DialogBuilder.ShowSubFlowDialog(IPayDialog.ADD_NOTE_DIALOG,
                StringIds.STRING_ADD_NOTE,
                (result, args) =>
                {

                }, addNoteDlgData);
#endif
        }

        void ShowSendReceiptCustomerDialog()
        {
#if false
            var customer = new Customer();
            customer.FirstName = "Bob";
            customer.LastName = "Smith";
            customer.Email = "bob@gmail.com";
            customer.PhoneNumber = "1234567890";
            customer.Address = "456 Landy street";
            customer.Birthday = DateTime.Now;
            customer.CreatedAt = DateTime.Now;

            DialogBuilder.Show(IPayDialog.SEND_RECEIPT_CUSTOMER_DIALOG, StringIds.STRING_RECEIPT, (iResult, args) =>
            {
               
            }, true, false, customer);
#endif
        }

        void ShowReprintMessageBox01()
        {
            ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_REPRINT_TITLE, (false) ? StringIds.STRING_REPRINTED_SUCCESSFULLY_UPCASE : StringIds.STRING_REPORT_PRINTED_FAILED, false, GlobalResource.MB_OK, (false) ? GlobalResource.MB_ICONAPPROVAL_BMP : GlobalResource.MB_ICONDECLINED_BMP, aboveMsg: StringIds.STRING_REPRINT_RECEIPT);
        }

        void ShowReprintMessageBox02()
        {
            ApplicationBaseFlow.CustomStringMessageBox(true, StringIds.STRING_REPRINT_TITLE, (true) ? StringIds.STRING_REPRINTED_SUCCESSFULLY_UPCASE : StringIds.STRING_REPORT_PRINTED_FAILED, false, GlobalResource.MB_OK, (true) ? GlobalResource.MB_ICONAPPROVAL_BMP : GlobalResource.MB_ICONDECLINED_BMP, aboveMsg: StringIds.STRING_REPRINT_RECEIPT);
        }

        void ShowCommonSplitPayDialog()
        {
#if false
            ApplicationBaseFlow.GlobalInitProcessData = new InitProcessData();

            ApplicationBaseFlow.GlobalInitProcessData.iFunctionButton = GlobalResource.FNC_SALE_BUTTON;

            ApplicationBaseFlow.GlobalInitProcessData.SplitPayModel = new SplitModel()
            {
                TotalAmount = 10000,
            };
            ApplicationBaseFlow.GlobalInitProcessData.SplitPayModel.Items = new List<SplitItem>();
            ApplicationBaseFlow.GlobalInitProcessData.SplitPayModel.Items.Add(new SplitItem() 
            { 
                Id=1, 
                Amount=3000, 
                SplitId="1", 
                CardType = "Visa", 
                CustomerDetailsData = new CustomerDetailsData(),
                GuestName="David",
                GuestNumber=4,
                IsAdjusted=false,
                IsCustomerDetailsInitialized=false,
                IsMultipleTender=false,
                IsPaid=false,
                PosTicketId="1",
                Reference="4324",
                ReferenceType=3,
                TransactionNote="no action",
                PayItems=new List<PayItem>()
                {
                    new PayItem(){Amount = 1000, AuthCode="1234", CardHolderName="David Smith", CardNumber="1234"}
                }
            });

            GlobalData.CurrentNote = "Please turn on the app";

            DialogBuilder.Show(IPayDialog.COMMON_SPLIT_PAY_DIALOG, StringIds.STRING_SPLIT_PAY, (iResult, args) =>
            {
                if (iResult == GlobalResource.OK_BUTTON || iResult == GlobalResource.SELECT_BUTTON || iResult == GlobalResource.ADJUST_BUTTON || iResult == GlobalResource.MENU_BUTTON || iResult == GlobalResource.INFO_BUTTON)
                {
                }
            }, true, false, ApplicationBaseFlow.GlobalInitProcessData, SubFlow.Standalone);
#endif
        }

        void ShowSelectPaymentModeDialog()
        {
            string paymentModeName = "";
            uint paymentModeId = 0;
            uint merchantId = 1;

            List<PaymentMode> paymentModes = new List<PaymentMode>();

            PaymentMode standalone = new PaymentMode();
            standalone.IdParent = 0;
            standalone.Id = paymentModeId++;
            standalone.fPreCashOutFee = true;
            standalone.lszIcon = IconIds.VECTOR_STANDALONE_PAY;
            standalone.Title = "Standalone Payments";
            standalone.Sort = 0;
            standalone.Level = 0;
            standalone.MerchantId = merchantId;
            standalone.Enable = true;
            standalone.SubLevel = true;
            standalone.Security = false;
            standalone.Default = true;
            standalone.ModeOptions = new PaymentModeOption();
            standalone.FNCFlows = FNCFlows.StandalonePay;
            paymentModes.Add(standalone);

            PaymentMode posintegration = new PaymentMode();
            posintegration.IdParent = 0;
            posintegration.Id = paymentModeId++;
            posintegration.fPreCashOutFee = true;
            posintegration.lszIcon = IconIds.VECTOR_INTERGRATED_PAYMENTS;
            posintegration.Title = "Integrated Payments";
            posintegration.Sort = 0;
            posintegration.Level = 0;
            posintegration.MerchantId = merchantId;
            posintegration.Enable = true;
            posintegration.SubLevel = true;
            posintegration.Security = false;
            posintegration.Default = false;
            posintegration.ModeOptions = new PaymentModeOption();
            posintegration.FNCFlows = FNCFlows.ECRPay;
            paymentModes.Add(posintegration);

            PaymentMode ticketPay = new PaymentMode();
            ticketPay.IdParent = 0;
            ticketPay.Id = paymentModeId++;
            ticketPay.lszIcon = IconIds.VECTOR_TICKET_PAY;
            ticketPay.Title = "Ticket Pay";
            ticketPay.Sort = 0;
            ticketPay.Level = 0;
            ticketPay.MerchantId = merchantId;
            ticketPay.Enable = true;
            ticketPay.SubLevel = true;
            ticketPay.Security = false;
            ticketPay.Default = false;
            paymentModes.Add(ticketPay);

            PaymentMode tablePay = new PaymentMode();
            tablePay.IdParent = ticketPay.Id;
            tablePay.Id = paymentModeId++;
            tablePay.fPreCashOutFee = true;
            tablePay.lszIcon = IconIds.VECTOR_TABLE_PAY;
            tablePay.Title = "Table Pay";
            tablePay.Sort = 0;
            tablePay.Level = 0;
            tablePay.MerchantId = merchantId;
            tablePay.Enable = true;
            tablePay.SubLevel = true;
            tablePay.Security = false;
            tablePay.Default = false;
            tablePay.ModeOptions = new PaymentModeOption();
            tablePay.FNCFlows = FNCFlows.PayAtTable;
            paymentModes.Add(tablePay);

            PaymentMode openTab = new PaymentMode();
            openTab.IdParent = ticketPay.Id;
            openTab.Id = paymentModeId++;
            openTab.fPreCashOutFee = true;
            openTab.lszIcon = IconIds.VECTOR_OPEN_TAB;
            openTab.Title = "Open Tab";
            openTab.Sort = 0;
            openTab.Level = 0;
            openTab.MerchantId = merchantId;
            openTab.Enable = true;
            openTab.SubLevel = true;
            openTab.Security = false;
            openTab.Default = false;
            openTab.ModeOptions = new PaymentModeOption();
            paymentModes.Add(openTab);

            PaymentMode roomAccounts = new PaymentMode();
            roomAccounts.IdParent = ticketPay.Id;
            roomAccounts.Id = paymentModeId++;
            roomAccounts.fPreCashOutFee = true;
            roomAccounts.lszIcon = IconIds.VECTOR_ROOM_ACCOUNT;
            roomAccounts.Title = "Room Account";
            roomAccounts.Sort = 0;
            roomAccounts.Level = 0;
            roomAccounts.MerchantId = merchantId;
            roomAccounts.Enable = true;
            roomAccounts.SubLevel = true;
            roomAccounts.Security = false;
            roomAccounts.Default = false;
            roomAccounts.ModeOptions = new PaymentModeOption();
            paymentModes.Add(roomAccounts);


            DialogBuilder.Show(IPayDialog.SELECT_PAYMENT_MODE_DIALOG, StringIds.STRING_PAYMENT_MODES, (result, args) =>
            {

            }, true, false, paymentModes, paymentModeName);
        }

        private void AddGiftCardAction(SelFncDlgData dialogData, int flowCommand, string title)
        {
            dialogData.FunctionButtons.Add(new SelectButton
            {
                iCommand = flowCommand,
                iCommandLang = title,
                Title = title,
                idImage = IconIds.VECTOR_GIFT_CARD,
                IdProcessor = 0
            });
        }

        void ShowGiftCardActionOptions()
        {
            var dialogData = new SelFncDlgData
            {
                iPage = 1,
                iMinPage = 1,
                iMaxPage = 1,
                pIdProcessor = 0,
                pIdSecurityUser = 0,
                fShowLogout = false,
                fGrid = false,
                strListTitle = string.Empty,
                fShowBackBtn = true,
                fShowLeftMenuBtn = false,
                fShowTopListHeader = false
            };

            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_RECHARGE, StringIds.STRING_GIFT_CARD_RECHARGE);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_ACTIVATE_RECHARGE, StringIds.STRING_GIFT_CARD_ACTIVATE_RECHARGE);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_TRANSFER, StringIds.STRING_GIFT_CARD_TRANSFER);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_ACTIVATE, StringIds.STRING_GIFT_CARD_ACTIVATE);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_VOID_ACTIVATION, StringIds.STRING_GIFT_CARD_VOID_ACTIVATION);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_DEACTIVATE, StringIds.STRING_GIFT_CARD_DEACTIVATE);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_CANCEL, StringIds.STRING_GIFT_CARD_CANCEL);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_SUSPEND, StringIds.STRING_GIFT_CARD_SUSPEND);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_RESUME, StringIds.STRING_GIFT_CARD_RESUME);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_REDEEM, StringIds.STRING_GIFT_CARD_REDEEM);
            AddGiftCardAction(dialogData, Command.FLOWCOMMAND_GIFT_BALANCE, StringIds.STRING_GIFT_CARD_BALANCE);

            int localSelectedFlowCommand = 0;
            DialogBuilder.Show(IPayDialog.SELECT_FUNCTION_DIALOG, StringIds.STRING_ACTION_OPTIONS, (result, args) =>
            {
                if (result == GlobalResource.OK_BUTTON)
                    localSelectedFlowCommand = args.GetDataOfIndex<int>(0);
            }, true, false, dialogData);

        }
    }
}