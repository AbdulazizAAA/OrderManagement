using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using OrderManagement.Frontend.ViewModels;

namespace OrderManagement.Frontend.Pages.Orders;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpFactory;

    public IndexModel(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

    public List<OrderViewModel> Orders { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }

    public async Task OnGetAsync(
        string? search, string? status, string? sortBy,
        bool sortDesc = false, int page = 1, int pageSize = 10)
    {
        Search = search;
        Status = status;
        SortBy = sortBy;
        SortDesc = sortDesc;
        PageNumber = page;
        PageSize = pageSize;

        var client = _httpFactory.CreateClient("API");
        var qs = $"?page={page}&pageSize={pageSize}" +
                 (search != null ? $"&search={Uri.EscapeDataString(search)}" : "") +
                 (status != null ? $"&status={status}" : "") +
                 (sortBy != null ? $"&sortBy={sortBy}" : "") +
                 (sortDesc ? "&sortDesc=true" : "");

        var resp = await client.GetAsync($"/api/Orders{qs}"); 
        if (resp.IsSuccessStatusCode)
        {
            var json = await resp.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PagedResultViewModel<OrderViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Orders = result?.Items ?? new();
            TotalCount = result?.TotalCount ?? 0;
            TotalPages = result?.TotalPages ?? 0;
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var client = _httpFactory.CreateClient("API");
        var resp = await client.DeleteAsync($"/api/orders/{id}");
        TempData[resp.IsSuccessStatusCode ? "SuccessMessage" : "ErrorMessage"] =
            resp.IsSuccessStatusCode ? "Order deleted successfully." : "Failed to delete order.";
        return RedirectToPage();
    }
}