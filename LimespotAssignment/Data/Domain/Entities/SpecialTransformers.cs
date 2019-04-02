using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimespotAssignment.Data.Domain.Entities
{
    //Keeps track of the names of special Transformers.
    public class SpecialTransformerNames
    {
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string TransformerName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
