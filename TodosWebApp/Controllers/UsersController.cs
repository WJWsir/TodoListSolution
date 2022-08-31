using DBConnectorandMapulation;
using DBConnectorandMapulation.Models;
using DBConnectorandMapulation.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodosWebApp.Utilities;

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
        public IActionResult Login([FromBody] t_user t_users)
        {
            HttpContext.Response.ContentType = "application/json; charset=UTF-8";
            if (!string.IsNullOrEmpty(t_users.user_username) && !string.IsNullOrEmpty(t_users.user_password))
            {
                byte[] username = System.Text.Encoding.UTF8.GetBytes(t_users.user_username);
                if (new DbContext().Authentication(t_users))
                {
                    HttpContext.Session.Set("UserName", username);
                    //HttpContext.Session.SetString("UserName", t_users.users_username);
                    HttpContext.Session.SetInt32("UserIdentity", new DbContext().GetUserId(t_users));

                    Log.Logger.Information($"用户登录: 用户{t_users.user_username} 使用IP{HttpContext.Connection.RemoteIpAddress} 于日期时间{DateTime.Now} 登录系统");
                    return Json(new { err = "0", errMsg = "登录验证成功" });
                }
                else
                {
                    HttpContext.Session.Clear();
                    return Json(new { err = "1", errMsg = "用户或密码错误" });
                }
            }
            else
                return Json(new { err = "2", errMsg = "用户名或密码不可为空" });
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
