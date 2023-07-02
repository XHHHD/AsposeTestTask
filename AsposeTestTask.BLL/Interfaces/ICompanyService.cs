using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.DTO.Company.Responses;

namespace AsposeTestTask.BLL.Interfaces
{
    public interface ICompanyService
    {
        Task<int> CreateCompanyAsync(CreateCompanyRequestDTO request, CancellationToken cancellationToken);
        Task<ReadCompanyResponseDTO> ReadCompanyAsync(int companyId, CancellationToken cancellationToken);
        Task<List<ReadCompanyResponseDTO>> ReadCompaniesAsync(CancellationToken cancellationToken);
        Task<double> QueryCompanyPaymentAsync(QueryCompanyPaymentRequestDTO request, CancellationToken cancellationToken);
        Task<bool> UpdateCompanyAsync(UpdateCompanyRequestDTO request, CancellationToken cancellationToken);
        bool DeleteCompany(int companyId);
    }
}
