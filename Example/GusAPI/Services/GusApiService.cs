using System.Text.Json;
using GusAPI.Models;

namespace GusAPI.Services;

public class GusApiService
{
    public async Task<ICollection<Area>> GetData()
    {
        var client = new HttpClient();
        var result = await client.GetAsync(new Uri("https://api-dbw.stat.gov.pl/api/1.1.0/area/area-area"));
        var deserialized = await JsonSerializer.DeserializeAsync<ICollection<Area>>(
            await result.Content.ReadAsStreamAsync());
        return deserialized ?? new List<Area>();
    }
}