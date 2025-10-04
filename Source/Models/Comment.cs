using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    public class Comment
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Content { get; set; }


        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
