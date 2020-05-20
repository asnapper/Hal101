using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asnapper.Hal101.Models.Hypermedia
{
    public class AddressRepresentation : Hal<Address>, 
                                        IHalLinks<Address>, 
                                        IHalEmbeddedAsync<Address>
    {
        public IEnumerable<Link> LinksFor(Address resource)
        {
            yield return new Link(Link.Self, $"/addresses/{resource.Id}");
            yield return new Link("people", $"/addresses/{resource.Id}/people");
        }

        public Task<object> EmbeddedForAsync(Address resource)
        {
            return Task.FromResult<object>(new
            {
                Hello = "World"
            });
        }
    }
    
    public class AddressListRepresentation : PagedListRepresentation<Address>
    {
        public AddressListRepresentation(AddressRepresentation AddressRepresentation) 
            : base("/addresses", AddressRepresentation) { }
    }
}