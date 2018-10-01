# Downloads and installs Unity for CI.
# Based on https://blog.bitrise.io/unity-for-the-win-on-bitrise-too.
# Note: Registering and unregistering is handled by build.cake.

# Unity tool download URLs.
# Version 2017.3.0f3
EDITOR_URL="https://download.unity3d.com/download_unity/ae1180820377/MacEditorInstaller/Unity-2018.2.8f1.pkg"
ANDROID_URL="https://download.unity3d.com/download_unity/ae1180820377/MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-2018.2.8f1.pkg"
IOS_URL="https://download.unity3d.com/download_unity/ae1180820377/MacEditorTargetInstaller/UnitySetup-iOS-Support-for-Editor-2018.2.8f1.pkg"

# Put downloads in a temporary folder
TEMP_FOLDER="UnityTempDownloads"
mkdir $TEMP_FOLDER

# Download locations
UNITY_EDITOR="$TEMP_FOLDER/unity.pkg"
ANDROID_SUPPORT="$TEMP_FOLDER/android.pkg"
IOS_SUPPORT="$TEMP_FOLDER/ios.pkg"

# Download/install Unity editor
curl -o $UNITY_EDITOR $EDITOR_URL
sudo -S installer -package $UNITY_EDITOR -target / -verbose

# Download/install Android support
curl -o $ANDROID_SUPPORT $ANDROID_URL
sudo -S installer -package $ANDROID_SUPPORT -target / -verbose

# Download/install iOS support
curl -o $IOS_SUPPORT $IOS_URL
sudo -S installer -package $IOS_SUPPORT -target / -verbose

# Remove downloads
rm -rf $TEMP_FOLDER
