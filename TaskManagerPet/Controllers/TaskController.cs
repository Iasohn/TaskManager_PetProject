using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerPet.Data;
using TaskManagerPet.Helpers;
using TaskManagerPet.Interface;
using TaskManagerPet.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/Task")]
    public class TaskController : Controller
    {
        ITaskInterface _task;
        ProjectContext _context;
        public TaskController(ITaskInterface taskInterface, ProjectContext project)
        {
            _context = project;
            _task = taskInterface;

        }

        [Authorize]
        
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var Task = _context.Task.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.TaskName))
            {
                Task = Task.Where(s => s.TaskName.Contains(query.TaskName));
            }
            var skipnumber = (query.PageNumber - 1) * query.PageSize;

            if (!string.IsNullOrEmpty(query.TaskDescription))
            {
                Task = Task.Where(s => s.TaskDescription.Contains(query.TaskDescription));
            }

            if (!string.IsNullOrEmpty(query.TaskStatus))
            {
                Task = Task.Where(s => s.TaskStatus.Contains(query.TaskStatus));
            }
            var paginatedTasks = await Task
                            .Skip(skipnumber)
                            .Take(query.PageSize)
                            .ToListAsync();
            return  Ok(paginatedTasks);
        }

        [Authorize]
        [HttpGet("Getbyid")]
        public async Task<IActionResult> Get(int id)
        {
            var Get = await _task.GetById(id);
            return Ok(Get);
        }

        [Authorize]
        [HttpPost("PostTask")]
        public async Task<IActionResult> Post(Tasks model)
        {
           if(model == null)
                return BadRequest();

            try
            {
               await _task.Post(model);
               Console.WriteLine("!!!!!!");
                
            }
            catch (Exception ex)
            {
                return BadRequest($"SERVER DOESNT WORK  {ex}" );
            }

            return Ok("Succesfully");
        }
        [HttpPut("ChangeTask")]
        [Authorize]
        public async Task<IActionResult> put(int id, Tasks model)
        {
            try
            {
                if (ModelState.IsValid) { return BadRequest(ModelState); };
                var PutItem = await _task.Put(id, model);
                return Ok("Succesfully");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return BadRequest();
            

        }
        [HttpDelete("DeleteTask")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _task.Delete(id);
            return Ok("Succesfully");
        }

    }
}
