using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileHighlight : Singleton<TileHighlight>
{
    public Image selectedTileHighlight;
    public Image moveHighlight;
    public Image captureHighlight;

    [Header("Last Move")]
    public Image lastMoveHighlight;   // 👈 NEW PREFAB

    private List<Image> activeMarks = new List<Image>();
    private List<Image> lastMoveMarks = new List<Image>(); // 👈 NEW

    public void HighlightSelectedTile(Tile tile)
    {
        CreateMark(tile, selectedTileHighlight, activeMarks);
    }

    public void HighlightMoveTile(Tile tile)
    {
        CreateMark(tile, moveHighlight, activeMarks);
    }

    public void HighlightCaptureTile(Tile tile)
    {
        CreateMark(tile, captureHighlight, activeMarks);
    }

    // ================= LAST MOVE =================

    public void HighlightLastMove(Tile fromTile, Tile toTile)
    {
        ClearLastMoveHighlight();

        CreateMark(fromTile, lastMoveHighlight, lastMoveMarks);
        CreateMark(toTile, lastMoveHighlight, lastMoveMarks);
    }

    public void ClearLastMoveHighlight()
    {
        foreach (var mark in lastMoveMarks)
        {
            Destroy(mark.gameObject);
        }
        lastMoveMarks.Clear();
    }

    // ================= CORE =================

    void CreateMark(Tile tile, Image prefab, List<Image> list)
    {
        Image mark = Instantiate(prefab, tile.transform);
        RectTransform rt = mark.rectTransform;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        list.Add(mark);
    }

    public void ClearAll()
    {
        foreach (var mark in activeMarks)
        {
            Destroy(mark.gameObject);
        }
        activeMarks.Clear();
    }
}
