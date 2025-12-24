


public interface ISaveStorage
{
    bool Exists(string key);


    void WriteText(string key, string content);


    string ReadText(string key);


    void Delete(string key);
}
