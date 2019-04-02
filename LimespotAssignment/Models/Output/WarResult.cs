using LimespotAssignment.Data.Domain.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LimespotAssignment.Models.Output
{
    //Holds the War Result, a string Message and a Collection of individual battle results.
    public class WarResult
    {
        public string Message { get; set; }

        [JsonIgnore]
        public List<BattleRoundResult> BattleResult { get; set; }

        public List<BattleRoundResult> SurvivingAutobots
        {
            get { return BattleResult.Where(x => x.Survivor.Allegiance == TransformerType.Autobots).ToList(); }
        }

        public List<BattleRoundResult> SurvivingDecepticons
        {
            get { return BattleResult.Where(x => x.Survivor.Allegiance == TransformerType.Decepticons).ToList(); }
        }

        public WarResult()
        {
            BattleResult = new List<BattleRoundResult>();
        }
    }
}
