using UnityEngine;
using UnityEngine.UI;

public class MatchController : Singleton<MatchController>
{
    public ChessClock chessClock;
    [SerializeField] private Button drawButton;
    [SerializeField] private Button resignButton;

    protected override void Awake()
    {
        base.Awake();
        drawButton.onClick.AddListener(() =>
        {
            CommonPopupController.Instance.ShowDrawConfirm();
        });
        resignButton.onClick.AddListener(() =>
        {
            CommonPopupController.Instance.ShowResignConfirm();
        });
    }

    public void DeclareDraw()
    {
        chessClock.StopClock();
    }

    public void Resign(TeamColor resigningPlayer)
    {
        chessClock.StopClock();
    }
}
