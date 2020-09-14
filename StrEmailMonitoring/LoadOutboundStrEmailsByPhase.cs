using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace StrEmailMonitoring
{
    /// <summary>
    /// Loads Outbound Str Emails from DB
    /// </summary>
    public sealed class LoadOutboundStrEmailsByPhase : NativeActivity<int>
    {
        #region .    Activity Name    .
        public LoadOutboundStrEmailsByPhase()
        {
            this.DisplayName = "LoadOutboundStrEmailsByPhase";
        }
        #endregion

        #region .    UiPath Properties    .

        [RequiredArgument, Category("Input"), Description("The Phase")]
        public InArgument<string> In_Phase
        {
            get;
            set;
        }


        [RequiredArgument, Category("Input"), Description("Path to Db file")]
        public InArgument<string> In_DbPath
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Current Batch")]
        public InArgument<int> In_CurrentBatch
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Per Batch")]
        public InArgument<int> In_PerBatch
        {
            get;
            set;
        }


        [Category("Output"), Description("Loaded entries")]
        public OutArgument<DataTable> Out_DataTable
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
                string phaseX = this.In_Phase.Get(context);
                string dbPath = this.In_DbPath.Get(context);
                int currentBatch = this.In_CurrentBatch.Get(context);
                int perBatch = this.In_PerBatch.Get(context);

                OutboundStrEmailsHandler oseh = new OutboundStrEmailsHandler(dbPath);
                DataTable resultDT = oseh.LoadPhaseXFromDb(phaseX, perBatch, currentBatch);
                this.Out_DataTable.Set(context, resultDT);
                
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
