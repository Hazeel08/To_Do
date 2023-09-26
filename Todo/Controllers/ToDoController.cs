using CapaDatos;
using CapaDatos.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace TestTodo.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDo _toDo;

        public ToDoController(IToDo toDo)
        {
            this._toDo = toDo;
        }

        public IActionResult NoEncontrado()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ToDo toDo)
        {

            if (!ModelState.IsValid)
            {
                return View(toDo);
            }

            DateTime fecha = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));

            toDo.FechaRegistro = fecha;
            toDo.EstadoId = 2;

            await _toDo.Crear(toDo);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("NoEncontrado", "ToDo");
            }

            await _toDo.ActualizarEstado(Convert.ToInt32(id));
            var model = await _toDo.ObtenerTodos();
            return PartialView("_TablaToDo", model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var ObjToDo = await _toDo.ObtenerPorId(id);

            if (ObjToDo is null)
            {
                return RedirectToAction("NoEncontrado");
            }

            return View(ObjToDo);
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var todo = await _toDo.ObtenerPorId(id);

            if(todo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarToDo(int id)
        {
            var todo = await _toDo.ObtenerPorId(id);

            if (todo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _toDo.Eliminar(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ToDo toDo)
        {
            var ObjTodo = await _toDo.ObtenerPorId(toDo.Id);

            if (ObjTodo is null)
            {
                return RedirectToAction("NoEncontrado");
            }

            await _toDo.Editar(toDo);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerToDo(int id)
        {
            var ObjToDo = await _toDo.ObtenerPorId(id);

            if (ObjToDo is null)
            {
                return RedirectToAction("NoEncontrado");
            }

            return View(ObjToDo);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodoPorIdEstado(int id)
        {
            var ObjToDo = await _toDo.ObtenerPorEstadoId(id);

            if (ObjToDo is null)
            {
                return RedirectToAction("NoEncontrado");
            }

            return PartialView("_TablaToDo", ObjToDo);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {

            List<ToDo> lista = await _toDo.ObtenerTodos();

            return PartialView("_TablaToDo", lista);
        }
    }
}
