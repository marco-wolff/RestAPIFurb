using System.ComponentModel.DataAnnotations;

namespace RestAPIFurb.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }

    public class EquipamentoPatchDto
    {
        // Todos opcionais: PUT deve aceitar somente os campos alterados
        [StringLength(100, MinimumLength = 2)]
        public string? Nome { get; set; }

        public int? TipoId { get; set; }
    }
}
