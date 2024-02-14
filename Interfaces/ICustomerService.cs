using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface ICustomerService
    {
        public Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer);
        public Task<LoginUserDTO> LogIn(LoginUserDTO loginCustomer);
    }
}
