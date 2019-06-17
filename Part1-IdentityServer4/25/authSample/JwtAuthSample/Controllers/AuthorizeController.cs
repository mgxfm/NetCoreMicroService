using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JwtAuthSample.ViewModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace JwtAuthSample.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeController : Controller
    {   
        private JwtSettings _jwtSettings;

        public AuthorizeController(IOptions<JwtSettings> _jwtSettingsAccesser)
        {
            _jwtSettings = _jwtSettingsAccesser.Value;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <remarks>
        /// Sample Request
        /// 
        ///     POST /api/authorize/token
        ///     {
        ///         "user":"jesse",
        ///         "password":"123456"        
        ///     }
        /// </remarks>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Token([FromBody]LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                if(!(viewModel.User=="jesse" && viewModel.Password=="123456"))
                {
                    return BadRequest();
                }

                var claims = new Claim[]{
                    new Claim(ClaimTypes.Name, "jesse"),
                    new Claim(ClaimTypes.Role,"user"),
                    new Claim("SuperAdminOnly","true")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claims, 
                    DateTime.Now,DateTime.Now.AddMinutes(30),
                    creds);

                return Ok(new { token=  new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest();
        }

    }
}