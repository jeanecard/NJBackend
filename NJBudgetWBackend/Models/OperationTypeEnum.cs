using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetBackEnd.Models
{
    public enum OperationTypeEnum
    {
        AddOnly = 1,
        DeleteOnly = 2,
        AddAndDelete = 3,
        None = 4
    }
}
