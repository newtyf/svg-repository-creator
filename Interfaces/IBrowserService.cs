using PuppeteerSharp;

namespace EmbedRepoGithub.Interfaces;

public interface IBrowserService
{
    Task<IBrowser?> GetBrowser();
    Task<string> GetPathBrowser();

    Task<byte[]> TakeScreenshotOfPage(IPage page, int width, int height);
}