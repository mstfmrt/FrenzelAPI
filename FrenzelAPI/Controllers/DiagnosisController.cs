using FrenzelAPI.Data;
using FrenzelAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FrenzelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private DiagnosisDbContext _context;
        private PatientDbContext _patientContext;

        public DiagnosisController(DiagnosisDbContext context, PatientDbContext patientContext)
        {
            _context = context;
            _patientContext = patientContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Diagnosis>> GetDiagnoses() => await _context.Diagnosis.ToListAsync();




        [HttpGet("id")]
        [ProducesResponseType(typeof(Diagnosis), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var diagnosis = await _context.Diagnosis.FindAsync(id);
            return diagnosis== null ? NotFound() : Ok(diagnosis);
        }

        // In DiagnosisController
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(typeof(IEnumerable<Diagnosis>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDiagnosesByPatientId(int patientId)
        {
            var patient = await _patientContext.Patients.Include(p => p.Diagnoses).FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var diagnoses = patient.Diagnoses;

            return Ok(diagnoses);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(Diagnosis diagnosis)
        {
            try
            {
                var patient = await _patientContext.Patients.Include(p => p.Diagnoses).FirstOrDefaultAsync(p => p.Id == diagnosis.PatientId);
                if (patient == null)
                {
                    return NotFound("Patient not found.");
                }

                await _context.Diagnosis.AddAsync(diagnosis);
                await _context.SaveChangesAsync();

                patient.Diagnoses.Add(diagnosis);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = diagnosis.Id }, diagnosis);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return BadRequest(ex.Message);
            }
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
            var diagnosisToDelete = await _context.Diagnosis.FindAsync(id);
            if (diagnosisToDelete == null) return NotFound();   

            _context.Diagnosis.Remove(diagnosisToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
