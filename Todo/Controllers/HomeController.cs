using CapaDatos;
using CapaDatos.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestTodo.Models;

namespace TestTodo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IToDo _toDo;

        public HomeController(IToDo toDo)
        {
            this._toDo = toDo;
        }

        public async Task<IActionResult> Index()
        {

            List<ToDo> lista = await _toDo.ObtenerTodos();

            return View(lista);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}