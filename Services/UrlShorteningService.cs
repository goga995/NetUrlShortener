using Microsoft.EntityFrameworkCore;

using NetUrlShortener.Data;

namespace NetUrlShortener.Services;

public class UrlShorteningService
{
    private readonly AppDbContext _dbContext;

    public UrlShorteningService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public const int NumberOfCharsInShortLink = 7;
    private const string Alphabet = "ASDFGHJKLQWERTYUIOPZXCVBNMasdfgjklqwertyuiopzxcvbnm1234567890";

    private readonly Random _random = new();

    public async Task<string> GenerateUniqueCode()
    {
        var codeChar = new char[NumberOfCharsInShortLink];

        while (true)
        {
            for (int i = 0; i < NumberOfCharsInShortLink; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);

                codeChar[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChar);

            if (!await _dbContext.ShortendUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }
    }
}