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
using Microsoft.AspNetCore.Http;

namespace TodosWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (IsLogin)
                return View();
            else
                return Redirect("/Users/Index");//返回登录页面
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
