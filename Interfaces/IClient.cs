using Octokit;

namespace EmbedRepoGithub.Interfaces;

public interface IClient
{
    Task<IReadOnlyList<Repository>?> GetAllRepositories(string user, int page);
    Task<User?> GetUser(string user);
}