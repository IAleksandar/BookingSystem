namespace BookingSystem.Services.Implementations
{
    using BookingSystem.DataAccess.Interfaces;
    using BookingSystem.Dtos;
    using BookingSystem.Services.Interfaces;
    using System.Text.RegularExpressions;
    using System.Security.Cryptography;
    using System.Text;
    using BookingSystem.Domain.Models;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;

    public class UsersService : IUsersService
    {
        private IUserRepository _userRepository;

        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Register(RegisterUserDto registerUserDto)
        {
            await ValidateUser(registerUserDto);

            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            byte[] passwordBytes = Encoding.ASCII.GetBytes(registerUserDto.Password);

            byte[] passwordHash = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            string hashedPassword = Encoding.ASCII.GetString(passwordHash);

            User newUser = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Username = registerUserDto.Username,
                Role = registerUserDto.Role,
                Password = hashedPassword
            };

            await _userRepository.Insert(newUser);
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] hashedBytes = mD5CryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(loginDto.Password));
            string hashedPassword = Encoding.ASCII.GetString(hashedBytes);

            User userDb = await _userRepository.LoginUser(loginDto.Username, hashedPassword);
            if (userDb == null)
            {
                throw new Exception($"Could not login user {loginDto.Username}");
            }

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our secret key");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature),
                
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, userDb.Username),
                        new Claim(ClaimTypes.NameIdentifier, userDb.Id.ToString()),
                        new Claim("userFullName", $"{userDb.FirstName} {userDb.LastName}"),
                        new Claim(ClaimTypes.Role, userDb.Role)
                    }
                )

            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);

        }

        private async Task ValidateUser(RegisterUserDto registerUserDto)
        {
            if (string.IsNullOrEmpty(registerUserDto.Username) || string.IsNullOrEmpty(registerUserDto.Password))
            {
                throw new Exception("Username and Password are required fields!");
            }

            if(string.IsNullOrEmpty(registerUserDto.Role))
            {
                throw new Exception("Role is required field!");
            }

            if (registerUserDto.Username.Length > 30)
            {
                throw new Exception("Username can contain maximum 50 characters!");
            }

            if (registerUserDto.FirstName.Length > 50 || registerUserDto.LastName.Length > 50)
            {
                throw new Exception("Firstname and Lastname can contain maximum 50 characters!");
            }

            bool isUserNameUnique = await IsUserNameUnique(registerUserDto.Username);

            if (!isUserNameUnique)
            {
                throw new Exception("A user with this username already exists!");
            }

            if (registerUserDto.Password != registerUserDto.ConfirmedPassword)
            {
                throw new Exception("The passwords do not match");
            }

            if (!IsPasswordValid(registerUserDto.Password))
            {
                throw new Exception("The password is not complex enough!");
            }
        }

        private async Task<bool> IsUserNameUnique(string username)
        {
            return await _userRepository.GetUserByUsername(username) == null;
        }

        private bool IsPasswordValid(string password)
        {
            Regex passwordRegex = new Regex("^(?=.*[0-9])(?=.*[a-z]).{6,20}$");
            return passwordRegex.Match(password).Success;
        }
    }
}
