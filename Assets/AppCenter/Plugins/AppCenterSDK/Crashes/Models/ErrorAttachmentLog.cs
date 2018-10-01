using System;

namespace Microsoft.AppCenter.Unity.Crashes
{
    public class ErrorAttachmentLog
    {
        public string Text { get; private set; }
        public byte[] Data { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public AttachmentType Type { get; private set; }

        public static ErrorAttachmentLog AttachmentWithText(string text, string fileName)
        {
            var attachment = new ErrorAttachmentLog();
            attachment.Text = text;
            attachment.FileName = fileName;
            attachment.Type = AttachmentType.Text;
            return attachment;
        }

        public static ErrorAttachmentLog AttachmentWithBinary(byte[] data, string fileName, string contentType)
        {
            var attachment = new ErrorAttachmentLog();
            attachment.Data = data;
            attachment.FileName = fileName;
            attachment.ContentType = contentType;
            attachment.Type = AttachmentType.Binary;
            return attachment;
        }

        public enum AttachmentType { Text, Binary }
    }
}
