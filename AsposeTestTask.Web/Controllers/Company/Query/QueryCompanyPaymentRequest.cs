using AsposeTestTask.DTO.Company.Requests;

namespace AsposeTestTask.Web.Controllers.Company.Query
{
    public class QueryCompanyPaymentRequest
    {
        public int CompanyId { get; set; }
        public DateTime PaymentDate { get; set; }


        public QueryCompanyPaymentRequestDTO GetDTO() => new()
        {
            CompanyId = CompanyId,
            PaymentDate = PaymentDate,
        };
    }
}
