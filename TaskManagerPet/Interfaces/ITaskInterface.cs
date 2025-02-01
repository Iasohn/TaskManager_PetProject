using Microsoft.AspNetCore.Mvc;
using TaskManagerPet.Models;
 

namespace TaskManagerPet.Interface
{
    public interface ITaskInterface
    {
        public Task<Tasks> GetById(int id);
        public Task<Tasks> Post(Tasks model);
        public Task<Tasks> Put(int id, Tasks model);
        public Task<Tasks> Delete(int id);
    }
}
