using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIConsume.ViewModels
{
    public class VmEmployee
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Image { get; set; }
        public string ImageBase64 { get; set; }
        public IFormFile ImageFile { get; set; }
        [MaxLength(30)]
        public string Surname { get; set; }
        public byte Age { get; set; }
        public bool IsMarried { get; set; }
        public int PositinId { get; set; }
        public VmPosition Position { get; set; }
    }
}
