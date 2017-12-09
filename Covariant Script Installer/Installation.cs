using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
            prog.Maximum = installation_field.Count;
            prog.Value = 1;
            foreach(Pair<string,string> info in installation_field)
            {
                label.Text = "安装中(" + prog.Value.ToString() + "/" + prog.Maximum.ToString() + ")...";
                DownloadFile(info.first, installation_path + info.second);
                if (prog.Value < prog.Maximum)
                    ++prog.Value;
                Application.DoEvents();
            }
            label.Text = "完成";
        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        private void DownloadFile(string URL, string filename)
        {
            File.Delete(filename);
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest Myrq = (HttpWebRequest)WebRequest.Create(URL);
                HttpWebResponse myrp = (HttpWebResponse)Myrq.GetResponse();
                Stream st = myrp.GetResponseStream();
                Stream so = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, by.Length);
                    Application.DoEvents();
                }
                so.Close();
                st.Close();
            }
            catch (Exception e)
            {
                label.Text = "错误";
                throw e;
            }
        }
    }
}
