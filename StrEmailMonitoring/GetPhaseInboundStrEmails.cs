using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace StrEmailMonitoring
{
    /// <summary>
    /// get's the phase via filename
    /// </summary>
    public sealed class GetPhaseInboundStrEmails : NativeActivity<int>
    {
        #region .    Activity Name    .
        public GetPhaseInboundStrEmails()
        {
            this.DisplayName = "GetPhaseInboundStrEmails";
        }
        #endregion

        #region .    UiPath Properties    .

        [RequiredArgument, Category("Input"), Description("Db files")]
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


        [Category("Output"), Description("The phase")]
        public OutArgument<string> Out_Phase
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
                string phaseX = iseh.GetCurrentPhaseViaFileName(fileName);
                this.Out_Phase.Set(context, phaseX);
                
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
