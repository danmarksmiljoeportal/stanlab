using Dmp.Stanlab.References.RequisitionApi.Specifications;
using System.Collections.Generic;

namespace Dmp.Stanlab.References.RequisitionApi.Repositories
{
    public class ContractRepository
    {
        private readonly List<LaboratoryContract> _contracts;
        private readonly List<BillingAccount> _billingAccounts;

        public ContractRepository()
        {
            _contracts = new List<LaboratoryContract>();
            _billingAccounts = new List<BillingAccount>();
        }

        public void Seed()
        {
            var contract = new LaboratoryContract
            {
                Reference = "default-contract",

                Description = "Description of the contract here",
                DeliveryTerms = "Normal delivery",

                Analyses = new List<AnalysisOffering>
                {
                    // TODO: Add analyses
                }
            };

            _contracts.Add(contract);


            var billingAccount = new BillingAccount
            {
                Reference = "account-1",
                Name = "Billing account 1",
                Description = "Description of the billing account"                
            };

            _billingAccounts.Add(billingAccount);
        }

        public IEnumerable<LaboratoryContract> GetContracts() => _contracts;
        public IEnumerable<BillingAccount> GetBillingAccounts() => _billingAccounts;
    }
}