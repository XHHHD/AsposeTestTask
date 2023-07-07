using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.Constants;
using AsposeTestTask.Entities;
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


        /// <summary>
        /// Creating person page.
        /// </summary>
        /// <returns>Person creation page with companies to follow.</returns>
        public async Task<IActionResult> CreatePerson()
        {
            var companies = await _companyService.ReadCompaniesAsync(CancellationToken.None);
            var model = new CreatePersonRequest
            {
                Companies = companies
            };
            return View(model);
        }


        /// <summary>
        /// Creating person request.
        /// </summary>
        /// <param name="request">Person creation request.</param>
        /// <returns>Person creation page with companies to follow.</returns>
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


        /// <summary>
        /// Person info page.
        /// </summary>
        /// <param name="id">Person Id.</param>
        /// <returns>Person info page.</returns>
        public async Task<IActionResult> ReadPerson(int id)
        {
            var result = await _personService.ReadPerson(id, CancellationToken.None);
            ViewBag.PersonId = id;
            return View(result);
        }


        /// <summary>
        /// Persons of current company request.
        /// </summary>
        /// <param name="companyId">Company Id.</param>
        /// <returns>View with all employees of current company.</returns>
        public async Task<IActionResult> ReadCompanyPerson(int companyId)
        {
            var result = await _personService.ReadCompanyPersons(companyId, CancellationToken.None);
            return View(result);
        }


        /// <summary>
        /// All registered persons view.
        /// </summary>
        /// <returns>View with all registered persons.</returns>
        public async Task<IActionResult> PersonList()
        {
            var result = await _personService.ReadAllPersons(CancellationToken.None);
            return View(result);
        }


        /// <summary>
        /// Calculate person payment page.
        /// </summary>
        /// <param name="id">Person Id.</param>
        /// <param name="paymentDate">Salary calculation day.</param>
        /// <returns>Calculate person payment page with calculated person salary.</returns>
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


        /// <summary>
        /// Person delates overview.
        /// </summary>
        /// <param name="personId">Person Id.</param>
        /// <returns>Person delates view.</returns>
        public async Task<IActionResult> DetailsPerson(int personId)
        {
            var result = await _personService.ReadPerson(personId, CancellationToken.None);
            return View(result);
        }


        /// <summary>
        /// Person editing delates overview.
        /// </summary>
        /// <param name="personId">Person Id.</param>
        /// <returns>Person editing delates view.</returns>
        public async Task<IActionResult> EditPerson(int personId)
        {
            var result = await _personService.ReadPerson(personId, CancellationToken.None);
            var bosses = await _personService.ReadPotentialBosses(personId, CancellationToken.None);
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
                Bosses = bosses,
                Companies = companies,
            };
            return View(model);
        }


        /// <summary>
        /// Editing person data request.
        /// </summary>
        /// <param name="request">Updating form.</param>
        /// <returns>Person editing delates view.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdatePerson(UpdatePersonRequest request)
        {
            var result = await _personService.UpdatePerson(request.GetDTO(), CancellationToken.None);
            return RedirectToAction("PersonList");
        }


        /// <summary>
        /// Deleting person request.
        /// </summary>
        /// <param name="Id">Person Id.</param>
        /// <returns>View with all registered persons.</returns>
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
