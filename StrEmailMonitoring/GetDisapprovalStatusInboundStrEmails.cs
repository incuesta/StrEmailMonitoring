using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace StrEmailMonitoring
{
    /// <summary>
    /// get's the approval status via filename
    /// </summary>
    public sealed class GetDisapprovalStatusInboundStrEmails : NativeActivity<int>
    {
        #region .    Activity Name    .
        public GetDisapprovalStatusInboundStrEmails()
        {
            this.DisplayName = "GetDisapprovalStatusInboundStrEmails";
        }
        #endregion

        #region .    UiPath Properties    .

        [RequiredArgument, Category("Input"), Description("dbPath")]
        public InArgument<string> In_DbPath
        {
            get;
            set;
        }


        [RequiredArgument, Category("Input"), Description("FileName")]
        public InArgument<string> In_FileName
        {
            get;
            set;
        }


        [Category("Output"), Description("True for Disapproved")]
        public OutArgument<bool> Out_IsDisapproved
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
                string fileName = this.In_FileName.Get(context);
                string dbPath = this.In_DbPath.Get(context);

                InboundStrEmailsHandler iseh = new InboundStrEmailsHandler(dbPath);
                bool isApproved = iseh.IsDisapproved(fileName);
                this.Out_IsDisapproved.Set(context, isApproved);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{nameof(this.GetType)}] Exception >>> {e.Message}");
                throw e;
            }
        }

        #endregion
    }

}
