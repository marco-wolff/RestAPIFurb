namespace RestAPIFurb.Dao
{
    // Camada DAO: única responsável por conversar com o banco (via EF Core).
    // Os Services não acessam o DbContext diretamente, apenas o DAO.
    public interface IDao<T> where T : class
    {
        Task<List<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task<T> InserirAsync(T entidade);
        Task<bool> AtualizarAsync(T entidade);
        Task<bool> RemoverAsync(int id);
    }
}
