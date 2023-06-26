using AsposeTestTask.Constants;

namespace AsposeTestTask.DTO.Person.Requests
{
    public class CreatePersonRequestDTO
    {
        public string PersonName { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public CompanyRole Role { get; set; }
        public int? BossId { get; set; }
        public int CompanyId { get; set; }
    }
}
