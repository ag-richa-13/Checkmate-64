using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    private Color NORMAL;
    private Color CHECK;
    private Color CHECKMATE;
    private Color STALEMATE;

    Coroutine statusRoutine;

    [Header("Footer")]
    public TMP_Text turnText;
    public TMP_Text statusText;

    [Header("Captured Pieces")]
    public Transform whiteCapturedGrid;
    public Transform blackCapturedGrid;
    public GameObject capturedPiecePrefab;

    [Header("Result Popup")]
    public GameObject resultPopup;
    public TMP_Text resultText;
    public Button restartButton;

    protected override void Awake()
    {
        base.Awake();

        NORMAL = Hex("#9CA3AF");
        CHECK = Hex("#F97316");
        CHECKMATE = Hex("#DC2626");
        STALEMATE = Hex("#6B7280");

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
        });
    }

    void Start()
    {
        ResetUI();
    }

    #region TURN & STATUS

    public void UpdateTurnUI(TeamColor turn)
    {
        turnText.text = turn == TeamColor.White ? "White to Move" : "Black to Move";
    }

    public void ShowCheck()
    {
        SetStatus("Check", CHECK, true);
    }

    public void ShowCheckmate()
    {
        SetStatus("Checkmate", CHECKMATE, false);
    }

    public void ShowStalemate()
    {
        SetStatus("Stalemate", STALEMATE, false);
    }

    public void ClearStatus()
    {
        if (statusRoutine != null)
            StopCoroutine(statusRoutine);

        statusText.text = "";
    }

    void SetStatus(string msg, Color col, bool autoClear)
    {
        if (statusRoutine != null)
            StopCoroutine(statusRoutine);

        statusText.text = msg;
        statusText.color = col;

        if (autoClear)
            statusRoutine = StartCoroutine(AutoClear());
    }

    IEnumerator AutoClear()
    {
        yield return new WaitForSeconds(2f);
        statusText.text = "";
    }

    #endregion

    #region CAPTURED PIECES

    public void AddCapturedPiece(Piece piece)
    {
        Transform grid =
            piece.teamColor == TeamColor.White
            ? blackCapturedGrid
            : whiteCapturedGrid;

        GameObject icon = Instantiate(capturedPiecePrefab, grid);
        icon.GetComponent<Image>().sprite = piece.image.sprite;
    }

    #endregion

    #region RESULT

    public void ShowGameEnd(string result)
    {
        resultPopup.SetActive(true);
        resultText.text = result;
    }

    public void ResetUI()
    {
        statusText.text = "";
        statusText.color = NORMAL;

        turnText.text = "White to Move";

        resultPopup.SetActive(false);
        resultText.text = "";

        ClearGrid(whiteCapturedGrid);
        ClearGrid(blackCapturedGrid);
    }

    void ClearGrid(Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
            Destroy(t.GetChild(i).gameObject);
    }

    #endregion

    Color Hex(string h)
    {
        ColorUtility.TryParseHtmlString(h, out Color c);
        return c;
    }
}
