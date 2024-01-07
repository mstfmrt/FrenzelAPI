using FrenzelAPI.Data;
using FrenzelAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrenzelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private PatientDbContext _context;

        public PatientController(PatientDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Patient>> GetPatients() => await _context.Patients.ToListAsync();


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (await _context.Patients.AnyAsync(p => p.TCKN == patient.TCKN))
            {
                ModelState.AddModelError("TCKN", "TCKN must be unique.");
                return BadRequest(ModelState);
            }

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }

        [HttpGet("tckn/{tckn}")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByTCKN(long tckn)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.TCKN == tckn);
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpPut("tckn/{tckn}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateByTCKN(long tckn, Patient patient)
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.TCKN == tckn);

            if (existingPatient == null)
            {
                return NotFound();
            }

            existingPatient.Name = patient.Name;
            existingPatient.Surname = patient.Surname;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("id")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            return patient == null ? NotFound() : Ok(patient);
        }





        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Patient patient)
        {
            if (id != patient.Id) return BadRequest();

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var patientToDelete = await _context.Patients.FindAsync(id);
            if (patientToDelete == null) return NotFound();

            _context.Patients.Remove(patientToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
