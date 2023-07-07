using AsposeTestTask.Constants;
using AsposeTestTask.DTO.Company.Responses;
using AsposeTestTask.DTO.Person;
using AsposeTestTask.DTO.Person.Requests;
using System.ComponentModel.DataAnnotations;

namespace AsposeTestTask.Web.Controllers.Person.Create
{
    public class CreatePersonRequest
    {
        [Required(ErrorMessage = "Person Name is required")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        public double Salary { get; set; }

        [Required(ErrorMessage = "Date of Hire is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfHire { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public CompanyRole Level { get; set; }

        public int? BossId { get; set; }

        [Required(ErrorMessage = "Company Id is required")]
        public int CompanyId { get; set; }

        public IEnumerable<ReadCompanyResponseDTO> Companies { get; set; }


        public CreatePersonRequestDTO GetDTO() => new()
        {
            PersonName = PersonName,
            Salary = Salary,
            DateOfHire = DateOfHire,
            Role = Level,
            BossId = BossId,
            CompanyId = CompanyId,
        };
    }
}
