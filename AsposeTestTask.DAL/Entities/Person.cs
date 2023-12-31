﻿using AsposeTestTask.Constants;

namespace AsposeTestTask.Entities
{
    public class Person
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        /// <summary>
        /// This is main person salary without any bonuses.
        /// </summary>
        public double Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public CompanyRole Role { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int? BossId { get; set; }
    }
}
