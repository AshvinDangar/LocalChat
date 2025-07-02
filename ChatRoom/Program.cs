using Microsoft.AspNetCore.Builder;
using System.Net;
using System.Text;
using System.Net.WebSockets;
using System.Security.Cryptography.Xml;
using System.Net.Sockets;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();


var connection = new List<WebSocket>();


app.MapGet("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var ws = await context.WebSockets.AcceptWebSocketAsync();

        connection.Add(ws);

        var curusername = context.Request.Query[" name "];

        await Broadcast($"{curusername} joined the meeting");
        await Broadcast($"{connection.Count} used joined till");
        await Recivemsg(ws, 
            async(result, buffer) =>
            {
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    await Broadcast(curusername + "" + message);
                }


                else if(result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
                {
                    connection.Remove(ws);
                    await Broadcast($"{curusername}  left the meeting");
                    await Broadcast($"{connection.Count}  used joined till");
                    await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);


                }
            }
            );
        //try
        //{
        //    while (ws.State == WebSocketState.Open)
        //    {
        //        var message = $"hello there {DateTime.Now}";
        //        var bytes = Encoding.UTF8.GetBytes(message);
        //        var arraysegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

        //        await ws.SendAsync(arraysegment, WebSocketMessageType.Text, true, CancellationToken.None);

        //        await Task.Delay(1000); // Non-blocking delay
        //    }
        //}
        //finally
        //{
        //    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        //}
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});


async Task Recivemsg(WebSocket webSocket , Action<WebSocketReceiveResult , byte[]>  handlemessage)
{
    var buffer = new byte[1024 * 4];
    while (webSocket.State == WebSocketState.Open)
    {
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),  CancellationToken.None);
        handlemessage(result, buffer);
    }
}

async Task Broadcast(string message)
{
    var bytes = Encoding.UTF8.GetBytes(message);
    foreach(var socket in connection)
    {
        if(socket.State == WebSocketState.Open)
        {
            var arraysegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await socket.SendAsync(arraysegment, WebSocketMessageType.Text, true, CancellationToken.None);

            
        }
    }

}

app.Run();
