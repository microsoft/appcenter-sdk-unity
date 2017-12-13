using UnityEngine;
using UnityEditor;

public class AppCenterSettingsContext : ScriptableObject
{
    private static readonly object SettingsLock = new object();
    private const string SettingsPath = "Assets/AppCenter/AppCenterSettings.asset";
    public static AppCenterSettings SettingsInstance
    {
        get
        {
            lock (SettingsLock)
            {
                var instance = AssetDatabase.LoadAssetAtPath<AppCenterSettings>(SettingsPath);
                if (instance == null)
                {
                    instance = CreateInstance<AppCenterSettings>();
                    AssetDatabase.CreateAsset(instance, SettingsPath);
                    AssetDatabase.SaveAssets();
                }
                return instance;
            }
        }
    }

}
