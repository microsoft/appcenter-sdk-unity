using UnityEngine;
using UnityEditor;

public class AppCenterSettingsContext : ScriptableObject
{
    private const string SettingsPath = "Assets/AppCenter/AppCenterSettings.asset";
    public static AppCenterSettings SettingsInstance
    {
        get
        {
            // No need to lock because this can only be accessed from the main thread
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
