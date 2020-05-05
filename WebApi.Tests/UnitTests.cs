using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using WebApi.Controllers;
using WebApi.Helpers;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests
{
  public class UnitTests
  {
    IUserService _userService;
    ICountryService _countryService;
    UsersController _userController;

    public IConfiguration Configuration { get; }

    public UnitTests()
    {

      // We need to get secret key from appconfig
      var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .Build();

      var appSettingsSection = config.GetSection("AppSettings");

      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = appSettings.Secret;

      _userService = new UserService(key);
      _countryService = new CountryService(key);

      _userController = new UsersController(_userService, _countryService);
    }


    [Fact]
    public void Invalid_Login()
    {
      var response = _userController.Authenticate(new Entities.User()
      { Username = "jalle", Password = "wrong" });

      var okObjectResult = response as OkObjectResult;
      Assert.NotNull(okObjectResult);
    }


    [Fact]
    public void Valid_Request()
    {
      var response = _userController.Authenticate(new Entities.User()
      { Username = "jalle", Password = "test" });

      var okObjectResult = response as OkObjectResult;

      if (okObjectResult.StatusCode == 200)
      {
        var countries = _userController.GetCountries("ar");
        var resp = countries as OkObjectResult;

        bool result = resp.StatusCode == 200 && resp.Value != null;
        // Test success
        Assert.True(result);
      }
      else
      {
        // Test failed
        Assert.True(false);
      }

    }

  }
}
