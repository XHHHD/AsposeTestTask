using AsposeTestTask.Constants;
using AsposeTestTask.DTO.Person.Requests;

namespace AsposeTestTask.Web.Controllers.Person.Update
{
    public class UpdatePersonRequest
    {
        public int? BossId { get; set; }
        public int? CompanyId { get; set; }
        public double? Salary { get; set; }
        public string? PersonName { get; set; }
        public DateTime? DateOfHire { get; set; }
        public CompanyRole? Role { get; set; }


        public UpdatePersonRequestDTO GetDTO(int personId) => new()
        {
            PersonId = personId,
            BossId = BossId,
            CompanyId = CompanyId,
            Salary = Salary,
            PersonName = PersonName,
            DateOfHire = DateOfHire,
            Role = Role,
        };
    }
}
