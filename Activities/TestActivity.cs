﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Service.QuickSettings;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CloudBanking.BaseControl;
using CloudBanking.Entities;
using CloudBanking.ServiceLocators;
using CloudBanking.ShellContainers;
using CloudBanking.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudBanking.Common;
using CloudBanking.Repositories;

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


        public IDialogBuilder DialogBuilder => ServiceLocator.Instance.Get<IDialogBuilder>();

        public IFileService FileService =>  ServiceLocator.Instance.Get<IFileService>();

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

                Window.SetStatusBarColor(this.GetThemeNavigationBarColor()); 
                Window.SetNavigationBarColor(this.GetThemeNavigationBarColor());
                //Window.SetStatusBarColor(this.Resources.GetColor(Resource.Color.setup_status_bar_color));
                //Window.SetNavigationBarColor(this.Resources.GetColor(Resource.Color.setup_status_bar_color));

                Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

                Window.DecorView.SystemUiVisibility =
                (StatusBarVisibility)(SystemUiFlags.HideNavigation |
                                 SystemUiFlags.ImmersiveSticky |
                SystemUiFlags.Fullscreen);
            }

            FileService.CopyFileResource(GlobalConstants.PRESENT_CARD_LOTTIE_FOLDER, false, false);

            InitializationDatabase();

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

        void InitializationDatabase()
        {
            CheckCurrencyDatabase();
        }

        void CheckCurrencyDatabase()
        {
            if (CurrencyRepository.Instance.Load() == null)
            {
                List<Currency> list = new List<Currency>();

                list.Add(AddCurrency(784, GlobalConstants.ID_CURRENCYFLAG_UAE, 2, "AED", "AED", "UAE Dirham", "United Arab Emirates Dirham"));
                list.Add(AddCurrency(32, GlobalConstants.ID_CURRENCYFLAG_ARGENTINE, 2, "ARS", "AR$", "Argentine Peso", "Argentine Peso", "$"));
                list.Add(AddCurrency(36, GlobalConstants.ID_CURRENCYFLAG_AUSTRALIAN, 2, "AUD", "AU$", "Australian Dollar", "Australian Dollar", "$"));
                list.Add(AddCurrency(986, GlobalConstants.ID_CURRENCYFLAG_BRAZILIAN, 2, "BRL", "R$", "Brazilian Real", "Brazilian Real", "R$"));
                list.Add(AddCurrency(554, GlobalConstants.ID_CURRENCYFLAG_NEWZEALAND, 2, "NZD", "NZ$", "New Zealand Dollar", "New Zealand Dollar", "$"));
                list.Add(AddCurrency(901, GlobalConstants.ID_CURRENCYFLAG_TAIWAN, 2, "TWD", "NT$", "Taiwan Dollar", "Taiwan Dollar", "NT$"));
                list.Add(AddCurrency(840, GlobalConstants.ID_CURRENCYFLAG_US, 2, "USD", "USD$", "US Dollar", "US Dollar", "$"));
                list.Add(AddCurrency(710, GlobalConstants.ID_CURRENCYFLAG_SOUTHAFRICAN, 2, "ZAR", "ZAR", "South African Rand", "South African Rand", "R"));
                list.Add(AddCurrency(124, GlobalConstants.ID_CURRENCYFLAG_CANADIAN, 2, "CAD", "CA$", "Canadian Dollar", "Canadian Dollar", "$"));
                list.Add(AddCurrency(756, GlobalConstants.ID_CURRENCYFLAG_SWISS, 2, "CHF", "CHF", "Swiss Franc", "Swiss Franc", "CHF"));
                list.Add(AddCurrency(208, GlobalConstants.ID_CURRENCYFLAG_DANISH, 2, "DKK", "Dkr", "Danish Krone", "Danish Krone", "kr"));
                list.Add(AddCurrency(242, GlobalConstants.ID_CURRENCYFLAG_FIJIAN, 2, "FJD", "FJ$", "Fijian  Dollar", "Fijian  Dollar", "$"));
                list.Add(AddCurrency(344, GlobalConstants.ID_CURRENCYFLAG_HONGKONG, 2, "HKD", "HK$", "Hong Kong Dollar", "Hong Kong Dollar", "$"));
                list.Add(AddCurrency(484, GlobalConstants.ID_CURRENCYFLAG_MEXICAN, 2, "MXN", "MX$", "Mexican Peso", "Mexican Peso", "$"));
                list.Add(AddCurrency(578, GlobalConstants.ID_CURRENCYFLAG_NORWEGIAN, 2, "NOK", "Nkr", "Norwegian Krone", "Norwegian Krone", "kr"));
                list.Add(AddCurrency(752, GlobalConstants.ID_CURRENCYFLAG_SWEDISH, 2, "SEK", "Skr", "Swedish Krona", "Swedish Krona", "kr"));
                list.Add(AddCurrency(702, GlobalConstants.ID_CURRENCYFLAG_SINGAPORE, 2, "SGD", "S$", "Singapore Dollar", "Singapore Dollar", "$"));
                list.Add(AddCurrency(360, GlobalConstants.ID_CURRENCYFLAG_INDONESIAN, 2, "IDR", "Rp", "Indonesian Rupiah", "Indonesian Rupiah", "Rp"));
                list.Add(AddCurrency(598, GlobalConstants.ID_CURRENCYFLAG_PNG, 2, "PGK", "Kina", "Papua New Guinean Kina", "Papua New Guinean Kina", "K"));
                list.Add(AddCurrency(458, GlobalConstants.ID_CURRENCYFLAG_MALAYSIAN, 2, "MYR", "RM", "Malaysian Ringgit", "Malaysian Ringgit", "RM"));
                list.Add(AddCurrency(978, GlobalConstants.ID_CURRENCYFLAG_EURO, 2, "EUR", "\xE2\x82\xAC", "Euro", "Euro", "\xE2\x82\xAC"));
                list.Add(AddCurrency(826, GlobalConstants.ID_CURRENCYFLAG_POUNDS, 2, "GBP", "\xC2\xA3", "Pound  Stirling", "British Pound  Stirling", "\xC2\xA3"));
                list.Add(AddCurrency(376, GlobalConstants.ID_CURRENCYFLAG_ISRAELI, 2, "ILS", "\xE2\x82\xAA", "Israeli Sheqel", "Israeli Sheqel", "\xE2\x82\xAA"));
                list.Add(AddCurrency(356, GlobalConstants.ID_CURRENCYFLAG_INDIAN, 2, "INR", "Rs", "Indian Rupee", "Indian Rupee", "\xE2\x82\xB9"));
                list.Add(AddCurrency(392, GlobalConstants.ID_CURRENCYFLAG_JAPANESE, 0, "JPY", "\xC2\xA5", "JapaneseYen", "JapaneseYen", "\xEF\xBF\xA5"));
                list.Add(AddCurrency(410, GlobalConstants.ID_CURRENCYFLAG_SOUTHKOREAN, 0, "KRW", "\xE2\x82\xA9", "South Korean Won", "South Korean Won", "\xE2\x82\xA9"));
                list.Add(AddCurrency(608, GlobalConstants.ID_CURRENCYFLAG_PHILIPPINE, 2, "PHP", "\xE2\x82\xB1", "Philippine Peso", "Philippine Peso", "\xE2\x82\xB1"));
                list.Add(AddCurrency(985, GlobalConstants.ID_CURRENCYFLAG_POLISH, 2, "PLN", "\x7A\xC5\x82", "Polish Zloty", "Polish Zloty", "\x7A\xC5\x82"));
                list.Add(AddCurrency(634, GlobalConstants.ID_CURRENCYFLAG_QATARI, 2, "QAR", "QR", "Qatari Rial", "Qatari Rial", "\xD8\xB1\x2E\xD9\x82"));
                list.Add(AddCurrency(643, GlobalConstants.ID_CURRENCYFLAG_RUSSIAN, 2, "RUB", "RUB", "Russian Ruble", "Russian Ruble", "\xe2\x82\xbd"));
                list.Add(AddCurrency(682, GlobalConstants.ID_CURRENCYFLAG_SAUDI, 2, "SAR", "SR", "Saudi Riyal", "Saudi Riyal", "\xd8\xb1\x2e\xd8\xb3"));
                list.Add(AddCurrency(764, GlobalConstants.ID_CURRENCYFLAG_THAI, 2, "THB", "\xe0\xb8\xbf", "Thai Bhat", "Thai Bhat", "\xe0\xb8\xbf"));
                list.Add(AddCurrency(704, GlobalConstants.ID_CURRENCYFLAG_VIETNAM, 0, "VND", "VND", "Viet Nam", "Viet Nam", "\xe0\xb8\xbf"));
                list.Add(AddCurrency(156, GlobalConstants.ID_CURRENCYFLAG_CHINA, 2, "CNY", "CNY", "China", "China", "¥"));

                CurrencyRepository.Instance.InsertAll(list);
            }
        }

        Currency AddCurrency(int iCurrencyCode, int iCurrencyCodeFlag, int usMinor, string wszCurrencyCode, string wszCurrencySymbol, string wszCurrencyName, string wszCurrencyFullName, string wszCurrencyNativeSymbol = null)
        {
            Currency item = new Currency();
            item.iCurrencyCode = iCurrencyCode;
            item.iCurrencyCodeFlag = iCurrencyCodeFlag;
            item.usMinor = usMinor;
            item.wszCurrencyCode = wszCurrencyCode;
            item.wszCurrencySymbol = wszCurrencySymbol;
            item.wszCurrencyName = wszCurrencyName;
            item.wszCurrencyFullName = wszCurrencyFullName;
            item.wszCurrencyNativeSymbol = wszCurrencyNativeSymbol;
            return item;
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

            //DialogBuilder.IsShowHeader = false;

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
            CASE8,
            CASE9,
            CASE10
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

#if false   //
            #region Id Check Flow

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdNotificationDialog CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdNotificationDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdNotificationDialog CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdNotificationDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE10",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE10);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE9",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE9);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE8",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE8);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE7",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE6",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE5",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE4",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE3",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdResultDialog CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdResultDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"CheckIdScanQRCodeDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowCheckIdScanQRCodeDialog();
                })
            });

            #endregion
