using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PickerController : MonoBehaviour
{
    [SerializeField]
    private FilePickerBehaviour filePicker;

    [SerializeField]
    private GameObject textBinaryAttachmentObject;

    [SerializeField]
    private GameObject pickerBinaryAttachmentObject;

    [SerializeField]
    private InputField textBinaryInput;

    private static string binaryAttachmentPath;
    private static bool usePicker;

    public void Awake()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR        textBinaryAttachmentObject.SetActive(false);
        pickerBinaryAttachmentObject.SetActive(true);
        usePicker = true;
#else        textBinaryAttachmentObject.SetActive(true);
        pickerBinaryAttachmentObject.SetActive(false);
        usePicker = false;
#endif

        filePicker.Completed += OnFilePicked;
    }

    public void OnPressShowPicker()
    {
        filePicker.Show();
    }

    private void OnFilePicked(string filePath)
    {
        binaryAttachmentPath = filePath;
    }

    private byte[] ParseBytes(string bytesString)    {        string[] bytesArray = bytesString.Split(' ');        if (bytesArray.Length == 0)        {            return new byte[] { 100, 101, 102, 103 };        }        byte[] result = new byte[bytesArray.Length];        int i = 0;        foreach (string byteString in bytesArray)        {            byte parsed;            bool isParsed = Byte.TryParse(bytesString, out parsed);            if (isParsed)            {                result[i] = parsed;            }            else            {                result[i] = 0;            }        }        return result;    }

    public static byte[] GetAttachmentBytes()
    {
        if (usePicker && !string.IsNullOrEmpty(binaryAttachmentPath))
        {
            return File.ReadAllBytes(binaryAttachmentPath);
        }

        return ParseBytes(PlayerPrefs.GetString(PuppetAppCenter.BinaryAttachmentKey));
    }
}
