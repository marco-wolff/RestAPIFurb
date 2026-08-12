using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestAPIFurb.Models
{
    // EF Core vai gerar a tabela "Usuarios" (plural) a partir da classe "Usuario" (singular)
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O login é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O login deve ter entre 3 e 50 caracteres")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(200, MinimumLength = 4)]
        [JsonIgnore] // nunca retorna a senha/hash no JSON de resposta
        public string SenhaHash { get; set; } = string.Empty;
    }
}
