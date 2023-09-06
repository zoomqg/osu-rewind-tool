using Reddit.Controllers;

namespace Rewind_Scraper;
using Reddit;
using System.Text.RegularExpressions;

public class PostInfo{ 
    public required string Type { get; init; }
    public required string Description { get; init; }
    public string? VideoLink { get; init; }
    public string? MapLink { get; init; }
    public string? Pp { get; init; }
    public required string RedditLink { get; init; }
    public string? ReplayLink { get; init; }
}


public class RedditWrapper
{
    private readonly RedditClient _reddit;
    
    public RedditWrapper(string appId, string appSecret, string refreshToken)
    {
        _reddit = new RedditClient(appId: appId, appSecret: appSecret, refreshToken: refreshToken);
    }

    public IEnumerable<Reddit.Controllers.Post> GetPostFromLastMonth(int limit=150)
    {
        return _reddit.Subreddit("osugame").Posts.GetTop(t: "month", limit: limit);
    }

    public string? GetLinkFromBody(string commentBody, string reference)
    {
        const string pattern = @"\b(https?://[^\s'""\[\]]+)\b";
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(commentBody);
        foreach (Match match in matches)
        {
            string url = match.Groups[1].Value; 
            if (url.Contains(reference))
            {
                return url;
            }
        }

        return null;
    }

    public string? GetPpFromTitle(string title)
    {
        const string pattern = @"\b(\d+pp)\b";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(title);
        
        return match.Success ? match.Value : null;
    }

    public List<PostInfo> ProcessPosts (int limit = 5)
    {
        List<PostInfo> result = new List<PostInfo>();
        IEnumerable<Reddit.Controllers.Post> posts = GetPostFromLastMonth(limit: limit);
        foreach (Post post in posts)
        {
            string commentBody = post.Comments.GetTop(limit: 1)[0].Body;
            var infoFromPost = new PostInfo
            {
                Description = post.Title,
                Type = post.Listing.LinkFlairText,
                VideoLink = GetLinkFromBody(commentBody, "https://youtu"),
                RedditLink = "https://www.reddit.com" + post.Permalink,
                MapLink = GetLinkFromBody(commentBody, "https://osu.ppy.sh/b/"),
                Pp = GetPpFromTitle(post.Title),
                ReplayLink = null,
            };
            result.Add(infoFromPost);
        }
        return result;
    }
}