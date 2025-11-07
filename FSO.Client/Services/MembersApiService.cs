using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using FSO.Client.Models;

namespace FSO.Client.Services;

public class MembersApiService
{
    private readonly HttpClient _httpClient;

    private readonly string? _apiMembersEndpoint;



    public MembersApiService(HttpClient httpClient, IConfiguration configuration)
    {

        _httpClient = httpClient;
        //_apiEndpoint = configuration.GetValue<string>("ApiEndpointUrl");
        _apiMembersEndpoint = Environment.GetEnvironmentVariable("MembersEndpointUrl");

    }


    public async Task<List<MembersApiDTO>> GetMembersAsync() // MemberApiDTO is the entity model to use here. DOn't use MemberViewModel
    {

        var response = await _httpClient.GetAsync(_apiMembersEndpoint);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var json = JsonSerializer.Deserialize<List<MembersApiDTO>>(content);
        return json ?? new List<MembersApiDTO>();

        //if (string.IsNullOrWhiteSpace(content))
        //{
        //    Debug.WriteLine(content);

        //    return new List<MembersApiDTO>();
        //}

        //try
        //{
        //    var json = JsonSerializer.Deserialize<List<MembersApiDTO>>(content);
        //    return json ?? new List<MembersApiDTO>();
        //}
        //catch (JsonException ex)
        //{
        //    throw new InvalidOperationException("Failed to deserialize response content.", ex);
        //}
    }

    [HttpGet("{id}")]
    public async Task<bool> GetMemberAsync(string id)
    {
        var response = await _httpClient.GetAsync($"{_apiMembersEndpoint}/{id}");
        response.EnsureSuccessStatusCode();

        return true;
    }



    [HttpDelete("{id}")]
    public async Task<bool> DeleteMembersAsync(string id)
    {

        var response = await _httpClient.DeleteAsync($"{_apiMembersEndpoint}/{id}");

        //var content = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        //return JsonSerializer.Deserialize<List<MembersApiDTO>>(content);
        return true;
    }

    [HttpPost("{name}")]
    public async Task<bool> PostMembersAsync(MembersApiDTO memberApiDTO)
    {
        //content = httpClient;

        var response = await _httpClient.PostAsJsonAsync(_apiMembersEndpoint, memberApiDTO);           

        //var content = await response.Content.ReadAsStringAsync();
        //response.EnsureSuccessStatusCode();

        //return JsonSerializer.Deserialize<List<MembersApiDTO>>(content);
        return true;
    }

    [HttpPut("{id}")]
    public async Task<bool> PutMembersAsync(string id, MembersApiDTO memberApiDTO)
    {
        //content = httpClient;

        var response = await _httpClient.PutAsJsonAsync(_apiMembersEndpoint, memberApiDTO.Id);           

        //var content = await response.Content.ReadAsStringAsync();
        //response.EnsureSuccessStatusCode();

        //return JsonSerializer.Deserialize<List<MembersApiDTO>>(content);
        return true;
    }


}