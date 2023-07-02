using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.Web.Controllers.Company.Create;
using AsposeTestTask.Web.Controllers.Company.Query;
using AsposeTestTask.Web.Controllers.Company.Update;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTestTask.Web.Controllers.Company
{
    public class CompanyController : Controller
    {
        private ICompanyService _companyService;


        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        public async Task<IActionResult> CreateCompany(string companyName)
        {
            try
            {
                var request = new CreateCompanyRequest(companyName);
                var result = await _companyService.CreateCompanyAsync(request.GetDTO(), CancellationToken.None);

                ViewBag.CompanyId = result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }


        public async Task<IActionResult> ReadCompany(int id, bool isEdit = false)
        {
            var result = await _companyService.ReadCompanyAsync(id, CancellationToken.None);

            if (isEdit)
            {
                var model = new UpdateCompanyRequest
                {
                    CompanyId = result.CompanyId,
                    CompanyName = result.CompanyName,
                    ParentCompanyId = result.ParentCompanyId
                };
                return View("EditCompany", model);
            }

            return View("ReadCompany", result);
        }


        public async Task<IActionResult> CompanyList()
        {
            var result = await _companyService.ReadCompaniesAsync(CancellationToken.None);
            return View(result);
        }


        public async Task<IActionResult> CalculateCompanyPayment(int id, DateTime paymentDate)
        {
            var request = new QueryCompanyPaymentRequest
            {
                CompanyId = id,
                PaymentDate = paymentDate
            };

            var result = await _companyService.QueryCompanyPaymentAsync(request.GetDTO(), CancellationToken.None);
            var company = await _companyService.ReadCompanyAsync(id, CancellationToken.None);
            ViewBag.Payment = result;
            ViewBag.Company = company;
            ViewBag.PaymentDate = paymentDate;

            return View("CalculateCompanyPayment", new QueryCompanyPaymentVM()
            {
                CompanyId = id,
                PaymentDate = paymentDate,
                Payment = result
            });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyRequest request)
        {
            await _companyService.UpdateCompanyAsync(request.GetDTO(), CancellationToken.None);
            var company = await _companyService.ReadCompanyAsync(request.CompanyId, CancellationToken.None);
            return View("ReadCompany", company);
        }


        public IActionResult DeleteCompany(int id)
        {
            try
            {
                _companyService.DeleteCompany(id);
                ViewBag.SuccessMessage = "Company deleted successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return RedirectToAction("CompanyList");
        }
    }
}