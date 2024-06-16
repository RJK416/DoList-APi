using FinalToDoAPI.Models;

namespace FinalToDoAPI.Models.Repository;

public interface ITaskRepository
{
    IQueryable<Task> Tasks { get; }

    void UpdateTask(Task t);

    void CreateTask(Task t);

    void DeleteTask(Task t);

    void FinishTask(Task t);

    Task GetTaskById(int taskId);
}
