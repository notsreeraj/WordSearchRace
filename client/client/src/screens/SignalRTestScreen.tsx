import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

const SignalRTestScreen = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [gameId, setGameId] = useState("");
  const [messages, setMessages] = useState<string[]>([]);
  const [size, setSize] = useState(15);
  const [joined , setJoined] = useState(false);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5267/gamehub")
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {
        console.log("Connected to hub");

        newConnection.on("GameCreatedAndWaitingForPlayer", (id: string) => {
          setJoined(true);
          setGameId(id); // auto fill the gameId field
          setMessages((prev) => [...prev, `GameCreatedAndWaitingForPlayer: ${id}`]);
        });

        newConnection.on("RoomReady", (message : any) => {
          setJoined(true);
          setMessages((prev) => [...prev, `RoomReady: message`]);
        });

        newConnection.on("BothPlayersReady",(gameDto : any)=>{
          setMessages((prev)=>[...prev,`BothPlayerReady: ${JSON.stringify(gameDto)}`])
        } )

      })
      .catch((err) => console.error("Connection failed: ", err));

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  const createGame = async () => {
    if (connection) {
      await connection.invoke("CreateGame", {
        playerID: "FirstPlayer",
        size
      });
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

const ReadyGame = async () => {
  if(connection && joined){
    // invoke the readgame method
    await connection.invoke("ReadyGame",{
      gameId : gameId,
      playerId : "NOtConsiderd",
      playersReady : true

    })
  }
}

  return (
    <div>
      <h2>SignalR Hub Test</h2>

      <div>
        <button onClick={createGame}>Create Game</button>
      </div>

      <select value={size} onChange={(e) => setSize(Number(e.target.value))}>
    {Array.from({ length: 11 }, (_, index) => {
      const value = index + 15;
      return (
        <option key={value} value={value}>
          {value}
        </option>
      );
    })}
  </select>


      <div style={{ marginTop: "1rem" }}>
        <input
          type="text"
          placeholder="Enter Game ID to join"
          value={gameId}
          onChange={(e) => setGameId(e.target.value)}
        />
        <button onClick={joinGame}>Join Game</button>

        <button
        onClick={ReadyGame}
          disabled={!joined}
          style={{
            marginLeft: "0.5rem",
            backgroundColor: joined ? "green" : "gray",
            color: "white",
            cursor: joined ? "pointer" : "not-allowed",
          }}
          >
        Ready
      </button>
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