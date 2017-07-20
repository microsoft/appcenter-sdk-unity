// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using System;
using System.Threading;

public class PuppetControls : MonoBehaviour
{
    public GUISkin customUISkin;
    private int controlHeight = 64;
    private int horizontalMargin = 20;
    private int space = 20;

    void OnGUI()
    {
        AutoResize(640, 1136);
        GUI.skin = customUISkin;

        int controlRectIdx = 0;

        GUI.Label(GetControlRect(++controlRectIdx), "Choose an action");
        
        if (GUI.Button(GetControlRect(++controlRectIdx), "Divide by zero"))
        {
            CrashWithDivideByZeroException();
        }

        if (GUI.Button(GetControlRect(++controlRectIdx), "Null reference"))
        {
            CrashWithNullReferenceException();
        }

        if (GUI.Button(GetControlRect(++controlRectIdx), "Track event"))
        {
            Microsoft.Azure.Mobile.Unity.Analytics.Analytics.TrackEvent("hi");
        }
    }

    private void CrashWithDivideByZeroException()
    {
        print(3 / int.Parse("0"));
    }

    private void CrashWithNullReferenceException()
    {
        string str = null;
        print(str.Length);
    }

    public void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }

    private Rect GetControlRect(int controlIndex)
    {
        return new Rect(horizontalMargin,
                        controlIndex * (controlHeight + space),
                        640 - (2 * horizontalMargin),
                        controlHeight);
    }
}
