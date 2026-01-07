using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SplashAnimator : MonoBehaviour
{
    [Header("UI")]
    public Image logo;
    public Image glowInner;
    public Image glowOuter;
    public Image footer;

    [Header("Timing")]
    public float totalDuration = 3.5f;

    [Header("Pulse")]
    public float pulseMin = 0.9f;
    public float pulseMax = 1.05f;
    public float pulseSpeed = 1.2f;

    AudioSource audioSource;

    Coroutine logoPulseRoutine;
    Coroutine glowInnerRoutine;
    Coroutine glowOuterRoutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PrepareInitialState();
        StartCoroutine(PlaySplash());
    }

    void PrepareInitialState()
    {
        SetAlpha(logo, 0);
        SetAlpha(glowInner, 0);
        SetAlpha(glowOuter, 0);
        SetAlpha(footer, 0);

        logo.transform.localScale = Vector3.one * 0.9f;
    }

    IEnumerator PlaySplash()
    {
        // ================= GLOWS =================
        glowOuterRoutine = StartCoroutine(GlowBreath(glowOuter, 1.05f));
        glowInnerRoutine = StartCoroutine(GlowBreath(glowInner, 1.03f));

        StartCoroutine(Rotate(glowOuter.transform, 25f));
        StartCoroutine(Rotate(glowInner.transform, -35f));

        yield return Fade(glowOuter, 0.5f);
        yield return Fade(glowInner, 0.5f);

        // ================= LOGO =================
        yield return Fade(logo, 0.4f);

        // 🔊 Play splash sound here
        audioSource.Play();

        logoPulseRoutine = StartCoroutine(LogoPulse());

        // ================= FOOTER =================
        yield return Fade(footer, 0.35f);

        // ================= WAIT =================
        yield return new WaitForSeconds(totalDuration - 1.5f);

        // ================= EXIT =================
        StopAllLoops();

        yield return Fade(logo, 0.25f, true);
        yield return Fade(glowInner, 0.25f, true);
        yield return Fade(glowOuter, 0.25f, true);
        yield return Fade(footer, 0.25f, true);

        SceneLoader.Instance.LoadScene(SceneLoader.SceneType.MainMenu);

    }

    // ==================================================
    // EFFECTS
    // ==================================================

    IEnumerator LogoPulse()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime * pulseSpeed;
            float scale = Mathf.Lerp(pulseMin, pulseMax, (Mathf.Sin(t) + 1f) / 2f);
            logo.transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    IEnumerator GlowBreath(Image glow, float scaleMax)
    {
        float t = 0;
        RectTransform rt = glow.rectTransform;

        while (true)
        {
            t += Time.deltaTime;
            float sin = (Mathf.Sin(t * 1.5f) + 1f) / 2f;

            Color c = glow.color;
            c.a = Mathf.Lerp(0.4f, 0.8f, sin);
            glow.color = c;

            rt.localScale = Vector3.one * Mathf.Lerp(1f, scaleMax, sin);
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

    IEnumerator Fade(Image img, float duration, bool fadeOut = false)
    {
        float t = 0;
        Color c = img.color;

        float start = fadeOut ? 1f : 0f;
        float end = fadeOut ? 0f : 1f;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            c.a = Mathf.Lerp(start, end, t);
            img.color = c;
            yield return null;
        }
    }

    void StopAllLoops()
    {
        if (logoPulseRoutine != null) StopCoroutine(logoPulseRoutine);
        if (glowInnerRoutine != null) StopCoroutine(glowInnerRoutine);
        if (glowOuterRoutine != null) StopCoroutine(glowOuterRoutine);
    }

    void SetAlpha(Image img, float value)
    {
        Color c = img.color;
        c.a = value;
        img.color = c;
    }
}
