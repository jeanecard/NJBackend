using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Services.Interface.Interface
{
    public interface IOperationService
    {
        public Task AddAsync(Operation operation);
        public Task RemoveAsync(Operation operation);
        public Task DeleteAsync(Guid operationid);


    }
}
