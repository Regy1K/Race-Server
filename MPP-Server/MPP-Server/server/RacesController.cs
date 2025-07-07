using Microsoft.AspNetCore.Mvc;
using MPP_Server.model;
using MPP_Server.service;

namespace MPP_Server.server
{
    [ApiController]
    [Route("api/[controller]")]
    public class RacesController : ControllerBase
    {
        private readonly IService _service;

        public RacesController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Race>> GetAll()
        {
            return Ok(_service.GetRaces());
        }

        [HttpGet("{id}")]
        public ActionResult<Race?> Get(int id)
        {
            var race = _service.FindRace(id);
            if (race == null) return NotFound();
            return Ok(race);
        }

        [HttpPost]
        public ActionResult<int> AddRace([FromBody] Race race)
        {
            if (race == null)
                return BadRequest("Race data is required.");

            var newId = _service.AddRace(race);
            if (newId == null)
                return BadRequest("Could not add the race.");

            return CreatedAtAction(nameof(Get), new { id = newId }, newId);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (_service.RemoveRace(id))
                return NoContent();

            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Race race)
        {
            if (race == null || race.ID != id)
                return BadRequest("Race ID mismatch or race is null.");

            if (_service.UpdateRace(race))
                return NoContent();

            return BadRequest("Could not update race.");
        }
    }
}