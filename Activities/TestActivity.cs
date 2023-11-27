using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CloudBanking.BaseControl;
using CloudBanking.ServiceLocators;
using System;
using System.Collections.Generic;

namespace CloudBanking.UITestApp
{
    [Activity(MainLauncher = true, LaunchMode = LaunchMode.SingleTask, NoHistory = false, Theme = "@style/WindowBackgroundTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public partial class TestActivity : BaseActivity
    {
        ListView list_view;
        TestItemAdapter _adapter;
        IList<ScreenViewModel> _lData;
        IDialogBuilder DialogBuilder => ServiceLocator.Instance.Get<IDialogBuilder>();

        string[] PERMISSIONS = new string[]{
                Android.Manifest.Permission.ReadExternalStorage,
                Android.Manifest.Permission.WriteExternalStorage,

                };

        const int PERMISSION_ALL = 100;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentNavigation);
                Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                Window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);

                Window.SetStatusBarColor(this.Resources.GetColor(Resource.Color.setup_status_bar_color));
                Window.SetNavigationBarColor(this.Resources.GetColor(Resource.Color.setup_status_bar_color));

                Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

                Window.DecorView.SystemUiVisibility =
                (StatusBarVisibility)(SystemUiFlags.HideNavigation |
                                 SystemUiFlags.ImmersiveSticky |
                                 SystemUiFlags.Fullscreen);
            }

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TestLayout);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!CheckPermission(PERMISSIONS))
                {
                    RequestPermission(PERMISSIONS, PERMISSION_ALL);
                }
                else
                {
                    StartApplication();
                }
            }
            else
            {
                StartApplication();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == PERMISSION_ALL)
            {
                if (grantResults.Length > 0 && grantResults[0] == Android.Content.PM.Permission.Granted)
                {
                    StartApplication();
                }
                else
                {
                    Toast.MakeText(this, "Application cannot get permission", ToastLength.Long).Show();
                    Finish();
                }
            }
        }

        void StartApplication()
        {
            list_view = FindViewById<ListView>(Resource.Id.list_view);

            _lData = new List<ScreenViewModel>();

            InitializeData();

            _adapter = new TestItemAdapter(this, _lData);
            list_view.Adapter = _adapter;

            list_view.ItemClick += (sender, e) =>
            {
                EventButtonClick(e.Position);
            };
        }

        private bool CheckPermission(string[] permissions)
        {
            foreach (var item in permissions)
            {
                Android.Content.PM.Permission result = ContextCompat.CheckSelfPermission(this, item);
                if (result != Android.Content.PM.Permission.Granted) return false;
            }

            return true;
        }

        private void RequestPermission(string[] permission, int code)
        {
            ActivityCompat.RequestPermissions(this, permission, code);
        }

        private void InitializeData()
        {

            #region AddNew

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ReceiptOptionsDialog",
                RightIconResName = "ReceiptOptionsDialog",
                ItemAction = new Action(() =>
                {
                    ShowReceiptOptionDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"AdvertisingDialog",
                RightIconResName = "AdvertisingDialog",
                ItemAction = new Action(() =>
                {
                    ShowAdvertisingDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"AdjustDonationDialog",
                RightIconResName = "AdjustDonationDialog",
                ItemAction = new Action(() =>
                {
                    ShowAdjustDonationDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectDonationDialog",
                RightIconResName = "select_donation",
                ItemAction = new Action(() =>
                {
                    ShowSelectDonationDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectMerchantDialog",
                RightIconResName = "SelectMerchantDialog",
                ItemAction = new Action(() =>
                {
                    ShowSelectMerchantDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"GetAmountCashOutDialog",
                RightIconResName = "GetAmountCashOutDialog",
                ItemAction = new Action(() =>
                {
                    ShowGetAmountCashOutDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SingleUserLoginDialog",
                RightIconResName = "SingleUserLoginDialog",
                ItemAction = new Action(() =>
                {
                    ShowSingleUserLoginDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"LogonDialog_1",
                RightIconResName = "logon_dialog_1",
                ItemAction = new Action(() =>
                {
                    ShowLogonDialogCase01();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"LogonDialog_2",
                RightIconResName = "logon_dialog_2",
                ItemAction = new Action(() =>
                {
                    ShowLogonDialogCase02();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthCompletePreAuthInfoDialog",
                RightIconResName = "PreAuthCompletePreAuthInfoDialog",
                ItemAction = new Action(() =>
                {
                    ShowPreAuthCompletePreAuthInfoDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CustomerDisplayGetCashOutDialog",
                RightIconResName = "CustomerDisplayGetCashOutDialog",
                ItemAction = new Action(() =>
                {
                    ShowCustomerDisplayGetCashOutDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"GetAmountDialog",
                RightIconResName = "GetAmountDialog",
                ItemAction = new Action(() =>
                {
                    ShowGetAmountDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectTipLayout",
                RightIconResName = "select_tip",
                ItemAction = new Action(() =>
                {
                    ShowSelectTipDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ListPaymentDialog",
                RightIconResName = "ListPaymentDialog",
                ItemAction = new Action(() =>
                {
                    ListPaymentRecordDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterTipAmount",
                RightIconResName = null,
                ItemAction = new Action(() =>
                {
                    ShowEnterTipAmountDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"AuthenticatingDialog",
                RightIconResName = "AuthenticatingDialog",
                ItemAction = new Action(() =>
                {
                    ShowAuthenticatingDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EmailReceiptSendResultDialogSuccess",
                RightIconResName = "EmailReceiptSendResultDialogSuccess",
                ItemAction = new Action(() =>
                {
                    ShowEmailReceiptSendResultDialogSuccess();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EmailReceiptSendResultDialogFail",
                RightIconResName = "EmailReceiptSendResultDialogFail",
                ItemAction = new Action(() =>
                {
                    ShowEmailReceiptSendResultDialogFail();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ConfirmServiceDialog",
                RightIconResName = "confirm_service_dialog",
                ItemAction = new Action(() =>
                {
                    ShowConfirmServiceDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ConfirmSurveyDialog",
                RightIconResName = "confirm_survey_dialog",
                ItemAction = new Action(() =>
                {
                    ShowConfirmSurveyDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"BasketItemReviewDialog",
                RightIconResName = "BasketItemReviewDialog",
                ItemAction = new Action(() =>
                {
                    ShowBasketItemReviewDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CustomerAuthenticateOptions",
                RightIconResName = "CustomerAuthenticateOptions",
                ItemAction = new Action(() =>
                {
                    CustomerAuthenticateOptions();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SurchargeConfirmDialog",
                RightIconResName = "surcharge_confirm",
                ItemAction = new Action(() =>
                {
                    ShowSurchargeConfirmDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DigitalSignatureDialog",
                RightIconResName = "DigitalSignatureDialog",
                ItemAction = new Action(() =>
                {
                    DigitalSignature();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SignOrPinDialog",
                RightIconResName = "SignOrPinDialog",
                ItemAction = new Action(() =>
                {
                    SignOrPin();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"BasketItemSelectOfferDialog",
                RightIconResName = "BasketItemSelectOfferDialog",
                ItemAction = new Action(() =>
                {
                    BasketItemSelectOffer();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ListPaymentDialog",
                RightIconResName = "list_payment_dialog",
                ItemAction = new Action(() =>
                {
                    ShowListPaymentDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthItemGetNewAmount",
                RightIconResName = "preauth_item_get_new_amount",
                ItemAction = new Action(() =>
                {
                    PreAuthItemGetNewAmount();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MainDialog",
                RightIconResName = "MainDialog_land",
                ItemAction = new Action(() =>
                {
                    ShowMainDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectMoto",
                RightIconResName = "select_moto",
                ItemAction = new Action(() =>
                {
                    ShowSelectMoto();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectDate",
                RightIconResName = "select_date",
                ItemAction = new Action(() =>
                {
                    ShowSelectDate();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"Settlement Options",
                RightIconResName = "settlement_select_options",
                ItemAction = new Action(() =>
                {
                    ShowSettlementOptions();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"Reprint Options",
                RightIconResName = "reprint_options",
                ItemAction = new Action(() =>
                {
                    ShowReprintOptions();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthEnterAmountDialog",
                RightIconResName = "preauth_enter_amount_dialog",
                ItemAction = new Action(() =>
                {
                    ShowPreAuthEnterAmountDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowSettlementGetDateDialog",
                RightIconResName = "settlement_get_date_dialog",
                ItemAction = new Action(() =>
                {
                    ShowSettlementGetDateDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"FindPurchaseOptionDialog",
                RightIconResName = "find_purchase_option",
                ItemAction = new Action(() =>
                {
                    ShowSearchOptions();
                })
            });
            #endregion

            #region RequestCardDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE1",
                RightIconResName = "RequestCardDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE2",
                RightIconResName = "RequestCardDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE3",
                RightIconResName = "RequestCardDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE4",
                RightIconResName = "RequestCardDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE5",
                RightIconResName = "RequestCardDialog_CASE5",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE6",
                RightIconResName = "RequestCardDialog_CASE6",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE7",
                RightIconResName = "RequestCardDialog_CASE7",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE8",
                RightIconResName = "RequestCardDialog_CASE8",
                ItemAction = new Action(() =>
                {
                    ShowRequestCardDialog(CaseDialog.CASE8);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CustomerDisplayRequestCardDialog",
                RightIconResName = "CustomerDisplayRequestCardDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusDisplayRequestCardDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MerchantSwipeCardDialog",
                RightIconResName = "MerchantSwipeCardDialog",
                ItemAction = new Action(() =>
                {
                    ShowSwipeMerchantCardDialog();
                })
            });

            #endregion

            #region SelectAccountTypeDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"DynamicOptionDialog CASE1",
                RightIconResName = "DynamicOptionDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowDynamicOptionDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DynamicOptionDialog CASE2",
                RightIconResName = "DynamicOptionDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowDynamicOptionDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DynamicOptionDialog CASE3",
                RightIconResName = "DynamicOptionDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowDynamicOptionDialog(CaseDialog.CASE3);
                })
            });
            #endregion

            #region MessageDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowMessageDialog CASE1",
                RightIconResName = "ShowMessageDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowMessageDialog CASE2",
                RightIconResName = "ShowMessageDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowMessageDialog CASE3",
                RightIconResName = "ShowMessageDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowMessageDialog CASE4",
                RightIconResName = "ShowMessageDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialog(CaseDialog.CASE4);
                })
            });
            #endregion

            #region PresentCardErrorDlg
            _lData.Add(new ScreenViewModel()
            {
                Title = $"PresentCardErrorDlg CASE1",
                RightIconResName = "PresentCardErrorDlg_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowPresentCardErrorDlg(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PresentCardErrorDlg CASE2",
                RightIconResName = "PresentCardErrorDlg_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowPresentCardErrorDlg(CaseDialog.CASE2);
                })
            });
            #endregion

            #region EnterPinDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE1",
                RightIconResName = "EnterPinDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowEnterPinDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE2",
                RightIconResName = "EnterPinDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowEnterPinDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE3",
                RightIconResName = "EnterPinDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowEnterPinDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE4",
                RightIconResName = "EnterPinDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowEnterPinDialog(CaseDialog.CASE4);
                })
            });
            #endregion

            #region ApprovalDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE1",
                RightIconResName = "ApprovalDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE2",
                RightIconResName = "ApprovalDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE3",
                RightIconResName = "ApprovalDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE4",
                RightIconResName = "ApprovalDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE5",
                RightIconResName = "ApprovalDialog_CASE5",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE6",
                RightIconResName = "ApprovalDialog_CASE6",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE7",
                RightIconResName = "ApprovalDialog_CASE7",
                ItemAction = new Action(() =>
                {
                    ShowApprovalDialog(CaseDialog.CASE7);
                })
            });
            #endregion

            #region SettlementApprovalDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SettlementApprovalDialog CASE1",
                RightIconResName = "SettlementApprovalDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowSettlementApprovalDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SettlementApprovalDialog CASE2",
                RightIconResName = "SettlementApprovalDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowSettlementApprovalDialog(CaseDialog.CASE2);
                })
            });
            #endregion

            #region MOTO FLOW
            _lData.Add(new ScreenViewModel()
            {
                Title = $"EntryCardNumberDialog",
                RightIconResName = "EntryCardNumberDialog",
                ItemAction = new Action(() =>
                {
                    ShowEntryCardNumberDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EntryExpiryDateDialog",
                RightIconResName = "EntryExpiryDateDialog",
                ItemAction = new Action(() =>
                {
                    ShowEntryExpiryDateDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EntryCVVDialog",
                RightIconResName = "EntryCVVDialog",
                ItemAction = new Action(() =>
                {
                    ShowEntryCVVDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog",
                RightIconResName = "StandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    ShowStandardSetupDialog();
                })
            });
            #endregion

            #region ProcessMessageDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"ProcessMessageDialog CASE1",
                RightIconResName = "ProcessMessageDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowProcessMessageDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ProcessMessageDialog CASE2",
                RightIconResName = "ProcessMessageDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowProcessMessageDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ProcessMessageDialog CASE3",
                RightIconResName = "ProcessMessageDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowProcessMessageDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ProcessMessageDialog CASE4",
                RightIconResName = "ProcessMessageDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowProcessMessageDialog(CaseDialog.CASE4);
                })
            });
            #endregion

            #region EOVProcessMessageDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"EOVProcessMessageDialog",
                RightIconResName = "EOVProcessDialog",
                ItemAction = new Action(() =>
                {
                    ShowEOVProcessingDialog();
                })
            });
            #endregion

            #region CancelPreAuthConfirmDialog
            _lData.Add(new ScreenViewModel()
            {
                Title = $"CancelPreAuthConfirmDialog",
                RightIconResName = "PreAuthCancelDialog",
                ItemAction = new Action(() =>
                {
                    ShowCancelPreAuthConfirmDialog();
                })
            });
            #endregion
        }

        private void EventButtonClick(int position)
        {
            _lData[position].ItemAction.Invoke();
        }

        private enum CaseDialog
        {
            CASE1,
            CASE2,
            CASE3,
            CASE4,
            CASE5,
            CASE6,
            CASE7,
            CASE8
        }

    }
}
