using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Repositories.Interface
{
    public interface IGroupRepository
    {
        Task<IEnumerable<GroupRawDB>> GetGroupsByAppartenanceAsync(Guid id);
        Task<IEnumerable<GroupRawDB>> GetGroupsAsync();
        Task<GroupRawDB> GetCompteHeader(Guid idGroup);
        
    }
}
