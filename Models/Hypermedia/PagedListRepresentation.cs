using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Hallo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asnapper.Hal101.Models.Hypermedia
{
    public abstract class PagedListRepresentation<TItem> : Hal<PagedList<TItem>>, IHalState<PagedList<TItem>>,
        IHalEmbedded<PagedList<TItem>>, IHalLinks<PagedList<TItem>>
        {
            private readonly string _keyName;
            private readonly IUrlHelper _urlHelper;
            private readonly IHalLinks<TItem> _itemLinks;

            private readonly IHttpContextAccessor _httpContextAccessor;

            protected PagedListRepresentation(string keyName, IHalLinks<TItem> itemLinks, IUrlHelper urlHelper, IHttpContextAccessor httpContextAccessor)
            {
                _keyName = keyName;
                _itemLinks = itemLinks;
                _urlHelper = urlHelper;
                _httpContextAccessor = httpContextAccessor;
            }

            public object StateFor(PagedList<TItem> resource)
            {
                return new
                {
                    resource.CurrentPage,
                        resource.TotalItems,
                        resource.TotalPages
                };
            }

            public object EmbeddedFor(PagedList<TItem> resource)
            {
                var items = from item in resource.Items
                let links = _itemLinks.LinksFor(item)
                select new HalRepresentation(item, links);

                var result = new Dictionary<string, object>();

                result[_keyName] = items;

                return result;
               

                // return new
                // {

                //     bla = items.ToList()
                // };
            }

            public IEnumerable<Link> LinksFor(PagedList<TItem> resource)
            {
                var self = _httpContextAccessor.HttpContext.Request.Path.ToString();
                yield return new Link("self", $"{self}?page={resource.CurrentPage}");
                yield return new Link("first", $"{self}?page=1");
                yield return new Link("last", $"{self}?page={resource.TotalPages}");

                if (resource.CurrentPage > 1)
                {
                    yield return new Link("prev", $"{self}?page={resource.CurrentPage - 1}");
                }

                if (resource.CurrentPage < resource.TotalPages)
                {
                    yield return new Link("next", $"{self}?page={resource.CurrentPage + 1}");
                }
            }
        }
}