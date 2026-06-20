using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using OrderManagement.Frontend.ViewModels;

namespace OrderManagement.Frontend.Pages.Orders;

public class DetailModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;

    public DetailModel(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

    public OrderViewModel? Order { get; set; }

    public async Task OnGetAsync(Guid id)
    {
        var client = _httpFactory.CreateClient("API");
        var resp = await client.GetAsync($"/api/orders/{id}");
        if (resp.IsSuccessStatusCode)
        {
            var json = await resp.Content.ReadAsStringAsync();
            Order = JsonSerializer.Deserialize<OrderViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}