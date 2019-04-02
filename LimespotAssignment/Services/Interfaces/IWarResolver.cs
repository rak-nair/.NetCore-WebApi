using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Models.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimespotAssignment.Services.Interfaces
{
    //War resolver for the application
    public interface IWarResolver
    {
        Task<WarResult> WageWarAsync(List<ITransformer> combatants);
    }
}
