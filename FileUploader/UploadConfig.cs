using System;
using System.Collections.Generic;
using System.Text;

namespace FileUploader
{
    public class UploadConfig
    {
        /// <summary>
        /// 上一次上传时生成的服务器端文件名，如果需要分段上传，需传入此文件名
        /// </summary>
        public string PreviosName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件字节流
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 是否分段上传
        /// </summary>
        public bool Chunked { get; set; }

        /// <summary>
        /// DFS分组名
        /// </summary>
        public string GroupName { get; set; } = "group1";
    }
}
