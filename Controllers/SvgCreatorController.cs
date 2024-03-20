using EmbedRepoGithub.Interfaces;
using EmbedRepoGithub.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace EmbedRepoGithub.Controllers;

[ApiController]
[Route("[controller]")]
public class SvgCreatorController : ControllerBase
{
    private readonly ILogger<SvgCreatorController> _logger;
    private readonly IRepositoryImageService _repositoryImageService;
    private readonly IClient _client;

    public SvgCreatorController(ILogger<SvgCreatorController> logger, IRepositoryImageService repositoryImageService, IClient client)
    {
        _logger = logger;
        _repositoryImageService = repositoryImageService;
        _client = client;
    }

    [HttpGet(Name = "GetSvg")]
    public async Task<IActionResult> Get()
    {
        string userQuery = Request.Query["user"].ToString();
        string pageQuery = Request.Query["page"].ToString();
        int page = 1;
        if (userQuery.Length == 0)
        {
            return BadRequest("debe especificar el usuario");
        }

        if (pageQuery.Length > 0)
        {
            page = Int32.Parse(pageQuery);
        }

        User? githubUser = await _client.GetUser(userQuery);

        if (githubUser == null)
        {
            _logger.LogError("Se envio un usuario que no existe");
            return BadRequest("Este usuario no existe");
        }

        RepositoryImage? response;
        try
        {
            response = await _repositoryImageService.GetRepositoryGithubImagesAsync(githubUser.Login, page);
        }
        catch (NotFoundException)
        {
            _logger.LogError("ocurrio un error al generar las imagenes");
            return BadRequest("ocurrio un error al generar las imagenes");
        }

        return Ok(response);
    }
}