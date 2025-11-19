using Cart.Application.Interface;
using Catalog.API.DTOs;
using System.Net.Http.Json;

namespace Cart.Infrastructure.Services;

public class CatalogClient : ICatalogClient
{
   private readonly HttpClient _http;
    private readonly ILogger _logger;
    public CatalogClient(HttpClient http, ILogger<CatalogClient> logger)
    {
        _http = http;
        _logger = logger;
                                        }

    //public async Task GetCartData()
    //{
    //    HttpResponseMessage response = await _http.GetAsync("api/Products");
    //    response.EnsureSuccessStatusCode();
    //    var jsonResponse = await response.Content.ReadAsStringAsync();
    //    _logger.LogInformation("catalog response {jsonResponse}", jsonResponse);
    
    //}

    public async Task<ProductDto?> GetProduct(Guid id, CancellationToken ct=default)
    {
        var response = await _http.GetAsync($"/api/Products/{id}");
        if (!response.IsSuccessStatusCode)
            return null;


        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }
}
