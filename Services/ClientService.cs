using EmbedRepoGithub.Interfaces;
using Octokit;
using Octokit.Internal;

namespace EmbedRepoGithub.Services;

public class ClientService : IClient
{

    private static GitHubClient? _client;
    
    public ClientService()
    {
        var tokerAuth = new Credentials(Environment.GetEnvironmentVariable("OAUTH_TOKEN_GITHUB"));
        var client = new GitHubClient(new ProductHeaderValue("myapp"), new InMemoryCredentialStore(tokerAuth));
        _client = client;
    }

    public async Task<IReadOnlyList<Repository>?> GetAllRepositories(string user, int page)
    {
        var allRepos = await _client?.Repository.GetAllForUser(user, new ApiOptions()
        {
            PageSize = 10,
            PageCount = page,
        })!;

        return allRepos;
    }

    public async Task<User?> GetUser(string user)
    {
        User? githubUser;
        try
        {
            githubUser = await _client?.User.Get(user)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            githubUser = null;
        }

        return githubUser;
    }
}