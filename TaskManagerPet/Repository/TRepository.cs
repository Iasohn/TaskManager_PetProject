using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerPet.Data;
using TaskManagerPet.Models;
using TaskManagerPet.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskManagerPet.Repository
{
    public class TaskRepository : ITaskInterface
    {
        ProjectContext _context;
        public TaskRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<Tasks> Delete(int id)
        {
            var Task = _context.Task.FirstOrDefault(x => x.Id == id);
            if (Task == null)
                return null;
            _context.Task.Remove(Task);
            return Task;
        }

        public async Task<Tasks> GetById(int id)
        {
            var get = await _context.Task.FirstOrDefaultAsync(x => x.Id == id);

            if (get == null)
                return null;

            return get;
        }

        public async Task<Tasks> Post(Tasks model)
        {
            await _context.Task.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Tasks> Put(int id, Tasks model)
        {
            var task = await _context.Task.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                return null;

            task.TaskName = model.TaskName;
            task.TaskStatus = model.TaskStatus;
            task.TaskDescription = model.TaskDescription;
            task.TaskTime = model.TaskTime;

            _context.SaveChangesAsync();

            return model;

        }

    }
}
