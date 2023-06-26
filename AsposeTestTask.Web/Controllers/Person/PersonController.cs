using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.Web.Controllers.Person.Create;
using AsposeTestTask.Web.Controllers.Person.Query;
using AsposeTestTask.Web.Controllers.Person.Update;
using Microsoft.AspNetCore.Mvc;

namespace AsposeTestTask.Web.Controllers.Person
{
    public class PersonController : Controller
    {
        private IPersonService _personService;


        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPut]
        public IActionResult Create([FromBody]CreatePersonRequest request)
        {
            var result = _personService.CreatePerson(request.GetDTO(), CancellationToken.None);
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Read([FromRoute]int id)
        {
            var result = _personService.ReadPerson(id, CancellationToken.None);
            return View();
        }

        [HttpPost("payments")]
        public IActionResult Query([FromBody]QueryPersonPaymentRequest request)
        {
            var result = _personService.QueryPersonPayment(request.GetDTO(), CancellationToken.None);
            return View();
        }

        [HttpPost("{id}")]
        public IActionResult Update([FromRoute]int Id, [FromQuery]UpdatePersonRequest request)
        {
            var result = _personService.UpdatePerson(request.GetDTO(Id), CancellationToken.None);
            return View();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int Id)
        {
            var result = _personService.DeletePerson(Id, CancellationToken.None);
            return View();
        }
    }
}
