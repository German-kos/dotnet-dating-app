using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        // create some methods to return different types of errors
        [Authorize] // this will return a 401 unauthorized error
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        // return not found error
        [HttpGet("not-found")] // GET api/buggy/not-found
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        // return server error
        [HttpGet("server-error")] // GET api/buggy/server-error
        public ActionResult<string> GetServerError()
        {

            try
            {
                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString(); // NullReferenceException: 'thing' will be null (checkout find inline doc)
                return thingToReturn;
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Computer says no!");
            }
        }

        // return bad request error
        [HttpGet("bad-request")] // GET api/buggy/bad-request
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request"); //this will return a 400 bad request error
        }
    }
}