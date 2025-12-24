using System.IO;
using UnityEngine;

public sealed class LocalFileStorage : ISaveStorage
{
    private readonly string root;


    public LocalFileStorage(string rootPath)
    {
        root = rootPath;

        Directory.CreateDirectory(root);
    }


    private string PathFor(string key) => System.IO.Path.Combine(root, key);


    public bool Exists(string key) => File.Exists(PathFor(key));


    public void WriteText(string key, string content)
    {
        File.WriteAllText(PathFor(key), content);
    }


    public string ReadText(string key)
    {
        return File.ReadAllText(PathFor(key));
    }


    public void Delete(string key)
    {
        var p = PathFor(key);


        if (File.Exists(p))
        {
            File.Delete(p);
        }
    }
}
