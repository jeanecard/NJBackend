using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetBackEnd.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public Appartenance Appartenance { get; set; }
        
        public String Caption { get; set; }
    }
}
//SELECT * FROM public."GROUP" WHERE "AppartenanceId" = '3841747d-8e40-4de8-acd4-4d2b49475cc3'::uuid