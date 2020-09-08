using Dmp.Stanlab.References.RequisitionApi.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Dmp.Stanlab.References.RequisitionApi.Pages
{
    public class RequisitionModel : PageModel
    {
        private readonly RequisitionRepository _repository;

        public RequisitionModel(RequisitionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public RequisitionRequestItem Item { get; set; }

        public void OnGet(string reference)
        {
            Item = _repository.GetRequisitionRequest(reference);
        }
    }
}