using HotPot.Models;
using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface ICustomerServices
    {
        public Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer);
        public Task<LoginUserDTO> LogIn(LoginUserDTO loginCustomer);
    }
}
