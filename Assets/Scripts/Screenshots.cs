using System;
using UnityEngine;

public class Screenshots : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ScreenCapture.CaptureScreenshot("screenshot-" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
        }
    }
}
