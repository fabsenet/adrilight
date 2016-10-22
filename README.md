# adrilight
An Ambilight clone for Windows based sources - HTPC or just a normal PC

# what you can expect

The project is capable to control a full color LED strip based on the screen content of a Windows system. It can deliver full 60 fps on a reasonable HTPC (2GHz CPU) with using only roughly 10% CPU. 

# how all fits together

The expected setup is a Windows PC providing the screen content for your television. The PC also runs adrilight which will analyze the screen content, calculate the color and brightness of each led and sends it over USB to an arduino. This arduino runs the integrating software which receives the color signals from USB and controls an LED light strip.  
The solution expects your LED strip to be around the edges of the television. A light strip with 228 LEDs (WS2812b) is known to work happily with this setup.

    PC (adrilight.exe) => arduino (adrilight.ino) => LEDs


# Installation
This should be improved but a quick rundown on what to do:

* Buy
  * Arduino UNO (or compatable)
  * WS2812b LED strip in the needed length
  * 12V DC power supply with enough power
  * (optionally) dc jack connector
* Build
  * Attach LED strip on the back of the television. The arrows should build a circle! Left and right as well as top and bottom should have the same number of LEDs each.
  * solder the edges together, but do not make a complete circle. The data line should still have an electrical start and end point
  * depending on the length of the strip, solder more power wires in between
  * connect power with the strip
  * connect the arduino with the data in and ground from the strip
* Software setup
  * download arduino IDE 
  * add FastLED module
  * configure the settings at the top of the script
  * program the arduino
  * start adrilight.exe and setup the same config
* *enjoy ambient lighting!*

# White balance

How to figure out what numbers to enter?! **TODO**

# Possible future features
The following list of things is more a list to not forget things. If something is on here, it does not necessarily mean, it will ever be developed.

* White balance (provide a way to align LEDs with TV white balance)
* better support for high dpi / font scaling
* WPF based modern UI
* Better logging
* Better preview (currently the preview is very slow and flickering)
* handling of letterboxed video (should not be confused by video containing partial black content for a bunch of seconds)

You have another idea for a feature? Please create an issue.


# Changelog

in order of newest changes first:

* Converted screen capturing to use the Desktop Duplication API (heavily based on the [sample code from jasonpang](https://github.com/jasonpang/desktop-duplication-net)) 
* initally forked from [bambilight by MrBoe](https://github.com/MrBoe/Bambilight) because of missing Windows 10 support