namespace FUEM.Domain.Common
{
    public class SearchEventCriteria
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? OrganizerId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
