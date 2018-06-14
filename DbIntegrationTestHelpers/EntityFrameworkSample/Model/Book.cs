using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkSample
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}