#endif

#if false   // 
            #region Epay Multi Issue

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EpayConfirmationDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowEpayConfirmationDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"EpayReviewPaymentDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowEpayReviewPaymentDialog();
                })
            });

            #endregion

#endif

#if false   //
            #region Unattended Flow

            this.DialogBuilder.IsShowHeader = false;

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectDCCCurrencyDialog CASE2 Unattended",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        SelectDCCCurrency(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectDCCCurrencyDialog CASE1",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        SelectDCCCurrency(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestAliPayWechatDialog1_None",
            //    RightIconResName = "RequestAliPayWechatDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestAliPayWeChatDialog("abcdef1234567889", true, true, Entities.ResultStatus.None);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestAliPayWechatDialog2_Declined",
            //    RightIconResName = "RequestAliPayWechatDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestAliPayWeChatDialog("abcdef1234567889", true, true, Entities.ResultStatus.Declined);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestAliPayWechatDialog3_Running",
            //    RightIconResName = "RequestAliPayWechatDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestAliPayWeChatDialog("abcdef1234567889", true, true, Entities.ResultStatus.Running);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestAliPayWechatDialog4_Approval",
            //    RightIconResName = "RequestAliPayWechatDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedRequestAliPayWeChatDialog("abcdef1234567889", true, true, Entities.ResultStatus.Approval);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectApplicationTypeDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectApplicationTypeDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendAdvertisingDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendAdvertisingDialog();
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

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedVendingStoreDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedVendingStoreDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"UnattendedApprovalDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowUnattendedApprovalDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedReviewTransDialog fuel",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedReviewTransDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedReviewTransDialog ev",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedReviewTransDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedReceiptOptionDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedReceiptOptionDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedDCCConfirmation",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedDCCConfirmation();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedSelectDCCCurrency",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedSelectDCCCurrency();
                })
            });


            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedProcessMessageDialog CASE1",
                RightIconResName = "ProcessMessageDialog_CASE1",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedProcessMessageDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedProcessMessageDialog CASE2",
                RightIconResName = "ProcessMessageDialog_CASE2",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedProcessMessageDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedProcessMessageDialog CASE3",
                RightIconResName = "ProcessMessageDialog_CASE3",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedProcessMessageDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedProcessMessageDialog CASE4",
                RightIconResName = "ProcessMessageDialog_CASE4",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedProcessMessageDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE1);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE3",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE3);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE4",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE4);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE5",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE5);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE6",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE6);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE7",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE7);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"UnattendedRequestCardDialog CASE8",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowUnattendedRequestCardDialog(CaseDialog.CASE8);
                })
            });

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

            this.DialogBuilder.IsShowHeader = true;

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
            //    Title = $"DonationAmountEnterDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationAmountEnterDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"StandaloneSelectDonationAmountDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowStandaloneSelectDonationAmountDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE1",
            //    RightIconResName = "RequestCardDialog_CASE1",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE2",
            //    RightIconResName = "RequestCardDialog_CASE2",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE3",
            //    RightIconResName = "RequestCardDialog_CASE3",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE3);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE4",
            //    RightIconResName = "RequestCardDialog_CASE4",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE4);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE5",
            //    RightIconResName = "RequestCardDialog_CASE5",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE5);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE6",
            //    RightIconResName = "RequestCardDialog_CASE6",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE6);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE7",
            //    RightIconResName = "RequestCardDialog_CASE7",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE7);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationRequestCardDialog CASE8",
            //    RightIconResName = "RequestCardDialog_CASE8",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationRequestCardDialog(CaseDialog.CASE8);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationReceiptOptionsDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationReceiptOptionsDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectOptionDialog",
            //    RightIconResName = "SelectOptionDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationSelectOptionDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationSelectAmountDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationSelectAmountDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationAdvertisingDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationAdvertising();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DonationEnterAccessCodeDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDonationEnterAccessCodeDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectDonationDialog",
            //    RightIconResName = "select_donation",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectDonationDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectCharityDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectCharityDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectOtherCharityDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSelectOtherCharityDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"PaymenDonationsDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowPaymenDonationsDialog();
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
                Title = $"SelectDCCCurrencyDialog CASE2",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    SelectDCCCurrency(CaseDialog.CASE2);
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"SelectDCCCurrencyDialog CASE1",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    SelectDCCCurrency(CaseDialog.CASE1);
                })
            });

            #endregion

