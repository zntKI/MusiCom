using MusiCom.Infrastructure.Data.Entities.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    public class Image
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        public byte[] Photo { get; set; }

        [Required]
        public string ContentType { get; set; }

        public New? New { get; set; }

        public EventPost? EventPost { get; set; }

        public Event? Event { get; set; }

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
