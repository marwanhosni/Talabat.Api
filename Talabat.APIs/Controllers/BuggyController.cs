using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository;


namespace Talabat.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly AppDbContext _context;

        public BuggyController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("notfound")] // api/Buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.products.Find(100);  // Null 
            if (product is null) return NotFound(new ApiRequest(404));
            return Ok(product);
        }

        [HttpGet("servererror")] // api/Buggy/servererror
        public ActionResult GetServerError()
        {
            var product = _context.products.Find(100);  // Null 
            var productToResult = product.ToString(); // Throw Exeption [NullReferenceExeption]
            return Ok(productToResult);

        }

        [HttpGet("BadRequest")] // api/Buggy/BadRequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiRequest(400));
        }

        [HttpGet("BadRequest/{id}")] // api/Buggy/BadRequest/Five
        public ActionResult GetBadRequest(int id) // Validation Error
        {
            return Ok();
        }
    }
}
