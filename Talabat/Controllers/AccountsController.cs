using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Exceptions;

namespace Talabat.Controllers
{

    public class AccountsController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
            ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        // Register 
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400,"Email is Used Already"));
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0],
            };
            var Reselt = await _userManager.CreateAsync(User,model.Password);
            if (!Reselt.Succeeded)
                return BadRequest(new ApiResponse(400));
            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if(User is null)
                return Unauthorized(new ApiResponse(401));
            var Reselt = await _signInManager.CheckPasswordSignInAsync(User,model.Password,false);
            if (!Reselt.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReternedUser = new UserDto()
            {
                Email = Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReternedUser);
        }

        [Authorize]
        [HttpGet("Address")]

        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(Email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var mappedUser = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            UpdatedAddress.Id = user.Address.Id;
            user.Address= mappedUser;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            //var user = await _userManager.FindByEmailAsync(Email);
            //if (user is null)
            //    return false;
            return await _userManager.FindByEmailAsync(Email) is not null;
        }
    }
}
