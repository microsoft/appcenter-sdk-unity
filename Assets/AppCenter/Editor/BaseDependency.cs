Public abstract class DependencyResolver {
    public MethodName() {

        Type versionHandler = Type.GetType("Google.VersionHandler, Google.VersionHandler, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null");
        if (versionHandler == null)
        {
            Debug.LogError("Unable to set up Android dependencies, class `Google.VersionHandler` is not found");
            return;
        }
        Type playServicesSupport = (Type)versionHandler.InvokeMember("FindClass", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport"
        });
        if (playServicesSupport == null)
        {
            Debug.LogError("Unable to set up Android dependencies, class `Google.JarResolver.PlayServicesSupport` is not found");
            return;
        }
        // Shared code
    }
}

public class FirebaseResolver extends DependencyResolver {

    @Override
    public SetupDependencies() {
        base.SetupDependencies();

  object svcSupport = versionHandler.InvokeMember("InvokeStaticMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            playServicesSupport, "CreateInstance", new object[] { "FirebaseMessaging", EditorPrefs.GetString("AndroidSdkRoot"), "ProjectSettings" }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.google.firebase", "firebase-messaging", FirebaseMessagingVersion },
            new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository", "extra-android-m2repository" } },
                { "repositories", null }
            }
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn",
            new object[] { "com.google.firebase", "firebase-core", FirebaseCoreVersion },
            new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository", "extra-android-m2repository" } },
                { "repositories", null }
            }
        });
        // Unique code
    }
}

Public class MsalResolver extends DependencyResolver{

    @Override
    Public SetupDependencies() {
        base.SetupDependencies();
                object svcSupport = versionHandler.InvokeMember("InvokeStaticMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            playServicesSupport, "CreateInstance", new object[] { "Msal", EditorPrefs.GetString("AndroidSdkRoot"), "ProjectSettings" }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.microsoft.identity.client", "msal", MsalVersion }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn",
            new object[] { "com.android.support", "appcompat-v7", SupportLibVersion },
            new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository", "extra-android-m2repository" } },
                { "repositories", null }
            }
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn",
            new object[] { "com.android.support", "customtabs", SupportLibVersion },
            new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository", "extra-android-m2repository" } },
                { "repositories", null }
            }
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.google.code.gson", "gson", GsonVersion }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.microsoft.identity", "common", IdentityCommonVersion }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.nimbusds", "nimbus-jose-jwt", NimbusVersion }, null
        });
    }
}