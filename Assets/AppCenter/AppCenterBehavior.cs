// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity;
using UnityEngine;
using System;
using System.Reflection;
using Microsoft.AppCenter.Unity.Internal;

// TODO Update documentation link
[HelpURL("https://docs.microsoft.com/en-us/mobile-center/sdk/")]
public class AppCenterBehavior : MonoBehaviour
{
    public static event Action InitializingServices;
    public static event Action InitializedAppCenterAndServices;
    public static event Action Started;

    private static AppCenterBehavior instance;

    public AppCenterSettings settings;

    private void Awake()
    {
        // Make sure that App Center have only one instance.
        if (instance != null)
        {
            Debug.LogError("App Center should have only one instance!");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize App Center.
        if (settings == null)
        {
            Debug.LogError("App Center isn't configured!");
            return;
        }
        StartAppCenter();
    }

    private void Start()
    {
        if (Started != null)
        {
            Started.Invoke();
        }
    }

    void OnEnable ()
	{
        #if !UNITY_EDITOR
        System.AppDomain.CurrentDomain.UnhandledException += OnHandleUnresolvedException;
        Application.logMessageReceived += OnHandleLogCallback;
	    #endif
	}
	
	void OnDisable ()
	{
		#if !UNITY_EDITOR
        System.AppDomain.CurrentDomain.UnhandledException -= OnHandleUnresolvedException;
        Application.logMessageReceived -= OnHandleLogCallback;
		#endif
	}

    public void OnHandleLogCallback (string logString, string stackTrace, LogType type)
    {
        #if !UNITY_EDITOR
        foreach (var service in settings.Services)
        {
            var OnHanldeLogMethod = service.GetMethod("OnHandleLog");
            if (OnHanldeLogMethod != null)
            {   
                object[] parametersArray = new object[] { logString, stackTrace, type };
                OnHanldeLogMethod.Invoke(this, parametersArray);
            }
        }	
        #endif
    }

    public void OnHandleUnresolvedException (object sender, UnhandledExceptionEventArgs args)
    {
        #if !UNITY_EDITOR
        foreach (var service in settings.Services)
        {
            var OnHandleUnresolvedExceptionMethod = service.GetMethod("OnHandleUnresolvedException");
            if (OnHandleUnresolvedExceptionMethod != null)
            {
                object[] parametersArray = new object[] { sender, args };
                OnHandleUnresolvedExceptionMethod.Invoke(this, parametersArray);
            }
        }	
        #endif
    }

    private void StartAppCenter()
    {
        var services = settings.Services;
        PrepareEventHandlers(services);
        InvokeInitializingServices();
        AppCenter.SetWrapperSdk();

        // On iOS and Android App Center starting automatically.
        #if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)
        AppCenter.LogLevel = settings.InitialLogLevel;
        if (settings.CustomLogUrl.UseCustomUrl)
        {
            AppCenter.SetLogUrl(settings.CustomLogUrl.Url);
        }
        var appSecret = AppCenter.GetSecretForPlatform(settings.AppSecret);
        var nativeServiceTypes = AppCenter.ServicesToNativeTypes(services);
        AppCenterInternal.Start(appSecret, nativeServiceTypes, services.Length);
        #endif
        InvokeInitializedServices();
    }

    private static void PrepareEventHandlers(Type[] services)
    {
        foreach (var service in services)
        {
            var method = service.GetMethod("PrepareEventHandlers");
            if (method != null)
            {
                method.Invoke(null, null);
            }
        }
    }

    private static void InvokeInitializingServices()
    {
        if (InitializingServices != null)
        {
            InitializingServices.Invoke();
        }
    }

    private static void InvokeInitializedServices()
    {
        if (InitializedAppCenterAndServices != null)
        {
            InitializedAppCenterAndServices.Invoke();
        }
    }
}
