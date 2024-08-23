using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;

namespace ToDoList.Models
{
    public class MyTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime TaskDate { get; set; }
        public bool IsCompleted { get; set; }
        public string TaskReminderInterval { get; set; }
        public string TaskRepeatInterval { get; set; }
    }
}
