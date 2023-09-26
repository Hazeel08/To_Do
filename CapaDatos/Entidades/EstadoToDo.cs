using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Entidades
{
    public class ToDoEstado
    {
        public int Id { get; set; }
        [Display(Name = "Estado")]
        public string Descripcion { get; set; }
    }
}
