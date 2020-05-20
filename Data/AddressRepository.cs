using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Asnapper.Hal101.Models;
using Microsoft.Extensions.Hosting;

namespace Asnapper.Hal101.Data
{
    public class AddressRepository
    {
        private readonly string _dataPath;
        private Address[] _addresses;

        public AddressRepository(IHostEnvironment hostingEnvironment)
        {
            _dataPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data/addresses.json");
        }

        private void EnsureDataLoaded()
        {
            if (_addresses != null)
            {
                return;
            }

            var json = File.ReadAllText(_dataPath);
            _addresses = JsonSerializer.Deserialize<Address[]>(json);
        }

        public PagedList<Address> List(Paging paging)
        {
            EnsureDataLoaded();

            var items = _addresses.Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToArray();

            return new PagedList<Address>
            {
                CurrentPage = paging.Page,
                TotalItems = _addresses.Length,
                TotalPages = (int) Math.Ceiling(_addresses.Length / (double) paging.PageSize),
                Items = items
            };
        }

        public Address Get(int id)
        {
            EnsureDataLoaded();
            return _addresses.SingleOrDefault(x => x.Id == id);
        }
    }
}