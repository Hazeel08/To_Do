using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Entidades
{
    public class ToDo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "La longitud del campo {0} debe estar entre {2} y {1}")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 250, MinimumLength = 5, ErrorMessage = "La longitud del campo {0} debe estar entre {2} y {1}")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
        [Display(Name = "Fecha de Finalización")]
        public DateTime? FechaFinalizada { get; set; }
        public int EstadoId { get; set; }
        public ToDoEstado ToDoEstado { get; set; }
        [Display(Name = "Completar")]
        public bool ChkEstado { get; set; }
    }
}