#endif

#if false    //  

            #region Report Flow

            // khong lam D8
            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SalesShiftDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSalesShiftDialog();
            //    })
            //});

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
                Title = $"SetupMerchantAccessDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowSetupMerchantAccessDialog();
                })
            });

            _lData.Add(new ScreenViewModel()
            {
                Title = $"ShellStandardSetupDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowShellStandardSetupDialog();
                })
            });

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

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RefundListCardDialog",
            //    RightIconResName = "RefundListCardDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRefundListCardDialog();
            //    })
            //});

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

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"AccessCodeEnterDialog",
            //    RightIconResName = "AccessCodeEnterDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        GetRefundAccessCode();
            //    })
            //});
#endif

            #endregion

#endif

#if false // 

            #region Table Flow

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GetTenderBalanceDialog",
            //    RightIconResName = "GetTenderBalanceDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGetTenderBalanceDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"StandardSetupDialog_EditTicket",
            //    RightIconResName = "EditTicket",
            //    ItemAction = new Action(() =>
            //    {
            //        EditTicket();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectTenderExtraAmountDialog",
            //    RightIconResName = "SelectTenderExtraAmountDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectTenderExtraAmountDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectTablePayTicketsDialog",
            //    RightIconResName = "SelectTablePayTicketsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectTablePayTicketsDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectGuestAccountDialog",
            //    RightIconResName = "SelectGuestAccountDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectGuestAccountDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"GuestBalanceDialog",
            //    RightIconResName = "GuestBalanceDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowGuestBalanceDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SplitPayDialog",
            //    RightIconResName = "SplitPayDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSplitPayDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"IncreaseSplitDialog",
            //    RightIconResName = "IncreaseSplitDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowIncreaseSplitDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"TicketSearchOptionsDialog",
            //    RightIconResName = "TicketSearchOptionsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowTicketSearchOptionsDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ReviewPaymentDialog",
            //    RightIconResName = "ReviewPaymentDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowReviewPaymentDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SplitReviewPaymentsDialog",
            //    RightIconResName = "SplitReviewPaymentsDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSplitReviewPaymentsDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"ConfirmClosingTableDialog",
            //    RightIconResName = "ConfirmClosingTableDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowConfirmClosingTableDialog();
            //    })
            //});

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

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"CustomerDisplayGetCashOutDialog",
            //    RightIconResName = "CustomerDisplayGetCashOutDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowCustomerDisplayGetCashOutDialog();
            //    })
            //});

            _lData.Add(new ScreenViewModel()
            {
                Title = $"AuthenticatingDialog",
                RightIconResName = "AuthenticatingDialog",
                ItemAction = new Action(() =>
                {
                    ShowAuthenticatingDialog();
                })
            });

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"BasketItemReviewDialog",
            //    RightIconResName = "BasketItemReviewDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowBasketItemReviewDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"CustomerAuthenticateOptions",
            //    RightIconResName = "CustomerAuthenticateOptions",
            //    ItemAction = new Action(() =>
            //    {
            //        CustomerAuthenticateOptions();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"BasketItemSelectOfferDialog",
            //    RightIconResName = "BasketItemSelectOfferDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        BasketItemSelectOffer();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"CustomerDisplayRequestCardDialog",
            //    RightIconResName = "CustomerDisplayRequestCardDialog",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowCusDisplayRequestCardDialog(CaseDialog.CASE1);
            //    })
            //});

            #endregion
