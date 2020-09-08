using Dmp.Stanlab.References.RequisitionApi.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Dmp.Stanlab.References.RequisitionApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RequisitionRepository _repository;

        public IndexModel(RequisitionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<RequisitionRequestItem> Items => _repository.GetRequisitionRequests();

        public void OnGet()
        {            
        }
    }
}
