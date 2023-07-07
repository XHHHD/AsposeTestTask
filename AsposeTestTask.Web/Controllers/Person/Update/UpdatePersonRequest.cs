using AsposeTestTask.Constants;
using AsposeTestTask.DTO.Company.Responses;
using AsposeTestTask.DTO.Person;
using AsposeTestTask.DTO.Person.Requests;

namespace AsposeTestTask.Web.Controllers.Person.Update
{
    public class UpdatePersonRequest
    {
        public int PersonId { get; set; }
        public int? BossId { get; set; }
        public int? CompanyId { get; set; }
        public double? Salary { get; set; }
        public string? PersonName { get; set; }
        public DateTime? DateOfHire { get; set; }
        public CompanyRole? Role { get; set; }
        public IEnumerable<PersonShortModelDTO> Bosses { get; set; }
        public IEnumerable<ReadCompanyResponseDTO> Companies { get; set; }


        public UpdatePersonRequestDTO GetDTO() => new()
        {
            PersonId = PersonId,
            BossId = BossId,
            CompanyId = CompanyId,
            Salary = Salary,
            PersonName = PersonName,
            DateOfHire = DateOfHire,
            Role = Role,
        };
    }
}
