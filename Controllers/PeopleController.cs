using System.Collections.Generic;
using System.Linq;
using Asnapper.Hal101.Data;
using Asnapper.Hal101.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asnapper.Hal101.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleRepository _repository;
        private readonly AddressRepository _addressRepository;
        private readonly AddressRelationRepository _addressRelationRepository;

        public PeopleController(PeopleRepository repository, AddressRepository addressRepository, AddressRelationRepository addressRelationRepository)
        {
            _repository = repository;
            _addressRepository = addressRepository;
            _addressRelationRepository = addressRelationRepository;
        }

        [HttpGet]
        public ActionResult<PagedList<Person>> List([FromQuery] Paging paging)
        {
            return _repository.List(paging);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Person> Get(int id)
        {
            return _repository.Get(id);
        }

        [HttpGet]
        [Route("{id}/addresses")]
        public ActionResult<PagedList<Address>> ListAddresses([FromQuery] Paging paging, int id) => _addressRelationRepository.ListForPerson(paging, id);
    }
}