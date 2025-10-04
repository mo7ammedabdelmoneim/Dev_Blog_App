using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    public class PostReacts
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
