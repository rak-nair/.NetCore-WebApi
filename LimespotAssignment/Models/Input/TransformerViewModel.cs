using LimespotAssignment.Data.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LimespotAssignment.Models.Input
{
    //Used to map to the Transformer object.
    //The non-essential int properties are defaulted to 1.
    public class TransformerViewModel
    {
        public TransformerType Allegiance { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }

        [Range(1, 10)]
        public int Strength { get; set; } = 1;

        [Range(1, 10)]
        public int Intelligence { get; set; } = 1;

        [Range(1, 10)]
        public int Speed { get; set; } = 1;

        [Range(1, 10)]
        public int Endurance { get; set; } = 1;

        [Required, Range(1, 10)]
        public int? Rank { get; set; }

        [Range(1, 10)]
        public int Courage { get; set; } = 1;

        [Range(1, 10)]
        public int Firepower { get; set; } = 1;

        [Range(1, 10)]
        public int Skill { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        public bool IsSpecial { get; set; } = false;
    }
}
