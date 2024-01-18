using System;
using Android.App;
using Android.Runtime;
using CloudBanking.BaseControl;
using CloudBanking.BaseHardware;
using CloudBanking.DroidCommon;
using CloudBanking.PhoneSdk;
using CloudBanking.ServiceLocators;
using CloudBanking.Utilities;
using Plugin.CurrentActivity;
using Plugin.DeviceInfo;

namespace CloudBanking.UITestApp
{
    [Application]
	public class MainApplication  : Application
	{
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
            ServiceLocator.Instance.Register<IUtilityService, CloudBanking.SinglePaymentApp.PaymentAppUtilityService>(this);
            ServiceLocator.Instance.Register<IUtilityService, CloudBanking.SinglePaymentApp.PaymentAppUtilityService>(this, CrossDeviceInfo.Current.IsLargeScreen());
            ServiceLocator.Instance.Register<IFileService, DroidFileService>(this, ServiceLocator.Instance.Get<IUtilityService>(), GlobalConstants.FOLDER_SINGLEAPP);
            ServiceLocator.Instance.Register<ILoggerService, DroidLoggerService>(this, ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IUtilityService>());
            ServiceLocator.Instance.Register<ISmartDevice, PhoneSmartDevice>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<IFileService>(), ServiceLocator.Instance.Get<IProfileService>());
            //ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard());
            //ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard(), !CrossDeviceInfo.Current.IsTerminalNotFullScreen());
            ServiceLocator.Instance.Register<IDialogBuilder, DialogBuilder>(this, ServiceLocator.Instance.Get<ILoggerService>(), ServiceLocator.Instance.Get<ISmartDevice>(), ServiceLocator.Instance.Get<IUtilityService>(), CrossDeviceInfo.Current.IsTerminalHasPhysicalNumKeyboard());

        }
    }
}

