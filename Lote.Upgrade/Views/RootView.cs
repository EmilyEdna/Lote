using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.HttpFramework.MultiOption;
using XExten.Advance.StaticFramework;


namespace Lote.Upgrade.Views
{
    [Obfuscation(Feature = "virtualization", Exclude = false)]
    public partial class RootView
    {
        private readonly string UpgradeURL = "https://hub.fastgit.org/EmilyEdna/ResouceFile/raw/main/LoteFile.zip";
        private long Long = 0;
        private void GetUpgradeFile()
        {
            var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "UpdateFile"));
            var fn = Path.Combine(dir, "LoteFile.zip");
            SyncStatic.DeleteFile(fn);
            try
            {
                WebClient client = new WebClient();
                client.DownloadDataAsync(new Uri(UpgradeURL));
                client.DownloadProgressChanged += (c, e) =>
                {
                    Grade.Value = double.Parse((e.BytesReceived * 100.00 / e.TotalBytesToReceive).ToString("F2"));
                };
                client.DownloadDataCompleted += (c, e) =>
                {
                    SyncStatic.WriteFile(e.Result, fn);
                };
                ExtractZip(fn);
            }
            catch (Exception)
            {
                var result = MessageBox.Show("已是最新版本!", "通知", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
        private void ExtractZip(string fn)
        {
            ZipFile.ExtractToDirectory(fn, Environment.CurrentDirectory, true);
            OpenExe();
        }
        private void OpenExe()
        {
            Process process = new Process();
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Lote.exe";
            process.StartInfo.CreateNoWindow = true;
            process.Start();//启动
            process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
            process.Close();//释放与此组件关联的所有资源
            Environment.Exit(0);
        }
    }
}
