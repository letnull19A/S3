using Microsoft.AspNetCore.Mvc;
using W2B.S3.Contexts;
using W2B.S3.Models;
using W2B.S3.Models.DTOs;

namespace W2B.S3.Controllers
{
    [Route("api/bucket")]
    [ApiController]
    public class S3BucketController(S3DbContext context) : ControllerBase
    {
        private readonly S3DbContext _context = context;

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_context.Buckets.ToList());
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var result = _context.Buckets.FirstOrDefault(w => w.Id == id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateBucketDto value)
        {
            var data = new S3BucketModel()
            {
                Name = value.Name,
                MaxVolumeBytes = value.MaxSizeBytes
            };

            _context.Buckets.Add(data);

            _context.SaveChanges();

            return Ok(data);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] EditBucketDto value)
        {
            var bucket = _context.Buckets.FirstOrDefault(o => o.Id == id);

            if (bucket == null)
                return NotFound("bucket not found");

            if (bucket.Status == "deleted")
                return BadRequest("bucket deleted");

            var originByteSize = bucket.MaxVolumeBytes;

            if (value.MaxSizeBytes <= originByteSize)
                return BadRequest("bucket max size is less than origin");

            bucket.MaxVolumeBytes = value.MaxSizeBytes;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var bucket = _context.Buckets.FirstOrDefault(o => o.Id == id);

            if (bucket == null)
                return NotFound();
            
            if (bucket.Status == "deleted")
                return BadRequest("bucket deleted");

            var originByteSize = bucket.MaxVolumeBytes;

            bucket.Status = "deleted";

            _context.SaveChanges();

            return NoContent();
        }
    }
}