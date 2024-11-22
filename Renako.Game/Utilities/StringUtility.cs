using System.IO;

namespace Renako.Game.Utilities;

public class StringUtility
{
    private const string replace_char = "_";

    /// <summary>
    /// Clean the folder name or file name from special characters that can't be used as folder name.
    /// </summary>
    /// <param name="fileName">The folder name to clean</param>
    /// <returns>The cleaned folder name</returns>
    public static string CleanFileName(string fileName)
    {
        foreach (var character in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(character.ToString(), replace_char);
        }

        return fileName;
    }
}
