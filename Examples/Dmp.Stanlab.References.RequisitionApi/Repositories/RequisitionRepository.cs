using Dmp.Stanlab.References.RequisitionApi.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dmp.Stanlab.References.RequisitionApi.Repositories
{
    public class RequisitionRepository
    {
        private readonly List<RequisitionRequestItem> _store = new List<RequisitionRequestItem>();

        public IEnumerable<RequisitionRequestItem> GetRequisitionRequests() => _store;

        public RequisitionRequestItem GetRequisitionRequest(string reference)
        {
            var item = _store
                .Where(x => x.Reference == reference)
                .Single();

            return item;
        }

        public SubmitRequisitionResponse SaveRequisitionRequest(SubmitRequisitionRequest request)
        {
            string reference = GenerateUniqueReference();

            _store.Add(new RequisitionRequestItem
            {
                SubmittedTime = DateTime.UtcNow,
                Reference = reference,
                Request = request
            });

            return new SubmitRequisitionResponse
            {
                LaboratoryTestReference = reference,
                StorageTime = request.StorageTime ?? 14
            };
        }

        public void Clear()
        {
            _store.Clear();
        }

        /// <summary>
        /// Generates a unique reference for demonstration purposes
        /// </summary>
        /// <returns></returns>
        private string GenerateUniqueReference()
        {
            var random = new Random();

            while (true)
            {
                var sequentialNumber = random.Next(1, 1000000);
                string reference = $"{DateTime.UtcNow.Year}-{sequentialNumber}";

                if (!_store.Any(x => x.Reference == reference))
                {
                    return reference;
                }
            }
        }
    }

    public class RequisitionRequestItem
    {
        public DateTime SubmittedTime { get; set; }

        public string Reference { get; set; }

        public SubmitRequisitionRequest Request { get; set; }
    }
}