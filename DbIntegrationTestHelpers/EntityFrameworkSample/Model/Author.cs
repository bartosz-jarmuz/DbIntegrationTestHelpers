namespace EntityFrameworkSample
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Author 
    {
        public int Id { get; set; }

        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}