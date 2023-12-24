using backendresume.Core.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backendresume.Core.Dtos.Company;
using AutoMapper;
using backendresume.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace backendresume.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private AppDbContext _context { get; }
        private IMapper _mapper;
        public CompanyController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCompany([FromBody] ComapanyCreateDto dto)
        {
            try
            {
                Company newCompany = _mapper.Map<Company>(dto);
                await _context.Companies.AddAsync(newCompany);
                await _context.SaveChangesAsync();

                return Ok("Company Create Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<CompanyGetDto>>> GetCompanies()
        {
            try
            {
                var companies = await _context.Companies.OrderByDescending(q => q.CreatedAt).ToListAsync();
                var convertedCompanies = _mapper.Map<IEnumerable<CompanyGetDto>>(companies);

                return Ok(convertedCompanies);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Read (Get Company By Id)



    }
}
