using AsposeTestTask.Entities;

namespace AsposeTestTask.BLL.Services
{
    public class SalaryService
    {
        private const double _managerSubsBonus = 0.005;
        private const double _salesSubsBonus = 0.003;
        private const double _employeeYearBonus = 0.03;
        private const double _managerYearBonus = 0.05;
        private const double _salesYearBonus = 0.01;
        private const double _employeeMaxBonus = 0.3;
        private const double _managerMaxBonus = 0.4;
        private const double _salesMaxBonus = 0.35;
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
        public double GetSalary(int personId)
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
            double bonus = GetYearsOfExperience(person.DateOfHire) * _employeeYearBonus;
            if (bonus > _employeeMaxBonus)
            {
                bonus = _employeeMaxBonus;
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
            double yearsBonus = GetYearsOfExperience(person.DateOfHire) * _managerYearBonus * person.Salary;
            var maxBonusSalary = person.Salary * _managerMaxBonus;
            var subsBonus = GetSubordinatesSalary(person, false) * _managerSubsBonus;
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
            double yearsBonus = GetYearsOfExperience(person.DateOfHire) * _salesYearBonus * person.Salary;
            var maxBonusSalary = person.Salary * _salesMaxBonus;
            var subsBonus = GetSubordinatesSalary(person, false) * _salesSubsBonus;
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
        /// <param name="isItInfinite">TRUE if it should be the salary of subordinates and the salary of their subordinates, 
        /// FALSE if it should be just salary of subordinates.</param>
        /// <returns>Subordinates salary.</returns>
        public double GetSubordinatesSalary(Person person, bool isItInfinite)
        {
            double subSalary = 0;
            var subordinates = _members.Where(m => m.BossId == person.PersonId);

            foreach (var member in subordinates)
            {
                if (isItInfinite)
                {
                    subSalary += GetSubordinatesSalary(member, isItInfinite);
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
