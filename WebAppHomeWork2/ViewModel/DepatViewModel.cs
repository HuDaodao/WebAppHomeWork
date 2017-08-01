using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppHomeWork.ViewModel
{
    public class DepatViewModel:BaseViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Note { get; set; }
    }
}