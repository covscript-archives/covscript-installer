using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Covariant_Script_Installer
{
    class Pair<_KeyT, _ValueT>
    {
        public _KeyT first;
        public _ValueT second;
        public Pair(_KeyT key,_ValueT value)
        {
            first = key;
            second = value;
        }
    }
    class Installation
    {
        private Label label;
        private ProgressBar prog;
        public Installation(Label l,ProgressBar p)
        {
            label = l;
            prog = p;
        }
        public List<Pair<string, string>> installation_field;
        public string installation_path;
        public void install()
        {
            Directory.CreateDirectory(installation_path + "\\Bin");
            Directory.CreateDirectory(installation_path + "\\Imports");
            Directory.CreateDirectory(installation_path + "\\Logs");
            foreach(Pair<string,string> info in installation_field)
            {
                DownloadFile(info.first, installation_path + info.second);
            }
        }
        private void DownloadFile(string URL, string filename)
        {
            File.Delete(filename);
            double percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                    prog.Maximum = (int)totalBytes;
                Stream st = myrp.GetResponseStream();
                Stream so = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                        prog.Value = (int)totalDownloadedByte;
                    osize = st.Read(by, 0, by.Length);
                    percent = totalDownloadedByte / (double)totalBytes * 100;
                    label.Text = "Installing... " + ((int)percent).ToString() + "%";
                    Application.DoEvents();
                }
                so.Close();
                st.Close();
            }
            catch (Exception e)
            {
                label.Text = "Error";
                if (prog != null)
                    prog.Value = 0;
                throw e;
            }
            label.Text = "Done";
            if (prog != null)
                prog.Value = 0;
        }
    }
}
