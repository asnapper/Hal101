using System.Collections.Generic;
using System.Threading.Tasks;
using Hallo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asnapper.Hal101.Models.Hypermedia
{
    public class AddressRepresentation : Hal<Address>,
        IHalLinks<Address>
        {
            public IEnumerable<Link> LinksFor(Address resource)
            {
                yield return new Link(Link.Self, $"/addresses/{resource.Id}");
                yield return new Link("people", $"/addresses/{resource.Id}/people");
            }

        }

    public class AddressListRepresentation : PagedListRepresentation<Address>
    {
        public AddressListRepresentation(AddressRepresentation AddressRepresentation, IUrlHelper urlHelper, IHttpContextAccessor httpContextAccessor) : base("addresses", AddressRepresentation, urlHelper, httpContextAccessor) { }
    }

}