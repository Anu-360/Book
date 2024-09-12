using Book_Coding_Challenge.Models;
using Book_Coding_Challenge.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Book_Coding_Challenge.Repository
{
    public class UserService:IUser
    {
        private readonly BookContext _context;
        public UserService(BookContext context)
        {
            _context = context;
        }
        public async Task AddUser(User user)
        {
            _context.Users.AddAsync(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new UserNotFoundException($"User with ID {id} Not Found");
            _context.Users.Remove(user);
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Task.FromResult(user);
        }

        public async Task<User?> ValidateUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(vu => vu.UserMail == email && vu.Password == password);
        }
    }
}
