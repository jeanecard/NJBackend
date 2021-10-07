using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Business.Interface;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Repositories.Interface;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Collections.Generic;

//Au debut du oi son donne l'argent a tous les comptes (opération epargne) en cous du ois on fait les depenses.
namespace NJBudgetWBackend.Business
{
    public class BudgetProcessor : IBudgetProcessor
    {
        private IAppartenanceService _apService = null;
        private IStatusProcessor _statusProcessor = null;
        private BudgetProcessor()
        {
        }
        public BudgetProcessor(
            IAppartenanceService apService, 
            IStatusProcessor sProcessor)
        {
            _apService = apService;
            _statusProcessor = sProcessor;
        }
        /// <summary>
        /// Calcul le budget consommé, épargné et restant sur le mois month de l'année year.
        /// </summary>
        /// <param name="compte"></param>
        /// <param name="operations"></param>
        /// <param name="month"></param>
        public void ProcessBudgetSpentAndLeft(
            out float budgetConsomme,
            out float budgetProvisonne,
            out float budgetRestant,
            in float budgetExpected,
            IEnumerable<IOperation> operations, 
            byte month, 
            ushort year)
        {
            if (month == 0 || month > 12 || budgetExpected < 0)
            {
                throw new ArgumentException("Ah ah, ils vous ont refiler toutes leurs merdes");
            }
            budgetConsomme = 0;
            budgetProvisonne = 0;
            budgetRestant = budgetExpected;
            if (operations != null)
            {
                foreach (IOperation iter in operations)
                {
                    if (iter.DateOperation.Month == month && iter.DateOperation.Year == year)
                    {
                        budgetConsomme += Math.Abs(iter.Value);
                        if (iter.Value > 0)
                        {
                            budgetProvisonne += iter.Value;
                        }
                    }
                }
                budgetRestant = budgetExpected - budgetConsomme;
            }
        }
 
        public SyntheseDepenseGlobalModel ProcessSyntheseOperations(
            IEnumerable<SyntheseOperationRAwDB> operations,
            IEnumerable<GroupRawDB> groups,
            byte month,
            ushort year)
        {
            if (operations == null || month == 0 || month > 12 || groups == null)
            {
                return null;
            }
            Dictionary<Guid, (OperationTypeEnum, float)> operationAndBudgetMap = CreateOperationAndBudgetMap(groups);
            List<SyntheseDepenseGlobalModelItem> retourData = new List<SyntheseDepenseGlobalModelItem>();
            Dictionary<Guid, Dictionary<Guid, List<IOperation>>> operationsByCompteByAppartenance = InitOperationsByCompteByAppartenance(groups);
            List<CompteStatusEnum> statusesByCategories = new List<CompteStatusEnum>();

            //1- Regroupement des opérations apparteance et par compte
            foreach (SyntheseOperationRAwDB iter in operations)
            {
                //1.1- Récupération ou réation de la liste des opération du compte de l'appartenance
                List<IOperation> iterOperations = operationsByCompteByAppartenance[iter.AppartenanceId][iter.CompteId];
                //1.2- Ajout de l'opération.
                iterOperations.Add(new BasicOperation()
                {
                    DateOperation = iter.DateOperation,
                    Value = iter.Value
                });
            }
            //2- Pour chaque appartenance, calcul du budget alloué et dépensé 
            // qui correspond a la somme de chacune de ces propriétés sur les comptes de l'appartenance.
            foreach(Guid iterGuidAppartenance in operationsByCompteByAppartenance.Keys)
            {
                SyntheseDepenseGlobalModelItem syntheseAppartenance = new SyntheseDepenseGlobalModelItem();
                syntheseAppartenance.AppartenanceId = iterGuidAppartenance;
                syntheseAppartenance.AppartenanceCaption = _apService.GetById(iterGuidAppartenance)?.Caption;
                syntheseAppartenance.Status = CompteStatusEnum.None;
                syntheseAppartenance.BudgetPourcentageDepense = 0;
                syntheseAppartenance.BudgetValueDepense = 0;
                syntheseAppartenance.BudgetValuePrevu = 0;
                List<CompteStatusEnum> statuses = new List<CompteStatusEnum>();
                foreach(Guid groupIterGuid in operationsByCompteByAppartenance[iterGuidAppartenance].Keys)
                {
                    float budgetDepense = 0, dummy1 = 0, dummy2 = 0;
                    ProcessBudgetSpentAndLeft(
                        out budgetDepense,
                        out dummy1,
                        out dummy2,
                        operationAndBudgetMap[groupIterGuid].Item2,
                        operationsByCompteByAppartenance[iterGuidAppartenance][groupIterGuid],
                        month,
                        year);
                    syntheseAppartenance.BudgetValueDepense += budgetDepense;
                    syntheseAppartenance.BudgetValuePrevu += operationAndBudgetMap[groupIterGuid].Item2;
                    statuses.Add(_statusProcessor.ProcessState(
                        operationAndBudgetMap[groupIterGuid].Item1, 
                        operationAndBudgetMap[groupIterGuid].Item2, 
                        operationsByCompteByAppartenance[iterGuidAppartenance][groupIterGuid])); 
                }
                syntheseAppartenance.BudgetPourcentageDepense = syntheseAppartenance.BudgetValuePrevu != 0.0f ? (syntheseAppartenance.BudgetValueDepense * 100.0f) / syntheseAppartenance.BudgetValuePrevu : 0.0f;
                syntheseAppartenance.Status = _statusProcessor.ProcessGlobal(statuses);
                statusesByCategories.Add(syntheseAppartenance.Status);
                retourData.Add(syntheseAppartenance);
            }
            return new SyntheseDepenseGlobalModel()
            {
                Data = retourData,
                Status = _statusProcessor.ProcessGlobalByCategories(retourData)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        private Dictionary<Guid, Dictionary<Guid, List<IOperation>>> InitOperationsByCompteByAppartenance(IEnumerable<GroupRawDB> groups)
        {
            Dictionary<Guid, Dictionary<Guid, List<IOperation>>> retour = new Dictionary<Guid, Dictionary<Guid, List<IOperation>>>();
            if (groups != null)
            {
                foreach (GroupRawDB iter in groups)
                {
                    if (!retour.ContainsKey(iter.AppartenanceId))
                    {
                        var iterOperations = new List<IOperation>();
                        var dic = new Dictionary<Guid, List<IOperation>>();
                        dic.Add(iter.Id, iterOperations);
                        retour.Add(iter.AppartenanceId, dic);
                    }
                    else
                    {
                        if (!retour[iter.AppartenanceId].ContainsKey(iter.Id))
                        {
                            var iterOperations = new List<IOperation>();
                            retour[iter.AppartenanceId].Add(iter.Id, iterOperations);
                        }
                    }
                }
            }
            return retour;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        private Dictionary<Guid, (OperationTypeEnum, float)> CreateOperationAndBudgetMap(IEnumerable<GroupRawDB> groups)
        {
            Dictionary<Guid, (OperationTypeEnum, float)> retour = new Dictionary<Guid, (OperationTypeEnum, float)>();
            if(groups != null)
            {
                foreach(GroupRawDB iter in groups)
                {
                    if (!retour.ContainsKey(iter.Id))
                    {
                        retour.Add(iter.Id, (iter.OperationAllowed, iter.BudgetExpected));
                    }
                }
            }
            return retour;
        }
    }
}
