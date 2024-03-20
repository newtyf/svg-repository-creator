using EmbedRepoGithub.Interfaces;
using EmbedRepoGithub.Models;
using Newtonsoft.Json;
using Octokit;
using Octokit.Internal;
using Credentials = Octokit.Credentials;

namespace EmbedRepoGithub.Services;

public struct Language
{
    public string color;
    public string url;
}

public class RespositoryImageService : IRepositoryImageService
{

    private readonly IBrowserService _browser;
    private readonly string? _host;
    private readonly IClient _client;
    private Dictionary<string, Language> _colors;
    
    public RespositoryImageService(IBrowserService browser, IHttpContextAccessor httpContextAccessor, IClient client)
    {
        _browser = browser;
        _host = httpContextAccessor.HttpContext?.Request.Host.Value;
        _client = client;
        
        string json = File.ReadAllText("./colors.json");
        _colors = JsonConvert.DeserializeObject<Dictionary<string, Language>>(json)!;
    }

    public async Task<RepositoryImage?> GetRepositoryGithubImagesAsync(string user, int page)
    {
        string repoTitle = "Not found name";
        string repoDescription = "Not Found description";
        string repoLanguage = "Not found language";
        string languageColor = "#188601";

        List<Image> images = new List<Image>();
        
        
        try
        {
            var allRepos = await _client.GetAllRepositories(user, page);
            if (allRepos != null)
            {
                var repos = allRepos.Where(repository1 => repository1.Name != user);
            
                Directory.CreateDirectory($"./storage/{user}");

                await using var browser = await _browser.GetBrowser();
                await using var browserPage = await browser?.NewPageAsync()!;
                string htmlTemplate = File.ReadAllText("template.html");
            
                foreach (var repo in repos)
                {
                    repoTitle = repo.Name ?? repoTitle;
                    repoDescription = repo.Description ?? repoDescription;
                    repoLanguage = repo.Language ?? repoLanguage;
                    if (_colors.TryGetValue(repoLanguage, out var color))
                    {
                        languageColor = color.color;
                    }
            
                    string htmlFinal = htmlTemplate
                        .Replace("{title}", repoTitle)
                        .Replace("{description}", repoDescription)
                        .Replace("{language}", repoLanguage)
                        .Replace("{color}", languageColor);
                
                    await browserPage.SetContentAsync(htmlFinal);
                    byte[] imageData = await _browser.TakeScreenshotOfPage(browserPage, 400, 102);
                    await File.WriteAllBytesAsync($"./storage/{user}/{repo.Name}.png", imageData);
                
                    images.Add(new Image($"https://{_host}/storage/{user}/{repo.Name}.png",repo.Name));
                }
            }
        }

        catch (NotFoundException)
        {
            Console.WriteLine("ocurrio un error al generar las imagenes");
        }

        return new RepositoryImage("Github", user,page, images);
    }
}