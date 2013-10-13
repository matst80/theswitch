using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheSwitch.Server
{
    public class CacheObject
    {
        public CacheObject(string url, FileInfo fi)
        {
            if (fi.Exists)
            {
                Url = url.ToLower();
                Data = new byte[fi.Length];
                _length = (int)fi.Length;
                using (var fs = fi.OpenRead())
                {
                    fs.Read(Data, 0, (int)fi.Length);
                }
                if (url.Contains(".htm"))
                {
                    ContentType = "text/html";
                }
                else if (url.Contains(".js"))
                {
                    ContentType = "application/javascript";
                }
                else if (url.Contains(".css"))
                {
                    ContentType = "text/css";
                }
                else if (url.Contains(".jpg"))
                {
                    ContentType = "image/jpeg";
                }
                else if (url.Contains(".svg"))
                {
                    ContentType = "image/svg+xml";
                }
                else if (url.Contains(".png"))
                {
                    ContentType = "image/png";
                }
                else if (url.Contains(".gif"))
                {
                    ContentType = "image/gif";
                }
            }
        }

        private int? _length;

        public string Url { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public void SendToStream(HttpListenerResponse res)
        {
            res.ContentLength64 = Length;
            res.ContentType = ContentType;
            res.OutputStream.Write(Data, 0, Length);
        }

        public int Length
        {
            get
            {
                if (_length == null)
                {
                    _length = Data.Length;
                }
                return (int)_length;
            }
        }
    }

    public class ServerCache
    {

        private List<CacheObject> cache;
        private string _baseDir;

        public ServerCache(string baseDir)
        {
            cache = new List<CacheObject>();
            _baseDir = baseDir.TrimEnd('\\');
        }

        public void ServeUrl(Uri url, HttpListenerResponse res)
        {
            var co = cache.FirstOrDefault(d => d.Url.Equals(url.LocalPath, StringComparison.InvariantCultureIgnoreCase));
            if (co == null || true)
            {
                var newfi = new FileInfo(_baseDir + url.LocalPath.Replace("/", "\\"));
                if (!newfi.Exists)
                {
                    res.StatusCode = 404;
                    return;
                }

                co = new CacheObject(url.LocalPath, newfi);
                cache.Add(co);
            }
            co.SendToStream(res);
        }
    }
}
