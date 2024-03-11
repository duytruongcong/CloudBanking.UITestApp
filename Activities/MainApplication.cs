using System;
using Android.App;
using Android.Runtime;
using CloudBanking.ApiLocators.Services;
using CloudBanking.BaseControl;
using CloudBanking.BaseHardware;
using CloudBanking.Common;
using CloudBanking.DroidCommon;
using CloudBanking.LinklyHttpClient;
using CloudBanking.PaxSdk;
using CloudBanking.PhoneSdk;
using CloudBanking.Repositories;
using CloudBanking.ServiceLocators;
using CloudBanking.ShellContainers;
using CloudBanking.ShellUI;
using CloudBanking.SinglePaymentApp;
using CloudBanking.Utilities;
using Plugin.CurrentActivity;
using Plugin.DeviceInfo;

namespace CloudBanking.UITestApp
{
    [Application]
    public class MainApplication : Application
    {
        public ApplicationFlow PaymentFlow { get; set; }

        public MainApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
            RegisterServices();
        }

        void RegisterServices()
        {
            ServiceLocator.Instance.Register<IUtilityService, PaymentAppUtilityService>(this);
            //ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard());
            //ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard(), !CrossDeviceInfo.Current.IsTerminalNotFullScreen());

            // add new
            ServiceLocator.Instance.Register<IUtilityService, PaymentAppUtilityService>(this, CrossDeviceInfo.Current.IsLargeScreen());
            ServiceLocator.Instance.Register<IFileService, DroidFileService>(this, ServiceLocator.Instance.Get<IUtilityService>(), GlobalConstants.FOLDER_SINGLEAPP);
            ServiceLocator.Instance.Register<ILoggerService, DroidLoggerService>(this, ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IUtilityService>());
            ServiceLocator.Instance.Register<ISecureStorageService, DroidSharePreferenceSecureStorageService>(this);
            ServiceLocator.Instance.Register<IDatabaseService, DroidDatabaseService>(ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IUtilityService>());
            ServiceLocator.Instance.Register<IEmbeddedResourceLoader, EmbeddedResourceLoader>();
            ServiceLocator.Instance.Register<IProfileService, DroidProfilesService>(ServiceLocator.Instance.Get<ILoggerService>());
            ServiceLocator.Instance.Register<IDiagnosticService, DiagnosticService>();
            ServiceLocator.Instance.Register<IPosInterfaceClient, HttpPosInterfaceClient>(new Uri(string.Format("https://{0}:{1}", "127.0.0.1", "5643")), string.Empty);

            //create smart card
            if (CrossDeviceInfo.Current.IsPaxTerminal())
            {
                ServiceLocator.Instance.Register<ISmartDevice, PaxSmartDevice>(this,
                    ServiceLocator.Instance.Get<ILoggerService>(),
                    ServiceLocator.Instance.Get<IFileService>(),
                    ServiceLocator.Instance.Get<IProfileService>(),
                    CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard());
                ServiceLocator.Instance.Register<IBarcodeService, PaxBarcodeService>(this, ServiceLocator.Instance.Get<ISmartDevice>());
            }
            else
            {
                ServiceLocator.Instance.Register<ISmartDevice, PhoneSmartDevice>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IProfileService>());
                ServiceLocator.Instance.Register<IBarcodeService, ZXingBarcodeService>(this, ServiceLocator.Instance.Get<ISmartDevice>());
            }

            ServiceLocator.Instance.Get<ISmartDevice>().SetHardwareModule(
                CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard(),
                CrossDeviceInfo.Current.IsTerminalHasBattery(),
                CrossDeviceInfo.Current.IsSwipeSupported(),
                CrossDeviceInfo.Current.IsInsertSupported(),
                CrossDeviceInfo.Current.IsTapSupported(),
                CrossDeviceInfo.Current.IsBuzzerSupport(),
                CrossDeviceInfo.Current.IsMagInsertDelay(),
                CrossDeviceInfo.Current.IsCTLSMagCloser());

            //create UI of shell
            ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard());

            if (CrossDeviceInfo.Current.IsPaxTerminal())
                ServiceLocator.Instance.Register<ITMSService, CloudBanking.PaxSdk.TMSService>(this, ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IUtilityService>(), ServiceLocator.Instance.Get<ILoggerService>(), PaxConstants.PAX_SINGLE_APP_KEY, PaxConstants.PAX_SINGLE_APP_SECRET);
            else
                ServiceLocator.Instance.Register<ITMSService, CloudBanking.PhoneSdk.TMSService>();

            ServiceLocator.Instance.Register<IShellClient, ShellContainerClient.ShellClient>(this, ServiceLocator.Instance.Get<IDialogBuilder>());
            ServiceLocator.Instance.Register<IShellServer, ShellServer>(this, ServiceLocator.Instance.Get<IDialogBuilder>());

            ServiceLocator.Instance.Register<ISendEmailService, SendGridService>(ServiceLocator.Instance.Get<ILoggerService>());
            ServiceLocator.Instance.Register<ISendSMSService, TwilioService>(ServiceLocator.Instance.Get<ILoggerService>());

            ServiceLocator.Instance.Register<IReceiptClient, ReceiptClient>(new Uri("https://receipt.project-jump-start.com/"), "");
        }
    }
}

