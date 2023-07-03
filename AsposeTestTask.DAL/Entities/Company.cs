namespace AsposeTestTask.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// Company can have hierarchy of other companies.
        /// </summary>
        public int? ParentCompanyId { get; set; }
        public virtual ICollection<Person> Members { get; set; } = new List<Person>();
    }
}
