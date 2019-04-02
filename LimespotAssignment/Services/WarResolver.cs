using LimespotAssignment.Data.Domain.Entities;
using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Models.Output;
using LimespotAssignment.Services.Interfaces;
using LimespotAssignment.Services.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimespotAssignment.Services
{
    public class WarResolver : IWarResolver
    {
        #region Public Methods
        public async Task<WarResult> WageWarAsync(List<ITransformer> combatants)
        {
            return await Task<WarResult>.Factory.StartNew(() => { return WageWar(combatants); });
        }
        #endregion

        #region Private Methods
        //Setup to compute war result.
        WarResult WageWar(List<ITransformer> combatants)
        {
            var autobots = combatants
                .Where(x => x.Allegiance == TransformerType.Autobots)
                .OrderByDescending(x => x.Rank)
                .ToList();

            var decepticons = combatants
                .Where(x => x.Allegiance == TransformerType.Decepticons)
                .OrderByDescending(x => x.Rank)
                .ToList();

            var containsSuperAutobot =
                autobots.Exists(x => x.IsSpecial); //Do the Autobots have a special Transformer?
            var containsSuperDecepticon =
                decepticons.Exists(x => x.IsSpecial); //Do the Decepticons have a special Transformer?

            if (containsSuperAutobot && containsSuperDecepticon) //No need to simulate the battle(s), if there are specials on both sides.
            {
                return new WarResult { Message = Responses.NO_RESOLUTION };
            }

            return GetWarResult(autobots, decepticons);
        }

        //Break war into battle rounds
        WarResult GetWarResult(List<ITransformer> autobots, List<ITransformer> decepticons)
        {
            var result = new WarResult();

            //Keep track of the battle round.
            int round = 0;

            bool wageWar = autobots.Count > 0 && decepticons.Count > 0;

            while (wageWar)
            {
                var nextAutobot = autobots.Skip(round).Take(1).First();
                var nextDecpticon = decepticons.Skip(round).Take(1).First();

                result.BattleResult.Add(GetRoundVictor(nextAutobot, nextDecpticon));
                round++;

                if (autobots.Count == round || decepticons.Count == round)//one of the lists has been exhausted, thus, no more battles
                {
                    wageWar = false;
                }
            }

            //Add non-combatants
            AddNonCombatants(autobots, decepticons, round, result.BattleResult);
            
            if (result.BattleResult.Count > 0)
                result.Message = Responses.WAR_RESOLUTION;
            else
                result.Message = Responses.NO_COMBATANTS;

            return result;
        }

        //Add non-combatants
        void AddNonCombatants(List<ITransformer> autobots, List<ITransformer> decepticons, int roundsCompleted, List<BattleRoundResult> battleRoundResults)
        {
            battleRoundResults.AddRange(
                autobots.Skip(roundsCompleted).Select(x => new BattleRoundResult //Add non-combatant Autobots to BattleRoundResult
                {
                    Survivor = x,
                    ResolutionStrategy = Responses.BATTLE_RESOLUTION_NON_COMBATANT
                }));

            battleRoundResults.AddRange(
                decepticons.Skip(roundsCompleted).Select(x => new BattleRoundResult //Add non-combatant Decepticons to BattleRoundResult
                {
                    Survivor = x,
                    ResolutionStrategy = Responses.BATTLE_RESOLUTION_NON_COMBATANT
                }));
        }

        //Get Round victor based on a hierarchy of rules.
        BattleRoundResult GetRoundVictor(ITransformer autobot, ITransformer decepticon)
        {
            var battleResult = CheckForSpecialsCondition(autobot, decepticon);
            if (battleResult != null)
                return battleResult;

            battleResult = CheckStrengthAndCourageCondition(autobot, decepticon);
            if (battleResult != null)
                return battleResult;

            battleResult = CheckSkillCondition(autobot, decepticon);
            if (battleResult != null)
                return battleResult;

            battleResult = CheckRatingCondition(autobot, decepticon);
            if (battleResult != null)
                return battleResult;
            //If we're here, this is the random winner condition.
            return GetRandomWinner(autobot, decepticon);
        }

        //Rule 1
        //If a Transformer is named Optimus or Predaking, the battle ends automatically with
        //them as the victor
        BattleRoundResult CheckForSpecialsCondition(ITransformer first, ITransformer second)
        {
            var resolution = Responses.BATTLE_RESOLUTION_SPECIAL;

            if (first.IsSpecial)
                return new BattleRoundResult { Survivor = first, ResolutionStrategy = resolution };
            else if (second.IsSpecial)
                return new BattleRoundResult { Survivor = second, ResolutionStrategy = resolution };

            return null;
        }

        //Rule 2
        //If Transformer A exceeds Transformer B in strength by 3 or more and Transformer B
        //has less than 5 courage, the battle is won by Transformer A(Transformer B ran away)
        BattleRoundResult CheckStrengthAndCourageCondition(ITransformer first, ITransformer second)
        {
            var strengthDifference = first.Strength - second.Strength;
            var resolution = Responses.BATTLE_RESOLUTION_STRENGTH_AND_COURAGE_DIFFERENCE;

            if (strengthDifference >= 3 && second.Courage < 5)
                return new BattleRoundResult { Survivor = first, ResolutionStrategy = resolution };
            else if (strengthDifference <= -3 && first.Courage < 5)
                return new BattleRoundResult { Survivor = second, ResolutionStrategy = resolution };

            return null;
        }

        //Rule 3
        //If Transformer A’s skill rating exceeds Transformer B's rating by 5 or more, Transformer
        //A wins the fight.
        BattleRoundResult CheckSkillCondition(ITransformer first, ITransformer second)
        {
            var resolution = Responses.BATTLE_RESOLUTION_SKILLDIFFERENCE;
            var skilldifference = first.Skill - second.Skill;

            if (skilldifference >= 5)
                return new BattleRoundResult { Survivor = first, ResolutionStrategy = resolution };
            else if (skilldifference <= -5)
                return new BattleRoundResult { Survivor = second, ResolutionStrategy = resolution };

            return null;
        }

        //Rule 4
        //Otherwise, the victor is whomever of Transformer A and B has the higher overall rating.
        BattleRoundResult CheckRatingCondition(ITransformer first, ITransformer second)
        {
            var resolution = Responses.BATTLE_RESOLUTION_RATINGDIFFERENCE;
            var ratingDifference = first.GetOverallRating()
                                   - second.GetOverallRating();

            if (ratingDifference > 0)
                return new BattleRoundResult { Survivor = first, ResolutionStrategy = resolution };
            else if (ratingDifference < 0)
                return new BattleRoundResult { Survivor = second, ResolutionStrategy = resolution };

            return null;
        }

        //Rule 5
        //(You can determine what to do in the event of a tie between two robots).
        BattleRoundResult GetRandomWinner(ITransformer first, ITransformer second)
        {
            var resolution = Responses.BATTLE_RESOLUTION_RANDOM;
            var rand = new Random();
            var number = rand.Next(1, 3);

            if (number == 1)
                return new BattleRoundResult { Survivor = first, ResolutionStrategy = resolution };
            else
                return new BattleRoundResult { Survivor = second, ResolutionStrategy = resolution };
        }
        #endregion
    }
}