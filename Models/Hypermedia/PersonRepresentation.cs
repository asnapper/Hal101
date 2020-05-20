using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asnapper.Hal101.Models.Hypermedia
{
    public class PersonRepresentation : Hal<Person>, 
                                        IHalLinks<Person>, 
                                        IHalEmbeddedAsync<Person>
    {
        public IEnumerable<Link> LinksFor(Person resource)
        {
            yield return new Link(Link.Self, $"/people/{resource.Id}");
            yield return new Link("addresses", $"/people/{resource.Id}/addresses");
        }

        public Task<object> EmbeddedForAsync(Person resource)
        {
            return Task.FromResult<object>(new
            {
                Hello = "World"
            });
        }
    }
    
    public class PersonListRepresentation : PagedListRepresentation<Person>
    {
        public PersonListRepresentation(PersonRepresentation personRepresentation) 
            : base("/people", personRepresentation) { }
    }
}