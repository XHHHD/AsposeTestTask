using AsposeTestTask.DTO.Person.Requests;

namespace AsposeTestTask.Web.Controllers.Person.Query
{
    public class QueryPersonPaymentRequest
    {
        public int PersonId { get; set; }
        public DateTime PaymentDate { get; set; }


        public QueryPersonPaymentRequestDTO GetDTO() => new()
        {
            PersonId = PersonId,
            PaymentDate = PaymentDate,
        };
    }
}
