using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalDoListAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = FinalDoListAPI.Models.Task;
using FinalDoListAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FinalDoListAPI.Services.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace DoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ITaskRepository _taskRepository;


        public TaskController(ITaskRepository taskRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] Task model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Access claims to retrieve user metadata
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    return Unauthorized(); // If user claim is not found, return unauthorized
            //}

            // Create the task with the provided model
            var task = new Task()
            {
                Name = model.Name,
                Description = model.Description,
                DeadlineDate = model.DeadlineDate, // Assuming DeadlineDate is provided in the model
                IsDone = false,
                AccID = model.AccID // Associate the task with the logged-in user using their ID
            };

            // Save the task using your TaskRepository or DbContext
            _taskRepository.CreateTask(task);

            return Ok("Task created successfully");
        }

        [HttpDelete("DeleteTask/{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            _taskRepository.DeleteTask(task);

            return Ok("Task deleted successfully");
        }

        [HttpPost("FinishTask/{taskId}")]
        public IActionResult FinishTask(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            task.IsDone = true;
            _taskRepository.UpdateTask(task);

            return Ok("Task finished successfully");
        }

        [HttpPut("EditTask/{taskId}")]
        public IActionResult EditTask(int taskId, [FromBody] Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTask = _taskRepository.GetTaskById(taskId);

            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.DeadlineDate = task.DeadlineDate;

            _taskRepository.UpdateTask(existingTask);

            return Ok("Task updated successfully");
        }
    }
}
