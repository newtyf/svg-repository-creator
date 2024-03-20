using EmbedRepoGithub.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace EmbedRepoGithub.Services;

public class BrowserService : IBrowserService
{
    private string _downloadPath = "./browser";

    public async Task<IBrowser?> GetBrowser()
    {
        var options = new LaunchOptions() { Headless = true, ExecutablePath = await GetPathBrowser() };
        var browser = await Puppeteer.LaunchAsync(options);
        return browser;
    }

    public async Task<string> GetPathBrowser()
    {
        var browserFetcherOptions = new BrowserFetcherOptions() { Path = _downloadPath };
        using var browserFetcher = new BrowserFetcher(browserFetcherOptions);
        var installedBrowser = await browserFetcher.DownloadAsync();
        return installedBrowser.GetExecutablePath();
    }

    public async Task<byte[]> TakeScreenshotOfPage(IPage page, int width, int height)
    {
        byte[] imageData = await page.ScreenshotDataAsync(new ScreenshotOptions
        {   Clip = new Clip()
            {
                X = 0,
                Y = 0,
                Width = width,
                Height = height
            }
        });

        return imageData;
    }
}