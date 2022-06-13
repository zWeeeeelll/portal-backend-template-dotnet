using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.Models;
using portal.Repositories;
using portal.Services;
using System.Security.Claims;

namespace portal.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
 
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public UserController(IUserRepository userRepository, IConfiguration configuration, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpGet("paises")]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var paises = _userRepository.ListAll();

                return Ok(paises);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public  ActionResult<string> Login(UserModel userModel)
        {
           if(userModel == null)
            {
                return BadRequest("Login Inválido");
            }

           bool existeUsuario = _userRepository.ExisteUsuario(userModel);

           if(existeUsuario)
            {
                var tokenString = _tokenService.GerarToken(_configuration["Jwt:Key"],
                                                          _configuration["Jwt:Issuer"],
                                                          _configuration["Jwt:Audience"],
                                                           userModel);
                return Ok(new { token = tokenString });
            }
           else
            {
                return BadRequest("Login Inválido");
            }
        }


        [HttpPost("registrar")]
        [AllowAnonymous]
        public IActionResult Registrar([FromBody] UserRegisterModel userRegisterModel)
        {
            return Ok(_userRepository.Registrar(userRegisterModel));
        }


        [HttpPost("recupear")]
        [AllowAnonymous]
        public IActionResult Recupear([FromBody] UserRegisterModel userRegisterModel)
        {
            string userAtual = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.Name);

            return Ok(_userRepository.Recupear(userRegisterModel, userAtual));
        }

        [HttpPost("deletar")]
        [AllowAnonymous]
        public IActionResult Deletar()
        {
            string userAtual = TokenService.GetValueFromClaim(HttpContext.User.Identity, ClaimTypes.Name);

            return Ok(_userRepository.Deletar(userAtual));
        }
    }
}