using AsposeTestTask.DTO.Person.Requests;
using AsposeTestTask.DTO.Person.Responses;

namespace AsposeTestTask.BLL.Interfaces
{
    public interface IPersonService
    {
        Task<int> CreatePerson(CreatePersonRequestDTO request, CancellationToken cancellationToken);
        Task<ReadPersonResponseDTO> ReadPerson(int personId, CancellationToken cancellationToken);
        Task<double> QueryPersonPayment(QueryPersonPaymentRequestDTO request, CancellationToken cancellationToken);
        Task<bool> UpdatePerson(UpdatePersonRequestDTO request, CancellationToken cancellationToken);
        Task<bool> DeletePerson(int personId, CancellationToken cancellationToken);
    }
}
