using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPIFurb.Models
{
    // EF Core vai gerar a tabela "Equipamentos" (plural) a partir da classe "Equipamento" (singular)
    public class Equipamento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do equipamento é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo é obrigatório")]
        public int TipoId { get; set; }

        [ForeignKey(nameof(TipoId))]
        public Tipo? Tipo { get; set; }
    }
}
