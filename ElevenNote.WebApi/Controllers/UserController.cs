using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Services.User;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElevenNote.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using ElevenNote.Services.Token;
using ElevenNote.Models.Token;

namespace ElevenNote.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
            if (registerResult)
            {
                TextResponse response = new("User was registered");
                return Ok(response);
            }

            return BadRequest(new TextResponse("User could not be registered"));
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int userId)
        {
            UserDetail? detail = await _userService.GetUserByIdAsync(userId);

            if (detail is null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        [HttpPost("~/api/Token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        TokenResponse? response = await _tokenService.GetTokenAsync(request);

        if (response is null)
        {
            return BadRequest(new TextResponse("Invalid username or password"));
        }
        return Ok(response);
        }
    }
}
