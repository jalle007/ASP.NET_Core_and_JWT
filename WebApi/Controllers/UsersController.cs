using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
  //[Authorize]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [ApiController]
  [Route("[controller]")]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;
    private ICountryService _countryService;

    public UsersController(IUserService userService, ICountryService countryService)
    {
      _userService = userService;
      _countryService = countryService;
    }

    public UsersController()
    {
    }



    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody]User userParam)
    {
      var user = _userService.Authenticate(userParam.Username, userParam.Password);

      if (user == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(user);
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public IActionResult Test()
    {
      return Ok();
    }

    //[AllowAnonymous]
    [HttpGet("countries")]
    public IActionResult GetCountries(string filter)
    {
      var countries = _countryService.GetCountries("ar").Result;

      return Ok(countries);
    }


  }
}
