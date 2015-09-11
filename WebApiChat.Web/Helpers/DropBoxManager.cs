using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApiChat.Web.Helpers
{
    public static class DropBoxManager
    {
        private const string AccessToken = "uBxMwG9GRMAAAAAAAAAABWFSj5a0CT_YVtngi_sKfigjO1vGkWjKd1lEbVfVyBi3";

        public static async Task Upload(string fileName, byte[] file)
        {
            using (var dbx = new DropboxClient(AccessToken))
            {
                using (var mem = new MemoryStream(file))
                {
                    var updated = await dbx.Files.UploadAsync("/" + fileName,
                        WriteMode.Overwrite.Instance,
                        body: mem);
                }
            }
        }

        public static async Task<byte[]> Download(string file)
        {
            using (var dbx = new DropboxClient(AccessToken))
            {
                using (var response = await dbx.Files.DownloadAsync("/" + file))
                {
                    return await response.GetContentAsByteArrayAsync();
                }
            }
        }
    }
}