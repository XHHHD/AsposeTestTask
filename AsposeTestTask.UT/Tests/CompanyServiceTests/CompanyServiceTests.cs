using AsposeTestTask.BLL.Services;
using AsposeTestTask.DAL.Data;
using AsposeTestTask.DTO.Company.Requests;
using AsposeTestTask.Entities;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using AsposeTestTask.Constants;

namespace AsposeTestTask.UT.Tests.CompanyServiceTests
{
    [TestClass]
    public class CompanyServiceTests
    {
        private AsposeContext _context;


        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<AsposeContext>()
                .UseInMemoryDatabase(databaseName: "AsposeTestDb")
                .Options;

            _context = new AsposeContext(options);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [DataRow("Test Company")]
        [TestMethod]
        public async Task Create_ShouldCreateCompany(string companyName)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var request = new CreateCompanyRequestDTO { CompanyName = companyName };


            //ACT
            var companyId = await companyService.CreateCompanyAsync(request, CancellationToken.None);


            //ASSERT
            var resultFromDB = _context.Companies.SingleOrDefault(c => c.CompanyName == companyName);
            Assert.IsNotNull(resultFromDB);
            Assert.AreEqual(companyId, resultFromDB.CompanyId);
        }

        [DataRow("Parent Company", "Child Company")]
        [TestMethod]
        public async Task Create_ShouldCreateChildCompany(string parentCompanyName, string childCompanyName)
        {
            //ARRANGE
            var parentCompany = new Company() { CompanyName = parentCompanyName };
            _context.Companies.Add(parentCompany);
            _context.SaveChanges();

            var companyService = new CompanyService(_context);
            var request = new CreateCompanyRequestDTO { CompanyName = childCompanyName, ParentCompanyId = parentCompany.CompanyId };


            //ACT
            var companyId = await companyService.CreateCompanyAsync(request, CancellationToken.None);


            //ASSERT
            var resultFromDB = _context.Companies.SingleOrDefault(c => c.CompanyName == childCompanyName);
            Assert.IsNotNull(resultFromDB);
            Assert.AreEqual(resultFromDB.CompanyId, companyId);
            Assert.AreEqual(resultFromDB.ParentCompanyId, parentCompany.CompanyId);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public async Task Create_ShouldThrowException_WhenCompanyNameIsNull_Or_Empty(string companyName)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var request = new CreateCompanyRequestDTO() { CompanyName = companyName };


            //ACT
            Func<Task> act = async () => await companyService.CreateCompanyAsync(request, CancellationToken.None);


            //ASSERT
            await act.Should().ThrowAsync<Exception>("You must fill in company name!");
        }

        [DataRow("Test Company", 0)]
        [DataRow("Test Company", 999)]
        [TestMethod]
        public async Task Create_ShouldThrowException_WhenParentCompanyNotExist(string companyName, int parentCompanyId)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var request = new CreateCompanyRequestDTO() { CompanyName = companyName, ParentCompanyId = parentCompanyId };


            //ACT
            Func<Task> act = async () => await companyService.CreateCompanyAsync(request, CancellationToken.None);


            //ASSERT
            await act.Should().ThrowAsync<Exception>("You must fill in company name!");
        }

        [DataRow("Test Company")]
        [TestMethod]
        public async Task Read_ShouldReturnCompany(string companyName)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var company = new Company() { CompanyName = companyName };
            _context.Companies.Add(company);
            _context.SaveChanges();


            //ACT
            var result = await companyService.ReadCompanyAsync(company.CompanyId, CancellationToken.None);


            //ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.CompanyName, company.CompanyName);
        }

        [DataRow("Test Company", "John Doe")]
        [TestMethod]
        public async Task Query_ShouldReturnPayments_OfCompany(string companyName, string name)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var company = new Company() { CompanyName = companyName };
            _context.Companies.Add(company);
            _context.SaveChanges();

            var manager1 = new Person()
            {
                PersonName = name,
                Salary = 100,
                DateOfHire = DateTime.Now.AddYears(-10),
                Role = CompanyRole.Manager,
                Company = company,
            };
            _context.Persons.Add(manager1);
            _context.SaveChanges();

            var manager2 = new Person()
            {
                PersonName = name,
                Salary = 100,
                DateOfHire = DateTime.Now.AddYears(-5),
                Role = CompanyRole.Sales,
                Company = company,
                BossId = manager1.PersonId,
            };
            _context.Persons.Add(manager2);
            _context.SaveChanges();

            var sales = new Person()
            {
                PersonName = name,
                Salary = 100,
                DateOfHire = DateTime.Now.AddYears(-1),
                Role = CompanyRole.Sales,
                Company = company,
                BossId = manager2.PersonId,
            };
            var employee = new Person()
            {
                PersonName = name,
                Salary = 100,
                DateOfHire = DateTime.Now,
                Role = CompanyRole.Employee,
                Company = company,
                BossId = manager2.PersonId,
            };
            _context.Persons.Add(sales);
            _context.Persons.Add(employee);
            _context.SaveChanges();

            var request = new QueryCompanyPaymentRequestDTO()
            {
                CompanyId = company.CompanyId,
                PaymentDate = DateTime.Now,
            };


            //ACT
            var result = await companyService.QueryCompanyPaymentAsync(request, CancellationToken.None);


            //ASSERT
            Assert.AreEqual(result, 446.6);
        }

        [DataRow("New company", "Old company")]
        [TestMethod]
        public async Task Update_ShouldMakeChangesInCompany(string firstCompanyName, string secondCompanyName)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var company = new Company() { CompanyName = firstCompanyName, };
            _context.Companies.Add(company);
            _context.SaveChanges();
            var request = new UpdateCompanyRequestDTO { CompanyId = company.CompanyId, CompanyName = secondCompanyName };


            //ACT
            await companyService.UpdateCompanyAsync(request, CancellationToken.None);


            //ASSERT
            var updatedCompany = _context.Companies.SingleOrDefault(c => c.CompanyId == company.CompanyId);
            Assert.IsNotNull(updatedCompany);
            Assert.AreEqual(updatedCompany.CompanyName, secondCompanyName);
        }

        [DataRow("Test Company")]
        [TestMethod]
        public async Task Delete_ShouldRemoveCompany(string companyName)
        {
            //ARRANGE
            var companyService = new CompanyService(_context);
            var company = new Company() { CompanyName = companyName };
            _context.Companies.Add(company);
            _context.SaveChanges();
            var companyId = company.CompanyId;


            //ACT
            await companyService.DeleteCompany(companyId, CancellationToken.None);


            //ASSERT
            Assert.IsNull(_context.Companies.FirstOrDefault(c => c.CompanyId == companyId));
        }
    }
}
