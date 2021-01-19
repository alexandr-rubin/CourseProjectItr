using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectItr.Models
{
    public class Collection
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Collection Name is required")]
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public string OwnerEmail { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
