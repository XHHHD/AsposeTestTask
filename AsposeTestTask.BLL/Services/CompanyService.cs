using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.DAL.Data;
using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.DTO.Company.Responses;
using AsposeTestTask.DTO.Person;
using AsposeTestTask.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AsposeTestTask.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private AsposeContext _context;


        public CompanyService(AsposeContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Creating company method.
        /// </summary>
        /// <returns>Created company Id.</returns>
        public async Task<int> CreateCompanyAsync(CreateCompanyRequestDTO request, CancellationToken cancellationToken)
        {
            #region CHECKS
            if (request.CompanyName.IsNullOrEmpty())
            {
                throw new Exception("You must fill in company name!");
            }

            if (request.ParentCompanyId is not null)
            {
                var parentCompany =
                    await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.ParentCompanyId, cancellationToken)
                    ?? throw new Exception("Parent company wasn't found!");
            }
            #endregion

            var company = new Company()
            {
                CompanyName = request.CompanyName,
                ParentCompanyId = request.ParentCompanyId,
            };

            await _context.Companies.AddAsync(company, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return company.CompanyId;
        }


        /// <summary>
        /// Reading current company info.
        /// </summary>
        /// <returns>Current company data.</returns>
        public async Task<ReadCompanyResponseDTO> ReadCompanyAsync(int companyId, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == companyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");
            var parentCompany = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == company.ParentCompanyId, cancellationToken);
            #endregion


            var result = new ReadCompanyResponseDTO()
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                ParentCompanyId = company.ParentCompanyId,
                ParentCompanyName = parentCompany?.CompanyName ?? "",
                Members = company.Members.Select(p => new PersonShortModelDTO()
                {
                    PersonId = p.PersonId,
                    PersonName = p.PersonName,
                }).ToList(),
            };


            return result;
        }



        /// <summary>
        /// Get info about all registered companies.
        /// </summary>
        /// <returns>List of registered companies data.</returns>
        public async Task<IEnumerable<ReadCompanyResponseDTO>> ReadCompaniesAsync(CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var companies =
                await _context.Companies.ToListAsync(cancellationToken)
                ?? throw new Exception("Company wasn't found!");
            #endregion


            var result = new List<ReadCompanyResponseDTO>();
            foreach (var company in companies)
            {
                result.Add(new ReadCompanyResponseDTO()
                {
                    CompanyId = company.CompanyId,
                    CompanyName = company.CompanyName,
                    ParentCompanyId = company.ParentCompanyId,
                    Members = company.Members.Select(p => new PersonShortModelDTO()
                    {
                        PersonId = p.PersonId,
                        PersonName = p.PersonName,
                    }).ToList(),
                });
            }


            return result;
        }


        /// <summary>
        /// Calculation salary of all employees assigned to current company.
        /// </summary>
        /// <returns>Calculated salaries value of current company.</returns>
        public async Task<double> QueryCompanyPaymentAsync(QueryCompanyPaymentRequestDTO request, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");
            #endregion

            double result = 0;
            var members = company.Members.ToHashSet();
            var salaryService = new SalaryService(members, request.PaymentDate);
            foreach (var member in members)
            {
                result += salaryService.GetSalary(member.PersonId);
            }

            return result;
        }


        /// <summary>
        /// Editing current company data.
        /// </summary>
        /// <returns>TRUE if editing was successful.</returns>
        public async Task<bool> UpdateCompanyAsync(UpdateCompanyRequestDTO request, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");
            #endregion

            #region REASSIGNING TO OTHER PARENT COMPANY
            if (request.ParentCompanyId is not null)
            {
                var parentCompany =
                    await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.ParentCompanyId, cancellationToken)
                    ?? throw new Exception("Parent company wasn't found!");
            }
            #endregion

            company.CompanyName = request.CompanyName.IsNullOrEmpty() ? company.CompanyName : request.CompanyName;
            company.ParentCompanyId = request.ParentCompanyId;


            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }


        /// <summary>
        /// Deleting current company.
        /// </summary>
        /// <returns>TRUE if deleting was successful.</returns>
        public bool DeleteCompany(int companyId)
        {
            #region DB REQUESTS
            var company = _context.Companies.FirstOrDefault(c => c.CompanyId == companyId)
                ?? throw new Exception("Company wasn't found!");
            #endregion

            _context.Companies.Remove(company);
            _context.SaveChanges();

            return true;
        }
    }
}
