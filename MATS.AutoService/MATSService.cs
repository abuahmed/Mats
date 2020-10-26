using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MATS.AutoService
{
    partial class MATSService : ServiceBase
    {
        public MATSService()
        {
            InitializeComponent();
        }

        public void DebugStart()//????
        {
            ObserverTimer_Elapsed(null, null);
        }

        protected override void OnStart(string[] args)
        {
            ObserverTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            ObserverTimer.Enabled = false;
        }
        private void ObserverTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ObserverTimer.Enabled = false;
            try
            {
                var post = new UploadFiles();
                post.Start();
            }
            catch (Exception ex)
            {
                Stop();
                ServiceLog.Log(ex.Message);                
            }
        }

        public void DoEventLogging() 
        {
            var toLog = "";
            var appLog = new System.Diagnostics.EventLog {Source = "_MATS_AutoService"};
            appLog.WriteEntry("Service Error: \r\n" + toLog);
        }
    }
}
