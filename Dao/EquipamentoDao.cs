using Microsoft.EntityFrameworkCore;
using RestAPIFurb.Data;
using RestAPIFurb.Models;

namespace RestAPIFurb.Dao
{
    public interface IEquipamentoDao : IDao<Equipamento>
    {
        Task<bool> TipoExisteAsync(int tipoId);
    }

    public class EquipamentoDao : IEquipamentoDao
    {
        private readonly AppDbContext _context;

        public EquipamentoDao(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Equipamento>> ObterTodosAsync()
        {
            return await _context.Equipamentos
                .Include(e => e.Tipo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Equipamento?> ObterPorIdAsync(int id)
        {
            return await _context.Equipamentos
                .Include(e => e.Tipo)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipamento> InserirAsync(Equipamento entidade)
        {
            _context.Equipamentos.Add(entidade);
            await _context.SaveChangesAsync();
            return entidade;
        }

        public async Task<bool> AtualizarAsync(Equipamento entidade)
        {
            _context.Equipamentos.Update(entidade);
            var linhas = await _context.SaveChangesAsync();
            return linhas > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento == null) return false;

            _context.Equipamentos.Remove(equipamento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TipoExisteAsync(int tipoId)
        {
            return await _context.Tipos.AnyAsync(t => t.Id == tipoId);
        }
    }
}
