using DBConnectorandMapulation;
using DBConnectorandMapulation.Models;
using DBConnectorandMapulation.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodosWebApp.Controllers
{
    public class UsersController : Controller
    {

        /// <summary>
        /// 登录主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login([FromBody] t_users t_users)
        {
            if (!string.IsNullOrEmpty(t_users.users_username) && !string.IsNullOrEmpty(t_users.users_password))
            {
                byte[] username = System.Text.Encoding.UTF8.GetBytes(t_users.users_username);
                byte[] password = System.Text.Encoding.UTF8.GetBytes(t_users.users_password);
                if (new DbContext().Authentication(t_users))
                {
                    HttpContext.Session.Set("UserName", username);
                    HttpContext.Session.Set("Password", password);
                }
            }
            //HttpContext.Response.ContentType = "";
            return Content("OK");
        }

        /// <summary>
        /// 注册操作
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }
    }
}
