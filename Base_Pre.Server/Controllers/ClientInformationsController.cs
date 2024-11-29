using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Base_Pre.Server.Models;

namespace Base_Pre.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientInformationsController : ControllerBase
    {
        private readonly PrimaryDbContext _context;

        public ClientInformationsController(PrimaryDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientInformation>>> GetClientInformations()
        {
            return await _context.ClientInformations.ToListAsync();
        }

        // GET: api/ClientInformations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientInformation>> GetClientInformation(int id)
        {
            var clientInformation = await _context.ClientInformations.FindAsync(id);

            if (clientInformation == null)
            {
                return NotFound();
            }

            return clientInformation;
        }

        // PUT: api/ClientInformations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientInformation(int id, ClientInformation clientInformation)
        {
            if (id != clientInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientInformationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClientInformations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClientInformation>> PostClientInformation(ClientInformation clientInformation)
        {
            _context.ClientInformations.Add(clientInformation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientInformationExists(clientInformation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClientInformation", new { id = clientInformation.Id }, clientInformation);
        }

        // DELETE: api/ClientInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientInformation(int id)
        {
            var clientInformation = await _context.ClientInformations.FindAsync(id);
            if (clientInformation == null)
            {
                return NotFound();
            }

            _context.ClientInformations.Remove(clientInformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientInformationExists(int id)
        {
            return _context.ClientInformations.Any(e => e.Id == id);
        }
    }
}
