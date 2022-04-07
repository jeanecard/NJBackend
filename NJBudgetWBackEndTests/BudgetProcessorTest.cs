using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Business;
using NJBudgetWBackend.Commun;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NJBudgetWBackEndTests
{
    public class BudgetProcessorTest
    {
        BudgetProcessor bProcesor = new BudgetProcessor(new AppartenanceService(), new StatusProcessor());

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Add_Operation_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = 5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = 5, DateOperation = new DateTime(2021, 1, 1) };

            ops.Add(ope1);
            ops.Add(ope2);

            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == 90);
            Assert.True(budgetData.budgetProvisonne == 10);
            Assert.True(budgetData.budgetConsomme == 10);
        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Remove_Operation_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 1, 1) };

            ops.Add(ope1);
            ops.Add(ope2);
            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == 90);
            Assert.True(budgetData.budgetProvisonne == 0);
            Assert.True(budgetData.budgetConsomme == 10);
        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Remove_Operation_But_One_Out_Of_Range_Month_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 2, 1) };

            ops.Add(ope1);
            ops.Add(ope2);
            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == 95);
            Assert.True(budgetData.budgetProvisonne == 0);
            Assert.True(budgetData.budgetConsomme == 5);
        }


        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Add_And_Remove_Operation_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = 15, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope3 = new Operation() { Value = -20, DateOperation = new DateTime(2021, 1, 1) };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == 60);
            Assert.True(budgetData.budgetProvisonne == 15);
            Assert.True(budgetData.budgetConsomme == 40);
        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Epargne_Consomme_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = 100, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = -120, DateOperation = new DateTime(2021, 1, 1) };

            ops.Add(ope1);
            ops.Add(ope2);
            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == -120);
            Assert.True(budgetData.budgetProvisonne == 100);
            Assert.True(budgetData.budgetConsomme == 220);

            //Suppression de l'épargne
            ops.RemoveAt(0);
            budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == -20);
            Assert.True(budgetData.budgetProvisonne == 0);
            Assert.True(budgetData.budgetConsomme == 120);
        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_Epargne_Et_Depense_Expected_Budget_Expect_No_Left()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = 50, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = -50, DateOperation = new DateTime(2021, 1, 1) };

            ops.Add(ope1);
            ops.Add(ope2);
            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == 0);
            Assert.True(budgetData.budgetProvisonne == 50);
            Assert.True(budgetData.budgetConsomme == 100);
        }


        [Fact]
        public void ProcessBudgetSpentAndLeft_Depense_Exeed_Budget_Expect_Left_Neg()
        {
            List<Operation> ops = new List<Operation>();
            Operation ope1 = new Operation() { Value = 50, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new Operation() { Value = -60, DateOperation = new DateTime(2021, 1, 1) };

            ops.Add(ope1);
            ops.Add(ope2);
            float budgetExpected = 100;
            var budgetData = bProcesor.ProcessBudgetSpentAndLeft(budgetExpected, ops, 1, 2021);
            Assert.True(budgetData.budgetRestant == -10);
            Assert.True(budgetData.budgetProvisonne == 50);
            Assert.True(budgetData.budgetConsomme == 110);
        }


        [Fact]
        public void ProcessSyntheseOperations_One_Appartenance_Multiple_Compte_Passing_Case()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            List<SyntheseOperationRAwDB> operations = new List<SyntheseOperationRAwDB>();
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 1000,
                    CompteId = guid1,
                    DateOperation = new DateTime(2021, 2, 5),
                    OperationAllowed = OperationTypeEnum.AddAndDelete,
                    Value = 1000
                });
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    CompteId = guid2,
                    DateOperation = new DateTime(2021, 2, 10),
                    OperationAllowed = OperationTypeEnum.DeleteOnly,
                    Value = -200
                });
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    CompteId = guid2,
                    DateOperation = new DateTime(2021, 2, 10),
                    OperationAllowed = OperationTypeEnum.DeleteOnly,
                    Value = -150
                });

            List<GroupRawDB> groups = new List<GroupRawDB>() {
                new GroupRawDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    Id = guid2,
                    OperationAllowed = OperationTypeEnum.DeleteOnly
                },
                new GroupRawDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 1000,
                    Id = guid1,
                    OperationAllowed = OperationTypeEnum.AddAndDelete
                }
                };

            var result = bProcesor.ProcessSyntheseOperations(operations, groups, 2, 2021);
            Assert.NotEmpty(result.Data);
            SyntheseDepenseGlobalModelItem item1 = result.Data.ToList()[0];
            Assert.True(item1.AppartenanceId == Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID));
            Assert.True(item1.BudgetValuePrevu == 1500);
            Assert.True(item1.BudgetValueDepense == 1350);
            Assert.True((int)item1.BudgetPourcentageDepense == 90);
            Assert.True(item1.Status == CompteStatusEnum.Good);
            Assert.True(result.Status == CompteStatusEnum.Good);
        }
    }
}
