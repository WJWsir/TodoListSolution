using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnectorandMapulation.Models
{
    public class t_todo
    {
        public int todo_identity { set; get; }
        public string todo_textContent { get; set; }
        public uint todo_isCompleted { get; set; }
    }
}
