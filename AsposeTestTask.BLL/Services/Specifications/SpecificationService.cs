using AsposeTestTask.BLL.Services.Specifications;
using AsposeTestTask.Constants;
using AsposeTestTask.Entities;

namespace AsposeTestTask.DAL.Constants.Specifications
{
    public static class SpecificationService
    {
        public static double GetMemberAdditionalInterest(int personId, int yearsOfExperience, CompanyRole role, List<Person> members)
        {
            var vM = new SpecificationVM();

            switch (role)
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

        public static int GetSubordinatesFirstLevelCount(int personId, List<Person> members)
        {
            var subordinates = members.Where(m => m.BossId == personId);

            return subordinates.Count();
        }

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
