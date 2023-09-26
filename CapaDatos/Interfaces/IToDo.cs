using CapaDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public interface IToDo
    {
        Task Crear(ToDo toDo);
        Task<ToDo> Editar(ToDo toDo);
        Task Eliminar(int id);
        Task ActualizarEstado(int id);
        Task<ToDo> ObtenerPorId(int id);
        Task<List<ToDo>> ObtenerPorEstadoId(int id);
        Task<List<ToDo>> ObtenerTodos();
    }
}
