using Dmp.Stanlab.References.RequisitionApi.Repositories;
using Dmp.Stanlab.References.RequisitionApi.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Dmp.Stanlab.References.RequisitionApi.Controllers
{
    [Authorize]
    public class LaboratoryTestController : LaboratoryTestControllerBase
    {
        private readonly RequisitionRepository _repository;

        public LaboratoryTestController(RequisitionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public override Task CancelLaboratoryTest([BindRequired] string reference, [FromBody] CancelLaboratoryTestRequest body)
        {
            // The user's organization
            var vat = User.FindFirst(DmpClaimTypes.Company);

            return Task.CompletedTask;
        }

        public override Task<SubmitRequisitionResponse> SubmitRequisition([FromBody] SubmitRequisitionRequest body)
        {
            // The user's organization
            var vat = User.FindFirst(DmpClaimTypes.Company);

            var response = _repository.SaveRequisitionRequest(body);
            return Task.FromResult(response);
        }
    }
}