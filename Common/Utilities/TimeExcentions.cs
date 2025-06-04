
namespace Common.Utilities
{
    public static class TimeExcentions
    {
        public static string GetTimeElapsedString(DateTime createdAt)
        {
            var now = DateTime.UtcNow;
            var diff = now - createdAt;

            if (diff.TotalDays < 1)
            {
                int hours = (int)diff.TotalHours;
                int minutes = diff.Minutes;
                if (hours > 0)
                    return $"{hours} ساعت پیش";
                else
                    return $"{minutes} دقیقه پیش";
            }
            else if (diff.TotalDays < 30)
            {
                int days = (int)diff.TotalDays;
                return $"{days} روز پیش";
            }
            else if (diff.TotalDays < 365)
            {
                int months = (int)(diff.TotalDays / 30);
                return $"{months} ماه پیش";
            }
            else
            {
                int years = (int)(diff.TotalDays / 365);
                return $"{years} سال پیش";
            }
        }

    }
}
