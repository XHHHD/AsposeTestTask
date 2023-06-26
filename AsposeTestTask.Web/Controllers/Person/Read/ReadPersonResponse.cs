
using AsposeTestTask.DTO.Company;
using AsposeTestTask.DTO.Person;
using AsposeTestTask.DTO.Person.Responses;

namespace AsposeTestTask.Web.Controllers.Person.Read
{
    public class ReadPersonResponse
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public string Role { get; set; }
        public PersonShortModelDTO? Boss { get; set; }
        public CompanyShortModelDTO Company { get; set; }


        public ReadPersonResponse(ReadPersonResponseDTO dTO)
        {
            PersonId = dTO.PersonId;
            PersonName = dTO.PersonName;
            Salary = dTO.Salary;
            DateOfHire = dTO.DateOfHire;
            Role = dTO.Role;
            Boss = dTO.Boss;
            Company = dTO.Company;
        }
    }
}
