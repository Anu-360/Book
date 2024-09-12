using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Book_Coding_Challenge.Models;
using Book_Coding_Challenge.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Book_Coding_Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BookContext _context;
        private readonly IUser _user;

        public UsersController(BookContext context,IUser user)
        {
            _context = context;
            _user = user;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _user.GetAllUser();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _user.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (id != user.UserId)
            {
                return BadRequest();
            }
            if (userRole != "Admin")
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (id != userId)
                {
                    return Forbid();
                }
            }

            _user.UpdateUser(user);

            try
            {
                await _user.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
           _user.AddUser(user);
            await _user.Save();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _user.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                await _user.DeleteUser(id);
                await _user.Save();

              
            }
            catch (Exception)
            {
                return NotFound("Error deleting the user");
            }

            return NoContent();
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
