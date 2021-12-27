using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Common
{
    public class FileUtily
    {
        public static FileUtily Instance => new Lazy<FileUtily>().Value;
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="FileRoute"></param>
        /// <returns></returns>
        public  string  CreateFile(string FileRoute, string FileName)
        {
            FileRoute = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileRoute);
            if (Directory.Exists(FileRoute) == false) Directory.CreateDirectory(FileRoute);
            FileName = Path.Combine(FileRoute, FileName);
            if (File.Exists(FileName) == false)
                File.Create(FileName).Dispose();
            return FileName;
        }

        /// <summary>
        /// 写入文件流
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="DirectoryName"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string DownFile(byte[] bytes,string DirectoryName, string FileName)
        {
            FileName = CreateFile($"Download{Path.DirectorySeparatorChar}{DirectoryName}",  FileName);
            Stream stream = new MemoryStream(bytes);
            FileStream fs = new FileStream(FileName, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(bytes);
            writer.Close();
            fs.Close();
            stream.Close();
            stream.Dispose();
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Download{Path.DirectorySeparatorChar}{DirectoryName}");
        }
    }
}
