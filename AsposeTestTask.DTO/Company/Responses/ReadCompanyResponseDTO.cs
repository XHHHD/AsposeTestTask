using AsposeTestTask.DTO.Person;

namespace AsposeTestTask.DTO.Company.Responses
{
    public class ReadCompanyResponseDTO
    {
        public int CompanyId { get; set; }
        public int? ParentCompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<PersonShortModelDTO> Members { get; set; }
    }
}
