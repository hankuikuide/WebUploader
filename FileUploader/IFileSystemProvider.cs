using System;
using System.Collections.Generic;
using System.Text;

namespace FileUploader
{
    public interface IFileSystemProvider
    {
        //文件上传
        UploadResult Upload(UploadConfig config);
        
    }
}
