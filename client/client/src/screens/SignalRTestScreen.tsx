import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const SignalRTestScreen = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [gameId, setGameId] = useState("");
  const [messages, setMessages] = useState<string[]>([]);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5267/gamehub")
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {
        console.log("Connected to hub");

        newConnection.on("GameCreatedAndWaitingForPlayer", (id: string) => {
          setGameId(id); // auto fill the gameId field
          setMessages((prev) => [...prev, `GameCreatedAndWaitingForPlayer: ${id}`]);
        });

        newConnection.on("RoomReady", (gameDto: any) => {
          setMessages((prev) => [...prev, `RoomReady: ${JSON.stringify(gameDto)}`]);
        });

      })
      .catch((err) => console.error("Connection failed: ", err));

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  const createGame = async () => {
    if (connection) {
      await connection.invoke("CreateGame", "firstplayer");
    }
  };

  const joinGame = async () => {
    if (connection && gameId) {
      await connection.invoke("JoinGame", {
        gameID: gameId,
        playerID: "secondplayer"
      });
    }
  };

  return (
    <div>
      <h2>SignalR Hub Test</h2>

      <div>
        <button onClick={createGame}>Create Game</button>
      </div>

      <div style={{ marginTop: "1rem" }}>
        <input
          type="text"
          placeholder="Enter Game ID to join"
          value={gameId}
          onChange={(e) => setGameId(e.target.value)}
        />
        <button onClick={joinGame}>Join Game</button>
      </div>

      <div style={{ marginTop: "1rem" }}>
        <h3>Messages:</h3>
        {messages.map((msg, index) => (
          <p key={index}>{msg}</p>
        ))}
      </div>
    </div>
  );
};

export default SignalRTestScreen;