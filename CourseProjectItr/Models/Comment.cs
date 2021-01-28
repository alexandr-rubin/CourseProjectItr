using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectItr.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserComment { get; set; }
        public string CommentAuthor { get; set; }
        public int FileModelId { get; set; }
    }
}
