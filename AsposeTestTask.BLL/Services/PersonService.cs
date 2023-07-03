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


        /// <summary>
        /// Creating person method.
        /// </summary>
        /// <returns>Created person Id.</returns>
        public async Task<int> CreatePerson(CreatePersonRequestDTO request, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");
            #endregion

            #region CHECKING BOSS
            if (request.BossId is not null)
            {
                var boss =
                    await _context.Persons
                        .FirstOrDefaultAsync(p => p.PersonId == request.BossId, cancellationToken)
                        ?? throw new Exception("Boss wasn't found!");

                //Check if supposed Boss have permissions get employees.
                if (boss.Role == CompanyRole.Employee)
                { throw new Exception("Employee can't be boss!"); }
            }
            #endregion

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


        /// <summary>
        /// Reading person data method.
        /// </summary>
        /// <returns>Person data.</returns>
        public async Task<ReadPersonResponseDTO> ReadPerson(int personId, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var person =
                await _context.Persons
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken)
                ?? throw new Exception("Person wasn't found!");
            #endregion

            var result = new ReadPersonResponseDTO()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Salary = person.Salary,
                DateOfHire = person.DateOfHire,
                Role = person.Role.ToString(),
                Boss = GetBoss(person.BossId),
                Company = new CompanyShortModelDTO()
                {
                    CompanyId = person.Company.CompanyId,
                    CompanyName = person.Company.CompanyName,
                },
            };


            return result;
        }

        /// <summary>
        /// Read data of all employees of current company.
        /// </summary>
        /// <returns>Data of employees of current company.</returns>
        public async Task<List<ReadPersonResponseDTO>> ReadCompanyPersons(int personId, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var company =
                await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == personId, cancellationToken)
                ?? throw new Exception("Company wasn't found!");
            #endregion

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
            #region DB REQUESTS
            var persons =
                await _context.Persons
                .Include(p => p.Company)
                .ToListAsync(cancellationToken);
            #endregion

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


        /// <summary>
        /// Person salary calculation.
        /// </summary>
        /// <param name="request">Calculation request form.</param>
        /// <returns>Calculated salary value.</returns>
        public async Task<double> QueryPersonPayment(QueryPersonPaymentRequestDTO request, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var person =
                await _context.Persons
                .Include(p => p.Company)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(p => p.PersonId == request.PersonId, cancellationToken)
                ?? throw new Exception("Person wasn't found!");
            #endregion


            var members = person.Company.Members.ToList();
            int yearsOfExperience = request.PaymentDate.Year - person.DateOfHire.Year;
            var additionalInterest = SpecificationService.GetMemberAdditionalInterest(person.PersonId, yearsOfExperience, members);
            double salary = person.Salary + person.Salary * additionalInterest;

            return salary;
        }


        /// <summary>
        /// Update person data.
        /// </summary>
        /// <returns>TRUE if update was successful.</returns>
        public async Task<bool> UpdatePerson(UpdatePersonRequestDTO request, CancellationToken cancellationToken)
        {
            #region DB REQUESTS
            var person =
                await _context.Persons
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.PersonId == request.PersonId, cancellationToken)
                ?? throw new Exception("Person wasn't found!");
            #endregion

            #region CHECKING BOSS
            if (request.BossId is not null)
            {
                var boss =
                    await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == request.BossId, cancellationToken)
                        ?? throw new Exception("Boss wasn't found!");

                //Check if supposed Boss have permissions get employees.
                if (boss.Role == CompanyRole.Employee)
                { throw new Exception("Employee can't be boss!"); }

                person.BossId = boss.PersonId;
            }
            #endregion

            #region REASSIGNING TO OTHER COMPANY
            if (request.CompanyId is not null)
            {
                var company =
                    await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == request.CompanyId, cancellationToken)
                    ?? throw new Exception("Company wasn't found!");

                company.Members.Add(person);
            }
            #endregion

            person.Salary = request.Salary is null ? person.Salary : (double)request.Salary;
            person.PersonName = request.PersonName.IsNullOrEmpty() ? person.PersonName : request.PersonName;
            person.DateOfHire = request.DateOfHire is null ? person.DateOfHire : (DateTime)request.DateOfHire;
            person.Role = request.Role is null ? person.Role : (CompanyRole)request.Role;


            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }


        /// <summary>
        /// Deleting current person.
        /// </summary>
        /// <returns>TRUE if deleting was successful.</returns>
        public bool DeletePerson(int personId)
        {
            #region DB REQUESTS
            var person = _context.Persons.FirstOrDefault(p => p.PersonId == personId)
                ?? throw new Exception("Person wasn't found!");
            #endregion

            _context.Persons.Remove(person);
            _context.SaveChanges();

            return true;
        }


        /// <summary>
        /// Searching Boss in DB.
        /// </summary>
        /// <returns>Boss person.</returns>
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
