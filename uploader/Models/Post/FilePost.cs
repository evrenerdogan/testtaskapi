using System;

namespace uploader.Models.Post
{
    public class FilePost
    {
        public Guid? Uid { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }
        public string Ext { get; set; }
    }
}
