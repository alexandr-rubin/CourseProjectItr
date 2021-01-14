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
        public int CollectionId { get; set; }
    }
}
