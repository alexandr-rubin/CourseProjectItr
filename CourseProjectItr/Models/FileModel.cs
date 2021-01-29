using CourseProjectItr.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectItr.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Tags { get; set; }
        public int IntField1 { get; set; }
        public int IntField2 { get; set; }
        public int IntField3 { get; set; }
        public string OneLineField1 { get; set; }
        public string OneLineField2 { get; set; }
        public string OneLineField3 { get; set; }
        public string TextField1 { get; set; }
        public string TextField2 { get; set; }
        public string TextField3 { get; set; }
        public DateTime DateField1 { get; set; }
        public DateTime DateField2 { get; set; }
        public DateTime DateField3 { get; set; }
        public bool BoolField1 { get; set; }
        public bool BoolField2 { get; set; }
        public bool BoolField3 { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<ApplicationUser> Likes { get; set; } = new List<ApplicationUser>();
        public int LikesNumber { get; set; } = 0;
        public int CollectionId { get; set; }
    }
}
