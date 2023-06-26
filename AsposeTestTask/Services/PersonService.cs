using AsposeTestTask.Constants;
using AsposeTestTask.DTO.Models;
using AsposeTestTask.Entities;

namespace AsposeTestTask.Services
{
    public class PersonService
    {
        public int AddNewPerson(PersonDTO personDTO)
        {
            if (personDTO is null) { throw new Exception("Bad Request!"); }
            var company = new Company(); //REQUEST TO DB

            var person = new Person()
            {
                Name = personDTO.Name,
                Level = personDTO.Level,
                DateOfHire = personDTO.DateOfHire,
                BossId = personDTO.BossId,
                Company = company,
            };

            switch (personDTO.Level)
            {
                case PersonLevel.Employee:
                    {

                        break;
                    }
                case PersonLevel.Manager:
                    {
                        break;
                    }
                case PersonLevel.Sales:
                    {
                        break;
                    }
            }

            int personId = 0; //ADD PERSON TO DB AND SAVE CHANGES

            return personId;
        }

        public double GetPersonPayment(Person person, DateTime currentDate)
        {
            double salary;
            int subordinatesCount = 0;
            int additionalInterestMax = 0;
            double additionalInterestCurrent = 0;
            int yearsOfExperience = currentDate.Year - person.DateOfHire.Year;

            switch (person.Level)
            {
                case PersonLevel.Employee:
                    {
                        additionalInterestMax = 30;
                        additionalInterestCurrent = yearsOfExperience * 3;
                        break;
                    }
                case PersonLevel.Manager:
                    {
                        additionalInterestMax = 40;
                        subordinatesCount = GetSubordinatesFirstLevelCount(person.Id);
                        double subordinatesAdditionalInterest = subordinatesCount * 0.5;
                        additionalInterestCurrent = yearsOfExperience * 5 + subordinatesAdditionalInterest;
                        break;
                    }
                case PersonLevel.Sales:
                    {
                        additionalInterestMax = 35;
                        subordinatesCount = GetSubordinatesAllLevelsCount(person.Id);
                        double subordinatesAdditionalInterest = subordinatesCount * 0.3;
                        additionalInterestCurrent = yearsOfExperience * 1 + subordinatesAdditionalInterest;
                        break;
                    }
            }
            if (additionalInterestCurrent > additionalInterestMax) { additionalInterestCurrent = additionalInterestMax; }

            salary = person.Salary + person.Salary * additionalInterestCurrent;

            return salary;
        }

        public int GetSubordinatesFirstLevelCount(int personId)
        {
            return 0;
        }

        public int GetSubordinatesAllLevelsCount(int personId)
        {
            return 0;
        }
    }
}
