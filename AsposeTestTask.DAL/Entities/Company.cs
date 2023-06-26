namespace AsposeTestTask.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? ParentCompanyId { get; set; }
        public ICollection<Person> Members { get; set; } = new List<Person>();
    }
}
