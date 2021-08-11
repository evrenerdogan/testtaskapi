using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace uploader.Data
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uid { get; set; }

        public string Name { get; set; }

        public string Ext { get; set; }

        public string ContentType { get; set; }

        public string Link { get; set; }

        public string Content { get; set; }

        public int Size { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
