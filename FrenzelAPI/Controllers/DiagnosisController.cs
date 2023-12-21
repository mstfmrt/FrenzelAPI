using FrenzelAPI.Data;
using FrenzelAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrenzelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private DiagnosisDbContext _context;

        public DiagnosisController(DiagnosisDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Diagnosis>> GetDiagnoses() => await _context.Diagnoses.ToListAsync();




        [HttpGet("id")]
        [ProducesResponseType(typeof(Diagnosis), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            return diagnosis== null ? NotFound() : Ok(diagnosis);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Diagnosis diagnosis)
        {
            await _context.Diagnoses.AddAsync(diagnosis);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = diagnosis.Id }, diagnosis);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Diagnosis diagnosis)
        {
            if (id != diagnosis.Id) return BadRequest();

            _context.Entry(diagnosis).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) 
        {
            var diagnosisToDelete = await _context.Diagnoses.FindAsync(id);
            if (diagnosisToDelete == null) return NotFound();   

            _context.Diagnoses.Remove(diagnosisToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
