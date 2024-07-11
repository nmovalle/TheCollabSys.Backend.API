using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Entity.Request;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public TokenController(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> GenerateTokenAsync(AuthenticationRequestBody authenticationRequestBody)
        {
            if (string.IsNullOrEmpty(authenticationRequestBody.UserName)) return BadRequest("username is required.");

            var token = await _jwtTokenGenerator.GenerateToken(authenticationRequestBody.UserName);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken(AuhenticationRefreshRequestBody auhenticationRefreshRequestBody)
        {
            //if (string.IsNullOrEmpty(auhenticationRefreshRequestBody.username)) return BadRequest("username is required.");
            if (string.IsNullOrEmpty(auhenticationRefreshRequestBody.refreshToken)) return BadRequest("refresh is required.");

            var token = await _jwtTokenGenerator.RefreshToken(auhenticationRefreshRequestBody.refreshToken);

            return Ok(token);
        }
    }
}
