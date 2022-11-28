using System.Net;
using System.Net.NetworkInformation;

namespace MauiApp1.Models;

public class PReply : BaseModel
{
    private IPAddress address;

    public IPAddress Address
    {
        get => address;
        set => SetProperty(ref address, value);
    }

    private long rounttripTime;

    public long RoundtripTime
    {
        get => rounttripTime;
        set => SetProperty(ref rounttripTime, value);
    }

    private IPStatus status;

    public IPStatus Status
    {
        get => status;
        set => SetProperty(ref status, value);
    }
}