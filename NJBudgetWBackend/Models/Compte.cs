using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetBackEnd.Models
{
    public class Compte
    {
        public Group Group { get; set; }
        public float BudgetExpected { get; set; } //Budget mensuel alloué
        public float BudgetConsummed { get; set; } //RemoveOperation and AddOperation
        public float BudgetLeft { get; set; } /// Expected - consumed
        public float BudgetProvision { get; set; } //AddOperation
        public IEnumerable<Operation> Operations { get; set; }
        public CompteStatusEnum State { get; set; }


        public OperationTypeEnum OperationAllowed { get; set; }
    }


}
