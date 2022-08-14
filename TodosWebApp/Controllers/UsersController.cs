﻿using DBConnectorandMapulation;
using DBConnectorandMapulation.Models;
using DBConnectorandMapulation.Models.Dtos;
using Microsoft.AspNetCore.Http;
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
            HttpContext.Response.ContentType = "application/json; charset=UTF-8";
            if (!string.IsNullOrEmpty(t_users.users_username) && !string.IsNullOrEmpty(t_users.users_password))
            {
                byte[] username = System.Text.Encoding.UTF8.GetBytes(t_users.users_username);
                if (new DbContext().Authentication(t_users))
                {
                    HttpContext.Session.Set("UserName", username);
                    //HttpContext.Session.SetString("UserName", t_users.users_username);
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
