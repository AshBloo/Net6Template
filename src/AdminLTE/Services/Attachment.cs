using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLTE.Services
{
    public class Attachment
    {
        public Attachment(string fileName)
        {
            //Content = content;
            FileName = fileName;
            Type = AttachmentType.Text;
        }

        public enum AttachmentType
        {
            Pdf,
            Doc,
            Json,
            Text
        }

        public object Content { get; set; }
        public string FileName { get; set; }
        public AttachmentType Type { get; set; }

        public async Task<MemoryStream> ContentToStreamAsync()
        {
            string text = FileName.ToString();
            //switch (Type)
            //{
            //    case AttachmentType.Pdf:
            //        text = iformfile(Content).ToString();
            //        break;
            //    case AttachmentType.Doc:
            //        text = Content.ToString();
            //        break;
            //    case AttachmentType.Json:
            //        text = Newtonsoft.Json.JsonConvert.SerializeObject(Content);
            //        break;
            //    case AttachmentType.Text:
            //        text = Content.ToString();
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

            // Would create a string extension method to be able to reuse the following.
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            await writer.WriteAsync(text);
            await writer.FlushAsync();
            stream.Position = 0;
            return stream;
        }
    }
}