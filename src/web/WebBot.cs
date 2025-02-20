using System.Net;

public class WebBot
{
    public static HttpListener HttpListener = new HttpListener();

    public static void Start()
    {
        int Port = 8090;
        HttpListener.Prefixes.Add("http://*:" + Port.ToString() + "/");
        HttpListener.Start();
        HttpListener.BeginGetContext(new AsyncCallback(Test), HttpListener);
    }

    static void Test(IAsyncResult result)
    {
        Console.WriteLine("okay");
    }
}