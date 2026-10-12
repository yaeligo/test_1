using System.Net.Http.Json;
using GithubSearch.Server.Models;

namespace GithubSearch.Server.Services;

public interface IGitHubService
{
    Task<GitHubSearchResult?> SearchRepositoriesAsync(string query);
}

public class GitHubService : IGitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com/");
        // GitHub API דורש כותרת User-Agent בכל בקשה
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "GitHubRepoSearchApp");
    }

    public async Task<GitHubSearchResult?> SearchRepositoriesAsync(string query)
    {
        var response = await _httpClient.GetAsync($"search/repositories?q={Uri.EscapeDataString(query)}");
        if (!response.IsSuccessStatusCode)
        {
            return new GitHubSearchResult();
        }

        return await response.Content.ReadFromJsonAsync<GitHubSearchResult>();
    }
}
