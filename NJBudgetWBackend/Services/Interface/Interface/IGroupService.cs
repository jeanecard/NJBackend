using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Services.Interface
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetGroupsAsync(Guid idAppartenance);
        Task<Compte> GetCompteAsync(Guid idGroup, byte month, ushort year);
    }
}
