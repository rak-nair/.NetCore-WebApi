using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LimespotAssignment.Data.Domain.Entities;

namespace LimespotAssignment.Data.Domain.Interfaces
{
    public interface ITransformer
    {
        int ID { get; set; }

        TransformerType Allegiance { get; set; }

        string Name { get; set; }

        int Strength { get; set; }

        int Intelligence { get; set; }

        int Speed { get; set; }

        int Endurance { get; set; }

        int Rank { get; set; }

        int Courage { get; set; }

        int Firepower { get; set; }

        int Skill { get; set; }

        bool IsActive { get; set; }

        bool IsSpecial { get; set; }

        int GetOverallRating();
    }
}
