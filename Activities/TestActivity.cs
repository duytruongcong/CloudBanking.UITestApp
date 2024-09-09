using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CloudBanking.BaseControl;
using CloudBanking.Entities;
using CloudBanking.ServiceLocators;
using CloudBanking.ShellContainers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudBanking.UITestApp
{
    [Activity(MainLauncher = true, LaunchMode = LaunchMode.SingleTask, NoHistory = false, Theme = "@style/WindowBackgroundTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public partial class TestActivity : BaseActivity, IGatewayDownloadCallbackListener
    {
        ListView list_view;
        TestItemAdapter _adapter;
        IList<ScreenViewModel> _lData;

        BaseDialog _baseDialog;
        private ApplicationFlow _paymentFlow => ((MainApplication)Application).PaymentFlow;


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

                //Window.SetStatusBarColor(this.GetThemeNavigationBarColor()); 
                //Window.SetNavigationBarColor(this.GetThemeNavigationBarColor());
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
            // Get the display metrics
            DisplayMetrics displayMetrics = new DisplayMetrics();

            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            // Access the device's metrics
            float density = displayMetrics.Density;
            int widthPixels = displayMetrics.WidthPixels;
            int heightPixels = displayMetrics.HeightPixels;
            float xdpi = displayMetrics.Xdpi;
            float ydpi = displayMetrics.Ydpi;

            list_view = FindViewById<ListView>(Resource.Id.list_view);

            _lData = new List<ScreenViewModel>();

            InitializeCommonData();

            _adapter = new TestItemAdapter(this, _lData);
            list_view.Adapter = _adapter;

            list_view.ItemClick += (sender, e) =>
            {
                EventButtonClick(e.Position);
            };

            //
            Task.Run((() =>
            {
                try
                {
                    ((MainApplication)Application).PaymentFlow = new ApplicationFlow();

                    ServiceLocator.Instance.Get<IShellServer>().SetApplicationFlow(_paymentFlow);

                    if (!_paymentFlow.IsInitialized && !_paymentFlow.IsInitializating)
                    {
                        _paymentFlow.IsInitializating = true;

                        _paymentFlow.InitGlobalRecords((m) =>
                        {

                        }, (sm) =>
                        {

                        }, this, true);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }));
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

        private void EventButtonClick(int position)
        {
            _lData[position].ItemAction.Invoke();
        }

        public void DownloadUpdating(int current, int total)
        {
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

        #region InitializeCusViewData
        private void InitializeCusViewData()
        {

            //handle later
            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"CusViewListPaymentDialog",
            //    RightIconResName = "CusViewListPaymentDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowCusViewListPaymentRecordDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewReceiptOptionDialog",
                RightIconResName = "CusViewReceiptOptionDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewReceiptOptionDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewSelectCharityDialog",
                RightIconResName = "CusViewSelectCharityDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewSelectCharityDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewBasketItemReviewDialog",
                RightIconResName = "CusViewBasketItemReviewDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewBasketItemReviewDialog();
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
                Title = $"CusViewConfirmSurveyDialog",
                RightIconResName = "CusViewConfirmSurveyDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewConfirmSurveyDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewConfirmServiceDialog",
                RightIconResName = "CusViewConfirmServiceDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewConfirmServiceDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewStandardSetupDialog",
                RightIconResName = "CusViewStandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewStandardSetupDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewEntryCVVDialog",
                RightIconResName = "CusViewEntryCVVDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewEntryCVVDialog();
                })
            });

            #region MessageDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowCusViewMessageDialog CASE1",
                RightIconResName = "ShowCusViewMessageDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowCusViewMessageDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowCusViewMessageDialog CASE2",
                RightIconResName = "ShowCusViewMessageDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowCusViewMessageDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowCusViewMessageDialog CASE3",
                RightIconResName = "ShowCusViewMessageDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowCusViewMessageDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShowCusViewMessageDialog CASE4",
                RightIconResName = "ShowCusViewMessageDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowCusViewMessageDialog(CaseDialog.CASE4);
                })
            });

            #endregion

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewAdjustDonationDialog",
                RightIconResName = "CusViewAdjustDonationDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewAdjustDonationDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewSelectTipDialog",
                RightIconResName = "CusViewSelectTipDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewSelectTipDialog();
                })
            });

            #region ApprovalDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE1",
                RightIconResName = "ApprovalDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE2",
                RightIconResName = "ApprovalDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE3",
                RightIconResName = "ApprovalDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE4",
                RightIconResName = "ApprovalDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE5",
                RightIconResName = "ApprovalDialog_CASE5",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE6",
                RightIconResName = "ApprovalDialog_CASE6",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ApprovalDialog CASE7",
                RightIconResName = "ApprovalDialog_CASE7",
                ItemAction = new Action(() =>
                {
                    ShowCusViewApprovalDialog(CaseDialog.CASE7);
                })
            });
            #endregion

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CusViewEntryCVVDialog",
                RightIconResName = "CusViewEntryCVVDialog",
                ItemAction = new Action(() =>
                {
                    ShowCusViewEntryCVVDialog();
                })
            });

            #region EnterPinDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE1",
                RightIconResName = "EnterPinDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowCusViewEnterPinDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE2",
                RightIconResName = "EnterPinDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowCusViewEnterPinDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE3",
                RightIconResName = "EnterPinDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowCusViewEnterPinDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EnterPinDialog CASE4",
                RightIconResName = "EnterPinDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowCusViewEnterPinDialog(CaseDialog.CASE4);
                })
            });

            #endregion

            #region CusViewRequestCardDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE1",
                RightIconResName = "RequestCardDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE2",
                RightIconResName = "RequestCardDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE3",
                RightIconResName = "RequestCardDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE4",
                RightIconResName = "RequestCardDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE5",
                RightIconResName = "RequestCardDialog_CASE5",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE6",
                RightIconResName = "RequestCardDialog_CASE6",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE7",
                RightIconResName = "RequestCardDialog_CASE7",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestCardDialog CASE8",
                RightIconResName = "RequestCardDialog_CASE8",
                ItemAction = new Action(() =>
                {
                    ShowCusViewRequestCardDialog(CaseDialog.CASE8);
                })
            });

            #endregion
        }
        #endregion

        #region InitializeCommonData
        private void InitializeCommonData()
        {
#if true
            #region Unattended Flow

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedReceiptOptionDialog",
                RightIconResName = "UnattendedReceiptOptionDialog",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedReceiptOptionDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE1",
                RightIconResName = "ApprovalDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE2",
                RightIconResName = "ApprovalDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE3",
                RightIconResName = "ApprovalDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE4",
                RightIconResName = "ApprovalDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE5",
                RightIconResName = "ApprovalDialog_CASE5",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE6",
                RightIconResName = "ApprovalDialog_CASE6",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedApprovalDialog CASE7",
                RightIconResName = "ApprovalDialog_CASE7",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedApprovalDialog(CaseDialog.CASE7);
                })
            });

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedDCCConfirmation",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedDCCConfirmation();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedSelectDCCCurrency",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedSelectDCCCurrency();
            //    })
            //});

            //

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedProcessMessageDialog CASE1",
            //    RightIconResName = "ProcessMessageDialog_CASE1",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedProcessMessageDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedProcessMessageDialog CASE2",
            //    RightIconResName = "ProcessMessageDialog_CASE2",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedProcessMessageDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedProcessMessageDialog CASE3",
            //    RightIconResName = "ProcessMessageDialog_CASE3",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedProcessMessageDialog(CaseDialog.CASE3);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedProcessMessageDialog CASE4",
            //    RightIconResName = "ProcessMessageDialog_CASE4",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedProcessMessageDialog(CaseDialog.CASE4);
            //    })
            //});

            //

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE1",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE2",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE3",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE3);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE4",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE4);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE5",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE5);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE6",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE6);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE7",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE7);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedRequestCardDialog CASE8",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestCardDialog(CaseDialog.CASE8);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedGetAmountDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedGetAmountDialog(CaseDialog.CASE1);
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectManagerMenu",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSelectManagerMenu();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedSingleUserLoginDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedSingleUserLoginDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectUnattendedMode",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSelectUnattendedMode();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"AttendedMenu",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowAttendedMenu();
                })
            });
            #endregion
