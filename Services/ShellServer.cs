using Android.Content;
using CloudBanking.Entities;
using CloudBanking.Language;
using CloudBanking.ServiceLocators;
using CloudBanking.ShellContainers;
using CloudBanking.Utilities;
using static CloudBanking.Utilities.UtilEnum;

namespace CloudBanking.UITestApp
{
    public class ShellServer : IShellServer
    {
        const string TAG = "ShellServer";
        ProcessingData _triggerProcessingData;
        IDialogBuilder _dialgoBuilder;

        public TransData TransData { get; set; }

        public ShellServer(Context context, IDialogBuilder dialgoBuilder)
        {
            _dialgoBuilder = dialgoBuilder;
        }

        public void StartProcessingTrigger()
        {
            if (_dialgoBuilder == null || _dialgoBuilder.CurrentAppState != AppState.Foreground)
                return;

            if (_triggerProcessingData == null)
            {
                _triggerProcessingData = new ProcessingData();
                _triggerProcessingData.hTextOne = StringIds.STRING_PROCESSING_NOW.GetString();

                _dialgoBuilder.ShowSeperateDialog(IShellDialog.PROCESSING_MESSAGE_DIALOG, StringIds.STRING_PROCESSING, null, false, false, _triggerProcessingData);
            }
        }

        public void StopProcessingTrigger()
        {
            if (_triggerProcessingData != null)
            {
                _triggerProcessingData?.OnDismiss?.Invoke(false);

                _triggerProcessingData = null;
            }
        }

        public void UpdateProcessingTrigger(string msg, string submessage)
        {
            if (_triggerProcessingData != null)
            {
                _triggerProcessingData.hTextOne = msg;
                _triggerProcessingData.hTextTwo = submessage;
            }
        }

        public void ExitApp()
        {
            Java.Lang.Runtime.GetRuntime().Exit(2);
        }

        public void Init()
        {
        }

        public void CompleteTransaction(TransResponse response)
        {
        }

        public void ShellAppLoaded()
        {
        }

        public void SetApplicationFlow(IShellApplicationFlow applicationFlow)
        {
        }

        public bool IsShellReady()
        {
            throw new System.NotImplementedException();
        }

        public TransResponse GetData(TransData transData, string action)
        {
            throw new System.NotImplementedException();
        }

        public TransResponse DoTransaction()
        {
            throw new System.NotImplementedException();
        }

        public bool StopTransaction()
        {
            throw new System.NotImplementedException();
        }
    }
}

