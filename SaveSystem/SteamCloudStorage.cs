using UnityEngine;

    // Hook this up with Steamworks.Net (RemoteStorage) when you're ready.
public sealed class SteamCloudStorage : ISaveStorage
{
    public bool Exists(string key)
    {
            // TODO: SteamRemoteStorage.FileExists(key)
        Debug.LogWarning("[SteamCloudStorage] Exists() not implemented yet.");

        return false;
    }

    public void WriteText(string key, string content)
    {
            // TODO: SteamRemoteStorage.FileWrite(key, bytes)
        Debug.LogWarning("[SteamCloudStorage] WriteText() not implemented yet.");
    }


    public string ReadText(string key)
    {
            // TODO: SteamRemoteStorage.FileRead(key, buffer)
        Debug.LogWarning("[SteamCloudStorage] ReadText() not implemented yet.");

        return null;
    }


    public void Delete(string key)
    {
            // TODO: SteamRemoteStorage.FileDelete(key)
        Debug.LogWarning("[SteamCloudStorage] Delete() not implemented yet.");
    }
}
