using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Zaabee.FastDfsClient;

namespace FileUploader.FastDfs
{
    public class FastDfsClientProvider : IFastDfsClientProvider
    {
        private readonly FastDfsOption _options;

        public FastDfsClientProvider(FastDfsOption options)
        {
            _options = options;
        }

        public FastDfsClient GetClient()
        {
            return new FastDfsClient(new List<IPEndPoint>
            {
                new IPEndPoint(IPAddress.Parse(_options.IpAddress), _options.Port)
            });
        }
    }
}
