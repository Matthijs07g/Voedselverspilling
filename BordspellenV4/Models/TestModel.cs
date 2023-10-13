using Domain.Models;

namespace BordspellenV4.Models
{
    public class TestModel
    {
        public IEnumerable<Game> Games { get; set; } = Enumerable.Empty<Game>();
    }
}
