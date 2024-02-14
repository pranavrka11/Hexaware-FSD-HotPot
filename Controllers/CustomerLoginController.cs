using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotPot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerLoginController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerLoginController> _logger;

        public CustomerLoginController(ICustomerService customerService , ILogger<CustomerLoginController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [Route("LogIn")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> CustomerLogin(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _customerService.LogIn(loginUser);
                return loginUser;
            }
            catch (InvalidUserException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized("Invalid username or password");
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<LoginUserDTO> CustomerRegistration(RegisterCustomerDTO registerCustomer)
        {
            var result = await _customerService.RegisterCustomer(registerCustomer);
            return result;
        }
    }
}
