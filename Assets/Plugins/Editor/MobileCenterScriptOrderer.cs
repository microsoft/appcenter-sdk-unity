﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections.Generic;
using UnityEditor;

[InitializeOnLoad]
public class MobileCenterScriptOrderer
{
    private static List<string> MobileCenterFeatureScripts = new List<string> {
        "MobileCenterAnalyticsBehavior",
        "MobileCenterDistributeBehavior",
        "MobileCenterPushBehavior",
        "MobileCenterCrashesBehavior" };
 
    private static string MobileCenterCoreScript = "MobileCenterCoreBehavior";

    static MobileCenterScriptOrderer()
    {
        MonoScript firstMobileCenterScript = null;
        MonoScript mobileCenterCoreScript = null;

        // Identify which Mobile Center behavior script runs earliest
        foreach (var script in MonoImporter.GetAllRuntimeMonoScripts())
        {
            if (script.name == MobileCenterCoreScript)
            {
                mobileCenterCoreScript = script;
                continue;
            }
            if (!MobileCenterFeatureScripts.Contains(script.name))
            {
                continue;
            }
            if (firstMobileCenterScript == null)
            {
                firstMobileCenterScript = script;
            }
            else if (firstMobileCenterScript.ExecutesAfter(script))
            {
                firstMobileCenterScript = script;
            }
        }

        // If the earliest MC script runs before the core script, swap their orders
        if (firstMobileCenterScript != null &&
            mobileCenterCoreScript != null)
        {
            var featurePos = MonoImporter.GetExecutionOrder(firstMobileCenterScript);
            var corePos = MonoImporter.GetExecutionOrder(mobileCenterCoreScript);

            if (corePos < featurePos)
            {
                return;
            }

            if (corePos == featurePos)
            {
                corePos--;
            }
            else if (corePos > featurePos)
            {
                var tmp = featurePos;
                featurePos = corePos;
                corePos = tmp;
            }

            MonoImporter.SetExecutionOrder(firstMobileCenterScript, featurePos);
            MonoImporter.SetExecutionOrder(mobileCenterCoreScript, corePos);
        }
    }
}

public static class MobileCenterMonoScriptExtension
{
    public static bool ExecutesAfter(this MonoScript @this, MonoScript other)
    {
        return MonoImporter.GetExecutionOrder(@this) >
                    MonoImporter.GetExecutionOrder(other);
    }
}
