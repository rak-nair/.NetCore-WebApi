using LimespotAssignment.Data.Domain.Entities;
using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Models.Input;

namespace LimespotAssignment.Models
{
    //Helps with Model to and from ViewModel transitions.
    public class ModelFactory
    {
        #region Public Methods
        //Creates ITransformer from TransformerViewModel
        public ITransformer Create(TransformerViewModel model)
        {
            return new Transformer
            {
                Allegiance = model.Allegiance,
                Name = model.Name,
                Strength = model.Strength,
                Intelligence = model.Intelligence,
                Speed = model.Speed,
                Endurance = model.Endurance,
                Rank = model.Rank.Value,
                Courage = model.Courage,
                Firepower = model.Firepower,
                Skill = model.Skill,
                IsActive = model.IsActive
            };
        }

        //Update the ITransformer object based on partial updates, simulating a PATCH operation.
        public ITransformer UpdateTransformer(TransformerUpdateViewModel model, Transformer transformer)
        {
            UpdateMatchingProperties(model, transformer);
            return transformer;
        }
        #endregion

        #region Private Methods
        //Loop thorugh and copy values for all non null properties to the ITransformer object
        void UpdateMatchingProperties(TransformerUpdateViewModel model, Transformer transformer)
        {
            var modelProperties = model.GetType().GetProperties();

            foreach (var property in modelProperties)
            {
                var valueToCopy = property.GetValue(model);

                if (valueToCopy != null)
                {
                    var propertyToUpdate = transformer.GetType().GetProperty(property.Name);
                    propertyToUpdate.SetValue(transformer, valueToCopy);
                }
            }
        }   
        #endregion
    }
}
