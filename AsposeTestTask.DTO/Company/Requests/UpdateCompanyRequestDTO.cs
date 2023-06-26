namespace AsposeTestTask.DTO.Company.Requests
{
    public class UpdateCompanyRequestDTO
    {
        public int CompanyId { get; set; }
        public int? ParentCompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}
