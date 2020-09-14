using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace StrEmailMonitoring
{
    /// <summary>
    /// Loads dT of inbound str emails from Database via FileName
    /// </summary>
    public sealed class LoadInboundStrEmailsByFileName : NativeActivity<int>
    {
        #region .    Activity Name    .
        public LoadInboundStrEmailsByFileName()
        {
            this.DisplayName = "LoadInboundStrEmailsByFileName";
        }
        #endregion

        #region .    UiPath Properties    .

        [RequiredArgument, Category("Input"), Description("FileName")]
        public InArgument<string> In_FileName
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



        [Category("Output"), Description("Loades a DT")]
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
                string fileName = this.In_FileName.Get(context);
                string dbPath = this.In_DbPath.Get(context);
              

                InboundStrEmailsHandler iseh = new InboundStrEmailsHandler(dbPath);
                DataTable resultDT = iseh.LoadFromDbViaFileName(fileName);
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
