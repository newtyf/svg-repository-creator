using System.Text.Json.Serialization;

namespace EmbedRepoGithub.Models;

public record RepositoryImage(
    [property: JsonPropertyName("owner")] string Owner,  
    [property: JsonPropertyName("repository")] string Repository,  
    [property: JsonPropertyName("page")] int Page,
    [property: JsonPropertyName("images")] IReadOnlyList<Image> Images  
);

public record Image(  
    [property: JsonPropertyName("url")] string Url,  
    [property: JsonPropertyName("name")] string? Name  
); 