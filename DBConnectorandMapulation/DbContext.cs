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


        public List<t_todos> selectAll()
        {
            var result = new List<t_todos>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM t_todos", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new t_todos()
                        {
                            todos_identity = reader.GetInt32("todos_identity"),
                            todos_textContent = reader.GetString("todos_textContent"),
                            todos_isCompleted = reader.GetUInt16("todos_isCompleted"),
                        });
                    }
                }
            }
            return result;
        }

        public void updateAll(List<t_todos> todos)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                // 删表所有数据
                MySqlCommand cmd = new MySqlCommand("DELETE FROM t_todos", connection);
                cmd.ExecuteNonQuery();
                // 插入
                foreach(var todo in todos)
                {
                   cmd.CommandText = $@"INSERT INTO t_todos
                                         (todos_identity, todos_textContent, todos_isCompleted)
                                        VALUES
                                        ({todo.todos_identity}, '{todo.todos_textContent}', {todo.todos_isCompleted})";
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public bool Authentication(t_users user)
        {
            bool result = false;
            if (doesUserExist(user) && doesUserPasswordRight(user))
                result = true;
            return result;
        }

        private bool doesUserExist(t_users user)
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM t_users WHERE users_username = '{user.users_username}'", connection);
                result = (Int64)cmd.ExecuteScalar() >= 1 ? true : false ;
            }
            return result;
        }
        private bool doesUserPasswordRight(t_users user)
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM t_users WHERE users_username = '{user.users_username}' AND users_password = '{user.users_password}'", connection);
                result = (Int64)cmd.ExecuteScalar() == 1 ? true : false;
            }
            return result;
        }
    }
}
