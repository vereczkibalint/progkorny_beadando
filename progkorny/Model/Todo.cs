using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace progkorny
{
    public class Todo
    {
        private string todo_id;

        public string Todo_ID
        {
            get { return todo_id; }
            set { todo_id = value; }
        }

        private string todo_title;

        public string Todo_Title
        {
            get { return todo_title; }
            set { todo_title = value; }
        }

        private string todo_body;

        public string Todo_Body
        {
            get { return todo_body; }
            set { todo_body = value; }
        }

        private string todo_author;

        public string Todo_Author
        {
            get { return todo_author; }
            set { todo_author = value; }
        }

        private string todo_deadline;

        public string Todo_Deadline
        {
            get { return todo_deadline; }
            set { todo_deadline = value; }
        }

        private Priority todo_priority;

        public Priority Todo_Priority
        {
            get { return todo_priority; }
            set { todo_priority = value; }
        }
    }
}
