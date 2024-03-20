using EmbedRepoGithub.Models;

namespace EmbedRepoGithub.Interfaces;

public interface IRepositoryImageService
{
    Task<RepositoryImage?> GetRepositoryGithubImagesAsync(string user, int page);
}