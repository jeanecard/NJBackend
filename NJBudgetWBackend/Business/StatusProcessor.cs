using NJBudgetBackEnd.Models;
using System;
using System.Collections.Generic;

namespace NJBudgetWBackend.Business
{
    public class StatusProcessor : IStatusProcessor
    {
        private readonly Dictionary<CompteStatusEnum, short> values = new Dictionary<CompteStatusEnum, short>();
        public StatusProcessor()
        {
            values.Add(CompteStatusEnum.Danger, -1);
            values.Add(CompteStatusEnum.Warning, 0);
            values.Add(CompteStatusEnum.Good, 1);
            values.Add(CompteStatusEnum.None, 1);
            values.Add(CompteStatusEnum.Shame, -2);
        }
        /// <summary>
        /// Very first version for UT passing
        /// </summary>
        /// <param name="statuses"></param>
        /// <returns></returns>
        public CompteStatusEnum ProcessGlobal(IEnumerable<CompteStatusEnum> statuses)
        {
            CompteStatusEnum retour = CompteStatusEnum.None;
            if (statuses != null)
            {
                bool firstStep = true;
                int result = 0;
                int nbValuesToProcess = 0;
                foreach (CompteStatusEnum iter in statuses)
                {
                    if (iter != CompteStatusEnum.None
                        && iter != CompteStatusEnum.Shame)
                    {
                        nbValuesToProcess++;
                        if (firstStep)
                        {
                            firstStep = false;
                            result = values[iter];
                        }
                        else
                        {
                            result += values[iter];
                        }
                    }
                }
                if (nbValuesToProcess > 0)
                {
                    float moyenne = (float)result / (float)nbValuesToProcess;
                    if (moyenne < -0.25)
                    {
                        retour = CompteStatusEnum.Danger;
                    }
                    else if (moyenne < 0.25)
                    {
                        retour = CompteStatusEnum.Warning;
                    }
                    else
                    {
                        retour = CompteStatusEnum.Good;
                    }
                }
            }
            return retour;
        }
        public CompteStatusEnum ProcessGlobalByCategories(IEnumerable<SyntheseDepenseGlobalModelItem> items)
        {
            if (items == null)
            {
                return CompteStatusEnum.None;
            }
            float budgetInitial = 0;
            float budgetConsomme = 0;
            foreach (SyntheseDepenseGlobalModelItem iter in items)
            {
                budgetInitial += iter.BudgetValuePrevu;
                budgetConsomme += iter.BudgetValueDepense;
            }
            if(budgetConsomme <= budgetInitial )
            {
                return CompteStatusEnum.Good;
            } else if(budgetConsomme < budgetInitial + budgetConsomme * 0.1 )
                {
                return CompteStatusEnum.Warning;
            }
            else
            {
                return CompteStatusEnum.Danger;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compte"></param>
        /// <param name="operations"></param>
        public CompteStatusEnum ProcessState(OperationTypeEnum operationAllowed, float budgetExpectedByMonth, IEnumerable<IOperation> operations)
        {
            CompteStatusEnum retour = CompteStatusEnum.None;
            if (operations == null)
            {
                throw new ArgumentNullException("Ah mais je connais Miwege !");
            }
            float epargne = 0;
            float depense = 0;
            Dictionary<(int, int), int> monthOperations = new Dictionary<(int, int), int>();
            foreach (IOperation iter in operations)
            {
                if (!monthOperations.ContainsKey((iter.DateOperation.Year, iter.DateOperation.Month)))
                {
                    monthOperations.Add((iter.DateOperation.Year, iter.DateOperation.Month), 2);
                }
                if (iter.Value < 0)
                {
                    depense += iter.Value;
                }
                else
                {
                    epargne += iter.Value;
                }
            }
            int jourDuMois = DateTime.Now.Day;
            switch (operationAllowed)
            {
                case OperationTypeEnum.AddOnly:
                    retour = ProcessStateAddOnly(budgetExpectedByMonth, epargne, monthOperations.Count);
                    break;
                case OperationTypeEnum.DeleteOnly:
                    retour = ProcessStateDeleteOnly(budgetExpectedByMonth, depense, monthOperations.Count, (byte)jourDuMois);
                    break;
                case OperationTypeEnum.AddAndDelete:
                    retour = ProcessStateAddAndDelete(budgetExpectedByMonth, depense, epargne, monthOperations.Count);
                    break;
                case OperationTypeEnum.None:
                    retour = ProcessStateNone(budgetExpectedByMonth, depense, epargne);
                    break;
                default:
                    break;
            }
            return retour;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compte"></param>
        public CompteStatusEnum ProcessStateNone(float budget, float depense, float epargne)
        {
            return CompteStatusEnum.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compte"></param>
        /// <param name="depense"></param>
        /// <param name="epargne"></param>
        /// <param name="count"></param>
        public CompteStatusEnum ProcessStateAddAndDelete(float budgetInitial, float depense, float provision, int nbMonth)
        {
            if (budgetInitial < 0)
            {
                return CompteStatusEnum.None;
            }
            float fullBudgetExpected = (nbMonth * budgetInitial);
            float delta = fullBudgetExpected - Math.Abs(depense) - Math.Abs(provision);
            var absDelta = Math.Abs(delta);
            if (delta >= 0)
            {
                if (provision <= depense)
                {
                    return CompteStatusEnum.Warning;
                }
                else
                {
                    if (provision >= 0.75 * fullBudgetExpected)
                    {
                        return CompteStatusEnum.Good;
                    }
                    else if (provision >= 0.25 * fullBudgetExpected)
                    {
                        return CompteStatusEnum.Warning;
                    }
                    else
                    {
                        return CompteStatusEnum.Danger;
                    }
                }
            }
            else
            {
                if (provision >= depense)
                {
                    if (absDelta <= 0.25 * fullBudgetExpected)
                    {
                        return CompteStatusEnum.Warning;
                    }
                    else
                    {
                        return CompteStatusEnum.Danger;
                    }
                }
                else
                {
                    if (absDelta <= 0.10 * fullBudgetExpected)
                    {
                        return CompteStatusEnum.Warning;
                    }
                    else
                    {
                        return CompteStatusEnum.Danger;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compte"></param>
        /// <param name="depense"></param>
        /// <param name="nbMonth"></param>
        public CompteStatusEnum ProcessStateDeleteOnly(float budgetInitial, float depense, int nbMonth, byte jourDuMois)
        {
            if (budgetInitial < 0)
            {
                return CompteStatusEnum.None;
            }
            float delta = (nbMonth * budgetInitial) - Math.Abs(depense);
            var absDelta = Math.Abs(delta);
            if (delta >= 0)
            {
                return CompteStatusEnum.Good;
            }
            else if (absDelta <= budgetInitial)
            {

                if (absDelta <= 0.15 * budgetInitial)
                {
                    return CompteStatusEnum.Good;
                }
                else if (absDelta <= 0.5 * budgetInitial)
                {
                    return jourDuMois < 15 ? CompteStatusEnum.Good : CompteStatusEnum.Warning;
                }
                else
                {
                    return CompteStatusEnum.Warning;
                }
            }
            else
            {
                return CompteStatusEnum.Danger;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compte"></param>
        /// <param name="epargne"></param>
        /// <param name="count"></param>
        public CompteStatusEnum ProcessStateAddOnly(float budgetInitial, float epargne, int count)
        {
            return CompteStatusEnum.None;//TODO
        }
    }
}
