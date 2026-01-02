using System.Collections;
using UnityEngine;

public class PieceAnimator
{
    public static IEnumerator MoveTo(RectTransform piece, RectTransform target, float duration = 0.2f)
    {
        Vector2 startPos = piece.position;
        Vector2 endPos = target.position;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            piece.position = Vector2.Lerp(startPos, endPos, time);
            yield return null;
        }

        piece.position = endPos;
    }

    public static IEnumerator CaptureEffect(RectTransform piece, float duration = 0.15f)
    {
        float time = 0f;
        Vector3 startScale = piece.localScale;

        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            piece.localScale = Vector3.Lerp(startScale, Vector3.zero, time);
            yield return null;
        }
    }
}
