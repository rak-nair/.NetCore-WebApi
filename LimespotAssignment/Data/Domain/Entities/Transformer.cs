using LimespotAssignment.Data.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LimespotAssignment.Data.Domain.Entities
{
    public class Transformer : ITransformer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public TransformerType Allegiance { get; set; }

        [Required]
        
        public string Name { get; set; }

        public int Strength { get; set; }

        public int Intelligence { get; set; }

        public int Speed { get; set; }

        public int Endurance { get; set; }

        [Required]
        public int Rank { get; set; }

        public int Courage { get; set; }

        public int Firepower { get; set; }

        public int Skill { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsSpecial { get; set; }

        public int GetOverallRating()
        {
            return Strength + Intelligence + Speed + Endurance + Rank + Courage + Firepower + Skill;
        }
    }
}
