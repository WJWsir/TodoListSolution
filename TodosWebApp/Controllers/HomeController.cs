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
using TodosWebApp.Utilities;
using Serilog;

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
                if (!IsLogin)// Session is expired
                    return Content("请重新登录!!!");
                IUserTodosOperation operation = new UserTodosOperation();
                Log.Logger.Information($"用户操作: 用户 {Session_UserName} 使用IP {HttpContext.Connection.RemoteIpAddress} 于日期时间 {DateTime.Now} 变更代办事项");
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
