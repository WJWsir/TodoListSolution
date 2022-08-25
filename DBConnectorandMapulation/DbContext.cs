using System;
using System.IO;
using MySql.Data.MySqlClient;
using DBConnectorandMapulation.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DBConnectorandMapulation
{
    public class DbContext
    {
        public static string ConnectionString = GetConnectionString();
        private static string GetConnectionString()
        {
            string result;
            string configfilePath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");//appsettings.json文件路径
            using (StreamReader file = File.OpenText(configfilePath))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    result = o["ConnectionStrings"]["TodoListDatabase"].ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Authentication(t_user user)
        {
            bool result = false;
            if (doesUserExist(user) && doesUserPasswordRight(user))
                result = true;
            return result;
        }

        /// <summary>
        /// 获取UserId
        /// </summary>
        /// <param name="user">用户名</param>
        /// <returns>Get users_identity</returns>
        public int GetUserId(t_user user)
        {
            int result;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT t_users.users_identity FROM t_users WHERE users_username = '{user.user_username}'", connection);
                result = (Int32)cmd.ExecuteScalar();
            }
            return result;
        }

        private bool doesUserExist(t_user user)
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM t_users WHERE users_username = '{user.user_username}'", connection);
                result = (Int64)cmd.ExecuteScalar() >= 1 ? true : false ;
            }
            return result;
        }
        private bool doesUserPasswordRight(t_user user)
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM t_users WHERE users_username = '{user.user_username}' AND users_password = '{user.user_password}'", connection);
                result = (Int64)cmd.ExecuteScalar() == 1 ? true : false;
            }
            return result;
        }
    }
}
