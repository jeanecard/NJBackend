using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Business;
using NJBudgetWBackend.Business.Interface;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Repositories.Interface;
using NJBudgetWBackend.Services.Interface;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Services
{
    public class GroupService : IGroupService
    {
        private IGroupRepository _groupRepo = null;
        private IOperationsRepository _opeRepo = null;
        private IAppartenanceService _appartenanceService = null;
        private IBudgetProcessor _budgetProcessor = null;
        private IStatusProcessor _statusProcessor = null;
        private GroupService()
        {
            //Dummy for DI.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="apService"></param>
        public GroupService(
            IGroupRepository repo,
            IAppartenanceService apService,
            IOperationsRepository opRepo,
            IBudgetProcessor budgetProcessor,
            IStatusProcessor statusProcessor)
        {
            _groupRepo = repo;
            _appartenanceService = apService;
            _opeRepo = opRepo;
            _budgetProcessor = budgetProcessor;
            _statusProcessor = statusProcessor;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idAppartenance"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync(Guid idAppartenance)
        {
            if (idAppartenance == Guid.Empty)
            {
                return null;
            }
            using var rawGroupsTask = _groupRepo.GetGroupsByAppartenanceAsync(idAppartenance);
            await rawGroupsTask;
            List<Group> retour = new List<Group>();
            if (rawGroupsTask.IsCompletedSuccessfully)
            {
                foreach (GroupRawDB iter in rawGroupsTask.Result)
                {
                    var ap = _appartenanceService.GetById(iter.AppartenanceId);
                    retour.Add(new Group() { Appartenance = ap, Caption = iter.Caption, Id = iter.Id });
                }
            }
            else
            {
                throw new Exception("J'aime combattre, voir mes ennemis mourrir sous mes pieds.");
            }
            return retour;
        }
        /// <summary>
        /// 1-
        /// </summary>
        /// <param name="idGroup"></param>
        /// <returns></returns>
        public async Task<Compte> GetCompteAsync(Guid idGroup, byte month, ushort year)
        {
            if (month == 0 || month > 12 || year < 2020)
            {
                return null;
            }

            DateTime thisMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime thisMonthEnd = thisMonthStart.AddMonths(1).AddDays(-1);

            Compte retour = new Compte();
            //1- GetGroup Header
            using var compteTask = _groupRepo.GetCompteHeader(idGroup);
            await compteTask;
            if (compteTask.IsCompletedSuccessfully && compteTask.Result != null)
            {
                var appartenance = _appartenanceService.GetById(compteTask.Result.AppartenanceId);
                GroupRawDB groupRaw = compteTask.Result;
                retour.Group = null; //TODO
                retour.OperationAllowed = groupRaw.OperationAllowed;
                retour.BudgetExpected = groupRaw.BudgetExpected;
                retour.Group = new Group()
                {
                    Appartenance = new Appartenance() { Id = groupRaw.AppartenanceId, Caption = appartenance?.Caption },
                    Caption = groupRaw.Caption,
                    Id = groupRaw.Id
                };
                //3- GetOperations du mois
                using var operationsDuMoisTask = _opeRepo.GetOperationsAsync(idGroup, thisMonthStart, thisMonthEnd);
                await operationsDuMoisTask;
                if (operationsDuMoisTask.IsCompletedSuccessfully)
                {
                    retour.Operations = operationsDuMoisTask.Result;
                    //4- Update budget left and spent for this month
                    var budgetData = _budgetProcessor.ProcessBudgetSpentAndLeft(groupRaw.BudgetExpected, retour.Operations, month, year);
                    retour.BudgetConsummed = budgetData.budgetConsomme;
                    retour.BudgetLeft = budgetData.budgetRestant;
                    retour.BudgetProvision = budgetData.budgetProvisonne;
                    //5- Update state of compte based on this month operations
                    using var operations6DerniersMoisTask = _opeRepo.GetOperationsAsync(idGroup, thisMonthStart.AddMonths(-6), thisMonthEnd);
                    await operations6DerniersMoisTask;
                    if (operations6DerniersMoisTask.IsCompletedSuccessfully)
                    {
                        var state = _statusProcessor.ProcessState(retour.OperationAllowed, retour.BudgetExpected, operations6DerniersMoisTask.Result);
                        retour.State = state;
                    }
                    else
                    {
                        throw new Exception("La magie c'est pour les femmes");
                    }
                }
                else
                {
                    throw new Exception("Je sens venir le tas de cendres ..");
                }
            }
            else
            {
                return null; 
            }
            return retour;
        }
    }
}