#endif

#if false    //
            #region Help Flow

            _lData.Add(new ScreenViewModel()
            {
                Title = $"HelpQRDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowHelpQRDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"HelpDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowHelpDialog();
                })
            });

            #endregion
#endif

#if false   // 
            #region Donation Flow

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"StandaloneSelectDonationAmountDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowStandaloneSelectDonationAmountDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE1",
                RightIconResName = "RequestCardDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE2",
                RightIconResName = "RequestCardDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE3",
                RightIconResName = "RequestCardDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE4",
                RightIconResName = "RequestCardDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE5",
                RightIconResName = "RequestCardDialog_CASE5",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE6",
                RightIconResName = "RequestCardDialog_CASE6",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE7",
                RightIconResName = "RequestCardDialog_CASE7",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationRequestCardDialog CASE8",
                RightIconResName = "RequestCardDialog_CASE8",
                ItemAction = new Action(() =>
                {
                    ShowDonationRequestCardDialog(CaseDialog.CASE8);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationReceiptOptionsDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowDonationReceiptOptionsDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectOptionDialog",
                RightIconResName = "SelectOptionDialog",
                ItemAction = new Action(() =>
                {
                    ShowDonationSelectOptionDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationSelectAmountDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowDonationSelectAmountDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationAdvertisingDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowDonationAdvertising();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DonationEnterAccessCodeDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowDonationEnterAccessCodeDialog();
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
                Title = $"SelectCharityDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSelectCharityDialog();
                })
            });

            #endregion
