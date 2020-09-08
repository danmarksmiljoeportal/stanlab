using Dmp.Stanlab.References.ReportingApi.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dmp.Stanlab.References.ReportingApi.Repositories
{
    public class ReportingRepository
    {
        private readonly List<SampleItem> _store = new List<SampleItem>();

        public IEnumerable<SampleItem> GetSampleItems() => _store;

        public SampleItem GetSampleItem(Guid sampleId)
        {
            var item = _store
                .Where(x => x.Item.SampleId == sampleId)
                .Single();

            return item;
        }

        public Guid SaveSampleRequest(SubmitSampleRequest request)
        {
            var id = request.SampleId.HasValue ? request.SampleId.Value : Guid.NewGuid();

            if (_store.Any(x => x.Item.SampleId == id))
            {
                throw new Exception("The sample already exists");
            }

            _store.Add(new SampleItem
            {
                SubmittedTime = DateTime.UtcNow,
                Item = new Sample
                {
                    SampleId = id,
                    Label = request.Label,

                    Link = null,
                    
                    ObservationFacility = new ObservationFacility
                    {
                        ObservationFacilityId = request.ObservationFacilityId,
                        Name = "Unknown facility"
                    },
                    Location = request.Location,

                    Matrix = request.Matrix,
                    Purpose = request.Purpose,
                    Type = request.Type,

                    Sampling = request.Sampling,

                    Measurements = request.Measurements,
                    Observations = request.Observations,

                    LaboratoryTests = new List<LaboratoryTest>()
                }
            });

            return id;
        }

        public void DeleteSampleRequest(Guid sampleId)
        {
            var sample = GetSampleItem(sampleId);
            _store.Remove(sample);
        }

        public void SaveLaboratoryTestRequest(Guid sampleId, SubmitLaboratoryTestRequest request)
        {
            var sample = GetSampleItem(sampleId);

            // Removing laboratory tests with the same reference
            sample.Item.LaboratoryTests
                .RemoveAll(x => x.Laboratory.Company == request.Laboratory.Company && x.Reference == request.Reference);

            sample.Item.LaboratoryTests.Add(new LaboratoryTest
            {
                Analyses = request.Analyses,
                Applicant = request.Applicant,
                Finished = request.Finished,
                Laboratory = request.Laboratory,
                Reference = request.Reference,
                Registered = request.Registered,
                Remarks = request.Remarks,                
                StorageTemperature = request.StorageTemperature
            });
        }

        public void DeleteLaboratoryTestRequest(Guid sampleId, string reference)
        {
            var sample = GetSampleItem(sampleId);
            sample.Item.LaboratoryTests.RemoveAll(x => x.Reference == reference);
        }

        public void Clear()
        {
            _store.Clear();
        }
    }

    public class SampleItem
    {
        public DateTime SubmittedTime { get; set; }

        public Sample Item { get; set; }
    }
}