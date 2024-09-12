using Book_Coding_Challenge.Models;

namespace Book_Coding_Challenge.Repository
{
    public interface IUser
    {
        Task<List<User>> GetAllUser();
        Task<User?> GetUserById(int id);
        Task AddUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(int id);
        Task Save();
        Task<User?> ValidateUser(string email, string password);
    }
}
