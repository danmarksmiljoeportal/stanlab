using Dmp.Stanlab.References.ReportingApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dmp.Stanlab.References.ReportingApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ReportingRepository _repository;

        public IndexModel(ReportingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<SampleItem> SampleItems => _repository.GetSampleItems();

        public void OnGet()
        {
        }
    }
}
