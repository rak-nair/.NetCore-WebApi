using LimespotAssignment.Data.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LimespotAssignment.Models.Input
{
    //Support for update operation. 
    public class TransformerUpdateViewModel
    {
        public TransformerType? Allegiance { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [Range(1, 10)]
        public int? Strength { get; set; }

        [Range(1, 10)]
        public int? Intelligence { get; set; }

        [Range(1, 10)]
        public int? Speed { get; set; }

        [Range(1, 10)]
        public int? Endurance { get; set; }

        [Range(1, 10)]
        public int? Rank { get; set; }

        [Range(1, 10)]
        public int? Courage { get; set; }

        [Range(1, 10)]
        public int? Firepower { get; set; }

        [Range(1, 10)]
        public int? Skill { get; set; }

    }
}
