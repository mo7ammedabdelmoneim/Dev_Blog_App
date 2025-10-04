using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    [Table(name:"Users")]
    public class ApplicationUser:IdentityUser
    {
        public List<Post> Posts { get; set; }=new List<Post>();
        public List<Comment> Comments { get; set; } =new List<Comment>();
    }
}
