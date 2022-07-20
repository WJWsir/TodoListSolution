using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBConnectorandMapulation;
using DBConnectorandMapulation.Models;
using Newtonsoft.Json;
using YouZack.FromJsonBody;
using DBConnectorandMapulation.Models.Dtos;

namespace mysqlConnectionDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<t_todos> data = new DbContext().selectAll();
            return this.Json(data);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SaveTodosDtos todos)
        {
            if (ModelState.IsValid) { 
                new DbContext().updateAll(todos.todos);
            }
            else
            {
                return Content("Data sended is not valid!!!");
            }
            return Content("Save successfully");
        }
    }
}
