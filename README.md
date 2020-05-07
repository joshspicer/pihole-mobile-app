# pihole-dashboard
A simple [Xamarin Forms Shell](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/shell/) app to manage your local [Pi-hole](https://pi-hole.net/) instance. 

Feel free to [open an issue](https://github.com/joshspicer/pihole-mobile-app/issues) for any ideas/concerns, after checking out the [feature road map](https://github.com/joshspicer/pihole-mobile-app/projects/1).

<kbd>
  <img width=250 src="Screenshots/1.png">
</kbd>
<kbd>
  <img width=250 src="Screenshots/2.png">
</kbd>
<kbd>
  <img width=250 src="Screenshots/3.png">
</kbd>

## Usage

### App Store (iPhone)

This app is available on the iOS app store under the name [Pi ContrHOLE](https://apps.apple.com/us/app/pi-contrhole/id1507963158).  

### Build yourself (iPhone/Android)

Download, [Visual Studio 2019 (Mac/Windows)](https://visualstudio.microsoft.com/), open the `.sln` file, and build for the appropriate device (ios/android).

## Note

You will need the `WEBPASSWORD` found under `/etc/pihole/setupVars.conf` for anything in "Settings".

The API token for this app is NOT the one found in the web GUI, nor is it your login password.
