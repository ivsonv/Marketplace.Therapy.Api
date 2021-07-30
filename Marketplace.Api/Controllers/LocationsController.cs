using Marketplace.Domain.Models.Response;
using Marketplace.Domain.Models.Response.locations;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.Api.Controllers
{
    [Route("api/locations")]
    public class LocationsController : DefaultController
    {
        private readonly LocationService _locationService;
        public LocationsController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("{zipcode}")]
        public async Task<BaseRs<locationRs>> FindById([FromRoute] string zipcode)
            => await _locationService.FindByZipCode(zipcode);
    }
}