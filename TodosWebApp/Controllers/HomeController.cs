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
            IUserTodosOperation operation = new UserTodosOperation();
            List<t_todo> data = operation.QueryTodos(new t_user { user_identity = Session_UserId });
            return this.Json(data);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SaveTodosDtos todos)
        {
            if (ModelState.IsValid)
            {
                IUserTodosOperation operation = new UserTodosOperation();
                operation.UpdateTodos(new t_user { user_identity = Session_UserId }, todos.todos);
            }
            else
            {
                return Content("Data sended is not valid!!!");
            }
            return Content("Save successfully");
        }
    }
}
