using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiChat.Models.Enums;
using WebApiChat.Models.Models;
using WebApiChat.Web.Helpers;
using WebApiChat.Web.Hubs;
using System.IO.Compression;

namespace WebApiChat.Web.Controllers
{
    public class FileController : ApiControllerWithHub<BaseHub>
    {
        public IHttpActionResult Post()
        {
            var httpRequest = HttpContext.Current.Request;
            var file = httpRequest.Files[0];
            var fileExtension = file.FileName.Split('.').Last();
            var uniqueName = this.CurrentUserId + Guid.NewGuid() + "." + fileExtension;

            byte[] buffer = null;
            using (var fs = file.InputStream)
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }

            var task = Task.Run(() => DropBoxManager.Upload(uniqueName, buffer));
            task.Wait();

            var link = "http://localhost:3660/api/File?fileName=" + uniqueName;
            var receiverId = this.CurrentUser.CurrentChatId;
            var receiver = this.Data.Users.All()
                .FirstOrDefault(u => u.Id == receiverId);
            var message = new PrivateMessage(){
                Text = link,
                ReceiverId = receiverId,
                SenderId = this.CurrentUserId,
                IsFileLink = true
            };

            var currentUsers = ConnectionManager.Users.Keys;
            if (currentUsers.Contains(receiver.UserName) && receiver.CurrentChatId == this.CurrentUser.Id)
            {
                message.Status = MessageStatus.Seen;
            }
            else if (currentUsers.Contains(receiver.UserName))
            {
                message.Status = MessageStatus.Sent;
            }
            else
            {
                message.Status = MessageStatus.NotDelivered;
            }

            this.Data.Messages.Add(message);
            this.Data.SaveChanges();

            var messageView =
                    new
                    {
                        message.Id,
                        message.Text,
                        Sender = this.CurrentUserUserName,
                        Receiver = receiver.UserName,
                        Status = message.Status.ToString(),
                        SenderId = this.CurrentUserId,
                        ReceiverId = receiver.Id,
                        MessageId = message.Id,
                        IsFileLink = true
                    };
            this.HubContex.Clients.User(receiver.UserName).pushMessageToClient(messageView);
            this.HubContex.Clients.User(this.CurrentUserUserName).pushSelfMessage(messageView);

            return this.Ok();
        }

        public HttpResponseMessage GetFile(string fileName)
        {
            HttpResponseMessage result = null;
            var task = Task.Run(() => DropBoxManager.Download(fileName));
            task.Wait();

            result = Request.CreateResponse(HttpStatusCode.OK);

            result.Content = new StreamContent(new MemoryStream(task.Result));
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = fileName;

            return result;
        }
    }
}