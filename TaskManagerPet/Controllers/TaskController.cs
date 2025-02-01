using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerPet.Data;
using TaskManagerPet.Interface;
using TaskManagerPet.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class TaskController : Controller
    {
        ITaskInterface _task;
        ProjectContext _context;
        public TaskController(ITaskInterface taskInterface, ProjectContext project)
        {
            _context = project;
            _task = taskInterface;

        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var People = _context.Task.ToList();
            return Ok(People);
        }

        
        [HttpGet("Getbyid")]
        public async Task<IActionResult> Get(int id)
        {
            var Get = await _task.GetById(id);
            return Ok(Get);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Tasks model)
        {
            if (ModelState.IsValid) { return BadRequest(ModelState); };

            try
            {
                await _task.Post(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok("Succesfully");
        }
        [HttpPut]
        public async Task<IActionResult> put(int id, Tasks model)
        {
            if (ModelState.IsValid) { return BadRequest(ModelState); };
            var PutItem = await _task.Put(id, model);
            return Ok("Succesfully");

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _task.Delete(id);
            return Ok("Succesfully");
        }

    }
}
