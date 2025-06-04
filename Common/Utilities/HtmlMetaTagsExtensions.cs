using System.Text;

namespace Common.Utilities;

public static class HtmlMetaTagsExtensions
{
    public static string AddTitle(this StringBuilder sb, string title)
    {
        sb.AppendLine($"<title>{title}</title>");
        return sb.ToString();
    }

    public static string AddMetaDescription(this StringBuilder sb, string description)
    {
        sb.AppendLine($"<meta name=\"description\" content=\"{description}\">");
        return sb.ToString();
    }

    public static string AddMetaKeywords(this StringBuilder sb, string keywords)
    {
        sb.AppendLine($"<meta name=\"keywords\" content=\"{keywords}\">");
        return sb.ToString();
    }

    public static string AddMetaAuthor(this StringBuilder sb, string author)
    {
        sb.AppendLine($"<meta name=\"author\" content=\"{author}\">");
        return sb.ToString();
    }

    public static string AddMetaRobots(this StringBuilder sb)
    {
        sb.AppendLine("<meta name=\"robots\" content=\"index, follow\">");
        return sb.ToString();
    }

    public static string AddCanonicalLink(this StringBuilder sb, string canonicalUrl)
    {
        sb.AppendLine($"<link rel=\"canonical\" href=\"{canonicalUrl}\">");
        return sb.ToString();
    }

    public static string AddOgTitle(this StringBuilder sb, string ogTitle)
    {
        sb.AppendLine($"<meta property=\"og:title\" content=\"{ogTitle}\">");
        return sb.ToString();
    }

    public static string AddOgDescription(this StringBuilder sb, string ogDescription)
    {
        sb.AppendLine($"<meta property=\"og:description\" content=\"{ogDescription}\">");
        return sb.ToString();
    }

    public static string AddOgImage(this StringBuilder sb, string ogImage)
    {
        sb.AppendLine($"<meta property=\"og:image\" content=\"{ogImage}\">");
        return sb.ToString();
    }

    public static string AddOgUrl(this StringBuilder sb, string ogUrl)
    {
        sb.AppendLine($"<meta property=\"og:url\" content=\"{ogUrl}\">");
        return sb.ToString();
    }
}