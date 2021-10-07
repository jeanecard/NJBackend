using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Repositories.Interface;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Services
{
    public class OperationService : IOperationService
    {
        private IOperationsRepository _opeRepo = null;
        private OperationService()
        {

        }

        public OperationService(IOperationsRepository repo)
        {
            _opeRepo = repo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public async Task AddAsync(Operation operation)
        {
            if(operation == null)
            {
                return;
            }
            operation.Value = Math.Abs(operation.Value);
            using var addTask = _opeRepo.InsertAsync(operation);
            await addTask;
            if(!addTask.IsCompletedSuccessfully)
            {
                throw new Exception("Pourquoi pas une laisse dans le cul, ça irait plus vite !");
            }
        }

        public async Task DeleteAsync(Guid operationid)
        {
            if (operationid == Guid.Empty)
            {
                return;
            }
            using var deleteTask = _opeRepo.DeleteAsync(operationid);
            await deleteTask;
            if (!deleteTask.IsCompletedSuccessfully)
            {
                throw new Exception("Il nous faudrait un plus gros bateau");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Operation operation)
        {
            if (operation == null)
            {
                return;
            }
            operation.Value = -Math.Abs(operation.Value);
            using var removeTask = _opeRepo.InsertAsync(operation);
            await removeTask;
            if (!removeTask.IsCompletedSuccessfully)
            {
                throw new Exception("C'est vous le doc, doc !");
            }
        }
    }
}
