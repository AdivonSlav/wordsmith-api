using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wordsmith.Utils.JsonConversion;

public class ConstantCaseEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : Enum
{
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var constantCase = reader.GetString();
        var pascalCase = ToPascalCase(constantCase ?? string.Empty);

        return (TEnum)Enum.Parse(typeof(TEnum), pascalCase);
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var pascalCase = value.ToString();
        var constantCase = ToConstantCase(pascalCase);
        writer.WriteStringValue(constantCase);        
    }

    private static string ToPascalCase(string constantCase)
    {
        var words = constantCase.ToLower().Split("_");

        for (var i = 0; i < words.Length; i++)
        {
            words[i] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(words[i]);
        }

        return string.Join("", words);
    }

    private static string ToConstantCase(string pascalCase)
    {
        var result = string.Empty;

        foreach (var c in pascalCase)
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result += "_";
            }

            result += char.ToUpperInvariant(c);
        }

        return result;
    }
}