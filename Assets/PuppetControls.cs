using UnityEngine;
using System;

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

        GUI.Label(GetControlRect(++controlRectIdx), "Choose an exception type");

        if (GUI.Button(GetControlRect(++controlRectIdx), "Crash"))
        {
		}
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
