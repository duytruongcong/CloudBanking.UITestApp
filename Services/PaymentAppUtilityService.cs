using Android.Content;
using CloudBanking.BaseControl;
using CloudBanking.DroidCommon;
using CloudBanking.Flow.Base;
using CloudBanking.ShellContainers;
using CloudBanking.ShellUI;
using CloudBanking.UI;
using CloudBanking.Utilities;
using System;

namespace CloudBanking.UITestApp
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
           
        }
    }
}