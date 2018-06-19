﻿using System;
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
        public List<Pair<string, string>> installation_field = new List<Pair<string, string>>();
        public string installation_path;
        public string repo_url;
        public void Install(bool clean = false,bool force=false)
        {
            if (clean && Directory.Exists(installation_path))
                Directory.Delete(installation_path, true);
            Directory.CreateDirectory(installation_path);
            Directory.CreateDirectory(installation_path + "\\bin");
            Directory.CreateDirectory(installation_path + "\\lib");
            Directory.CreateDirectory(installation_path + "\\imports");
            Directory.CreateDirectory(installation_path + "\\logs");
            Directory.CreateDirectory(installation_path + "\\docs");
            label.Text = "获取组件信息中，请稍候...";
            Application.DoEvents();
            if (!Environment.Is64BitOperatingSystem)
                MessageBox.Show("警告！您正在使用32位操作系统。\n由于32位操作系统无法支持SEH，Covariant Script的性能可能受限。\n要获得更好的体验，请升级至最新操作系统！", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            string dest_url = DownloadText(repo_url + "/covscript_distribution/destination_url.txt");
            string[] urls = RemoveSpace(DownloadText(repo_url + (Environment.Is64BitOperatingSystem && !force ? "/covscript_distribution/windows_x86_64.txt" : "/covscript_distribution/windows_x86_32.txt"))).Split(';');
            foreach (string url in urls)
            {
                string[] info = url.Split('@');
                if (info.Length == 2)
                    installation_field.Add(new Pair<string, string>(info[0], info[1]));
            }
            prog.Maximum = installation_field.Count;
            prog.Value = 1;
            foreach (Pair<string, string> info in installation_field)
            {
                label.Text = "安装中(" + prog.Value.ToString() + "/" + prog.Maximum.ToString() + ")...";
                DownloadFile(dest_url + info.first, installation_path + info.second);
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
        private string RemoveSpace(string str)
        {
            string nstr = "";
            StringReader reader = new StringReader(str);
            while (reader.Peek() > -1)
                nstr += reader.ReadLine();
            return nstr;
        }
        private string DownloadText(string URL)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest Myrq = (HttpWebRequest)WebRequest.Create(URL);
                HttpWebResponse myrp = (HttpWebResponse)Myrq.GetResponse();
                StreamReader sr = new StreamReader(myrp.GetResponseStream());
                return sr.ReadToEnd();
            }
            catch (Exception e)
            {
                label.Text = "错误";
                throw e;
            }
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
