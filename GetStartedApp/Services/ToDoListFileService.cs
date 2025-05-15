using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GetStartedApp.Models;
using Path = System.IO.Path;

namespace GetStartedApp.Services;

public static class ToDoListFileService
{
    // Hardcoded path to grab the save file
    private static string _jsonFileName = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Avalonia.SimpleTodoList", "MyTodoList.txt");

    /// <summary>
    /// Stores given items to a file on disk
    /// </summary>
    /// <param name="itemsToSave">The items to save</param>
    /// <returns></returns>
    public static async Task SaveToFileAsync(IEnumerable<ToDoItem> itemsToSave)
    {
        // Ensure all directories exist
        Directory.CreateDirectory(Path.GetDirectoryName(_jsonFileName)!);

        // Use a FileStream to write all items to disk
        using (var fs = File.Create(_jsonFileName))
        {
            await JsonSerializer.SerializeAsync(fs, itemsToSave);
        }
    }

    /// <summary>
    /// Loads the file from disk and returns the items stored inside.
    /// </summary>
    /// <returns>An IEnumerable of items loaded or null in case the file was not found</returns>
    public static async Task<IEnumerable<ToDoItem>?> LoadFromFileAsync()
    {
        try
        {
            using (var fs = File.OpenRead(_jsonFileName))
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<ToDoItem>>(fs);
            }
        }
        catch (Exception e)
        {
            // In case the file was not found, return null
            return null;
        }
    }
}