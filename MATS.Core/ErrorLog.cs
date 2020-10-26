using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATS.Core
{
    public class ErrorLog
    {
        public static void Log(string txt)
        {
            string fileLoc = @"C:\matslog.txt";

            try
            {
                if (!File.Exists(fileLoc))
                {
                    File.Create(fileLoc).Close();
                }
            }
            catch { }

            try
            {
                System.IO.FileStream wFile;
                byte[] byteData = null;
                byteData = Encoding.ASCII.GetBytes("\r\n" + DateTime.Now.ToLongTimeString() +
                DateTime.Now.ToLongDateString() + txt);
                wFile = new FileStream(fileLoc, FileMode.Append, FileAccess.Write);
                wFile.Write(byteData, 0, byteData.Length);
                wFile.Close();
            }
            catch
            {

            }

        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
