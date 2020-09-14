using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace StrEmailMonitoring
{
    /// <summary>
    /// Loads inbound str emails from Database
    /// </summary>
    public sealed class LoadInboundStrEmailsByFileNameAndUsername : NativeActivity<int>
    {
        #region .    Activity Name    .
        //Use this section to define how the activity name will be displayed
        public LoadInboundStrEmailsByFileNameAndUsername()
        {
            this.DisplayName = "Load Inbound Str Emails By FileName And Username";
        }
        #endregion

        #region .    UiPath Properties    .

        [RequiredArgument, Category("Input"), Description("FileName")]
        public InArgument<string> In_FileName
        {
            get;
            set;
        }


        [RequiredArgument, Category("Input"), Description("Username")]
        public InArgument<string> In_Username
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



        [Category("Output"), Description("Loades a DT containing 1 entry")]
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
                string userName = this.In_Username.Get(context);
                string dbPath = this.In_DbPath.Get(context);
              

                InboundStrEmailsHandler iseh = new InboundStrEmailsHandler(dbPath);
                DataTable resultDT = iseh.LoadFromDbViaFileNameAndUsername(fileName, userName);
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
