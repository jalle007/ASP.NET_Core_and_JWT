using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
  public interface IUserService
  {
    User Authenticate(string username, string password);
  }

  public class UserService : IUserService
  {
    private List<User> _users = new List<User>
        {
            new User { 
              Id = 1, 
              FirstName = "Jasmin", 
              LastName = "Ibrisimbegovic", 
              Username = "jalle", 
              Password = "test" 
            }
        };
    
    private AppSettings appSettings;
    private readonly string _key;

    public UserService(string key)
    {
      _key =key;
    }
     

    public User Authenticate(string username, string password)
    {
      var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

      // return null if user not found
      if (user == null)
        return null;

      try
      {
        // authentication successful so generate jwt token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
            }),
          Expires = DateTime.UtcNow.AddDays(7),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        user.Token = tokenHandler.WriteToken(token);
      }
      catch (Exception ex)
      {
        throw ex;
      }

      // remove password before returning
      user.Password = null;

      return user;
    }

  
  }
}