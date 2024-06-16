using System.Linq;
using FinalToDoAPI.Models;
using FinalToDoAPI.Services.Database;

namespace FinalToDoAPI.Models.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public IQueryable<Task> Tasks => _context.Tasks;

        public Task GetTaskById(int taskId)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public void UpdateTask(Task task)
        {
            _context.Update(task);
            _context.SaveChanges();
        }

        public void CreateTask(Task task)
        {
            _context.Add(task);
            _context.SaveChanges();
        }

        public void DeleteTask(Task task)
        {
            _context.Remove(task);
            _context.SaveChanges();
        }

        public void FinishTask(Task task)
        {
            task.IsDone = true;
            _context.Update(task);
            _context.SaveChanges();
        }
    }
}
