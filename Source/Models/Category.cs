using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    public class Category
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(40,MinimumLength =3,ErrorMessage ="Name Must be Between 3 to 30 char")]
        public string Name { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public List<Post>? Posts { get; set; }
    }
}
