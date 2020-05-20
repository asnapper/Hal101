using Asnapper.Hal101.Data;
using Asnapper.Hal101.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asnapper.Hal101.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AddressRepository _repository;

        public AddressesController(AddressRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<PagedList<Address>> List([FromQuery] Paging paging)
        {
            return _repository.List(paging);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Address> Get(int id)
        {
            return _repository.Get(id);
        }
    }
}