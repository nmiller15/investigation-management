namespace Investigations.Features.Shared;

public static class StringCaseConverter
{
    public static string SnakeToPascalCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var words = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
        var result = string.Concat(words.Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        return result;
    }

    public static string PascalToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = string.Concat(input.Select((c, i) =>
            char.IsUpper(c) && i > 0 ? "_" + c.ToString().ToLower() : c.ToString().ToLower()));
        return result;
    }
}
