namespace LimespotAssignment.Services.ResponseMessages
{
    //Hols response messages.
    public class Responses
    {
        #region General Responses
        public const string NO_RESOLUTION = "The War cannot be resolved since there are special transformers on either side!";
        public const string WAR_RESOLUTION = "Here are the results of the war -";
        public const string NO_COMBATANTS = "No transformers have been set up for the war.";
        public const string ERROR_INVALID_INPUTS_ADD_TRANSFORMER = "Invalid inputs, please ensure that the Name and Rank have a value and that all other integer fields have a value between 1 and 10.";
        public const string INVALID_TRANSFORMER_ID = "Invalid Transformer ID supplied - the ID may not exist or it may have been removed.";
        #endregion

        #region Battle-specific Responses
        public const string BATTLE_RESOLUTION_SPECIAL = "Special Transformer wins.";
        public const string BATTLE_RESOLUTION_STRENGTH_AND_COURAGE_DIFFERENCE = "Strength Difference of 3 and courage less than 5.";
        public const string BATTLE_RESOLUTION_SKILLDIFFERENCE = "Skill Difference of 5.";
        public const string BATTLE_RESOLUTION_RATINGDIFFERENCE = "Rating Difference.";
        public const string BATTLE_RESOLUTION_RANDOM = "Random Winner.";
        public const string BATTLE_RESOLUTION_NON_COMBATANT = "Non-combatant.";
        #endregion
    }
}
