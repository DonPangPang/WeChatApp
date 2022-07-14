using System.Collections.Concurrent;

namespace WeChatApp.Client.Services
{
    public interface IMemoryDataService
    {
        List<TaskItem> GetTaskItemsAsync();

        void Set(TaskItem taskItem);
    }

    public class MemoryDataService : IMemoryDataService
    {
        private ConcurrentBag<TaskItem> _taskItems = new ConcurrentBag<TaskItem>();
        public MemoryDataService()
        {

        }
        public List<TaskItem> GetTaskItemsAsync()
        {
            return _taskItems.ToList();
        }

        public void Set(TaskItem taskItem)
        {
            _taskItems.Add(taskItem);
        }
    }

    public class TaskItem
    {
        public string? Name { get; set; }
        public string? Descrption { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
