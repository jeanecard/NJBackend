using NJBudgetWBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Services.Interface.Interface
{
    public interface ISyntheseService
    {
        Task<SyntheseDepenseGlobalModel> GetSyntheseByAppartenanceAsync(byte month);
        Task<SyntheseDepenseByAppartenanceModel> GetSyntheseForAppartenanceAsync(Guid appartenanceId, byte month);
        Task<SyntheseMoisModel> GetSyntheseGlobal(byte month);
    }
}
