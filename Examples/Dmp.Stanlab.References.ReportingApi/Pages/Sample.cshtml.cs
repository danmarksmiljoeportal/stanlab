using Dmp.Stanlab.References.ReportingApi.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Dmp.Stanlab.References.ReportingApi.Pages
{
    public class SampleModel : PageModel
    {
        private readonly ReportingRepository _repository;

        public SampleModel(ReportingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public SampleItem SampleItem { get; set; }

        public void OnGet(Guid sampleId)
        {
            SampleItem = _repository.GetSampleItem(sampleId);
        }
    }
}
