using System.Net.WebSockets;
using System.Text;

var ws = new ClientWebSocket();

string name;

Console.WriteLine("Enter the username");
name = Console.ReadLine();


Console.WriteLine("connecting to the server");
await ws.ConnectAsync(new Uri($"ws://localhost:5117/ws?name={name}"), CancellationToken.None);

Console.WriteLine("connected to the client");


var sendTask = Task.Run(async () =>
{
    while (true)
    {
        var message = Console.ReadLine();
        if (message == "exit")
        {
            break;
        }

        var bytes = Encoding.UTF8.GetBytes(message);
        await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);


    }
});


var recievetask = Task.Run(async () =>
{
    var buffer = new byte[1024*5];

    while (true)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Close)
        {
            Console.WriteLine("WebSocket connection closed.");
            break;
        }

        Console.WriteLine($"Received message  : {System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count)}");
    }
});


await Task.WhenAny(recievetask, sendTask);

if(ws.State != WebSocketState.Closed)
{

    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);

}

await Task.WhenAll(sendTask, recievetask);
Console.WriteLine("Enter to exit");
Console.ReadLine();




