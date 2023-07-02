using AsposeTestTask.DTO.Company.Responses;
using AsposeTestTask.DTO.Person;

namespace AsposeTestTask.Web.Controllers.Company.Read
{
    public class ReadCompanyResponse
    {
        public int CompanyId { get; set; }
        public int? ParentCompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<PersonShortModelDTO> Members { get; set; }


        public ReadCompanyResponse(ReadCompanyResponseDTO dTO)
        {
            CompanyId = dTO.CompanyId;
            ParentCompanyId = dTO.ParentCompanyId;
            CompanyName = dTO.CompanyName;
            Members = dTO.Members;
        }
    }
}
