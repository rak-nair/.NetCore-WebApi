using LimespotAssignment.Data.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimespotAssignment.Services.Interfaces
{
    //Datastore for the application
    public interface IDatastore
    {
        Task<ITransformer> AddTransformerAsync(ITransformer transformer);

        Task<ITransformer> GetTransformerDetailsAsync(int transformerID);

        Task<ITransformer> UpdateTransformerAsync(int transformerid, ITransformer transformer);

        Task<bool> RemoveTransformerAsync(int transformerID);

        Task<List<ITransformer>> GetAllAutobotsAsync();

        Task<List<ITransformer>> GetAllDecepticonsAsync();

        Task<int> GetTransformerScoreAsync(int transformerID);

        Task<List<ITransformer>> GetAllTransformersAsync();
    }
}
