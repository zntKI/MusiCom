using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusiCom.Infrastructure.Data.Entities
{
    public class Editor
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime HireDate { get; set; }

        [Column(TypeName = "DECIMAL(7, 2)")]
        public decimal Salary { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }

    //HireDate smalldatetime NOT NULL,
    //Salary money NOT NULL,
}
