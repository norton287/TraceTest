using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

using MauiApp1.Models;

using static Microsoft.Maui.ApplicationModel.MainThread;

namespace MauiApp1.ViewModel;

public class MainPageViewModel : BaseViewModel
{
    private const string DATA = "onerowtworowthreerowfourrow";
    private static readonly byte[] _buffer = Encoding.ASCII.GetBytes(DATA);
    private const int MAX_HOPS = 25;
    private const int REQUEST_TIMEOUT = 4000;

    private PReply _newP;

    public PReply NewP
    {
        get => _newP;
        set => SetProperty(ref _newP, value);
    }

    private List<PReply> pingList;

    public List<PReply> PingList
    {
        get => pingList;
        set => SetProperty(ref pingList, value);
    }

    private ObservableCollection<PReply> pingObservableCollection;

    public ObservableCollection<PReply> PingObservableCollection
    {
        get => pingObservableCollection;
        set => SetProperty(ref pingObservableCollection, value);
    }

    private List<ToolsServer> servers;

    public List<ToolsServer> Servers
    {
        get => servers;
        set => SetProperty(ref servers, value);
    }

    private string serverName;

    public string ServerName
    {
        get => serverName;
        set => SetProperty(ref serverName, value);
    }

    private string server;

    public string Server
    {
        get => server;
        set => SetProperty(ref server, value);
    }

    private string _remove;

    public MainPageViewModel()
    {
        Title = "Main Page";

        NewP = new PReply();

        PingList = new List<PReply>();
        Servers = new List<ToolsServer>();
        PingObservableCollection = new ObservableCollection<PReply>();
    }

    public void OnAppearing()
    {
        BuildServersList();
    }

    private void BuildServersList()
    {
        if (Servers.Count > 0  && Servers != null) Servers.Clear();

        Servers = new List<ToolsServer>()
        {
            new()
            {
                Server = "https://www.google.com"
            },
            new()
            {
                Server = "https://www.facebook.com"
            },
            new()
            {
                Server = "https://www.twitter.com"
            },
            new()
            {
                Server = "https://www.tiktok.com"
            },
            new()
            {
                Server = "https://www.microsoft.com"
            },
            new()
            {
                Server = "https://www.dell.com"
            },
            new()
            {
                Server = "https://www.apple.com"
            },
            new()
            {
                Server = "https://www.hp.com"
            }
        };

        Debug.WriteLine("Servers count is " + Servers.Count);
    }

    public async Task<bool> DoTrace(string ipAddress)
    {
        if (ipAddress.Contains("https://"))
        {
            _remove = ipAddress.Remove(0, 8);
        }
        else if (ipAddress.Contains("http://"))
        {
            _remove = ipAddress.Remove(0, 7);
        }

        if (_remove.Contains(':'))
        {
            var s = _remove.IndexOf(':');
            var t = _remove[..(s)];
            _remove = t;
        }

        Debug.WriteLine("Hostname was " + _remove);

        for (var zeroBasedHop = 0; zeroBasedHop < MAX_HOPS; zeroBasedHop++)
        {
            var result = await TraceReply(_remove, zeroBasedHop).ConfigureAwait(false);
            if (result) Debug.WriteLine("Hop " + zeroBasedHop);
        }

        return await Task.FromResult(PingObservableCollection.Count > 0);
    }

    private IPAddress previousAddress;

    public IPAddress PreviousAddress
    {
        get => previousAddress;
        set => SetProperty(ref previousAddress, value);
    }


    private async Task<bool> TraceReply(string hostNameOrAddress, int zeroBasedHop)
    {
        using var pingSender = new Ping();
        var hop = zeroBasedHop + 1;

        var pingOptions = new PingOptions();
        pingOptions.DontFragment = true;
        pingOptions.Ttl = hop;

        var pingReply = await pingSender.SendPingAsync(
            hostNameOrAddress,
            REQUEST_TIMEOUT,
            _buffer,
            pingOptions
        );

        var reply = pingReply;

        if (reply != null)
        {
            Debug.WriteLine(reply.Address);
            NewP = new PReply()
            {
                Address = reply.Address,
                Status = reply.Status,
                RoundtripTime = reply.RoundtripTime
            };

        }
        
        BeginInvokeOnMainThread(() => { PingObservableCollection.Add(NewP); });

        return true;
    }
}