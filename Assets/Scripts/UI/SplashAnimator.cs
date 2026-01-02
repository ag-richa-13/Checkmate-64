using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashAnimator : MonoBehaviour
{
    public Image logo;
    public Image glowInner;
    public Image glowOuter;
    public Image footer;

    public float splashDuration = 3f;

    void Start()
    {
        PrepareInitialState();
        StartCoroutine(PlaySplash());
    }

    void PrepareInitialState()
    {
        SetAlpha(logo, 0);
        SetAlpha(glowInner, 0);
        SetAlpha(glowOuter, 0);
        SetAlpha(footer, 0);

        logo.transform.localScale = Vector3.one * 0.3f;
    }

    IEnumerator PlaySplash()
    {
        // Outer glow
        StartCoroutine(Rotate(glowOuter.transform, 30f));
        yield return Fade(glowOuter, 0.6f);

        // Inner glow
        StartCoroutine(Rotate(glowInner.transform, -45f));
        yield return Fade(glowInner, 0.6f);

        // Logo fade + scale (IMPORTANT FIX)
        yield return Fade(logo, 0.4f);
        yield return ScaleUp(logo.transform, 0.3f, 1f, 0.4f);

        // Footer
        yield return Fade(footer, 0.4f);

        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator Fade(Image img, float duration)
    {
        float t = 0;
        Color c = img.color;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            c.a = Mathf.Clamp01(t);
            img.color = c;
            yield return null;
        }
    }

    IEnumerator ScaleUp(Transform target, float from, float to, float duration)
    {
        float t = 0;
        Vector3 start = Vector3.one * from;
        Vector3 end = Vector3.one * to;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            target.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    IEnumerator Rotate(Transform target, float speed)
    {
        while (true)
        {
            target.Rotate(0, 0, speed * Time.deltaTime);
            yield return null;
        }
    }

    void SetAlpha(Image img, float value)
    {
        Color c = img.color;
        c.a = value;
        img.color = c;
    }
}
