using Microsoft.AspNetCore.Mvc;
using MPP_Server.model;
using MPP_Server.service;

namespace MPP_Server.server
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipationsController : ControllerBase
    {
        private readonly IService _service;

        public ParticipationsController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Participation>> GetAll()
        {
            return Ok(_service.GetParticipations());
        }

        [HttpGet("{id}")]
        public ActionResult<Participation?> Get(int id)
        {
            var p = _service.FindParticipation(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public ActionResult<int> AddParticipation([FromBody] Participation p)
        {
            if (p == null)
                return BadRequest("Participation data is required.");
            bool ok = _service.AddParticipation(p.ID, p.Participant, p.Race, p.Points);
            if (!ok)
                return BadRequest("Could not add the participation.");
            return Ok(p.ID);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (_service.RemoveParticipation(id))
                return NoContent();
            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Participation p)
        {
            if (p == null || p.ID != id)
                return BadRequest("Participation ID mismatch or participation is null.");
            if (_service.UpdateParticipation(id, p.Participant, p.Race, p.Points))
                return NoContent();
            return BadRequest("Could not update participation.");
        }
    }
}