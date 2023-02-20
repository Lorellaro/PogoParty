using System;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public static Screenshot Instance;
    private string path;
    

    private void Awake()
    {
        Instance = this;
    }

    public void TakeScreenshot(int size)
    {
        path = Application.persistentDataPath;
        path += "/screenshot ";
        path += Guid.NewGuid() + ".png";

        Debug.Log(path);
        Debug.Log(size);
        
        ScreenCapture.CaptureScreenshot(path, size);
    }
}
