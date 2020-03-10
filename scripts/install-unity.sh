# Downloads and installs Unity for CI.
# Based on https://blog.bitrise.io/unity-for-the-win-on-bitrise-too.
# Note: Registering and unregistering is handled by build.cake.

# Unity tool download URLs.
EDITOR_URL="$EDITOR_URL"
ANDROID_URL="$ANDROID_SUPPORT_URL"
IOS_URL="$IOS_SUPPORT_URL"

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
