using AsposeTestTask.DTO.Company.Requests;

namespace AsposeTestTask.Web.Controllers.Company.Update
{
    public class UpdateCompanyRequest
    {
        public int CompanyId { get; set; }
        public int? ParentCompanyId { get; set; }
        public string? CompanyName { get; set; }


        public UpdateCompanyRequestDTO GetDTO() => new()
        {
            CompanyId = CompanyId,
            ParentCompanyId = ParentCompanyId,
            CompanyName = CompanyName,
        };
    }
}
