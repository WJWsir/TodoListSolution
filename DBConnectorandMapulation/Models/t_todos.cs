using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnectorandMapulation.Models
{
    public class t_todos
    {
        public int todos_identity { set; get; }
        public string todos_textContent { get; set; }
        public uint todos_isCompleted { get; set; }
    }
}
