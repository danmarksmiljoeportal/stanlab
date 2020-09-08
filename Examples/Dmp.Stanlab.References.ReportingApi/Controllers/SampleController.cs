using Dmp.Stanlab.References.ReportingApi.Repositories;
using Dmp.Stanlab.References.ReportingApi.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Dmp.Stanlab.References.ReportingApi
{
    [Authorize]
    public class SampleController : SampleControllerBase
    {
        private readonly ReportingRepository _repository;

        public SampleController(ReportingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override Task DeleteSample([BindRequired] Guid sampleId)
        {
            _repository.DeleteSampleRequest(sampleId);
            return Task.CompletedTask;
        }

        public override Task<Sample> GetSample([BindRequired] Guid sampleId)
        {
            var sample = _repository.GetSampleItem(sampleId);
            return Task.FromResult(sample.Item);
        }

        public override Task<Guid> SubmitSample([FromBody] SubmitSampleRequest body)
        {
            var sampleId = _repository.SaveSampleRequest(body);
            return Task.FromResult(sampleId);
        }

        public override Task DeleteLaboratoryTest([BindRequired] Guid sampleId, [BindRequired] string reference)
        {
            _repository.DeleteLaboratoryTestRequest(sampleId, reference);
            return Task.CompletedTask;
        }

        public override Task SubmitLaboratoryTest([BindRequired] Guid sampleId, [FromBody] SubmitLaboratoryTestRequest body)
        {
            _repository.SaveLaboratoryTestRequest(sampleId, body);
            return Task.CompletedTask;
        }
    }
}
