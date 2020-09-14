using StrEmailMonitoring.StrEmailMonitoring;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace StrEmailMonitoring
{
    /// <summary>
    /// Create StrEmailsRecipients Table
    /// </summary>
    public sealed class CreateStrEmailsRecipientsTable : NativeActivity<int>
    {
        #region .    Activity Name    .
        //Use this section to define how the activity name will be displayed
        public CreateStrEmailsRecipientsTable()
        {
            this.DisplayName = "Create Str Emails Recipients Table";
        }
        #endregion

        #region .    UiPath Properties    .

        [RequiredArgument, Category("Input"), Description("Db Path")]
        public InArgument<string> In_DbPath
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("DT containing STR Recipients DT")]
        public InArgument<DataTable> In_StrEmailsRecipientsDT
        {
            get;
            set;
        }

      

        #endregion           

        #region .    Methods    .

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string dbPath = this.In_DbPath.Get(context);
                DataTable recipientsDT = this.In_StrEmailsRecipientsDT.Get(context);

                StrEmailsRecipientsHandler sERH = new StrEmailsRecipientsHandler(dbPath, recipientsDT);
                sERH.LoadToTable();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[CreateStrEmailsRecipientsTable] Exception >>> {e.Message}");
                throw e;
            }
        }

        #endregion
    }

}
