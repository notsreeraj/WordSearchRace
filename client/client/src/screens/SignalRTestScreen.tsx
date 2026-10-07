import { useState, useEffect, useCallback } from "react";
import * as signalR from "@microsoft/signalr";

interface PuzzleDto {
  gridSingleD: string[];
  listOfWords: string[];
}

interface GameDTO {
  id: string;
  players: string[];
  puzzledto: PuzzleDto | null;
}

interface Cell {
  row: number;
  col: number;
  letter: string;
}

const SignalRTestScreen = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [gameId, setGameId] = useState("");
  const [messages, setMessages] = useState<string[]>([]);
  const [size, setSize] = useState(15);
  const [joined, setJoined] = useState(false);
  const [gameDto, setGameDto] = useState<GameDTO | null>(null);
  const [selectedCells, setSelectedCells] = useState<Cell[]>([]);
  const [isDragging, setIsDragging] = useState(false);
  const [startCell, setStartCell] = useState<Cell | null>(null);
  const [isInvalid, setIsInvalid] = useState(false);
  const [playerId,setPlayerID] = useState("");
  const [playersProgress, setPlayersProgress] = useState<Record<string, number> | null>(null);


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
          setGameId(id);
          setMessages((prev) => [...prev, `GameCreated: ${id}`]);
        });

        newConnection.on("GameJoined", (message : string) => {
          setJoined(true);
          setMessages((prev) => [...prev, message]);
        }); 

        newConnection.on("BothPlayersReady", (dto: GameDTO) => {
          setGameDto(dto);
          setMessages((prev) => [...prev, `BothPlayersReady: game starting`]);
        });
        newConnection.on("WaitingSecPlayerToReady",(message : string) =>{
          setMessages((prev)=>
            [...prev, message]
          );
        });
        newConnection.on("ValidSelection" ,(message : string) =>{
          setMessages((prev)=>[...prev, message])
        });
        newConnection.on("InvalidSelection" ,(message : string) =>{
          setMessages((prev)=>[...prev, message])
        });
        newConnection.on("UpdatedPlayersProgress" ,(newPlayersProgress : Record<string, number>)=>{
          setPlayersProgress(newPlayersProgress);
          console.log(playersProgress);
        })


      })
      .catch((err) => console.error("Connection failed: ", err));

    setConnection(newConnection);

    return () => { newConnection.stop(); };
  }, []);

  // stop drag if mouse released outside the grid
  useEffect(() => {
    const handleMouseUp = () => setIsDragging(false);
    window.addEventListener("mouseup", handleMouseUp);
    return () => window.removeEventListener("mouseup", handleMouseUp);
  }, []);

  const getCellsBetween = (start: Cell, end: Cell): Cell[] | null => {
    const rowDiff = end.row - start.row;
    const colDiff = end.col - start.col;
    const absRow = Math.abs(rowDiff);
    const absCol = Math.abs(colDiff);

    const isHorizontal = rowDiff === 0;
    const isVertical = colDiff === 0;
    const isDiagonal = absRow === absCol;

    if (!isHorizontal && !isVertical && !isDiagonal) return null;

    const rowStep = rowDiff === 0 ? 0 : rowDiff / absRow;
    const colStep = colDiff === 0 ? 0 : colDiff / absCol;
    const steps = Math.max(absRow, absCol);

    const grid = gameDto!.puzzledto!.gridSingleD;
    const cells: Cell[] = [];

    for (let i = 0; i <= steps; i++) {
      const r = start.row + i * rowStep;
      const c = start.col + i * colStep;
      cells.push({ row: r, col: c, letter: grid[r][c] });
    }

    return cells;
  };

  const handleMouseDown = (letter: string, row: number, col: number) => {
    const cell = { row, col, letter };
    setIsDragging(true);
    setStartCell(cell);
    setSelectedCells([cell]);
    setIsInvalid(false);
  };

  const handleMouseEnter = (letter: string, row: number, col: number) => {
    if (!isDragging || !startCell) return;
    const endCell = { row, col, letter };
    const cells = getCellsBetween(startCell, endCell);
    if (cells) {
      setSelectedCells(cells);
      setIsInvalid(false);
    } else {
      setIsInvalid(true);
    }
  };

  const handleMouseUp = () => {
    setIsDragging(false);
    if (isInvalid) setSelectedCells([]);
    // lets call the submit word method here instead of the submit button
    SubmitWord()
  };

  const isSelected = (row: number, col: number) =>
    selectedCells.some((c) => c.row === row && c.col === col);

  const createGame = async () => {
    if (connection) {
      // set player id 
      setPlayerID("FirstPlayer");
      await connection.invoke("CreateGame", { playerID: "FirstPlayer", size });
    }
  };

  const joinGame = async () => {
    if (connection && gameId) {
      setPlayerID("Secondplayer");
      await connection.invoke("JoinGame", { gameID: gameId, playerID: "Secondplayer" });
    }
  };

  const readyGame = async () => {
    if (connection && joined) {
      await connection.invoke("ReadyGame", {
        gameId,
        playerId: "NotConsidered",
        playersReady: true,
      });
    }
  };

  const SubmitWord = async ()=>{
    if(connection && gameId){
      await connection.invoke("SubmitWord",{
        // submit word dto
        playerId,
        gameId,
        ClientSelection : selectedCells
      })
    }
  }

  const renderGrid = () => {
    if (!gameDto?.puzzledto?.gridSingleD) return null;
    const grid = gameDto.puzzledto.gridSingleD;

    return (
      
      <div style={{ userSelect: "none" }}>
        <table style={{ borderCollapse: "collapse", marginTop: "1rem" }}>
          <tbody>
            {grid.map((row, rowIndex) => (
              <tr key={rowIndex}>
                {row.split("").map((letter, colIndex) => (
                  <td key={colIndex}>
                    <button
                      onMouseDown={() => handleMouseDown(letter, rowIndex, colIndex)}
                      onMouseEnter={() => handleMouseEnter(letter, rowIndex, colIndex)}
                      onMouseUp={handleMouseUp}
                      style={{
                        width: "36px",
                        height: "36px",
                        fontSize: "14px",
                        fontWeight: "bold",
                        cursor: "pointer",
                        backgroundColor: isInvalid && isSelected(rowIndex, colIndex)
                          ? "#FEE2E2"
                          : isSelected(rowIndex, colIndex)
                          ? "#4F46E5"
                          : "#F3F4F6",
                        color: isSelected(rowIndex, colIndex) ? (isInvalid ? "#991B1B" : "white") : "black",
                        border: "1px solid #D1D5DB",
                        borderRadius: "4px",
                      }}
                    >
                      {letter}
                    </button>
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>

        {/* coordinate display */}
        {/* <div style={{ marginTop: "1rem" }}>
          <strong>Selected word:</strong> {selectedCells.map((c) => c.letter).join("") || "—"}
          <button
          onClick={SubmitWord}
          disabled={selectedCells.length === 0}
          style={{
            marginLeft: "1rem",
            backgroundColor: selectedCells.length > 0 ? "#4F46E5" : "gray",
            color: "white",
            padding: "4px 12px",
            borderRadius: "4px",
            cursor: selectedCells.length > 0 ? "pointer" : "not-allowed",
          }}
        >
          Submit
        </button>
        </div> */}
        {/* Progress Bars */}
{playersProgress && gameDto?.puzzledto && (
  <div style={{ marginTop: "1rem" }}>
    <h3>Progress</h3>
    {Object.entries(playersProgress).map(([playerId, count]) => {
      const total = gameDto.puzzledto!.listOfWords.length;
      const percentage = Math.round((count / total) * 100);

      return (
        <div key={playerId} style={{ marginBottom: "0.75rem" }}>
          <div style={{ fontSize: "13px", marginBottom: "4px" }}>
            {playerId}: {count} / {total} words
          </div>
          <div style={{
            width: "100%",
            height: "20px",
            background: "#E5E7EB",
            borderRadius: "10px",
            overflow: "hidden"
          }}>
            <div style={{
              width: `${percentage}%`,
              height: "100%",
              background: "#4F46E5",
              borderRadius: "10px",
              transition: "width 0.3s ease"
            }} />
          </div>
          <div style={{ fontSize: "12px", color: "#6B7280", marginTop: "2px" }}>
            {percentage}%
          </div>
        </div>
      );
    })}
  </div>
)}
        <textarea
          readOnly
          rows={5}
          style={{ width: "100%", fontFamily: "monospace", marginTop: "0.5rem" }}
          value={selectedCells
            .map((c) => `row=${c.row}, col=${c.col}, letter=${c.letter}`)
            .join("\n")}
        />
         <div style={{ marginTop: "1rem" }}>
            <h3>Messages:</h3>
            {messages.map((msg, index) => <p key={index}>{msg}</p>)}
          </div>
        <div style={{ marginTop: "1rem" }}>
          <h3>Words to find:</h3>
          <ul>
            {gameDto.puzzledto.listOfWords.map((word, index) => (
              <li key={index}>{word}</li>
            ))}
          </ul>
        </div>
      </div>
    );
  };

  return (
    <div style={{ padding: "1rem" }}>
      <h2>SignalR Hub Test</h2>

      {!gameDto && (
        <>
          <div>
            <select value={size} onChange={(e) => setSize(Number(e.target.value))}>
              {Array.from({ length: 11 }, (_, i) => {
                const v = i + 15;
                return <option key={v} value={v}>{v}</option>;
              })}
            </select>
            <button onClick={createGame} style={{ marginLeft: "0.5rem" }}>Create Game</button>
          </div>

          <div style={{ marginTop: "1rem" }}>
            <input
              type="text"
              placeholder="Enter Game ID to join"
              value={gameId}
              onChange={(e) => setGameId(e.target.value)}
            />
            <button onClick={joinGame} style={{ marginLeft: "0.5rem" }}>Join Game</button>
            <button
              onClick={readyGame}
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
            {messages.map((msg, index) => <p key={index}>{msg}</p>)}
          </div>
        </>
      )}

      {gameDto && renderGrid()}
    </div>
  );
};

export default SignalRTestScreen;