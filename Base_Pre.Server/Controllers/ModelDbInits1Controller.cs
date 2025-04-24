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
    public class ModelDbInits1Controller : ControllerBase
    {
        private readonly PrimaryDbContext _context;

        public ModelDbInits1Controller(PrimaryDbContext context)
        {
            _context = context;
        }

        // GET: api/ModelDbInits1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelDbInit>>> GetModelDbInits()
        {
            return await _context.ModelDbInits.ToListAsync();
        }

        // GET: api/ModelDbInits1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelDbInit>> GetModelDbInit(int id)
        {
            var modelDbInit = await _context.ModelDbInits.FindAsync(id);

            if (modelDbInit == null)
            {
                return NotFound();
            }

            return modelDbInit;
        }

    }
}
