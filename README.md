# 🎮 Tic-Tac-Toe with AI (Unity + C#)

![Unity](https://img.shields.io/badge/Unity-2021%2B-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-Game%20Scripts-239120?logo=csharp&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

This project is a simple **Tic-Tac-Toe (Noughts and Crosses)** game built in Unity.  
It features a **Minimax-based AI opponent**, ensuring the computer always plays optimally.

---

## 🚀 Features
- ✅ Single-player Tic-Tac-Toe against AI  
- ✅ AI powered by **Minimax algorithm** (never loses)  
- ✅ Randomly assigns **X** or **O** at the start  
- ✅ Turn indicator (Player vs. Enemy)  
- ✅ Win / Lose / Draw detection  
- ✅ Restart game at any time  

---

## 🛠 Code Overview

### `GameManager.cs`
- Controls overall game logic  
- Manages turns and player/enemy symbols  
- Handles UI updates with **TextMeshPro**  
- Calls **Minimax** to calculate the enemy’s best move  
- Detects game results and restarts games  

### `GridManager.cs`
- Manages the tic-tac-toe board state  
- Resets the grid for new games  
- Checks for winning combinations or full board  

### `GridSquareManager.cs`
- Represents each individual square  
- Handles **player input** via clicks  
- Updates visual state (`X`, `O`, or empty)  

### Enums
- `Turn` → current turn (`playerTurn` / `enemyTurn`)  
- `GameResult` → ongoing / player win / enemy win / draw  
- `GridSquareState` → square state (`empty`, `x`, `o`)  

---

## 🤖 Minimax AI
The enemy AI uses the **Minimax algorithm**:
- Explores all possible moves  
- Simulates both player and enemy turns  
- Scores outcomes:  
  - Enemy win = `+10`  
  - Player win = `-10`  
  - Draw = `0`  
- Always selects the best available move  

➡️ The AI will **never lose** (only win or draw).  

---

## 🖥 How to Run
1. You can find the exe here -> https://marizitoss.itch.io/tic-tac-toe

---

## 📂 Project Structure
TICTACTOE
┣ 📂 Assets
┃ ┣ 📂 Arts
┃ ┣ 📂 Fonts
┃ ┣ 📂 Prefabs
┃ ┣ 📂 Resources
┃ ┣ 📂 Scenes
┃ ┣ 📂 Scripts
┃ ┃ ┣ 📜 GameManager.cs
┃ ┃ ┣ 📜 GridManager.cs
┃ ┃ ┗ 📜 GridSquareManager.cs
┣ 📂 Settings
┣ 📂 TextMesh Pro
┣ 📂 Packages
┣ 📜 .gitignore
┣ 📜 README.md
┣ 📜 ticTacToe.sln
┗ 📜 Assembly-CSharp.csproj

---

## 📜 License
This project is open-source and free to use for educational or personal purposes.
