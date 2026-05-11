using UnityEngine;
using TMPro;

public class ChessClock : MonoBehaviour
{
    [Header("Time (seconds)")]
    public float initialTime = 600f; // 10 min per player

    [Header("UI")]
    public TMP_Text whiteTimeText;
    public TMP_Text blackTimeText;

    private float whiteTime;
    private float blackTime;

    private bool isRunning = false;

    void Start()
    {
        ResetClock();
    }

    void Update()
    {
        if (!isRunning) return;

        TeamColor turn =
            GameContext.Instance.GameMode is OnlineGameMode
                ? OnlineTurnManager.Instance.CurrentTurn
                : TurnManager.Instance.currentTurn;

        if (turn == TeamColor.White)
            whiteTime -= Time.deltaTime;
        else
            blackTime -= Time.deltaTime;

        if (whiteTime <= 0)
            TimeUp(TeamColor.White);
        if (blackTime <= 0)
            TimeUp(TeamColor.Black);

        UpdateUI();
    }


    void TimeUp(TeamColor loser)
    {
        isRunning = false;

        CommonPopupController.Instance.ShowResult(
            loser == TeamColor.White ? "Black Wins (Time)" : "White Wins (Time)"
        );
    }

    public void StartClock()
    {
        isRunning = true;
    }

    public void StopClock()
    {
        isRunning = false;
    }

    public void ResetClock()
    {
        whiteTime = initialTime;
        blackTime = initialTime;
        UpdateUI();
        isRunning = true;
    }

    void UpdateUI()
    {
        whiteTimeText.text = FormatTime(whiteTime);
        blackTimeText.text = FormatTime(blackTime);
    }

    string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        return $"{min:00}:{sec:00}";
    }
}
