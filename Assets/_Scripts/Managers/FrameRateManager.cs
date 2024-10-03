using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : StaticInstance<FrameRateManager>
{
    [Header("Frame Settings")]
    [SerializeField] private int _frameRate = 60;

    private static float _deltaTime;

    private int _fps;
    private float _currentFrameTime;

    protected override void Awake()
    {
        base.Awake();

        SetFPS(_frameRate);
    }

    public static void SetFPS(int fps)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fps;
        //Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    public static string FPS()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;

        return Mathf.Ceil(fps).ToString();
    }

    private void OnGUI()
    {
        string content = FPS();

        GUILayout.Label($"<size=40>State: {content}</size>");
    }
}
