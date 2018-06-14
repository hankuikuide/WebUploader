using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zaabee.FastDfsClient;

namespace FileUploader.FastDfs
{
    public class FastDfsProvider : IFileSystemProvider
    {
        private FastDfsClient _client;
        private IFastDfsClientProvider _provider;

        public FastDfsProvider(FastDfsOption option)
        {
            _provider = new FastDfsClientProvider(option);
            _client = _provider.GetClient();
        }


        public UploadResult Upload(UploadConfig config)
        {
            if (string.IsNullOrEmpty(config.PreviosName))
            {
                var fileName = string.Empty;
                var storageNode = _client.GetStorageNode(config.GroupName);

                if (!config.Chunked)
                {
                    fileName = _client.UploadFile(storageNode, config.Buffer, Path.GetExtension(config.FileName));
                }
                else
                {
                    //分段上传需要调用这个方法
                    fileName = _client.UploadAppenderFile(storageNode, config.Buffer, Path.GetExtension(config.FileName));
                }

                return new UploadResult { FilePath = fileName, OriginalName = config.FileName };
            }
            else
            {
                //分段上传：需要提供上传GroupName, 文件上传地址PreviosName，文件上传内容filebody
                //续传 地址config.PreviosName
                _client.AppendFile(config.GroupName, config.PreviosName, config.Buffer);

                return new UploadResult { FilePath = config.PreviosName, OriginalName = config.FileName };
            }
        }
    }
}
