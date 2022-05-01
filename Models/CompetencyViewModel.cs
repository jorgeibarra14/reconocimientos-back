using System;

namespace Reconocimientos.Models
{
    public class CompetencyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Img { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
