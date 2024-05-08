using FinalDoListAPI.Models;
using FinalDoListAPI.Services.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace DoListAPI.Controllers
{
    [Route("api/[controller]")]
    public class MyAccountController : ControllerBase
    {
        private readonly AccountDbContext _context;
        private readonly TaskDbContext _taskContext;

        public MyAccountController(AccountDbContext context, TaskDbContext taskContext)
        {
            _context = context;
            _taskContext = taskContext;
        }



        [HttpPost("GetActiveTasks")]
        public IActionResult GetActiveTasks(int id)
        {
            var ActiveTasks = this._taskContext.Tasks
                            .Where(t => t.AccID == id && !t.IsDone)
                            .ToList();
            return Ok(ActiveTasks);

        }

        [HttpPost("GetFinishTasks")]
        public IActionResult GetFinishTasks(int id)
        {
            var FinishTasks = this._taskContext.Tasks
                            .Where(t => t.AccID == id && t.IsDone)
                            .ToList();
            return Ok(FinishTasks);

        }




        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount(string accName, string password, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(accName) || string.IsNullOrEmpty(password))
            {
                return BadRequest("AccName and Password are required");
            }

            // Check if the username is already taken
            if (_context.Accounts.Any(a => a.AccName == accName))
            {
                return Conflict("The username is already taken");
            }

            // Generate a salt
            string salt = GenerateSalt();

            // Hash the password using the generated salt
            string hashedPassword = HashPassword(password, salt);

            // Create a new Account entity
            var newAccount = new AccountModel
            {
                AccName = accName,
                Name = firstName,
                LastName = lastName,
                // Add other properties as needed
                PasswordHash = hashedPassword,
                Salt = salt,
            };

            // Add the new Account to the context and save changes
            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            return Ok("Account created successfully");
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(string accName, string password)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(a => a.AccName == accName);

            if (user == null || !VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return Conflict("Incorrect username or password");
            }

            // Create claims for the user
            var claims = new List<Claim>
             {
        new Claim(ClaimTypes.Name, user.AccName), // Use AccName from the retrieved user
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Assuming user.Id is the unique identifier
        // Add other claims as needed
        // For example:
        // new Claim(ClaimTypes.Email, user.Email), // If you have user email
             };

            // Create identity
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create authentication properties
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Persist the cookie
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1), // Set the expiration time for the cookie
                AllowRefresh = true, // Allow refreshing the authentication session
            };

            // Create the principal
            var principal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            return Ok(new { success = true, message = "Login successful", userId = user.Id.ToString(), accName = user.AccName });
        }

        [HttpPost("Bober")]
        public IActionResult Bober()
        {
            // Access userIdClaim here
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            return Ok("bober");

        }

        // Function to verify the password
        private bool VerifyPassword(string enteredPassword, string storedHash, string salt)
        {
            string hashedEnteredPassword = HashPassword(enteredPassword, salt);
            return hashedEnteredPassword == storedHash;
        }

        // Function to hash the password
        private string HashPassword(string password, string salt)
        {
            // Combine the password and salt
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Array.Copy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Array.Copy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            // Hash the combined bytes
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hashedBytes);
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[32]; // You can adjust the size of the salt as needed
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}
