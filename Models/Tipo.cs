using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestAPIFurb.Models
{
    // EF Core vai gerar a tabela "Tipos" (plural) a partir da classe "Tipo" (singular)
    public class Tipo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do tipo é obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 60 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Equipamento>? Equipamentos { get; set; }
    }
}
