using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using OrderManagement.Frontend.ViewModels;

namespace OrderManagement.Frontend.Pages.Orders;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;

    public CreateModel(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

    public List<CustomerViewModel> Customers { get; set; } = new();
    public List<DiscountStrategyViewModel> DiscountStrategies { get; set; } = new();

    public async Task OnGetAsync()
    {
        var client = _httpFactory.CreateClient("API");
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var custResp = await client.GetAsync("/api/customers");
        if (custResp.IsSuccessStatusCode)
            Customers = JsonSerializer.Deserialize<List<CustomerViewModel>>(await custResp.Content.ReadAsStringAsync(), opts) ?? new();

        var discResp = await client.GetAsync("/api/orders/discount/strategies");
        if (discResp.IsSuccessStatusCode)
            DiscountStrategies = JsonSerializer.Deserialize<List<DiscountStrategyViewModel>>(await discResp.Content.ReadAsStringAsync(), opts) ?? new();
    }

    // This page submits via Fetch API — server-side post handler for fallback
    public async Task<IActionResult> OnPostAsync([FromForm] CreateOrderFormModel form)
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpFactory.CreateClient("API");
        var payload = JsonSerializer.Serialize(form);
        var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
        var resp = await client.PostAsync("/api/orders", content);

        if (resp.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToPage("Index");
        }

        TempData["ErrorMessage"] = "Failed to create order.";
        await OnGetAsync();
        return Page();
    }
}

public class CreateOrderFormModel
{
    public Guid CustomerId { get; set; }
    public int DiscountStrategy { get; set; }
    public List<CreateItemFormModel> Items { get; set; } = new();
}

public class CreateItemFormModel
{
    public string ProductName { get; set; } = "";
    public string ProductCode { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}