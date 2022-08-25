using DBConnectorandMapulation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace DBConnectorandMapulation
{
    public class UserTodosOperation : IUserTodosOperation
    {
        public UserTodosOperation() { }
        private DbContext dbContext = new DbContext();

        List<t_todo> IUserTodosOperation.QueryTodos(t_user t_User)
        {
            var result = new List<t_todo>();
            using (MySqlConnection connection = new MySqlConnection(DbContext.ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($@"CALL sp_queryOwnerAllTodos({t_User.user_identity})", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new t_todo()
                        {
                            todo_identity = reader.GetInt32("todos_identity"),
                            todo_textContent = reader.GetString("todos_textContent"),
                            todo_isCompleted = reader.GetUInt16("todos_isCompleted"),
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 业务: 新增指定用户的Todos
        /// </summary>
        /// <param name="t_User"></param>
        /// <param name="t_Todos"></param>
        /// <param name="transaction"></param>
        void AddTodos(t_user t_User, List<t_todo> t_Todos, MySqlTransaction transaction)
        {
            using (MySqlConnection connection = new MySqlConnection(DbContext.ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction();
                foreach (var t_Todo in t_Todos)
                {
                    MySqlCommand sqlCommand = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"CALL sp_AddOwnerTodo({t_User.user_identity}, '{t_Todo.todo_textContent}', {t_Todo.todo_isCompleted});"
                    };
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                }
                if (transaction == null) transaction.Commit();
                connection.Close();
            }
        }

        /// <summary>
        /// 业务: 删除指定用户的所有Todos
        /// </summary>
        /// <param name="t_User"></param>
        /// <param name="transaction"></param>
        void DeleteTodos(t_user t_User, MySqlTransaction transaction)
        {
            using (MySqlConnection connection = new MySqlConnection(DbContext.ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction();


                MySqlCommand sqlCommand = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = $@"CALL sp_DeleteOwnerAllTodos({t_User.user_identity});"
                };
                int rowsAffected = sqlCommand.ExecuteNonQuery();

                if (transaction == null) transaction.Commit();
                connection.Close();
            }
        }

        void IUserTodosOperation.UpdateTodos(t_user t_User, List<t_todo> t_Todos)
        {
            using (MySqlConnection connection = new MySqlConnection(DbContext.ConnectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();
                DeleteTodos(t_User, transaction);
                AddTodos(t_User, t_Todos, transaction);
                connection.Close();
            }
        }

        /// <summary>
        /// 业务: 新增指定用户的Todo
        /// </summary>
        /// <param name=""></param>
        void AddTodo(t_user t_User, t_todo t_Todo)
        {
            using (MySqlConnection connection = new MySqlConnection(DbContext.ConnectionString))
            {
                connection.Open();
                MySqlCommand sqlCommand = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = $@"CALL sp_AddOwnerTodo({t_User.user_identity}, '{t_Todo.todo_textContent}', {t_Todo.todo_isCompleted});"
                };
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        /// <summary>
        /// 业务: 修改指定用户的指定Todo
        /// </summary>
        /// <param name="t_User"></param>
        /// <param name="t_Todo"></param>
        void ModifyTodo(t_user t_User, t_todo t_Todo)
        {

        }
    }
}
