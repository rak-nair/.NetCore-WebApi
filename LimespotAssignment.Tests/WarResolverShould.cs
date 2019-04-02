using LimespotAssignment.Data.Domain.Entities;
using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Models.Output;
using LimespotAssignment.Services;
using LimespotAssignment.Services.ResponseMessages;
using System.Collections.Generic;
using Xunit;

namespace LimespotAssignment.Tests
{
    public class WarResolverShould
    {
        WarResolver _sut;

        public WarResolverShould()
        {
            _sut = new WarResolver();
        }

        [Fact]
        public async void War_SpecialsOnBothSides_NoResolution()
        {
            //Specials on both sides
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 1,Rank = 7,Name = "Optimus",IsSpecial = true},
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 2,Rank = 7,Name = "Predaking",IsSpecial = true},
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 3, Rank = 2,Name = "Heist"},
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.NO_RESOLUTION, result.Message);
            Assert.Empty(result.BattleResult);
        }

        [Fact]
        public async void Battle_SpecialTransformerOnASide_WinsRound()
        {
            //Special on one side
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 1, Rank = 7,Name = "Optimus",IsSpecial = true},
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 5, Rank = 9,Name = "Thrust"},
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.WAR_RESOLUTION, result.Message);
            Assert.Single(result.BattleResult);
            Assert.Single(result.SurvivingAutobots);
            Assert.Empty(result.SurvivingDecepticons);
            Assert.Equal(1, result.BattleResult[0].Survivor.ID);
        }

        [Fact]
        public async void Battle_StrengthAndCourageCondition_WinsRound()
        {
            //A decepticon and an autobut win a round each, based on strength difference >=3 and lower strength transformer having courage <5.
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 4, Rank = 7,Name = "Hound", Strength = 4, Courage = 4},//Loses Round 1
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 3, Rank = 5,Name = "Mirage", Strength = 6, Courage = 1},//Wins Round 2
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 5, Rank = 9,Name = "Thrust", Strength = 7, Courage = 4},//Wins Round 1
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 6, Rank = 2,Name = "Acid Storm", Strength = 2, Courage = 4}//Loses Round 2
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.WAR_RESOLUTION, result.Message);
            Assert.Equal(2, result.BattleResult.Count);
            Assert.Single(result.SurvivingAutobots);
            Assert.Single(result.SurvivingDecepticons);
            Assert.Equal(5, result.BattleResult[0].Survivor.ID);
            Assert.Equal(3, result.BattleResult[1].Survivor.ID);
            Assert.Equal(Responses.BATTLE_RESOLUTION_STRENGTH_AND_COURAGE_DIFFERENCE, result.BattleResult[0].ResolutionStrategy);
            Assert.Equal(Responses.BATTLE_RESOLUTION_STRENGTH_AND_COURAGE_DIFFERENCE, result.BattleResult[1].ResolutionStrategy);
        }

        [Fact]
        public async void Battle_SkillCondition_WinsRound()
        {
            //A decepticon and an autobut wins a round each, based on skill difference >=5.
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 4, Rank = 7,Name = "Hound", Skill = 7},//Wins Round 1
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 3, Rank = 5,Name = "Mirage", Skill = 1},//Loses Round 2
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 5, Rank = 9,Name = "Thrust", Skill = 2},//Loses Round 1
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 6, Rank = 2,Name = "Acid Storm", Skill = 6}//Wins Round 2
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.WAR_RESOLUTION, result.Message);
            Assert.Equal(2, result.BattleResult.Count);
            Assert.Single(result.SurvivingAutobots);
            Assert.Single(result.SurvivingDecepticons);
            Assert.Equal(4, result.BattleResult[0].Survivor.ID);
            Assert.Equal(6, result.BattleResult[1].Survivor.ID);
            Assert.Equal(Responses.BATTLE_RESOLUTION_SKILLDIFFERENCE, result.BattleResult[0].ResolutionStrategy);
            Assert.Equal(Responses.BATTLE_RESOLUTION_SKILLDIFFERENCE, result.BattleResult[1].ResolutionStrategy);
        }

        [Fact]
        public async void Battle_HigherRating_WinsRound()
        {
            //An autobut wins the round based on higher overall rating.
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 4, Rank = 7,Name = "Hound", Skill = 2, Courage = 3},//Wins Round 1, Overall Rating - 12
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 5, Rank = 7,Name = "Thrust", Skill = 2, Courage = 2}//Loses Round 1, Overall Rating - 11
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.WAR_RESOLUTION, result.Message);
            Assert.Single(result.BattleResult);
            Assert.Single(result.SurvivingAutobots);
            Assert.Empty(result.SurvivingDecepticons);
            Assert.Equal(4, result.BattleResult[0].Survivor.ID);
            Assert.Equal(Responses.BATTLE_RESOLUTION_RATINGDIFFERENCE, result.BattleResult[0].ResolutionStrategy);
        }

        [Fact]
        public async void Battle_AllPropertiesEqual_RandomWinner()
        {
            //Random winner, since everthing is equal.
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 4, Rank = 7,Name = "Hound", Skill = 2, Courage = 2},
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 5, Rank = 7,Name = "Thrust", Skill = 2, Courage = 2}
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.WAR_RESOLUTION, result.Message);
            Assert.Single(result.BattleResult);
            Assert.Contains(result.BattleResult, x => x.Survivor.ID == 4 || x.Survivor.ID == 5);
            Assert.Equal(Responses.BATTLE_RESOLUTION_RANDOM, result.BattleResult[0].ResolutionStrategy);
        }

        [Fact]
        public async void Battle_AddNonCombatantsToResult()
        {
            //A random winner and 2 autobot non combatants end up as survivors.
            var transformerList = new List<ITransformer>
            {
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 4, Rank = 7,Name = "Hound", Skill = 2, Courage = 2},//First round fighter
                new Transformer{ Allegiance = TransformerType.Decepticons, ID = 5, Rank = 7,Name = "Thrust", Skill = 2, Courage = 2},//First round fighter
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 8, Rank = 4,Name = "Hoist"},//Non-combatant
                new Transformer{ Allegiance = TransformerType.Autobots, ID = 9, Rank = 5,Name = "Inferno"}//Non-combatant
            };

            var result = await _sut.WageWarAsync(transformerList);

            Assert.IsType<WarResult>(result);
            Assert.Equal(Responses.WAR_RESOLUTION, result.Message);
            Assert.Equal(3, result.BattleResult.Count);//Two non-combatants and the winner of the first round
            Assert.Contains(result.BattleResult, x => x.Survivor.ID == 8);
            Assert.Equal(Responses.BATTLE_RESOLUTION_NON_COMBATANT, result.BattleResult[1].ResolutionStrategy);
            Assert.Contains(result.BattleResult, x => x.Survivor.ID == 9);
            Assert.Equal(Responses.BATTLE_RESOLUTION_NON_COMBATANT, result.BattleResult[2].ResolutionStrategy);
        }
    }
}