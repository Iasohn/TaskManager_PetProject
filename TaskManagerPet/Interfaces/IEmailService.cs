
namespace TaskManagerPet.Interfaces
{
    public interface IEmailService
    {
        Task SendCode(string email, string code);
    }
}
