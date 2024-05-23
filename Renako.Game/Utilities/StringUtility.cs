namespace Renako.Game.Utilities;

public class StringUtility
{
    /// <summary>
    /// Clean the folder name from special characters that can't be used as folder name.
    /// </summary>
    /// <param name="folderName">The folder name to clean</param>
    /// <returns>The cleaned folder name</returns>
    public static string CleanFolderName(string folderName)
    {
        return folderName.Replace(":", "_").Replace("/", "_").Replace("\\", "_").Replace("*", "_").Replace("?", "_").Replace("\"", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_");
    }
}
