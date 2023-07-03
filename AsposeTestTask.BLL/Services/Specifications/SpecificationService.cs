using AsposeTestTask.BLL.Services.Specifications;
using AsposeTestTask.Constants;
using AsposeTestTask.Entities;

namespace AsposeTestTask.DAL.Constants.Specifications
{
    public static class SpecificationService
    {
        /// <summary>
        /// Calculating bonuses for current person.
        /// </summary>
        /// <param name="personId">Current person Id.</param>
        /// <param name="yearsOfExperience">Work experience in current company.</param>
        /// <param name="members">All members in current company.</param>
        /// <returns>Person salary value.</returns>
        public static double GetMemberAdditionalInterest(int personId, int yearsOfExperience, List<Person> members)
        {
            var vM = new SpecificationVM();
            var person = members.FirstOrDefault(m => m.PersonId == personId)
                ?? throw new Exception("Person wasn't found in collection!");

            switch (person.Role)
            {
                case CompanyRole.Employee:
                    {
                        vM.AdditionalInterestMax = 0.3;
                        vM.AdditionalInterestCurrent = yearsOfExperience * 0.03;
                        break;
                    }
                case CompanyRole.Manager:
                    {
                        vM.AdditionalInterestMax = 0.4;
                        vM.SubordinatesCount = GetSubordinatesFirstLevelCount(personId, members);
                        double subordinatesAdditionalInterest = vM.SubordinatesCount * 0.005;
                        vM.AdditionalInterestCurrent = yearsOfExperience * 0.05 + subordinatesAdditionalInterest;
                        break;
                    }
                case CompanyRole.Sales:
                    {
                        vM.AdditionalInterestMax = 0.35;
                        vM.SubordinatesCount = GetSubordinatesAllLevelsCount(personId, members);
                        double subordinatesAdditionalInterest = vM.SubordinatesCount * 0.003;
                        vM.AdditionalInterestCurrent = yearsOfExperience * 0.01 + subordinatesAdditionalInterest;
                        break;
                    }
            }
            vM.AdditionalInterestCurrent = vM.AdditionalInterestCurrent > vM.AdditionalInterestMax
                ? vM.AdditionalInterestMax
                : vM.AdditionalInterestCurrent;


            return vM.AdditionalInterestCurrent;
        }


        /// <summary>
        /// Get subordinates first level of current boss-person.
        /// </summary>
        /// <param name="personId">Current Boss Id.</param>
        /// <param name="members">All employees of company.</param>
        /// <returns>Subordinates first level count.</returns>
        public static int GetSubordinatesFirstLevelCount(int personId, List<Person> members)
        {
            var subordinates = members.Where(m => m.BossId == personId);

            return subordinates.Count();
        }


        /// <summary>
        /// Get subordinates of current boss-person.
        /// </summary>
        /// <param name="personId">Current Boss Id.</param>
        /// <param name="members">All employees of company.</param>
        /// <returns>Subordinates count.</returns>
        public static int GetSubordinatesAllLevelsCount(int personId, List<Person> members)
        {
            var subordinates = members.Where(m => m.BossId == personId);
            int subordinatesCount = subordinates.Count();

            foreach (var member in subordinates)
            {
                subordinatesCount += GetSubordinatesAllLevelsCount(member.PersonId, members);
            }

            return subordinatesCount;
        }
    }
}
