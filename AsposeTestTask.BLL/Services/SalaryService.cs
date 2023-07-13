using AsposeTestTask.Entities;

namespace AsposeTestTask.BLL.Services
{
    public class SalaryService
    {
        private const double managerSubsBonus = 0.005;
        private const double salesSubsBonus = 0.003;
        private const double employeeYearBonus = 0.03;
        private const double managerYearBonus = 0.05;
        private const double salesYearBonus = 0.01;
        private const double employeeMaxBonus = 0.3;
        private const double managerMaxBonus = 0.4;
        private const double salesMaxBonus = 0.35;
        private readonly DateTime _salaryDate;
        private readonly IEnumerable<Person> _members;

        public SalaryService(IEnumerable<Person> members, DateTime salaryDate)
        {
            _salaryDate = salaryDate;
            _members = members;
        }


        /// <summary>
        /// Calculate person salary for current date.
        /// </summary>
        /// <returns>Person salary</returns>
        public double CalculateSalary(int personId)
        {
            var member = _members.FirstOrDefault(m => m.PersonId == personId)
                ?? throw new Exception("Person wasn't found in collection!");
            double salary = 0;

            switch (member.Role)
            {
                case Constants.CompanyRole.Employee:
                    salary = GetEmployeeSalary(member);
                    break;
                case Constants.CompanyRole.Manager:
                    salary = GetManagerSalary(member);
                    break;
                case Constants.CompanyRole.Sales:
                    salary = GetSalesSalary(member);
                    break;
            }

            return salary;
        }


        /// <summary>
        /// Calculate Employee salary.
        /// </summary>
        /// <param name="person">Employee person.</param>
        /// <returns>Employee salary.</returns>
        public double GetEmployeeSalary(Person person)
        {
            double bonus = GetYearsOfExperience(person.DateOfHire) * employeeYearBonus;
            if (bonus > employeeMaxBonus)
            {
                bonus = employeeMaxBonus;
            }

            double result = person.Salary + person.Salary * bonus;

            return result;
        }


        /// <summary>
        /// Calculate Manager salary.
        /// </summary>
        /// <param name="person">Manager person.</param>
        /// <returns>Manager salary.</returns>
        public double GetManagerSalary(Person person)
        {
            double yearsBonus = GetYearsOfExperience(person.DateOfHire) * managerYearBonus * person.Salary;
            var maxBonusSalary = person.Salary * managerMaxBonus;
            var subsBonus = GetSubordinatesSalary(person, false) * managerSubsBonus;
            var bonusSalary = subsBonus + yearsBonus;

            bonusSalary = bonusSalary <= maxBonusSalary
                ? bonusSalary
                : maxBonusSalary;

            double result = person.Salary + bonusSalary;

            return result;
        }


        /// <summary>
        /// Calculate Sales salary.
        /// </summary>
        /// <param name="person">Sales person.</param>
        /// <returns>Sales salary.</returns>
        public double GetSalesSalary(Person person)
        {
            double yearsBonus = GetYearsOfExperience(person.DateOfHire) * salesYearBonus * person.Salary;
            var maxBonusSalary = person.Salary * salesMaxBonus;
            var subsBonus = GetSubordinatesSalary(person, true) * salesSubsBonus;
            var bonusSalary = subsBonus + yearsBonus;

            bonusSalary = bonusSalary <= maxBonusSalary
                ? bonusSalary
                : maxBonusSalary;

            double result = person.Salary + bonusSalary;

            return result;
        }


        /// <summary>
        /// Calculate subordinates salary.
        /// </summary>
        /// <param name="person">Subordinate person.</param>
        /// <param name="isAllLevels">TRUE if it should be the salary of subordinates and the salary of their subordinates, 
        /// FALSE if it should be just salary of subordinates.</param>
        /// <returns>Subordinates salary.</returns>
        public double GetSubordinatesSalary(Person person, bool isAllLevels)
        {
            double subSalary = 0;
            var subordinates = _members.Where(m => m.BossId == person.PersonId);

            foreach (var member in subordinates)
            {
                if (isAllLevels)
                {
                    subSalary += GetSubordinatesSalary(member, isAllLevels);
                }

                subSalary += member.Salary;
            }

            return subSalary;
        }


        /// <summary>
        /// Calculate person experience count on current date.
        /// </summary>
        /// <param name="dateOfHire">Date of hiring person.</param>
        /// <returns>Finished years of work count.</returns>
        public int GetYearsOfExperience(DateTime dateOfHire)
        {
            int yearsOfExperience = _salaryDate.Year - dateOfHire.Year;

            if (dateOfHire.DayOfYear > _salaryDate.DayOfYear)
            {
                yearsOfExperience--;
            }
            if (yearsOfExperience < 0)
            {
                yearsOfExperience = 0;
            }

            return yearsOfExperience;
        }
    }
}
