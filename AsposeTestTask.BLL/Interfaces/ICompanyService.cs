using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.DTO.Company.Responses;

namespace AsposeTestTask.BLL.Interfaces
{
    public interface ICompanyService
    {
        Task<int> CreateCompany(CreateCompanyRequestDTO request, CancellationToken cancellationToken);
        Task<ReadCompanyResponseDTO> ReadCompany(int companyId, CancellationToken cancellationToken);
        Task<double> QueryCompanyPayment(QueryCompanyPaymentRequestDTO request, CancellationToken cancellationToken);
        Task<bool> UpdateCompany(UpdateCompanyRequestDTO request, CancellationToken cancellationToken);
        Task<bool> DeleteCompany(int companyId, CancellationToken cancellationToken);
    }
}
