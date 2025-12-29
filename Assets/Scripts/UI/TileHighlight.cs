using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileHighlight : Singleton<TileHighlight>
{
    public Image selectedTileHighlight;
    public Image moveHighlight;
    public Image captureHighlight;

    private List<Image> activeMarks = new List<Image>();

    public void HighlightSelectedTile(Tile tile)
    {
        CreateMark(tile, selectedTileHighlight);
    }

    public void HighlightMoveTile(Tile tile)
    {
        CreateMark(tile, moveHighlight);
    }

    public void HighlightCaptureTile(Tile tile)
    {
        CreateMark(tile, captureHighlight);
    }

    void CreateMark(Tile tile, Image prefab)
    {
        Image mark = Instantiate(prefab, tile.transform);
        RectTransform rt = mark.rectTransform;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        activeMarks.Add(mark);
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
