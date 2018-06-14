using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileUploader.LocalFs
{
    public class LocalFsProvider : IFileSystemProvider
    {
        private LocalFsOption _option;

        private readonly string subDirectory;

        public LocalFsProvider(LocalFsOption option)
        {
            _option = option;
            subDirectory = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public UploadResult Upload(UploadConfig config)
        {
            //相对路径
            string path = GetFileName(config);

            //根据路径创建一个新的文件，并在指定位置写入字节，如果文件已存在，就追加
            using (FileStream fs = File.OpenWrite(Path.Combine(_option.RootFilePath, path)))
            {
                if (fs.CanWrite)
                {
                    fs.Seek(fs.Length, SeekOrigin.Current);
                    fs.Write(config.Buffer, 0, config.Buffer.Length);
                    fs.Flush();
                    fs.Close();
                }
            }

            return new UploadResult { FilePath = path, OriginalName = config.FileName };
        }

        private string GetFileName(UploadConfig config)
        {
            string path;

            if (string.IsNullOrEmpty(config.PreviosName))
            {
                path = Path.Combine(subDirectory, $"{GuidTo16String()}.{config.FileName}"); //返回的相对路径和文件名

                CheckSavePath();
            }
            else
            {
                path = config.PreviosName;
                if (string.IsNullOrEmpty(config.PreviosName))
                {
                    throw new ArgumentNullException("PreviosName 参数不能为空");
                }
            }

            return path;
        }

        private string CheckSavePath()
        {
            var path = Path.Combine(_option.RootFilePath, subDirectory);

            // 如何路径不存在，就创建文件路径
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
        /// <summary> 
        /// 根据GUID获取16位的唯一字符串 
        /// </summary> 
        /// <param name=\"guid\"></param> 
        /// <returns></returns> 
        private string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}
