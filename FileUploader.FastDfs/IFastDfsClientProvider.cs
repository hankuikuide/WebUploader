using System;
using System.Collections.Generic;
using System.Text;
using Zaabee.FastDfsClient;

namespace FileUploader.FastDfs
{
    public interface IFastDfsClientProvider
    {
        FastDfsClient GetClient();
    }
}
