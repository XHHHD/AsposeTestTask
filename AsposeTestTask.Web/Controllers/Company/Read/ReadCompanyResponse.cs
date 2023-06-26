using AsposeTestTask.DTO.Company.Responses;
using AsposeTestTask.DTO.Person;

namespace AsposeTestTask.Web.Controllers.Company.Read
{
    public class ReadCompanyResponse
    {
        public int Id { get; set; }
        public int? ParentCompanyId { get; set; }
        public string Name { get; set; }
        public List<PersonShortModelDTO> Members { get; set; }


        public ReadCompanyResponse(ReadCompanyResponseDTO dTO)
        {
            Id = dTO.CompanyId;
            ParentCompanyId = dTO.ParentCompanyId;
            Name = dTO.CompanyName;
            Members = dTO.Members;
        }
    }
}
