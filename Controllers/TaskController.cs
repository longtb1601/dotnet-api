using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {  
        private SampleWebApiContext _context;

        public TaskController(SampleWebApiContext context)
        {
            _context = context;
        }

        //GET : api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllTasks()
        {
            return await _context.TaskItems.ToListAsync();
        }

        //GET: api/Tasks/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        //DELETE: api/Tasks/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/Tasks/1
        [HttpPut("{id}")]
        public async Task<ActionResult> EditTask(int id, TaskModel taskEdit)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = taskEdit.Title;
            task.IsCompleted = taskEdit.IsCompleted;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult> AddTask(TaskModel taskNew)
        {
            var checkExistId = await _context.TaskItems.FindAsync(taskNew.Id);

            if(checkExistId == null) {

                var task = new TaskModel
                {
                    Id = taskNew.Id,
                    Title = taskNew.Title,
                    IsCompleted = taskNew.IsCompleted
                };
                
                _context.TaskItems.Add(task);
                
                await _context.SaveChangesAsync();

            }

            return NoContent();
        }

        //POST: api/Tasks/add-tasks
        [Route("add-tasks")]
        [HttpPost]
        public async Task<ActionResult> AddTasks(List<TaskModel> tasksNew)
        {
            var temp = new TaskModel();
            var checkExistId = new TaskModel();

            foreach (var task in tasksNew) {

                checkExistId = await _context.TaskItems.FindAsync(task.Id);
                
                if(checkExistId == null) {

                    temp.Id = task.Id;
                    temp.Title = task.Title;
                    temp.IsCompleted = task.IsCompleted;
                    
                    _context.TaskItems.Add(temp);

                    await _context.SaveChangesAsync();
                }
            }

            return NoContent();
        }

        //POST: api/Tasks/add-tasks
        [Route("delete-tasks")]
        [HttpDelete]
        public async Task<ActionResult> DeleteTasks(List<int> listId)
        {
            var task = new TaskModel();

            foreach (var id in listId) {

                task = await _context.TaskItems.FindAsync(id);

                if (task != null)
                {
                    _context.TaskItems.Remove(task);

                    await _context.SaveChangesAsync();
                }
            }

            return NoContent();
        }
        
    }
}