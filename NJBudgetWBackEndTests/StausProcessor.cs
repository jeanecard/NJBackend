using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Business;
using System.Collections.Generic;
using Xunit;

namespace NJBudgetWBackEndTests
{
    public class StatusProcessorTest
    {
        StatusProcessor sProcesor = new StatusProcessor();

        [Fact]
        public void ProcessGlobal_With_Null_Or_Empty_Input_Expect_None()
        {
            Assert.True(CompteStatusEnum.None == sProcesor.ProcessGlobal(null));
            Assert.True(CompteStatusEnum.None == sProcesor.ProcessGlobal(new List<CompteStatusEnum>()));
        }
        [Fact]
        public void ProcessGlobal_With_All_Equls_Status_Expect_Same_Status()
        {
            var list = new List<CompteStatusEnum>() {CompteStatusEnum.Danger, CompteStatusEnum.Danger, CompteStatusEnum.Danger };
            Assert.True(CompteStatusEnum.Danger == sProcesor.ProcessGlobal(list));

            list = new List<CompteStatusEnum>() { CompteStatusEnum.Warning, CompteStatusEnum.Warning, CompteStatusEnum.Warning };
            Assert.True(CompteStatusEnum.Warning == sProcesor.ProcessGlobal(list));

            list = new List<CompteStatusEnum>() { CompteStatusEnum.Good, CompteStatusEnum.Good, CompteStatusEnum.Good };
            Assert.True(CompteStatusEnum.Good == sProcesor.ProcessGlobal(list));
        }
        [Fact]
        public void ProcessGlobal_With_One_Of_All_Status_Expect_Warning_Status()
        {
            var list = new List<CompteStatusEnum>() { CompteStatusEnum.Danger, CompteStatusEnum.Warning, CompteStatusEnum.Good };
            Assert.True(CompteStatusEnum.Warning == sProcesor.ProcessGlobal(list));
        }
        [Fact]
        public void ProcessGlobal_Containing_None_Status_Expect_Unmodified_Status()
        {
            var list = new List<CompteStatusEnum>() { CompteStatusEnum.None, CompteStatusEnum.Good, CompteStatusEnum.Good };
            Assert.True(CompteStatusEnum.Good == sProcesor.ProcessGlobal(list));
        }
        [Fact]
        public void ProcessGlobal_Containing_Shame_Status_Expect_Unmodified_Status()
        {
            var list = new List<CompteStatusEnum>() { CompteStatusEnum.Shame, CompteStatusEnum.Shame, CompteStatusEnum.Good, CompteStatusEnum.Good };
            Assert.True(CompteStatusEnum.Good == sProcesor.ProcessGlobal(list));
        }

        [Fact]
        public void ProcessGlobal_Containing_2Good_And_1Danger_Expect_Good_Status()
        {
            var list = new List<CompteStatusEnum>() { CompteStatusEnum.Good, CompteStatusEnum.Danger, CompteStatusEnum.Good };
            Assert.True(CompteStatusEnum.Good == sProcesor.ProcessGlobal(list));
        }
        [Fact]
        public void ProcessStateDeleteOnly_LessExpense_ThanExpectedBudget_Expect_Good()
        {
            Assert.True(sProcesor.ProcessStateDeleteOnly(300, -1000, 5, 1) == CompteStatusEnum.Good);
            Assert.True(sProcesor.ProcessStateDeleteOnly(300, -299, 1, 1) == CompteStatusEnum.Good);
        }

        [Fact]
        public void ProcessStateDeleteOnly_MoreExpense_ThanExpected_ButLess_Than_15percent_Budget_Expect_Good()
        {
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -1149, 5, 1) == CompteStatusEnum.Good);
        }

        [Fact]
        public void ProcessStateDeleteOnly_MoreExpense_ThanExpected_ButLess_Than_50percent_Budget_Expect_Good_Or_Warning()
        {
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -5499, 5, 12) == CompteStatusEnum.Good);
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -5499, 5, 16) == CompteStatusEnum.Warning);
        }

        [Fact]
        public void ProcessStateDeleteOnly_MoreExpense_ThanExpected_ButMore_Than_50percent_Budget_And_Less_Than_Expected_Expect_Warning()
        {
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -5501, 5, 12) == CompteStatusEnum.Warning);
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -5501, 5, 16) == CompteStatusEnum.Warning);
        }

        [Fact]
        public void ProcessStateDeleteOnly_MoreExpense_ThanBudgetExpected_Expect_Danger()
        {
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -6001, 5, 12) == CompteStatusEnum.Danger);
            Assert.True(sProcesor.ProcessStateDeleteOnly(1000, -6001, 5, 16) == CompteStatusEnum.Danger);
        }

        [Fact]
        public void ProcessStateAddAndDelete_BudgetExpected_Not_Reached_But_Provision_LT_Depense_Expect_Warning()
        {
            Assert.True(sProcesor.ProcessStateAddAndDelete(1000, 400, 300, 2) == CompteStatusEnum.Warning);
        }

        [Fact]
        public void ProcessStateAddAndDelete_BudgetExpected_Not_Reached_But_Provision_GT_Depense_And_More_Than_75Percent_Expect_Good()
        {
            Assert.True(sProcesor.ProcessStateAddAndDelete(500, 10, 800, 2) == CompteStatusEnum.Good);
        }

        [Fact]
        public void ProcessStateAddAndDelete_BudgetExpected_Not_Reached_But_Provision_GT_Depense_And_At_30Percent_Expect_Good()
        {
            Assert.True(sProcesor.ProcessStateAddAndDelete(500, 10, 300, 2) == CompteStatusEnum.Warning);
        }

        [Fact]
        public void ProcessStateAddAndDelete_BudgetExpected_Not_Reached_But_Provision_GT_Depense_And_Less_25Percent_Expect_Good()
        {
            Assert.True(sProcesor.ProcessStateAddAndDelete(500, 10, 100, 2) == CompteStatusEnum.Danger);
        }

    }
}
