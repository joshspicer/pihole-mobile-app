# Help

## URI

The Pi-Hole URI is the beginning of whatever you use to connect to the Pi-Hole web interface. For example if you enter: 

`http://192.168.1.200:8181/admin`

to access the web interface, you should enter:

`http://192.168.1.200:8181` into PiContrHOLE.

Ensure you're entering the correct protocol (HTTP/HTTPS) and (if necessary) ports.

You can test the API by visiting:

`http://YOUR_PI_IP/admin/api.php?summary`

If you see data on the screen, your pi-hole is setup correctly.

If the page does not load, please first fix the configuration of your pi-hole and check your firewall settings.

Note that if you are accessing your pi-hole outside of your local network, you must communicate over HTTPS (An iOS rule).

## WEBPASSWORD
Due to pi-hole limitations, this app does NOT support the API key generated from the webapp.

Instead, you'll need to connect to your pi-hole over SSH (or physically), navigate to the:

`/etc/pihole/setupVars.conf` file, and copy the WEBPASSWORD entry.

Enter *just* the part *after* the `WEBPASSWORD=`
