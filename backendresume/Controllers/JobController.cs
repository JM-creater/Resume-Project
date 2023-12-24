using AutoMapper;
using backendresume.Core.Context;
using backendresume.Core.Dtos.Job;
using backendresume.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendresume.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private AppDbContext _context { get; }
        private IMapper _mapper;
        public JobController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateJob([FromBody] JobCreateDto dto)
        {
            try
            {
                var newJob = _mapper.Map<Job>(dto);
                await _context.Jobs.AddAsync(newJob);
                await _context.SaveChangesAsync();

                return Ok("Job Created Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Read
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<JobGetDto>>> GetJobs()
        {
            try
            {
                var jobs = await _context.Jobs.Include(job => job.Company).OrderByDescending(q => q.CreatedAt).ToListAsync();
                var convertedJobs = _mapper.Map<IEnumerable<JobGetDto>>(jobs);

                return Ok(convertedJobs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
