using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlaceApp.Db;
using PlaceApp.Db.Models;

namespace PlaceApp.Controllers
{
    [ApiController]
    [Route("places")]
    public class PlacesController : ControllerBase
    {
        private readonly PlaceDbContext _dbContext;
        private readonly IHubContext<PlaceAppHub> _hubContext;

        public PlacesController(PlaceDbContext dbContext, IHubContext<PlaceAppHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetTopPlaces()
        {
            var places = _dbContext.Places.OrderByDescending(o => o.Reviews).Take(10);
            return Ok(places);
        }

        [HttpPost]
        public IActionResult CreateNewPlace([FromBody] Place place)
        {
            var newId = _dbContext.Places.Select(x => x.Id).Max() + 1;
            place.Id = newId;
            place.LastUpdated = DateTime.Now;

            _dbContext.Places.Add(place);
            int rowsAffected = _dbContext.SaveChanges();

            if (rowsAffected > 0)
            {
                _hubContext.Clients.All.SendAsync("NotifyNewPlaceAdded", place.Id, place.Name);
            }

            return Ok("New place has been added successfully.");
        }

        [HttpPut]
        public IActionResult UpdatePlace([FromBody] Place place)
        {
            var placeUpdate = _dbContext.Places.Find(place.Id);

            if (placeUpdate == null)
            {
                return NotFound();
            }

            placeUpdate.Name = place.Name;
            placeUpdate.Location = place.Location;
            placeUpdate.About = place.About;
            placeUpdate.Reviews = place.Reviews;
            placeUpdate.ImageData = place.ImageData;
            placeUpdate.LastUpdated = DateTime.Now;

            _dbContext.Update(placeUpdate);
            _dbContext.SaveChanges();

            return Ok("Place has been updated successfully.");
        }
    }
}