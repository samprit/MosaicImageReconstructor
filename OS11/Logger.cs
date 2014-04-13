using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OS11
{
    class Logger
    {
        public string myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public void Log(string toLog)
        {
            if (!(System.IO.Directory.Exists(myDocsPath+"\\MosaiQ\\Log")))
            {
                System.IO.Directory.CreateDirectory(myDocsPath+"\\MosaiQ\\Log");
            }

            if (!File.Exists(myDocsPath+"\\MosaiQ\\Log\\mosaiq.log"))
            {
                File.Create(myDocsPath + "\\MosaiQ\\Log\\mosaiq.log");
            }

            DateTime currTime = DateTime.Now;
            string currString = "[" + currTime.ToShortDateString() + "  "+ currTime.ToShortTimeString() + "]  "+toLog;

            using (StreamWriter writer = new StreamWriter(myDocsPath+"\\MosaiQ\\Log\\mosaiq.log", true))
            {
                writer.WriteLine(currString);
            }
        }
    }
}
