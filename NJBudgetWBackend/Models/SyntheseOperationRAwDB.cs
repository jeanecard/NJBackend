using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Models
{
    public class SyntheseOperationRAwDB : IOperation
    {
        public Guid CompteId { get; set; }
        public DateTime DateOperation { get; set; }
        public float Value { get; set; }
        public Guid AppartenanceId { get; set; }
        public OperationTypeEnum OperationAllowed { get; set; }
        public float BudgetExpected { get; set; }
    }
}
