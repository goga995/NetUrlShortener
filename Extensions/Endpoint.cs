using Microsoft.EntityFrameworkCore;

using NetUrlShortener.Data;
using NetUrlShortener.Entities;
using NetUrlShortener.Models;
using NetUrlShortener.Services;

namespace NetUrlShortener.Extensions;

public static class Endpoint
{
    public static void AddEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/shorten", async (ShortenUrlRequest request,
                                        UrlShorteningService urlShorteningService,
                                        AppDbContext dbContext,
                                        HttpContext httpContext) =>
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return Results.BadRequest("Specified Url is invalid");
            }

            var code = await urlShorteningService.GenerateUniqueCode();

            var shortendUrl = new ShortendUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = request.Url,
                Code = code,
                ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
                CreatedOnDateUtc = DateTime.UtcNow
            };

            await dbContext.ShortendUrls.AddAsync(shortendUrl);

            await dbContext.SaveChangesAsync();
            //Treba da se doda error handling jer SaveChanges moze da fajluje
            return Results.Ok(shortendUrl.ShortUrl);
        });

        builder.MapGet("api/{code}", async (string code, AppDbContext dbContext) =>
        {
            var shortendUrl = await dbContext.ShortendUrls.FirstOrDefaultAsync(s => s.Code == code);

            if (shortendUrl is null)
            {
                Results.NotFound();
            }

            return Results.Redirect(shortendUrl!.LongUrl);
        });
        //Treba da se doda Cache zbog Scaling
    }
}