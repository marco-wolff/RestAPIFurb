using RestAPIFurb.Dao;
using RestAPIFurb.Dtos;
using RestAPIFurb.Models;

namespace RestAPIFurb.Services
{
    // Resultado genérico para o service comunicar ao controller o que aconteceu,
    // sem o service precisar conhecer HTTP (status codes ficam só no controller).
    public enum ResultadoTipo { Ok, NaoEncontrado, ReferenciaInvalida }

    public class ServiceResult<T>
    {
        public ResultadoTipo Tipo { get; set; }
        public T? Dado { get; set; }
        public string? Mensagem { get; set; }
    }

    public interface IEquipamentoService
    {
        Task<List<Equipamento>> ListarAsync();
        Task<ServiceResult<Equipamento>> ObterPorIdAsync(int id);
        Task<ServiceResult<Equipamento>> CriarAsync(Equipamento equipamento);
        Task<ServiceResult<Equipamento>> AtualizarParcialAsync(int id, EquipamentoPatchDto dto);
        Task<ServiceResult<bool>> RemoverAsync(int id);
    }

    public class EquipamentoService : IEquipamentoService
    {
        private readonly IEquipamentoDao _dao;

        public EquipamentoService(IEquipamentoDao dao)
        {
            _dao = dao;
        }

        public Task<List<Equipamento>> ListarAsync() => _dao.ObterTodosAsync();

        public async Task<ServiceResult<Equipamento>> ObterPorIdAsync(int id)
        {
            var equipamento = await _dao.ObterPorIdAsync(id);
            if (equipamento == null)
                return new ServiceResult<Equipamento> { Tipo = ResultadoTipo.NaoEncontrado };

            return new ServiceResult<Equipamento> { Tipo = ResultadoTipo.Ok, Dado = equipamento };
        }

        public async Task<ServiceResult<Equipamento>> CriarAsync(Equipamento equipamento)
        {
            if (!await _dao.TipoExisteAsync(equipamento.TipoId))
                return new ServiceResult<Equipamento>
                {
                    Tipo = ResultadoTipo.ReferenciaInvalida,
                    Mensagem = "O TipoId informado não existe"
                };

            var criado = await _dao.InserirAsync(equipamento);
            var completo = await _dao.ObterPorIdAsync(criado.Id);
            return new ServiceResult<Equipamento> { Tipo = ResultadoTipo.Ok, Dado = completo };
        }

        public async Task<ServiceResult<Equipamento>> AtualizarParcialAsync(int id, EquipamentoPatchDto dto)
        {
            var existente = await _dao.ObterPorIdAsync(id);
            if (existente == null)
                return new ServiceResult<Equipamento> { Tipo = ResultadoTipo.NaoEncontrado };

            // Só altera os campos que vieram preenchidos no PUT; os demais permanecem idênticos.
            if (!string.IsNullOrWhiteSpace(dto.Nome))
                existente.Nome = dto.Nome;

            if (dto.TipoId.HasValue)
            {
                if (!await _dao.TipoExisteAsync(dto.TipoId.Value))
                    return new ServiceResult<Equipamento>
                    {
                        Tipo = ResultadoTipo.ReferenciaInvalida,
                        Mensagem = "O TipoId informado não existe"
                    };

                existente.TipoId = dto.TipoId.Value;
            }

            await _dao.AtualizarAsync(existente);
            var atualizado = await _dao.ObterPorIdAsync(id);
            return new ServiceResult<Equipamento> { Tipo = ResultadoTipo.Ok, Dado = atualizado };
        }

        public async Task<ServiceResult<bool>> RemoverAsync(int id)
        {
            var removeu = await _dao.RemoverAsync(id);
            return new ServiceResult<bool>
            {
                Tipo = removeu ? ResultadoTipo.Ok : ResultadoTipo.NaoEncontrado,
                Dado = removeu
            };
        }
    }
}
