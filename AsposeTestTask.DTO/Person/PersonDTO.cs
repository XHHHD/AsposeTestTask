using AsposeTestTask.Constants;

namespace AsposeTestTask.DTO.Person
{
    public class PersonDTO
    {
        public string Name { get; set; }
        public DateTime DateOfHire { get; set; }
        public CompanyRole Role { get; set; }
        public int? BossId { get; set; }
        public int CompanyId { get; set; }
    }
}
