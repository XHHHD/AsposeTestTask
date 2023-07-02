using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.Constants;
using AsposeTestTask.Web.Controllers.Person.Create;
using AsposeTestTask.Web.Controllers.Person.Query;
using AsposeTestTask.Web.Controllers.Person.Update;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTestTask.Web.Controllers.Person
{
    public class PersonController : Controller
    {
        private IPersonService _personService;
        private ICompanyService _companyService;


        public PersonController(IPersonService personService, ICompanyService companyService)
        {
            _personService = personService;
            _companyService = companyService;
        }

        public async Task<IActionResult> CreatePerson()
        {
            var companies = await _companyService.ReadCompaniesAsync(CancellationToken.None);
            var model = new CreatePersonRequest
            {
                Companies = companies
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePerson(CreatePersonRequest request)
        {
            await _personService.CreatePerson(request.GetDTO(), CancellationToken.None);

            var companies = await _companyService.ReadCompaniesAsync(CancellationToken.None);
            var model = new CreatePersonRequest
            {
                Companies = companies
            };
            return View(model);
        }


        public async Task<IActionResult> ReadPerson(int id)
        {
            var result = await _personService.ReadPerson(id, CancellationToken.None);
            ViewBag.PersonId = id;
            return View(result);
        }


        public async Task<IActionResult> ReadCompanyPerson(int companyId)
        {
            var result = await _personService.ReadCompanyPersons(companyId, CancellationToken.None);
            return View(result);
        }


        public async Task<IActionResult> PersonList()
        {
            var result = await _personService.ReadAllPersons(CancellationToken.None);
            return View(result);
        }


        public async Task<IActionResult> CalculatePersonPayment(int id, DateTime paymentDate)
        {
            var request = new QueryPersonPaymentRequest
            {
                PersonId = id,
                PaymentDate = paymentDate
            };

            var result = await _personService.QueryPersonPayment(request.GetDTO(), CancellationToken.None);
            var person = await _personService.ReadPerson(id, CancellationToken.None);
            ViewBag.Person = person;
            ViewBag.PaymentDate = paymentDate;
            ViewBag.Payment = result;

            return View("CalculatePersonPayment", new QueryPersonPaymentVM()
            {
                PersonId = id,
                PaymentDate = paymentDate,
                Payment = result
            });
        }


        public async Task<IActionResult> DetailsPerson(int personId)
        {
            var result = await _personService.ReadPerson(personId, CancellationToken.None);
            return View(result);
        }

        public async Task<IActionResult> EditPerson(int personId)
        {
            var result = await _personService.ReadPerson(personId, CancellationToken.None);
            var companies = await _companyService.ReadCompaniesAsync(CancellationToken.None);
            var model = new UpdatePersonRequest
            {
                PersonId = result.PersonId,
                PersonName = result.PersonName,
                Salary = result.Salary,
                DateOfHire = result.DateOfHire,
                Role = Enum.Parse<CompanyRole>(result.Role),
                BossId = result.Boss?.PersonId,
                CompanyId = result.Company.CompanyId,
                Companies = companies
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePerson(UpdatePersonRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _personService.UpdatePerson(request.GetDTO(), CancellationToken.None);
                return View(result);
            }
            else
            {
                var companies = await _companyService.ReadCompaniesAsync(CancellationToken.None);
                request.Companies = companies;
                return View("EditPerson", request);
            }
        }


        public IActionResult DeletePerson(int Id)
        {
            try
            {
                _personService.DeletePerson(Id);
                ViewBag.SuccessMessage = "Person deleted successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return RedirectToAction("PersonList");
        }
    }
}
