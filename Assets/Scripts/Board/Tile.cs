using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int boardPosition;
    public Image image;
    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClicked);
    }

    void OnTileClicked()
    {
        SelectionManager.Instance.OnTileSelected(this);
    }

    public void Init(int x, int y, Sprite sprite)
    {
        boardPosition = new Vector2Int(x, y);
        image.sprite = sprite;
    }
}
