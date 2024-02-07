using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Mappers;
using HotPot.Models;
using HotPot.Models.DTO;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Services
{
    public class RestaurantOwnerService : IRestaurantService
    {
        private readonly IRepository<int, RestaurantOwner> _restOwnerRepo;
        private readonly IRepository<string, User> _userRepo;
        private readonly ILogger<RestaurantOwnerService> _logger;

        public RestaurantOwnerService(IRepository<int ,RestaurantOwner> restOwnerRepo,
                                IRepository<string, User> userRepo,
                                ILogger<RestaurantOwnerService> logger)
        {
            _restOwnerRepo = restOwnerRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<LoginUserDTO> LogInRestaurant(LoginUserDTO loginRestaurant)
        {
            var user = await _userRepo.GetAsync(loginRestaurant.UserName);
            if (user == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginRestaurant.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginRestaurant.Password = "";
                loginRestaurant.Role = user.Role;
                return loginRestaurant;
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


        public async Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            registerRestaurant.Role = "RestaurantOwner";
            User myUser = new RegisterToUser(registerRestaurant).getUser();
            myUser = await _userRepo.Add(myUser);
            RestaurantOwner myRestaurantOwner = new RegisterToRestaurant(registerRestaurant).GetRestaurantOwner();
            myRestaurantOwner = await _restOwnerRepo.Add(myRestaurantOwner);
            LoginUserDTO result = new LoginUserDTO
            {
                UserName = myUser.UserName,
                Role = myUser.Role
            };
            return result;
            
        }
    }
}
