using AsposeTestTask.DTO.Company;

namespace AsposeTestTask.DTO.Person.Responses
{
    public class ReadPersonResponseDTO
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public string Role { get; set; }
        public PersonShortModelDTO? Boss { get; set; }
        public CompanyShortModelDTO Company { get; set; }
    }
}
