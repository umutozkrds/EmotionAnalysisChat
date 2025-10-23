using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public TestController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public IActionResult Get() => Ok("API çalışıyor!");

    [HttpPost("analyze")]
public async Task<IActionResult> Analyze([FromBody] AnalyzeRequest request)
{
    try
    {
        using var client = new HttpClient();
        
        // Step 1: POST to get event ID
        var callUrl = "https://umutt000-ai-sentiment-service.hf.space/gradio_api/call/predict";
        var payload = new { 
            data = new[] { request.Text }
        };
        
        var callResponse = await client.PostAsJsonAsync(callUrl, payload);
        
        if (!callResponse.IsSuccessStatusCode)
        {
            return BadRequest($"Failed to initiate prediction: {callResponse.StatusCode}");
        }
        
        var callResult = await callResponse.Content.ReadAsStringAsync();
        
        // Debug: Log the actual response to see the format
        Console.WriteLine($"Gradio response: {callResult}");
        
        // Try to extract event ID from different possible formats
        string? eventId = null;
        
        try
        {
            // Try parsing as JSON first
            using var doc = JsonDocument.Parse(callResult);
            
            // Common Gradio response formats to try
            if (doc.RootElement.TryGetProperty("event_id", out var eventIdProp))
            {
                eventId = eventIdProp.GetString();
            }
            else if (doc.RootElement.TryGetProperty("eventId", out var eventIdProp2))
            {
                eventId = eventIdProp2.GetString();
            }
            else if (doc.RootElement.TryGetProperty("id", out var idProp))
            {
                eventId = idProp.GetString();
            }
        }
        catch
        {
            // If JSON parsing fails, maybe it's just a plain string
            eventId = callResult.Trim().Trim('"');
        }
        
        if (string.IsNullOrEmpty(eventId))
        {
            return BadRequest($"No event ID received. Response was: {callResult}");
        }
        
        // Step 2: GET the result using event ID
        var resultUrl = $"https://umutt000-ai-sentiment-service.hf.space/gradio_api/call/predict/{eventId}";
        
        // Poll for result (Gradio might need a moment to process)
        for (int i = 0; i < 20; i++) // Try up to 20 times
        {
            await Task.Delay(1000); // Wait 1 second between attempts
            
            var resultResponse = await client.GetAsync(resultUrl);
            
            if (resultResponse.IsSuccessStatusCode)
            {
                var result = await resultResponse.Content.ReadAsStringAsync();
                
                // Check if we got actual data (not just status)
                if (!string.IsNullOrEmpty(result) && !result.Contains("\"msg\": \"estimation\""))
                {
                    return Ok(result);
                }
            }
        }
        
        return StatusCode(408, "Timeout waiting for prediction result");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error: {ex.Message}");
    }
}

}

public class AnalyzeRequest
{
    public string Text { get; set; } = string.Empty;
}

public class EventResponse
{
    public string? EventId { get; set; }
}
