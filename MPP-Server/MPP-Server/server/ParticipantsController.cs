using Microsoft.AspNetCore.Mvc;
using MPP_Server.model;
using MPP_Server.service;

namespace MPP_Server.server
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IService _service;

        public ParticipantsController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Participant>> GetAll()
        {
            return Ok(_service.GetParticipants());
        }

        [HttpGet("{id}")]
        public ActionResult<Participant?> Get(int id)
        {
            var p = _service.FindParticipant(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public ActionResult<int> AddParticipant([FromBody] Participant p)
        {
            if (p == null)
                return BadRequest("Participant data is required.");
            bool ok = _service.AddParticipant(p.ID, p.Name);
            if (!ok)
                return BadRequest("Could not add the participant.");
            return Ok(p.ID);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (_service.RemoveParticipant(id))
                return NoContent();
            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Participant p)
        {
            if (p == null || p.ID != id)
                return BadRequest("Participant ID mismatch or participant is null.");
            if (_service.UpdateParticipant(id, p.Name))
                return NoContent();
            return BadRequest("Could not update participant.");
        }
    }
}