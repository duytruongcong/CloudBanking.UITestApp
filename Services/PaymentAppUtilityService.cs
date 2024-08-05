using System;
using Android.Content;
using CloudBanking.DroidCommon;
using CloudBanking.Flow.Base;
using CloudBanking.ShellContainers;
using CloudBanking.ShellUI;
using CloudBanking.UI;
using CloudBanking.Utilities;
using CloudBanking.BaseControl;
using System.Linq;
using Android.App;

namespace CloudBanking.SinglePaymentApp
{
    public class PaymentAppUtilityService : DroidUtilityService
    {
        public override bool IsSingleApp => true;

        public PaymentAppUtilityService(Context context, bool isLargeScreen) : base(context, isLargeScreen)
        {

        }

        public override string GetDialogName(object idialog)
        {
            if (idialog is IPayDialog)
                return ((IPayDialog)idialog).GetClassName();

            if (idialog is IShellDialog)
                return ((IShellDialog)idialog).GetClassName();

            return ((ICommonDialog)idialog).GetClassName();

        }

        public override string GetDialogNameFromCommon(ICommonDialog commonDialog)
        {
            switch (commonDialog)
            {
                case ICommonDialog.ENTER_PIN_DIALOG: return typeof(EnterPinDialog).AssemblyQualifiedName;
                default:
                    throw new Exception("Class not found");
            }
        }

        public override void ToggleMainLauncher(bool isMainLauncher)
        {
            var packageManager = _context.PackageManager;

            ActivityAttribute launcherAttr =
                (ActivityAttribute)Attribute.GetCustomAttribute(typeof(MainActivityLauncher), typeof(ActivityAttribute));

            var mainLauncherComponent = new ComponentName(_context, launcherAttr.Name);

            var status = packageManager.GetComponentEnabledSetting(mainLauncherComponent);

            if ((status == Android.Content.PM.ComponentEnabledState.Enabled && isMainLauncher) || ((status == Android.Content.PM.ComponentEnabledState.Default || status == Android.Content.PM.ComponentEnabledState.Disabled) && !isMainLauncher))
                return;

            packageManager.SetComponentEnabledSetting(
                mainLauncherComponent,
                 isMainLauncher ? Android.Content.PM.ComponentEnabledState.Enabled : Android.Content.PM.ComponentEnabledState.Disabled,
                 Android.Content.PM.ComponentEnableOption.DontKillApp
            );

            ActivityAttribute defaultAttr =
                (ActivityAttribute)Attribute.GetCustomAttribute(typeof(MainActivity), typeof(ActivityAttribute));

            var mainComponent = new ComponentName(_context, defaultAttr.Name);

            packageManager.SetComponentEnabledSetting(
                mainComponent,
                 isMainLauncher ? Android.Content.PM.ComponentEnabledState.Disabled : Android.Content.PM.ComponentEnabledState.Enabled,
                 Android.Content.PM.ComponentEnableOption.DontKillApp
            );
        }
    }
}