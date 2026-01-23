using UnityEngine;
using UnityEngine.UI;

public class GlobalLoaderController : Singleton<GlobalLoaderController>
{
    [Header("Loader UI")]
    [SerializeField] private GameObject loaderRoot;
    [SerializeField] private RectTransform chessPiece;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 180f;

    bool isLoading;

    protected override void Awake()
    {
        base.Awake();

        if (loaderRoot != null)
            loaderRoot.SetActive(false);
    }

    void Update()
    {
        if (!isLoading || chessPiece == null)
            return;

        chessPiece.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    // ================= PUBLIC API =================

    public void Show()
    {
        if (loaderRoot == null)
            return;

        loaderRoot.SetActive(true);
        isLoading = true;
    }

    public void Hide()
    {
        if (loaderRoot == null)
            return;

        isLoading = false;
        loaderRoot.SetActive(false);
    }

    public bool IsVisible()
    {
        return isLoading;
    }
}
