using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.Constants;
using AsposeTestTask.DAL.Constants.Specifications;
using AsposeTestTask.DAL.Data;
using AsposeTestTask.DTO.Company;
using AsposeTestTask.DTO.Person;
using AsposeTestTask.DTO.Person.Requests;
using AsposeTestTask.DTO.Person.Responses;
using AsposeTestTask.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AsposeTestTask.Services
{
    public class PersonService : IPersonService
    {
        private AsposeContext _context;


        public PersonService(AsposeContext context)
        {
            _context = context;
        }


        public async Task<int> CreatePerson(CreatePersonRequestDTO request, CancellationToken cancellationToken)
        {
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");


            if (request.BossId is not null)
            {
                var boss =
                    await _context.Persons
                        .FirstOrDefaultAsync(p => p.PersonId == request.BossId, cancellationToken)
                        ?? throw new Exception("Boss wasn't found!");

                if (boss.Role == CompanyRole.Employee)
                { throw new Exception("Employee can't be boss!"); }
            }

            var person = new Person()
            {
                PersonName = request.PersonName,
                Salary = request.Salary,
                DateOfHire = request.DateOfHire,
                Role = request.Role,
                BossId = request.BossId,
                Company = company,
            };


            await _context.Persons.AddAsync(person, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return person.PersonId;
        }

        public async Task<ReadPersonResponseDTO> ReadPerson(int personId, CancellationToken cancellationToken)
        {
            var person =
                await _context.Persons
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken)
                ?? throw new Exception("Person wasn't found!");


            PersonShortModelDTO? boss = null;
            if (person.BossId is not null)
            {
                var personBoss =
                    await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == person.BossId, cancellationToken)
                    ?? throw new Exception("Boss wasn't found!");

                if (personBoss.Role == CompanyRole.Employee)
                { throw new Exception("Employee can't be boss!"); }

                boss = new()
                {
                    PersonId = personBoss.PersonId,
                    PersonName = personBoss.PersonName,
                };
            }

            var result = new ReadPersonResponseDTO()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Salary = person.Salary,
                DateOfHire = person.DateOfHire,
                Role = person.Role.ToString(),
                Boss = boss,
                Company = new CompanyShortModelDTO()
                {
                    CompanyId = person.Company.CompanyId,
                    CompanyName = person.Company.CompanyName,
                },
            };


            return result;
        }


        public async Task<List<ReadPersonResponseDTO>> ReadCompanyPersons(int personId, CancellationToken cancellationToken)
        {
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == personId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");

            var result = company.Members.Select(m => new ReadPersonResponseDTO()
            {
                PersonId = m.PersonId,
                PersonName = m.PersonName,
                Salary = m.Salary,
                DateOfHire = m.DateOfHire,
                Role = m.Role.ToString(),
                Boss = GetBoss(m.BossId),
                Company = new CompanyShortModelDTO()
                {
                    CompanyId = m.Company.CompanyId,
                    CompanyName = m.Company.CompanyName,
                },
            }).ToList();

            return result;
        }


        public async Task<List<ReadPersonResponseDTO>> ReadAllPersons(CancellationToken cancellationToken)
        {
            var persons =
                await _context.Persons
                .Include(p => p.Company)
                .ToListAsync(cancellationToken);

            var result = persons.Select(m => new ReadPersonResponseDTO()
            {
                PersonId = m.PersonId,
                PersonName = m.PersonName,
                Salary = m.Salary,
                DateOfHire = m.DateOfHire,
                Role = m.Role.ToString(),
                Boss = GetBoss(m.BossId),
                Company = new CompanyShortModelDTO()
                {
                    CompanyId = m.Company.CompanyId,
                    CompanyName = m.Company.CompanyName,
                },
            }).ToList();

            return result;
        }


        public async Task<double> QueryPersonPayment(QueryPersonPaymentRequestDTO request, CancellationToken cancellationToken)
        {
            var person =
                await _context.Persons
                .Include(p => p.Company)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(p => p.PersonId == request.PersonId, cancellationToken)
                ?? throw new Exception("Person wasn't found!");


            var members = person.Company.Members.ToList();
            int yearsOfExperience = request.PaymentDate.Year - person.DateOfHire.Year;
            var additionalInterest = SpecificationService.GetMemberAdditionalInterest(person.PersonId, yearsOfExperience, person.Role, members);
            double salary = person.Salary + person.Salary * additionalInterest;

            return salary;
        }

        public async Task<bool> UpdatePerson(UpdatePersonRequestDTO request, CancellationToken cancellationToken)
        {
            var person =
                await _context.Persons
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.PersonId == request.PersonId, cancellationToken)
                ?? throw new Exception("Person wasn't found!");


            if (request.BossId is not null)
            {
                var boss =
                    await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == request.BossId, cancellationToken)
                    ?? throw new Exception("Boss wasn't found!");

                person.BossId = boss.PersonId;
            }

            if (request.CompanyId is not null)
            {
                var company =
                    await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                    ?? throw new Exception("Company wasn't found!");

                company.Members.Add(person);
            }

            person.Salary = request.Salary is null ? person.Salary : (double)request.Salary;
            person.PersonName = request.PersonName.IsNullOrEmpty() ? person.PersonName : request.PersonName;
            person.DateOfHire = request.DateOfHire is null ? person.DateOfHire : (DateTime)request.DateOfHire;
            person.Role = request.Role is null ? person.Role : (CompanyRole)request.Role;


            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public bool DeletePerson(int personId)
        {
            var person = _context.Persons.FirstOrDefault(p => p.PersonId == personId)
                ?? throw new Exception("Person wasn't found!");

            _context.Persons.Remove(person);
            _context.SaveChanges();

            return true;
        }


        public PersonShortModelDTO? GetBoss(int? bossId)
        {
            if (bossId is null)
            {
                return null;
            }
            else
            {
                var boss = _context.Persons.FirstOrDefault(p => p.PersonId == bossId);

                if (boss == null)
                {
                    return null;
                }

                var result = new PersonShortModelDTO()
                {
                    PersonId = boss.PersonId,
                    PersonName = boss.PersonName,
                };

                return result;
            }
        }
    }
}
