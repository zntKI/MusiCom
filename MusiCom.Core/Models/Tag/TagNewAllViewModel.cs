using MusiCom.Infrastructure.Data.Entities.News;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Tag
{
    /// <summary>
    /// Contains Data for the Collection of Tags in the NewAddViewModel
    /// </summary>
    public class TagNewAllViewModel
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; }

        public bool IsDeleted { get; set; }
    }
}
