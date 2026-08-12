using Microsoft.EntityFrameworkCore;
using RestAPIFurb.Data;
using RestAPIFurb.Models;

namespace RestAPIFurb.Dao
{
    public interface IUsuarioDao
    {
        Task<Usuario?> ObterPorLoginAsync(string login);
    }

    public class UsuarioDao : IUsuarioDao
    {
        private readonly AppDbContext _context;

        public UsuarioDao(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterPorLoginAsync(string login)
        {
            return await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
