using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Mappers;
using HotPot.Models;
using HotPot.Models.DTO;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IRepository<int, string, Customer> _custRepo;
        private readonly IRepository<int, string, User> _userRepo;
        private readonly ILogger<CustomerServices> _logger;

        public CustomerServices(IRepository<int, string, Customer> custRepo,
                                IRepository<int, string, User> userRepo,
                                ILogger<CustomerServices> logger)
        {
            _custRepo = custRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<LoginUserDTO> LogIn(LoginUserDTO loginCustomer)
        {
            var user = await _userRepo.GetAsync(loginCustomer.UserName);
            if (user == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginCustomer.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginCustomer.Password = "";
                loginCustomer.Role = user.Role;
                return loginCustomer;
            }
            throw new InvalidUserException();
        }

        private bool passwordMatch(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        private byte[] getEncryptedPassword(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userPwd = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userPwd;
        }

        public async Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer)
        {
            registerCustomer.Role = "Admin";
            User myUser = new RegisterToUser(registerCustomer).getUser();
            myUser = await _userRepo.Add(myUser);
            Customer myCustomer = new RegisterToCustomer(registerCustomer).GetCustomer();
            myCustomer=await _custRepo.Add(myCustomer);
            LoginUserDTO result = new LoginUserDTO
            {
                UserName=myUser.UserName,
                Role=myUser.Role
            };
            return result;
        }
    }
}
