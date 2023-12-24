using AutoMapper;
using backendresume.Core.Context;
using backendresume.Core.Dtos.Candidate;
using backendresume.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendresume.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private AppDbContext _context { get; }
        private IMapper _mapper;
        public CandidateController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDto dto, IFormFile pdfFile)
        {
            try
            {
                var fiveMegaByte = 5 * 1024 * 1024;
                var pdfMimeType = "application/pdf";

                if (pdfFile.Length > fiveMegaByte || pdfFile.ContentType != pdfMimeType)
                {
                    return BadRequest("File is not valid");
                }

                var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "pdfs", resumeUrl);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }
                var newCandidate = _mapper.Map<Candidate>(dto);
                newCandidate.ResumeUrl = resumeUrl;
                await _context.Candidates.AddAsync(newCandidate);
                await _context.SaveChangesAsync();

                return Ok(newCandidate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<CandidateGetDto>>> GetCandidate()
        {
            try
            {
                var candidates = await _context.Candidates.Include(c => c.Job).OrderByDescending(q => q.CreatedAt).ToListAsync();
                var convertedCandidates = _mapper.Map<IEnumerable<CandidateGetDto>>(candidates);

                return Ok(convertedCandidates);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("download/{url}")]
        public IActionResult DownloadPdfFile(string url)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "pdfs", url);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File Not Found");
                }

                var pdfBytes = System.IO.File.ReadAllBytes(filePath);
                var file = File(pdfBytes, "application/pdf", url);

                return file;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
