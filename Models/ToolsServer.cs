namespace MauiApp1.Models;

public class ToolsServer : BaseModel
{
    private string server;

    public string Server
    {
        get => server;
        set => SetProperty(ref server, value);
    }
}