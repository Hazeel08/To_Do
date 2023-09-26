using CapaDatos.Entidades;
using CapaDatos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Interfaces
{
    public interface IToDoEstado
    {
        Task<List<ToDoEstado>> ObtenerTodos();
        Task<ToDoEstado> ObtenerPorId(int codigo);
    }
}
