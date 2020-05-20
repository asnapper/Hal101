using System;
using System.Collections.Generic;
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

        private readonly AddressRepository _addressRepository;

        public AddressRelationRepository(IHostEnvironment hostingEnvironment, AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
            _dataPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data/addressRelations.json");
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
                TotalPages = (int) Math.Ceiling(_addressRelations.Length / (double) paging.PageSize),
                Items = items
            };
        }

        public PagedList<Address> ListForPerson(Paging paging, int id)
        {
            EnsureDataLoaded();

            var relevantItems = _addressRelations
                .Where(x => x.PersonId == id)
                .ToArray();

            var items = relevantItems
                .Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .Select(x => _addressRepository.Get(x.AddressId))
                .ToArray();

            return new PagedList<Address>
            {
                CurrentPage = paging.Page,
                TotalItems = relevantItems.Length,
                TotalPages = (int) Math.Ceiling(relevantItems.Length / (double) paging.PageSize),
                Items = items
            };
        }

        public AddressRelation[] GetForPerson(int id)
        {
            EnsureDataLoaded();

            return _addressRelations.Where(x => x.PersonId == id).ToArray();
        }

    }
}