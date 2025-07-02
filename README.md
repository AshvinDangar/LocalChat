```markdown
# ChatApp

A simple console-based chat application built with .NET 8 using WebSockets. This project demonstrates real-time, bidirectional communication between a client and a server using the WebSocket protocol.

## Features

- Real-time messaging between client and server
- User can send and receive messages concurrently
- Graceful connection closing
- Simple username prompt for identification

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or later (recommended)

## Getting Started

### 1. Clone the Repository

```
git clone <your-repo-url>
cd <your-repo-directory>
```

### 2. Run the Server

1. Open the server project in Visual Studio or your preferred editor.
2. Build and run the server:

```
   dotnet run --project <ServerProjectPath>
```

The server will start and listen for WebSocket connections on `ws://localhost:5117/ws`.

### 3. Run the Client

1. Open the client project in Visual Studio or your preferred editor.
2. Build and run the client:

```
   dotnet run --project <ClientProjectPath>
```

3. Enter your username when prompted.
4. Start sending messages! Type `exit` to disconnect.

## How It Works

- The client connects to the server using a WebSocket connection.
- The client can send messages to the server and receive messages from the server concurrently.
- The server can broadcast or echo messages as per your implementation.

## Customization

- You can extend the server to support multiple clients, broadcast messages, or add authentication.
- Update the server and client logic as needed for your use case.
