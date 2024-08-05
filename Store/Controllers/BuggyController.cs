using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{

    public class BuggyController : BaseApiController
    {
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ProblemDetails { Title = "Este es un bad request"});
        }

        [HttpGet("unathorized")]
        public ActionResult GetUnathorized()
        {
            return Unauthorized();
        }

        [HttpGet("validation-error")]
        public ActionResult GerValidationError()
        {
            ModelState.AddModelError("Problem1", "Este es el primer error");
            ModelState.AddModelError("Problem2", "Este es el segundo error");
            return ValidationProblem();
        }

        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            throw new Exception("Este es un error del server");
        }
    }
}
