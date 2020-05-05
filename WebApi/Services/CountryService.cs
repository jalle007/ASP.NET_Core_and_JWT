using RESTCountries.Models;
using RESTCountries.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
  public interface ICountryService
  {
    Task<List<CountryShort>> GetCountries(string filter);
  }

  public class CountryService : ICountryService
  {
    private readonly string _key;

    public CountryService(string key)
    {
      _key = key;
    }

    // Filter by country name
    public async Task<List<CountryShort>> GetCountries(string filter)
    {
      List<Country> countries = await RESTCountriesAPI.GetCountriesByNameContainsAsync(filter);
      var list = from item in countries
                 select new CountryShort
                 {
                   Name = item.Name,
                   Alpha2Code = item.Alpha2Code,
                   Flag = item.Flag
                 };

      return list.ToList();
    }


  }
}