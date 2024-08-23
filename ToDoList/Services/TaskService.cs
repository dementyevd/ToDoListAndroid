using SQLite;
using ToDoList.Models;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using ToDoList.Views;

namespace ToDoList.Services
{
    public class TaskService
    {
        SQLiteAsyncConnection _connection;
        public TaskService()
        {
            if (_connection == null)
            {
                _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                _connection.CreateTableAsync<MyTask>();
            }
        }

        public async Task<List<MyTask>> GetTasksAsync()
        {
            return await _connection.Table<MyTask>().OrderBy(x => x.TaskDate).ToListAsync();
        }

        public async Task<MyTask> GetTaskByIdAsync(int id)
        {
            return await _connection.Table<MyTask>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(MyTask task)
        {
            await _connection.InsertAsync(task);
            await AddNotification(task);
        }

        public async Task Update(MyTask task)
        {
            LocalNotificationCenter.Current.Cancel(task.Id);
            await _connection.UpdateAsync(task);
            if (!task.IsCompleted)
            {
                await AddNotification(task);
            }
        }
        public async Task Delete(MyTask task)
        {
            LocalNotificationCenter.Current.Cancel(task.Id);
            await _connection.DeleteAsync(task);

        }
        public DateTime GetNotifyDate(MyTask task)
        {
            var result = DateTime.Now;
            switch (task.TaskReminderInterval)
            {
                case "Нет":
                    result = task.TaskDate;
                    break;
                case "За 5 минут":
                    result = task.TaskDate.AddMinutes(-5);
                    break;
                case "За 10 минут":
                    result = task.TaskDate.AddMinutes(-10);
                    break;
                case "За 15 минут":
                    result = task.TaskDate.AddMinutes(-15);
                    break;
                case "За 30 минут":
                    result = task.TaskDate.AddMinutes(-30);
                    break;
                case "За 1 час":
                    result = task.TaskDate.AddHours(-1);
                    break;
                case "За 2 часа":
                    result = task.TaskDate.AddHours(-2);
                    break;
                case "За 1 день":
                    result = task.TaskDate.AddDays(-1);
                    break;
            }
            return result;
        }
        public TimeSpan GetRepeatInterval(MyTask task)
        {
            var result = TimeSpan.Zero;
            switch (task.TaskRepeatInterval)
            {
                case "Никогда":
                    result = TimeSpan.Zero;
                    break;
                case "Каждый час":
                    result = TimeSpan.FromHours(1);
                    break;
                case "Каждый день":
                    result = TimeSpan.FromDays(1);
                    break;
                case "Каждую неделю":
                    result = TimeSpan.FromDays(7);
                    break;
                case "Каждый месяц":
                    result = (task.TaskDate.AddMonths(1) - task.TaskDate);
                    break;
                case "Каждый год":
                    result = (task.TaskDate.AddYears(1) - task.TaskDate);
                    break;
            }
            return result;
        }
        async Task AddNotification(MyTask task)
        {
            var request = new NotificationRequest
            {
                NotificationId = task.Id,
                Title = task.Description,
                Subtitle = "Напоминание",
                Description = $"{task.TaskDate:dd.MM.yyyy}, {task.TaskDate:HH:mm}",
                BadgeNumber = 42,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = GetNotifyDate(task),
                    RepeatType = NotificationRepeat.TimeInterval,
                    NotifyRepeatInterval = GetRepeatInterval(task),
                }
            };
            try
            {
                await LocalNotificationCenter.Current.Show(request);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Внимание!", ex.Message, "OK");
            }
        }
    }
}
