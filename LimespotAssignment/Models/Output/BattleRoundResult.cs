using LimespotAssignment.Data.Domain.Interfaces;

namespace LimespotAssignment.Models.Output
{
    //Holds the result of a battle round.
    public class BattleRoundResult
    {
        public string ResolutionStrategy { get; set; }
        public ITransformer Survivor { get; set; }
    }
}
