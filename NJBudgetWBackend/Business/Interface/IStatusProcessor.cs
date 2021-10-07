using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Business
{
    public interface IStatusProcessor
    {
        CompteStatusEnum ProcessGlobal(IEnumerable<CompteStatusEnum> statuses);
        CompteStatusEnum ProcessGlobalByCategories(IEnumerable<SyntheseDepenseGlobalModelItem> retourData);

        CompteStatusEnum ProcessStateAddOnly(float budgetInitial, float epargne, int count);
        CompteStatusEnum ProcessStateDeleteOnly(float budgetInitial, float depense, int nbMonth, byte jourDuMois);
        CompteStatusEnum ProcessStateAddAndDelete(float budgetInitial, float depense, float provision, int nbMonth);
        CompteStatusEnum ProcessStateNone(float budget, float depense, float epargne);
        CompteStatusEnum ProcessState(OperationTypeEnum operationAllowed, float budgetExpectedByMonth, IEnumerable<IOperation> operations);
    }
}
