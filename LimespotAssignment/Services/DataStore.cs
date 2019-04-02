using LimespotAssignment.Data;
using LimespotAssignment.Data.Domain.Entities;
using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LimespotAssignment.Services
{
    public class DataStore : IDatastore
    {
        TransformersDbContext _dbContext;

        public DataStore(TransformersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Public Methods

        public async Task<ITransformer> AddTransformerAsync(ITransformer transformer)
        {
            transformer.IsSpecial = await IsSpecialTransformer(transformer.Name);

            await _dbContext.Transformers.AddAsync(transformer as Transformer);
            await SaveChanges();

            return transformer;
        }

        public async Task<List<ITransformer>> GetAllAutobotsAsync()
        {
            return await GetFilteredTransformerListAsync(x => x.Allegiance == TransformerType.Autobots);
        }

        public async Task<List<ITransformer>> GetAllDecepticonsAsync()
        {
            return await GetFilteredTransformerListAsync(x => x.Allegiance == TransformerType.Decepticons);
        }

        public async Task<List<ITransformer>> GetAllTransformersAsync()
        {
            return await GetFilteredTransformerListAsync(x => true);
        }

        public async Task<ITransformer> GetTransformerDetailsAsync(int transformerID)
        {
            return await _dbContext.Transformers
                .Where(x => x.ID == transformerID && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetTransformerScoreAsync(int transformerID)
        {
            var inputParam = new SqlParameter("TransformerID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = transformerID
            };

            var outputParam = new SqlParameter("retVal", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlCommandAsync("EXEC @retVal = GetOverallScore @TransformerID",
                inputParam, outputParam);

            return Convert.ToInt32(outputParam.Value);
        }

        public async Task<bool> RemoveTransformerAsync(int transformerID)
        {
            var transformer = await GetTransformerDetailsAsync(transformerID);

            if (transformer != null)
            {
                transformer.IsActive = false;
                await AttachTransformerObjectAndSave(transformer);
                return true;
            }
            return false;
        }

        public async Task<ITransformer> UpdateTransformerAsync(int transformerID, ITransformer transformer)
        {
            var transformerObject = await GetTransformerDetailsAsync(transformerID);

            if (transformerObject != null)
            {
                transformer.IsSpecial = await IsSpecialTransformer(transformer.Name);
                await AttachTransformerObjectAndSave(transformer);

                return transformer;
            }
            return null;
        }
        #endregion

        #region Private Methods
        async Task<int> AttachTransformerObjectAndSave(ITransformer transformer)
        {
            _dbContext.Transformers.Attach(transformer as Transformer);
            _dbContext.Entry(transformer).State = EntityState.Modified;
            return await SaveChanges();
        }

        async Task<List<ITransformer>> GetFilteredTransformerListAsync(Func<ITransformer, bool> predicate)
        {
            return await Task.FromResult(
                _dbContext.Transformers
                    .Where(x => x.IsActive)
                    .Where(predicate)
                    .OrderBy(x => x.Name)
                    .ToList());
        }

        async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }

        async Task<bool> IsSpecialTransformer(string transformerName)
        {
            var specialNames = await _dbContext.SpecialNames
                .Where(x => x.IsActive)
                .ToListAsync();

            //Check for Special Names.
            if (specialNames.Any(x => x.TransformerName.ToUpper() == transformerName.ToUpper()))
            {
               return true;
            }

            return false;
        }
        #endregion
    }
}