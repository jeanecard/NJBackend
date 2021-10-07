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

//INSERT INTO public."OPERATION"(
//    "Id", "CompteId", "DateOperation", "Value", "Caption")

//    VALUES(
//        '3841747d-8e40-4de8-acd4-4d2b49475c31',
//        '3841747d-8e40-4de8-acd4-4d2b49475c30',
//        '10/25/2021',
//        50,
//        'test');