
using System;

using System.Text;
using Terraria.Localization;
using Terraria.ModLoader;
namespace StarsAbove.Utilities;

internal static class LangHelper
{

	internal static string GetTextValue(string key, params object[] args)
	{
		return GetModTextValue(StarsAbove.Instance, key, args);
	}
	private static string GetModTextValue(Mod mod, string key, params object[] args)
	{
		return Language.GetTextValue($"Mods.{mod.Name}.{key}", args);
	}
    private static int GetModTextCategorySize(Mod mod, string key)
    {
        return Language.GetCategorySize($"Mods.{mod.Name}.{key}");
    }
    internal static int GetCategorySize(string key)
    {
        return GetModTextCategorySize(StarsAbove.Instance, key);

    }

    public static string LegacyWrap(string v, int size)
	{
	    v = v.TrimStart();
	    if (v.Length <= size) return v;
	    var nextspace = v.LastIndexOf(' ', size);
	    if (-1 == nextspace) nextspace = Math.Min(v.Length, size);
	    return v.Substring(0, nextspace) + ((nextspace >= v.Length) ?
	    "" : "\n" + Wrap(v.Substring(nextspace), size));
	}

	/// <summary>
	/// Auto add newline according to <paramref name="limit" />
	/// </summary>
	/// <param name="text"> Input Text </param>
	/// <param name="limit"> Character count limit </param>
	/// <returns> </returns>
	internal static string Wrap(ReadOnlySpan<char> text, int limit)
	{
		const int MaxNewLine = 8;


		// Just try 2.275f and found it fits
		limit = (GameCulture.CultureName)Language.ActiveCulture.LegacyId switch
		{
			GameCulture.CultureName.Chinese => (int)(limit / 2.275f),
			_ => limit,
		};
		text = text.Trim();
		int start = 0;
		StringBuilder stringBuilder = new StringBuilder(text.Length + MaxNewLine);
		while (limit + start < text.Length)
		{
			var line = text.Slice(start, limit);
			int length = line.Length, skip = 0;
			for (int i = 0; i < line.Length; i++)
			{
				if (line[i] == '\n')
				{
					length = i;
					skip = 1;
					break;
				}
				else if (char.IsWhiteSpace(line[i]))
				{
					length = i;
					skip = 1;
				}
			}
			stringBuilder.Append(line[..length]).AppendLine();
			start += length + skip;
		}
		stringBuilder.Append(text[start..]);

		//Bad memory copy
		return stringBuilder.ToString();
	}

}