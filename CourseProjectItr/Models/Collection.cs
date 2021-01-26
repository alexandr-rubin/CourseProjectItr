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
        [Required(ErrorMessage = "Collection Theme is required")]
        public string Theme { get; set; }
        public string Description { get; set; }
        public string OwnerEmail { get; set; }
        public bool IntField1 { get; set; }
        public bool IntField2 { get; set; }
        public bool IntField3 { get; set; }
        public bool OneLineField1 { get; set; }
        public bool OneLineField2 { get; set; }
        public bool OneLineField3 { get; set; }
        public bool TextField1 { get; set; }
        public bool TextField2 { get; set; }
        public bool TextField3 { get; set; }
        public bool DateField1 { get; set; }
        public bool DateField2 { get; set; }
        public bool DateField3 { get; set; }
        public bool BoolField1 { get; set; }
        public bool BoolField2 { get; set; }
        public bool BoolField3 { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
