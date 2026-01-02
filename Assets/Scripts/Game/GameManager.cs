using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void RestartGame()
    {
        // 1️⃣ Clear selection
        SelectionManager.Instance.ClearSelectionExternally();

        // 2️⃣ Clear board
        BoardManager.Instance.ClearBoard();

        // 3️⃣ Reset UI
        UIManager.Instance.ResetUI();

        // 4️⃣ Reset turn
        TurnManager.Instance.ResetTurn();

        // 5️⃣ Respawn pieces
        PieceSpawner.Instance.SpawnAllPieces();

        Debug.Log("Game Restarted");
    }
}