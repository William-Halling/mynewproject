public static class PendingLoad
{
    public static string NextSceneName { get; private set; }

    public static void SetNextScene(string sceneName)
    {
        NextSceneName = sceneName;
    }

    public static void Clear()
    {
        NextSceneName = null;
    }
}