#endif

#if false   //   

            #region DCC

            _lData.Add(new ScreenViewModel()
            {
                Title = $"DCCConfirmationDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    DCCConfirmation();
                })
            });

#if false
            //don't need yet
            ////_lData.Add(new ScreenViewModel()
            ////{
            ////    Title = $"DCCRateApprovalDialog",
            ////    RightIconResName = "DCCRateApprovalDialog",
            ////    ItemAction = new Action(() =>
            ////    {
            ////        DCCRateApproval();
            ////    })
            ////});
#endif

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectDCCCurrencyDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    SelectDCCCurrency();
                })
            });

            #endregion

#endif


#if false    //  

            #region Report Flow

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MenuDialog_SelectPaymentMethod",
                RightIconResName = "SelectPaymentMethod",
                ItemAction = new Action(() =>
                {
                    ShowSelectPaymentMethod();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShiftEnterDateRangeDialog",
                RightIconResName = "ShiftEnterDateRangeDialog",
                ItemAction = new Action(() =>
                {
                    ShowShiftEnterDateRangeDialog();
                })
            });

            #endregion

#endif

#if false    // 

            #region Setup Flow

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog_PullConnections",
                RightIconResName = "StandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    ShowPullConnections();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog_PrintHeaderSetup",
                RightIconResName = "StandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    ShowPrintHeaderSetup();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog_TerminalInfo",
                RightIconResName = "StandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    ShowTerminalInfo();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog_SecurityInfo",
                RightIconResName = "StandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    ShowSecurityInfo();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog_MerchantInfo",
                RightIconResName = "StandardSetupDialog",
                ItemAction = new Action(() =>
                {
                    MerchantInfo();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MenuDialog_SetupInfo",
                RightIconResName = "MenuDialog",
                ItemAction = new Action(() =>
                {
                    SetupInfo();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MenuDialog_SetupMenu",
                RightIconResName = "MenuDialog",
                ItemAction = new Action(() =>
                {
                    SetupMenu();
                })
            });

            #endregion

#endif

#if false    // 

            #region Refund flow

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SearchFilterOptionsDialog",
            //    RightIconResName = "SearchFilterOptionsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSearchFilterOptionsDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RefundOptionsDialog",
            //    RightIconResName = "RefundOptionsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        RefundOptions();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"REFUND_SELECT_TYPE_DIALOG",
            //    RightIconResName = "REFUND_SELECT_TYPE_DIALOG",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRefundTypes();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"FindPurchaseOptionDialog_CASE01",
            //    RightIconResName = "FindPurchaseOptionDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowFindPurchaseOptionDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"FindPurchaseOptionDialog_CASE02",
            //    RightIconResName = "FindPurchaseOptionDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowFindPurchaseOptionDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ManualScanQRCodeDialog",
            //    RightIconResName = "ManualScanQRCodeDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowManualScanQRCodeDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ListPaymentDialog",
            //    RightIconResName = "ListPaymentDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowListPaymentDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RefundPurchaseListItemsDialog",
            //    RightIconResName = "RefundPurchaseListItemsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRefundPurchaseListItemsDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RefundReasonDialog",
            //    RightIconResName = "RefundReasonDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRefundReasonDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"AdvancedSearchDialog",
            //    RightIconResName = "AdvancedSearchDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowAdvancedSearchDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetAmountRefundAlipayWeChatDialog_01",
            //    RightIconResName = "GetAmountRefundAlipayWeChatDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetAmountRefundAlipayWeChatDialog_01();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetAmountRefundAlipayWeChatDialog_02",
            //    RightIconResName = "GetAmountRefundAlipayWeChatDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetAmountRefundAlipayWeChatDialog_02();
            //    })
            //});


            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RefundSearchDetailDialog",
            //    RightIconResName = "RefundSearchDetailDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRefundSearchDetailDialog();
            //    })
            //});

#if true
            // work later, don't need now

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RefundListCardDialog",
                RightIconResName = "RefundListCardDialog",
                ItemAction = new Action(() =>
                {
                    ShowRefundListCardDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RefundNFCDialog",
                RightIconResName = "RefundNFCDialog",
                ItemAction = new Action(() =>
                {
                    ShowRefundNFCDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RefundSeachResultDialog",
                RightIconResName = "RefundSeachResultDialog",
                ItemAction = new Action(() =>
                {
                    ShowRefundSeachResultDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"AccessCodeEnterDialog",
                RightIconResName = "AccessCodeEnterDialog",
                ItemAction = new Action(() =>
                {
                    GetRefundAccessCode();
                })
            });
#endif

            #endregion

#endif

#if false // 

            #region Table Flow


            _lData.Add(new ScreenViewModel()
            {
                Title = $"GetTenderBalanceDialog",
                RightIconResName = "GetTenderBalanceDialog",
                ItemAction = new Action(() =>
                {
                    ShowGetTenderBalanceDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"StandardSetupDialog_EditTicket",
                RightIconResName = "EditTicket",
                ItemAction = new Action(() =>
                {
                    EditTicket();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectTenderExtraAmountDialog",
                RightIconResName = "SelectTenderExtraAmountDialog",
                ItemAction = new Action(() =>
                {
                    ShowSelectTenderExtraAmountDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectTablePayTicketsDialog",
                RightIconResName = "SelectTablePayTicketsDialog",
                ItemAction = new Action(() =>
                {
                    ShowSelectTablePayTicketsDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectGuestAccountDialog",
                RightIconResName = "SelectGuestAccountDialog",
                ItemAction = new Action(() =>
                {
                    ShowSelectGuestAccountDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"GuestBalanceDialog",
                RightIconResName = "GuestBalanceDialog",
                ItemAction = new Action(() =>
                {
                    ShowGuestBalanceDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SplitPayDialog",
                RightIconResName = "SplitPayDialog",
                ItemAction = new Action(() =>
                {
                    ShowSplitPayDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"IncreaseSplitDialog",
                RightIconResName = "IncreaseSplitDialog",
                ItemAction = new Action(() =>
                {
                    ShowIncreaseSplitDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"TicketSearchOptionsDialog",
                RightIconResName = "TicketSearchOptionsDialog",
                ItemAction = new Action(() =>
                {
                    ShowTicketSearchOptionsDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ReviewPaymentDialog",
                RightIconResName = "ReviewPaymentDialog",
                ItemAction = new Action(() =>
                {
                    ShowReviewPaymentDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SplitReviewPaymentsDialog",
                RightIconResName = "SplitReviewPaymentsDialog",
                ItemAction = new Action(() =>
                {
                    ShowSplitReviewPaymentsDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ConfirmClosingTableDialog",
                RightIconResName = "ConfirmClosingTableDialog",
                ItemAction = new Action(() =>
                {
                    ShowConfirmClosingTableDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectOptionDialog",
                RightIconResName = "SelectOptionDialog",
                ItemAction = new Action(() =>
                {
                    ShowSelectOptionDialog();
                })
            });

            #endregion

#endif

#if false // 

            #region Customer Display

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
                Title = $"AuthenticatingDialog",
                RightIconResName = "AuthenticatingDialog",
                ItemAction = new Action(() =>
                {
                    ShowAuthenticatingDialog();
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
                Title = $"BasketItemSelectOfferDialog",
                RightIconResName = "BasketItemSelectOfferDialog",
                ItemAction = new Action(() =>
                {
                    BasketItemSelectOffer();
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

            #endregion
#endif

#if false // 

            #region Main Payment Flow

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectFunctionDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSelectFunctionDialog();
                })
            });

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"AdjustDonationDialog",
            //    RightIconResName = "AdjustDonationDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowAdjustDonationDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectMerchantDialog",
            //    RightIconResName = "SelectMerchantDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectMerchantDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetTipAmountDialog",
            //    RightIconResName = null,
            //    ItemAction = new Action(() =>
            //    {
            //        ShowEnterTipAmountDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DigitalSignatureDialog",
            //    RightIconResName = "DigitalSignatureDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        DigitalSignature();
            //    })
            //});

            ////_lData.Add(new ScreenViewModel()
            ////{
            ////    Title = $"DigitalSignatureConfirmDialog",
            ////    RightIconResName = "",
            ////    ItemAction = new Action(() =>
            ////    {
            ////        ShowDigitalSignatureConfirmDialog();
            ////    })
            ////});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectTipDialog",
            //    RightIconResName = "select_tip",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectTipDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetAmountDialog_CASE1",
            //    RightIconResName = "GetAmountDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetAmountDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetAmountDialog_CASE2",
            //    RightIconResName = "GetAmountDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetAmountDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetAmountCashOutDialog CASE1",
            //    RightIconResName = "GetAmountCashOutDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetAmountCashOutDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetAmountCashOutDialog CASE2",
            //    RightIconResName = "GetAmountCashOutDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetAmountCashOutDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SingleUserLoginDialog",
            //    RightIconResName = "SingleUserLoginDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSingleUserLoginDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"LogonDialog_1",
            //    RightIconResName = "logon_dialog_1",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowLogonDialogCase01();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"LogonDialog_2",
            //    RightIconResName = "logon_dialog_2",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowLogonDialogCase02();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"MainDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowMainDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"AdvertisingDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowAdvertisingDialog();
            //    })
            //});


            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SurchargeConfirmDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSurchargeConfirmDialog();
            //    })
            //});
#if false
                        _lData.Add(new ScreenViewModel()
                        {
                            Title = $"SignOrPinDialog",
                            RightIconResName = "SignOrPinDialog",
                            ItemAction = new Action(() =>
                            {
                                SignOrPin();
                            })
                        });
#endif

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ReceiptOptionsDialog",
            //    RightIconResName = "ReceiptOptionsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowReceiptOptionDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"QrCodeReceiptClaimDialog",
            //    RightIconResName = "QrCodeReceiptClaimDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowQrCodeReceiptClaimDialog();
            //    })
            //});


            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ReceiptEmailAddressDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowReceiptEmailAddressDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"EnterCellNumberDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowEnterCellNumberDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ConfirmServiceDialog",
            //    RightIconResName = "confirm_service_dialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowConfirmServiceDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ConfirmSurveyDialog",
            //    RightIconResName = "confirm_survey_dialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowConfirmSurveyDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EmailReceiptSendResultDialog Email Success",
                RightIconResName = "EmailReceiptSendResultDialogSuccess",
                ItemAction = new Action(() =>
                {
                    ShowEmailReceiptSendResultDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EmailReceiptSendResultDialog Email Fail",
                RightIconResName = "EmailReceiptSendResultDialogFail",
                ItemAction = new Action(() =>
                {
                    ShowEmailReceiptSendResultDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EmailReceiptSendResultDialog Text Success",
                RightIconResName = "EmailReceiptSendResultDialogSuccess",
                ItemAction = new Action(() =>
                {
                    ShowEmailReceiptSendResultDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EmailReceiptSendResultDialog Text Fail",
                RightIconResName = "EmailReceiptSendResultDialogFail",
                ItemAction = new Action(() =>
                {
                    ShowEmailReceiptSendResultDialog(CaseDialog.CASE4);
                })
            });


            #endregion
#endif

#if false    // 

            #region Settlement & Reprint

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SettlementApprovalDialog",
                RightIconResName = "SettlementApprovalDialog",
                ItemAction = new Action(() =>
                {
                    SettlementApproval();
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
                Title = $"SettlementGetDateDialog",
                RightIconResName = "settlement_get_date_dialog",
                ItemAction = new Action(() =>
                {
                    ShowSettlementGetDateDialog();
                })
            });
            #endregion
#endif

#if false   //  

            #region Preauth Flow

            ////not used
            ////_lData.Add(new ScreenViewModel()
            ////{
            ////    Title = $"ConfirmPreauthAutoTopUpDialog",
            ////    RightIconResName = "ConfirmPreauthAutoTopUpDialog",
            ////    ItemAction = new Action(() =>
            ////    {
            ////        ShowConfirmPreauthAutoTopUpDialog();
            ////    })
            ////});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ListPaymentDialog",
            //    RightIconResName = "ListPaymentDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ListPaymentRecordDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"PreAuthCompletePreAuthInfoDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowPreAuthCompletePreAuthInfoDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ListPaymentDialog",
            //    RightIconResName = "list_payment_dialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowListPaymentDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthCompleGetNewAmountDialog PreAuthPartial CASE01",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    PreAuthItemGetNewAmount(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthCompleGetNewAmountDialog PreAuthComplete CASE02",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    PreAuthItemGetNewAmount(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthCompleGetNewAmountDialog PreAuthPartial CASE03",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    PreAuthItemGetNewAmount(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthEnterAmountDialog CASE1",
                RightIconResName = "preauth_enter_amount_dialog",
                ItemAction = new Action(() =>
                {
                    ShowPreAuthEnterAmountDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PreAuthEnterAmountDialog CASE2",
                RightIconResName = "preauth_enter_amount_dialog",
                ItemAction = new Action(() =>
                {
                    ShowPreAuthEnterAmountDialog(CaseDialog.CASE2);
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

#endif

#if false   //  

            #region Request Card Flow

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
                Title = $"MerchantSwipeCardDialog",
                RightIconResName = "MerchantSwipeCardDialog",
                ItemAction = new Action(() =>
                {
                    ShowSwipeMerchantCardDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestAliPayWechatDialog1_None",
                RightIconResName = "RequestAliPayWechatDialog",
                ItemAction = new Action(() =>
                {
                    ShowRequestAliPayWechat("abcdef1234567889", true, true, Entities.ResultStatus.None);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestAliPayWechatDialog2_Declined",
                RightIconResName = "RequestAliPayWechatDialog",
                ItemAction = new Action(() =>
                {
                    ShowRequestAliPayWechat("abcdef1234567889", true, true, Entities.ResultStatus.Declined);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestAliPayWechatDialog3_Running",
                RightIconResName = "RequestAliPayWechatDialog",
                ItemAction = new Action(() =>
                {
                    ShowRequestAliPayWechat("abcdef1234567889", true, true, Entities.ResultStatus.Running);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"RequestAliPayWechatDialog4_Approval",
                RightIconResName = "RequestAliPayWechatDialog",
                ItemAction = new Action(() =>
                {
                    ShowRequestAliPayWechat("abcdef1234567889", true, true, Entities.ResultStatus.Approval);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ViewLeftIconRightQuadrupleTextOverlayDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowViewLeftIconRightQuadrupleTextOverlayDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ListCardBrandDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowListCardBrandDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SurchargeFeeDetailDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSurchargeFeeDetailDialog();
                })
            });

            #endregion
#endif

#if false   //  

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
#endif

#if false    //  

            #region MessageDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE3",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE4",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE5",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE6",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE7",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Droid CASE8",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogDroid(CaseDialog.CASE8);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell Confirm CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ConfirmTopUpMessageBox(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell Confirm CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ConfirmTopUpMessageBox(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE3",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE4",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE5",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE6",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE7",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"MessageDialog Shell CASE8",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowMessageDialogShell(CaseDialog.CASE8);
                })
            });
            #endregion

#endif

#if false   //  

            #region PresentCardErrorDialog

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PresentCardErrorDialog CASE1",
                RightIconResName = "PresentCardErrorDlg_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowPresentCardErrorDlg(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PresentCardErrorDialog CASE2",
                RightIconResName = "PresentCardErrorDlg_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowPresentCardErrorDlg(CaseDialog.CASE2);
                })
            });
            #endregion

#endif

#if false    //  

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
#endif

#if false   //  

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

#endif

#if false   //   

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
#endif

#if false    //  

            #region MOTO FLOW

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
#endif

#if false   //  

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
#endif
        }
        #endregion

    }
}
