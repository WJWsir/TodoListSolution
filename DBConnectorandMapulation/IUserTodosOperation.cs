using DBConnectorandMapulation.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnectorandMapulation
{
    public interface IUserTodosOperation
    {
        /// <summary>
        /// 业务: 查询指定用户的所有Todos
        /// </summary>
        /// <param name="t_User"></param>
        List<t_todo> QueryTodos(t_user t_User);

        /// <summary>
        /// 业务: 删除用户的所有Todos, 再添加接收的所有Todos
        /// </summary>
        /// <param name="t_User"></param>
        /// <param name="t_Todos"></param>
        void UpdateTodos(t_user t_User, List<t_todo> t_Todos);
    }
}
