using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TodosWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsLogin
        {
            get
            {
                string username = null;
                //从session中获取用户信息来判断用户是否登陆
                if(HttpContext.Session.TryGetValue("UserName", out byte[] bytes))
                {
                    username = Encoding.UTF8.GetString(bytes);
                }
                return !string.IsNullOrWhiteSpace(username);
            }
        }
    }
}
