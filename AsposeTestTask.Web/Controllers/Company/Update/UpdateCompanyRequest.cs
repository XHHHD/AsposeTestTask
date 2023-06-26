using AsposeTestTask.DTO.Company.Requests;

namespace AsposeTestTask.Web.Controllers.Company.Update
{
    public class UpdateCompanyRequest
    {
        public int? ParentId { get; set; }
        public string? Name { get; set; }


        public UpdateCompanyRequestDTO GetDTO(int id) => new()
        {
            CompanyId = id,
            ParentCompanyId = ParentId,
            CompanyName = Name,
        };
    }
}
