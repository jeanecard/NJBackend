using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Models;
using System.Collections.Generic;

namespace NJBudgetWBackend.Business.Interface
{
    public interface IBudgetProcessor
    {
        void ProcessBudgetSpentAndLeft(
            out float budgetConsomme,
            out float budgetProvisonne,
            out float budgetRestant,
            in float budgetExpected, 
            IEnumerable<IOperation> operations, 
            byte month, 
            ushort year);
        

        SyntheseDepenseGlobalModel ProcessSyntheseOperations(
            IEnumerable<SyntheseOperationRAwDB> operations,
            IEnumerable<GroupRawDB> comptes,
            byte month,
            ushort year);
    }
}
