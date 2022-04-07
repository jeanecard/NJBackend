using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetBackEnd.Models
{
    public interface IOperation
    {
        public DateTime DateOperation { get; set; }
        public float Value { get; set; }
    }

    public class BasicOperation : IOperation
    {
        public DateTime DateOperation { get; set; }
        public float Value { get; set; }
    }

    public class Operation : IOperation
    {
        public Guid? Id { get; set; }
        public Guid CompteId { get; set; }
        public DateTime DateOperation { get; set; }
        public float Value { get; set; }
        public string Caption { get; set; }
        public String User { get; set; }

    }
}
