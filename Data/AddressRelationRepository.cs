using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Asnapper.Hal101.Models;
using Microsoft.Extensions.Hosting;

namespace Asnapper.Hal101.Data
{
    public class AddressRelationRepository
    {
        private readonly string _dataPath;
        private AddressRelation[] _addressRelations;

        public AddressRelationRepository(IHostEnvironment hostingEnvironment)
        {
            _dataPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data/addressesRelations.json");
        }
                
        private void EnsureDataLoaded()
        {
            if (_addressRelations != null)
            {
                return;
            }
            
            var json = File.ReadAllText(_dataPath);
            _addressRelations = JsonSerializer.Deserialize<AddressRelation[]>(json);
        }

        public PagedList<AddressRelation> List(Paging paging)
        {
            EnsureDataLoaded();

            var items = _addressRelations.Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToArray();
            
            return new PagedList<AddressRelation>
            {
                CurrentPage = paging.Page,
                TotalItems = _addressRelations.Length,
                TotalPages = (int)Math.Ceiling(_addressRelations.Length / (double)paging.PageSize),
                Items = items
            };
        }

        public AddressRelation GetForPerson(int id)
        {
            EnsureDataLoaded();
            return _addressRelations.SingleOrDefault(x => x.PersonId == id);
        }
        
    }
}