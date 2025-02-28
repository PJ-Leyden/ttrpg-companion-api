using System.Collections.Generic;
using Newtonsoft.Json;

namespace ttrpg_companion_api.Models.Response;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();
}
