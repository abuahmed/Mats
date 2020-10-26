using MATS.Core.Enumerations;
using MATS.OA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MATS.AutoService
{
    public class UploadFiles
    {
        private System.Timers.Timer _monitorTimer;
        private const int MonitorTimerDelay = 60000; //1 minute
        private readonly MATSServerDbContext _dbContext;
        private const string ConnectionStringName = @"Data Source=.;Initial Catalog=MatsDb1;User ID=sa;pwd=amihan; Connect Timeout=2000; Pooling='true'; Max Pool Size=200";

        public UploadFiles() 
        {
            _dbContext = new MATSServerDbContext(ConnectionStringName);
        }

        public void StartUploadFiles()
        {                      
            try
            {
                ServiceLog.Log(" - upload started");
                Upload("");
                ServiceLog.Log(" - upload completed");
            }
            catch
            {
                ServiceLog.Log(" - upload failed");
            }

        }
        private void Initialize()
        {
            //this.monitorTimerDelay = monitorTimerDelay * Convert.ToInt32(ConfigurationManager.AppSettings.Get("MonitorObserverDelay"));

            _monitorTimer = new System.Timers.Timer(MonitorTimerDelay);
            _monitorTimer.Elapsed += OnMonitorTimerElapsed;
        }
        public void Start()
        {
            try
            {
                Initialize();
                _monitorTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                ServiceLog.Log(ex.Message);
            }
        }
        private void OnMonitorTimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            _monitorTimer.Enabled = false;

            try
            {
                StartUploadFiles();
            }
            catch (Exception ex)
            {
                ServiceLog.Log(ex.Message);
            }

            _monitorTimer.Enabled = true;
        }

        public void Upload(string ftpServer)//for eg. Upload("ftp://ftpserver.com/matsfiles/clientId")
        {
            try
            {
                var client = new WebClient
                {
                    Credentials = new NetworkCredential("username", "password")
                };

                if (!Directory.Exists(ftpServer))
                    Directory.CreateDirectory(ftpServer);

                var source = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\MATS_Files" + "\\";

                var backupSource = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\MATS_Files_Backup" + "\\";
                if (!Directory.Exists(backupSource))
                    Directory.CreateDirectory(backupSource);

                IEnumerable<FileInfo> fileList = new DirectoryInfo(source).GetFiles();

                foreach (FileInfo fi in fileList)
                {
                    try
                    {
                        client.UploadFile(ftpServer + "/" + fi.Name, "STOR", fi.FullName);
                        //move the file after upload
                        fi.MoveTo(Path.Combine(backupSource, fi.Name));
                        var attachment = _dbContext.Attachments.FirstOrDefault(a =>
                             a.AttachmentName == fi.Name.Substring(fi.Name.LastIndexOf("_" + 1)) &&
                             a.Ticket.ROWGUID == fi.Name.Substring(0, fi.Name.LastIndexOf("_" + 1)));
                        if (attachment != null)
                        {
                            attachment.AttachmentStatus = (int)AttachmentStatus.Completed;
                            _dbContext.Add(attachment);
                            _dbContext.SaveChanges();
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLog.Log(ex.Message);
            }
        }

        
    }
}
