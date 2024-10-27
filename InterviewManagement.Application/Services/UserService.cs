using InterviewManagement.Application.DTOs;
using InterviewManagement.Application.Interfaces;
using InterviewManagement.Domain.Entities;
using InterviewManagement.Infrastructure.Repositories.Interfaces;
using InterviewManagement.Application.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InterviewManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Registers a new user if the credentials are valid.
        /// </summary>
        public async Task<string?> RegisterUserAsync(RegisterUserDto userDto)
        {
            // Check if passwords match
            if (userDto.Password != userDto.ConfirmPassword)
                return "Passwords do not match";

            // Check if the username or email already exists
            var isTaken = await _userRepository.IsUsernameOrEmailTakenAsync(userDto.Username, userDto.Email);
            if (isTaken)
                return "Username or Email already exists";

            // Create the user object with hashed password
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = HashPassword(userDto.Password)
            };

            // Save the user in the database
            await _userRepository.AddUserAsync(user);
            return null;
        }

        /// <summary>
        /// Logs in a user if the credentials are valid and returns a JWT token.
        /// </summary>
        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            // Find the user by username or email
            var user = await _userRepository.GetUserByCredentialsAsync(loginDto.Identifier);
            if (user == null)
                return "Invalid username or email";

            // Verify the password
            if (user.PasswordHash != HashPassword(loginDto.Password))
                return "Invalid password";

            // Generate and return a JWT token
            return _jwtTokenService.GenerateToken(user.Id, user.Username);
        }

        /// <summary>
        /// Hashes a password using SHA-256.
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
