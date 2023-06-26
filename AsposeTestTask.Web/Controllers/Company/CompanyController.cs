using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.Web.Controllers.Company.Create;
using AsposeTestTask.Web.Controllers.Company.Query;
using AsposeTestTask.Web.Controllers.Company.Update;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTestTask.Web.Controllers.Company
{
    public class CompanyController : Controller
    {
        ICompanyService _companyService;


        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPut]
        public IActionResult Create([FromBody] CreateCompanyRequest request)
        {
            var result = _companyService.CreateCompany(request.GetDTO(), CancellationToken.None);
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Read([FromRoute] int id)
        {
            var result = _companyService.ReadCompany(id, CancellationToken.None);
            return View();
        }

        [HttpPost("payment")]
        public IActionResult Query([FromBody] QueryCompanyPaymentRequest request)
        {
            var result = _companyService.QueryCompanyPayment(request.GetDTO(), CancellationToken.None);
            return View();
        }

        [HttpPost("{id}")]
        public IActionResult Update([FromRoute] int id, [FromQuery] UpdateCompanyRequest request)
        {
            var result = _companyService.UpdateCompany(request.GetDTO(id), CancellationToken.None);
            return View();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = _companyService.DeleteCompany(id, CancellationToken.None);
            return View();
        }
    }
}
