using UnityEngine;
using UnityEngine.InputSystem;

public class NavigationBack : Singleton<NavigationBack>
{
    public delegate bool BackHandler();
    public static event BackHandler OnBackRequested;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleBack();
        }
    }

    void HandleBack()
    {
        if (OnBackRequested == null)
        {
            Application.Quit();
            return;
        }

        // Call handlers in reverse order (top-most UI first)
        foreach (BackHandler handler in OnBackRequested.GetInvocationList())
        {
            if (handler.Invoke())
                return; // Back was handled
        }

        // Nobody handled it
        Application.Quit();
    }
}
