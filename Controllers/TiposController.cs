using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIFurb.Data;

namespace RestAPIFurb.Controllers
{
    [ApiController]
    [Route("api/tipos")]
    public class TiposController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/tipos -> 200
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var tipos = await _context.Tipos.AsNoTracking().ToListAsync();
            return Ok(tipos);
        }
    }
}
