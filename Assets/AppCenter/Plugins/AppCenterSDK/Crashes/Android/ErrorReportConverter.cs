using Microsoft.AppCenter.Unity;
using Microsoft.AppCenter.Unity.Crashes;
using Microsoft.AppCenter.Unity.Crashes.Models;
using Microsoft.AppCenter.Unity.Internal.Utility;
using System.Text;
using UnityEngine;

public class ErrorReportConverter
{
    public static ErrorReport Convert(AndroidJavaObject errorReport)
    {
        if (errorReport == null)
        {
            return null;
        }
        var id = errorReport.Call<string>("getId");
        var threadName = errorReport.Call<string>("getThreadName");
        var startTime = JavaDateHelper.DateTimeConvert(errorReport.Call<AndroidJavaObject>("getAppStartTime"));
        var errorTime = JavaDateHelper.DateTimeConvert(errorReport.Call<AndroidJavaObject>("getAppErrorTime"));
        var exception = ConvertException(errorReport.Call<AndroidJavaObject>("getThrowable"));
        var device = ConvertDevice(errorReport.Call<AndroidJavaObject>("getDevice"));
        return new ErrorReport(id, startTime, errorTime, exception, device, threadName, 0, string.Empty, string.Empty, false);
    }

    private static Exception ConvertException(AndroidJavaObject throwable)
    {
        var message = throwable.Call<string>("toString");
        var stackTrace = throwable.Call<AndroidJavaObject[]>("getStackTrace");
        var stackTraceString = new StringBuilder();
        foreach (var element in stackTrace)
        {
            stackTraceString.AppendLine("at " + element.Call<string>("toString"));
        }
        return new Exception(message, stackTraceString.ToString().TrimEnd());
    }

    private static Device ConvertDevice(AndroidJavaObject device)
    {
        return new Device(
            device.Call<string>("getSdkName"),
            device.Call<string>("getSdkVersion"),
            device.Call<string>("getModel"),
            device.Call<string>("getOemName"),
            device.Call<string>("getOsName"),
            device.Call<string>("getOsVersion"),
            device.Call<string>("getOsBuild"),
            GetIntValue(device, "getOsApiLevel"),
            device.Call<string>("getLocale"),
            GetIntValue(device, "getTimeZoneOffset"),
            device.Call<string>("getScreenSize"),
            device.Call<string>("getAppVersion"),
            device.Call<string>("getCarrierName"),
            device.Call<string>("getCarrierCountry"),
            device.Call<string>("getAppBuild"),
            device.Call<string>("getAppNamespace"));
    }

    private static int GetIntValue(AndroidJavaObject javaObject, string getterName)
    {
        var integer = javaObject.Call<AndroidJavaObject>(getterName);
        return integer.Call<int>("intValue");
    }
}
