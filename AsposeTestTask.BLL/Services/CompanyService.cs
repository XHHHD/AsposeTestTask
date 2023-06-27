using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.Constants;
using AsposeTestTask.DAL.Constants.Specifications;
using AsposeTestTask.DAL.Data;
using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.DTO.Company.Responses;
using AsposeTestTask.DTO.Person;
using AsposeTestTask.Entities;
using AsposeTestTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

namespace AsposeTestTask.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private AsposeContext _context;


        public CompanyService(AsposeContext context)
        {
            _context = context;
        }


        public async Task<int> CreateCompany(CreateCompanyRequestDTO request, CancellationToken cancellationToken)
        {
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

            var company = new Company()
            {
                CompanyName = request.CompanyName,
                ParentCompanyId = request.ParentCompanyId,
            };

            await _context.Companies.AddAsync(company, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return company.CompanyId;
        }

        public async Task<ReadCompanyResponseDTO> ReadCompany(int companyId, CancellationToken cancellationToken)
        {
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == companyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");


            var result = new ReadCompanyResponseDTO()
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                ParentCompanyId = company.ParentCompanyId,
                Members = company.Members.Select(p => new PersonShortModelDTO()
                {
                    PersonId = p.PersonId,
                    PersonName = p.PersonName,
                }).ToList(),
            };


            return result;
        }

        public async Task<double> QueryCompanyPayment(QueryCompanyPaymentRequestDTO request, CancellationToken cancellationToken)
        {
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");


            var members = company.Members.ToList();
            double result = 0;

            foreach (var member in members)
            {
                int yearsOfExperience = request.PaymentDate.Year - member.DateOfHire.Year;
                var additionalInterest = SpecificationService.GetMemberAdditionalInterest(member.PersonId, yearsOfExperience, member.Role, members);
                result += member.Salary + member.Salary * additionalInterest;
            }


            return result;
        }

        public async Task<bool> UpdateCompany(UpdateCompanyRequestDTO request, CancellationToken cancellationToken)
        {
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");

            if (request.ParentCompanyId is not null)
            {
                var parentCompany =
                    await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.ParentCompanyId, cancellationToken)
                    ?? throw new Exception("Parent company wasn't found!");
            }


            company.CompanyName = request.CompanyName.IsNullOrEmpty() ? company.CompanyName : request.CompanyName;
            company.ParentCompanyId = request.ParentCompanyId;


            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteCompany(int companyId, CancellationToken cancellationToken)
        {
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == companyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