#endif

#if false //  

            #region Main Payment Flow

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"NotificationDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowNotificationDialog();
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"SelectFunctionDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowSelectFunctionDialog();
            //    })
            //});

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

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"DigitalSignatureConfirmDialog",
            //    RightIconResName = "",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowDigitalSignatureConfirmDialog();
            //    })
            //});

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

            _lData.Add(new ScreenViewModel()
            {
                Title = $"QrCodeReceiptClaimDialog",
                RightIconResName = "QrCodeReceiptClaimDialog",
                ItemAction = new Action(() =>
                {
                    ShowQrCodeReceiptClaimDialog();
                })
            });

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
                Title = $"PreAuthCompletePreAuthInfoDialog",
                RightIconResName = "",
                ItemAction = new Action(() =>
                {
                    ShowPreAuthCompletePreAuthInfoDialog();
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

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE1",
            //    RightIconResName = "RequestCardDialog_CASE1",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE1);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE2",
            //    RightIconResName = "RequestCardDialog_CASE2",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE2);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE3",
            //    RightIconResName = "RequestCardDialog_CASE3",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE3);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE4",
            //    RightIconResName = "RequestCardDialog_CASE4",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE4);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE5",
            //    RightIconResName = "RequestCardDialog_CASE5",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE5);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE6",
            //    RightIconResName = "RequestCardDialog_CASE6",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE6);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE7",
            //    RightIconResName = "RequestCardDialog_CASE7",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE7);
            //    })
            //});

            //_lData.Add(new ScreenViewModel()
            //{
            //    Title = $"RequestCardDialog CASE8",
            //    RightIconResName = "RequestCardDialog_CASE8",
            //    ItemAction = new Action(() =>
            //    {
            //        ShowRequestCardDialog(CaseDialog.CASE8);
            //    })
            //});

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

#if true //  

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
                Title = $"EOVProcessDialog",
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
