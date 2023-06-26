using AsposeTestTask.Constants;
using AsposeTestTask.DTO.Person.Requests;

namespace AsposeTestTask.Web.Controllers.Person.Create
{
    public class CreatePersonRequest
    {
        public string Name { get; set; }
        public DateTime DateOfHire { get; set; }
        public CompanyRole Level { get; set; }
        public int? BossId { get; set; }
        public int CompanyId { get; set; }


        public CreatePersonRequestDTO GetDTO() => new()
        {
            PersonName = Name,
            DateOfHire = DateOfHire,
            Role = Level,
            BossId = BossId,
            CompanyId = CompanyId,
        };
    }
}
