using Android.App;
using Android.Runtime;
using CloudBanking.ApiLocators.Services;
using CloudBanking.BaseControl;
using CloudBanking.BaseHardware;
using CloudBanking.Common;
using CloudBanking.DroidCommon;
using CloudBanking.Flow.Base;
using CloudBanking.Logger;
using CloudBanking.PaxSdk;
using CloudBanking.PaymentLoyalty;
using CloudBanking.PhoneSdk;
using CloudBanking.POSContainer;
using CloudBanking.Printing;
using CloudBanking.Repositories;
using CloudBanking.ServiceLocators;
using CloudBanking.ShellContainers;
using CloudBanking.Utilities;
using CloudBanking.WebSocket;
using Plugin.CurrentActivity;
using Plugin.DeviceInfo;
using System;

namespace CloudBanking.UITestApp
{
    [Application]
    public class MainApplication : BaseApplication
    {
        public ApplicationFlow PaymentFlow { get; set; }

        public MainApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void RegisterServices()
        {
            //create smart card
            if (CrossDeviceInfo.Current.IsPaxTerminal())
            {
                ServiceLocator.Instance.Register<ISmartDevice, PaxSmartDevice>(this,
                    ServiceLocator.Instance.Get<ILoggerService>(),
                    ServiceLocator.Instance.Get<IFileService>(),
                    ServiceLocator.Instance.Get<IProfileService>(),
                    CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard(),
                    ServiceLocator.Instance.Get<IUtilityService>());
                ServiceLocator.Instance.Register<IBarcodeService, PaxBarcodeService>(this, ServiceLocator.Instance.Get<ISmartDevice>());
            }
            else
            {
                ServiceLocator.Instance.Register<ISmartDevice, PhoneSmartDevice>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IProfileService>());
                ServiceLocator.Instance.Register<IBarcodeService, ZXingBarcodeService>(this, ServiceLocator.Instance.Get<ISmartDevice>());
            }

            //create UI of shell
            ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard(), ServiceLocator.Instance.Get<IDiagnosticService>());
            ServiceLocator.Instance.Register<IDatabaseService, DroidDatabaseService>(ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IUtilityService>());
            ServiceLocator.Instance.Register<IEmbeddedResourceLoader, EmbeddedResourceLoader>();

            ServiceLocator.Instance.Register<IShellClient, ShellClient>(this, ServiceLocator.Instance.Get<IDialogBuilder>());
            ServiceLocator.Instance.Register<IShellServer, ShellServer>(ServiceLocator.Instance.Get<IDialogBuilder>());

            ServiceLocator.Instance.Register<ISendEmailService, SendGridService>(ServiceLocator.Instance.Get<ILoggerService>());

            ServiceLocator.Instance.Register<ISendSMSService, TwilioService>(ServiceLocator.Instance.Get<ILoggerService>());

            ServiceLocator.Instance.Register<IReceiptClient, ReceiptClient>(new Uri("https://receipt.project-jump-start.com/"), "");

            ServiceLocator.Instance.Register<IPosService, POSService>(this, ServiceLocator.Instance.Get<IDialogBuilder>());

            ServiceLocator.Instance.Register<IPrinting, DroidPrintingServices>();

            ServiceLocator.Instance.Register<IRebootWarningService, RebootWarningService>(this, ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISecureStorageService>(), ServiceLocator.Instance.Get<IDialogBuilder>());

            ServiceLocator.Instance.Register<ILogUploadSchedule, DroidLogUploadSchedule>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), ServiceLocator.Instance.Get<ISecureStorageService>());

            ServiceLocator.Instance.Get<IUtilityService>().Init();

            ServiceLocator.Instance.Get<IDiagnosticService>()?.UpdateDiagnosticReboots();

            ServiceLocator.Instance.Get<ISmartDevice>().SetHardwareModule(
                  CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard(),
                  CrossDeviceInfo.Current.IsTerminalHasBattery(),
                  CrossDeviceInfo.Current.IsSwipeSupported(),
                  CrossDeviceInfo.Current.IsInsertSupported(),
                  CrossDeviceInfo.Current.IsTapSupported(),
                  CrossDeviceInfo.Current.IsBuzzerSupport(),
                  CrossDeviceInfo.Current.IsMagInsertDelay(),
                  CrossDeviceInfo.Current.IsCTLSMagCloser());

            ServiceLocator.Instance.Register<ILedService, DroidLedService>();
        }
    }
}

