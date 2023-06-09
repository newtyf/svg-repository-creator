using System.Diagnostics;
using System.Text.Json;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Octokit;
using IConfiguration = AngleSharp.IConfiguration;

namespace EmbedRepoGithub.Controllers;

[ApiController]
[Route("[controller]")]
public class SvgCreatorController : ControllerBase
{
    private readonly ILogger<SvgCreatorController> _logger;

    public SvgCreatorController(ILogger<SvgCreatorController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetSvg")]
    public async Task<IActionResult> Get()
    {
        string user = Request.Query["user"].ToString();
        string repository = Request.Query["repository"].ToString();

        if (user.Length == 0)
        {
            return BadRequest("debe especificar el usuario");
        }

        if (repository.Length == 0)
        {
            return BadRequest("debe especificar el repositorio");
        }

        string borderColor = "#30363d";
        string repoTitle = "Not found name";
        string repoDescription = "Not Found description";
        string repoLanguage = "Not found language";
        string languageColor = "#188601";


        var client = new GitHubClient(new ProductHeaderValue("myapp"));
        string json = System.IO.File.ReadAllText("./colors.json");
        Dictionary<string, Language> colors = JsonConvert.DeserializeObject<Dictionary<string, Language>>(json)!;
        
        try
        {
            var repo = await client.Repository.Get(user, repository);
            repoTitle = repo.Name;
            repoDescription = repo.Description;
            repoLanguage = repo.Language;
            if (colors.ContainsKey(repoLanguage))
            {
                languageColor = colors[repoLanguage].color;
            }
            
        }

        catch (NotFoundException)
        {
            Debug.WriteLine("No se encontro el repositorio");
        }

        IConfiguration config = Configuration.Default;
        IBrowsingContext context = BrowsingContext.New(config);

        string source = "<html><body></body></html>";
        IDocument document = await context.OpenAsync(req => req.Content(source));
        IParentNode body = document.QuerySelector("body");

        IElement repoSvg =
            SvgRepo.Create(document, repoTitle, repoDescription, repoLanguage, languageColor, borderColor);
        body.Append(repoSvg);

        System.IO.File.WriteAllText("out.svg", document.DocumentElement.OuterHtml);

        byte[] fileBytes = System.IO.File.ReadAllBytes("./out.svg");

        string mimeType = "image/svg+xml";

        return File(fileBytes, mimeType);
    }
}