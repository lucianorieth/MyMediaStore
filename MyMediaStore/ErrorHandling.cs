using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyMediaStore
{
    public class ErrorHandling
    {
        public static void GenerateLog(string log)
        {
            string path = "InfoCam_log.txt";
            StreamWriter streamWriter = new StreamWriter(path, true);

            streamWriter.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToShortDateString(), log));
            streamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");
            streamWriter.Close();
        }
    }
}
