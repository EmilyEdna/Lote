using System;
using System.Collections.Generic;
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
        internal  string  CreateFile(string FileRoute, string FileName)
        {
            FileRoute = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileRoute);
            if (Directory.Exists(FileRoute) == false) Directory.CreateDirectory(FileRoute);
            FileName = Path.Combine(FileRoute, FileName);
            if (File.Exists(FileName) == false)
                File.Create(FileName).Dispose();
            return FileName;
        }
    }
}
