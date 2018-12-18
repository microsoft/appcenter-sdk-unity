using Microsoft.AppCenter.Unity.Crashes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorReportConverter
{
    public static ErrorReport Convert(AndroidJavaObject errorReport)
    {
        var report = new ErrorReport(
            "id",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            new Microsoft.AppCenter.Unity.Crashes.Models.Exception("condition", "stack trace"),
            1,
            "reporter key",
            "reporter signal",
            true);
        return report;
    }
}
