using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.DTO.Company.Responses;

namespace AsposeTestTask.Web.Controllers.Company.Update
{
    public class UpdateCompanyRequest
    {
        public int CompanyId { get; set; }
        public int? ParentCompanyId { get; set; }
        public string? ParentCompanyName { get; set; }
        public string? CompanyName { get; set; }
        public IEnumerable<ReadCompanyResponseDTO> Companies { get; set; }


        public UpdateCompanyRequestDTO GetDTO() => new()
        {
            CompanyId = CompanyId,
            ParentCompanyId = ParentCompanyId,
            CompanyName = CompanyName,
        };
    }
}
