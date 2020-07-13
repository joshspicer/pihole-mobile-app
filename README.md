# pihole-dashboard

[![Build status](https://build.appcenter.ms/v0.1/apps/35dc5804-64c0-441f-adee-04ccfb1cdd2e/branches/master/badge)](https://appcenter.ms)

A simple [Xamarin Forms Shell](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/shell/) app to manage your local [Pi-hole](https://pi-hole.net/) instance. 

The idea of this app is to provide "quick toggles" and display information that would be useful at a glance. My goal is *not* to reach parity with the (awesome) web app that already exists.  Use cases include:

1. Allow less technically inclined/interested people to disable your home's pi-hole temporarily.
2. Ensure your pi-hole instance is up and running.
3. Check that a given device is routing through pi-hole.

Feel free to [open an issue](https://github.com/joshspicer/pihole-mobile-app/issues) for any ideas/concerns, after checking out the [feature road map](https://github.com/joshspicer/pihole-mobile-app/projects/1).  **Pull requests are welcomed and encouraged!**

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

### Note

You will need an API key for anything in that requires authorization. For more details check [here](./help.md). 

The Pi-Hole web UI's generated API key can be found here:  `Settings > API/Web Interface > Show API Token`.  The app also lets you scan the QR code that pi-hole generates.

One final place you can find the key is as the `WEBPASSWORD` found under `/etc/pihole/setupVars.conf` 

The API utilized is outlined [here](https://discourse.pi-hole.net/t/pi-hole-api/1863).

## Development

Please feel free to push feature/bug PRs!  I'll merge it into `dev` branch, which automatically builds and releases a TestFlight build via AppCenter.  _If you'd like to join the experimental Testflight channel, [contact me](http://joshspicer.com/contact)!_

Once we agree everything is working on iOS, i'll merge to master, and a new App Store version will be released.  


## License
<a rel="license" href="http://creativecommons.org/licenses/by/3.0/"><img alt="Creative Commons License" style="border-width:0" src="https://i.creativecommons.org/l/by/3.0/88x31.png" /></a><br />This work is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by/3.0/">Creative Commons Attribution 3.0 Unported License</a>.

This work is licensed under the Creative Commons Attribution 3.0 Unported License. To view a copy of this license, visit http://creativecommons.org/licenses/by/3.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
