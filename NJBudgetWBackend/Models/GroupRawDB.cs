using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Models
{
    public class GroupRawDB
    {
        public Guid Id { get; set; }
        public Guid AppartenanceId { get; set; }
        public String Caption { get; set; }
        public float BudgetExpected { get; set; }
        public OperationTypeEnum OperationAllowed { get; set; }
    }
}
