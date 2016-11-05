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
  * Number of LEDs in arduino code differs from the number of spots in the app!!
* *enjoy ambient lighting!*

# White balance

How to figure out what numbers to enter?! **TODO**

# What is my maximum LED framerate?

There are 3 tasks which take time and might limit the framerate of the LEDs:
* **Analyzing the screen content:** 
The process is optimized and runs in parallel to the other two tasks and as such should be fairly capable to meet your screen refresh rate of most probably 60 fps at minor CPU usage. 
* **Transmitting the data to the arduino:** The USB simulates a serial port and this is currently using a baudrate of 115200 Baud (=Bit/s). Each frame has a size of `10+number_of_LEDs*3 Byte`. Considering an example installation of  228 LEDs, the data package has a size of 694 Byte or 5552 Bit. Transmitting this data takes `5552/8*10/115200 = 60.2ms`. **New:** the Baudrate was changed to 1000000 Baud and it seems to be rock stable so far. The example of 228 LEDs now only needs `6.94ms` to transfer the data to the arduino.
* **The arduino transmitting the data to the LEDs:** The data protocol of WS2812b LED strips needs 0.030ms per LED to transmit the color data. In the given example of 228 LEDs, doing this takes `6.84ms`.

The strict timing requirements of the WS2812b protocol require the arduino code to either receive serial data *or* transmit LED data. Therefore, a single frame in our example needs at least `60.2ms+6.84ms = 67ms`. So the maximum framerate for this setup is (only) `1000ms/67ms = 14.9 fps`.

**New:** The changed boudrate needs only `6.94ms+6.84ms = 13.8ms` per frame. So with this change, this setup is capable of up to `1000ms/13.8ms= 72fps` 

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

* Upped the serial connection speed to allow a more fluent experience
* the ambilight goes dark (instead of back to the animation mode) after the connection to the pc is lost (for example when in goes into standby)
* Performance work in how the color of each spot is detected
* Converted screen capturing to use the Desktop Duplication API (heavily based on the [sample code from jasonpang](https://github.com/jasonpang/desktop-duplication-net)) 
* initally forked from [bambilight by MrBoe](https://github.com/MrBoe/Bambilight) because of missing Windows 10 support


# Thanks

* This is a fork from the originally ambilight clone project [bambilight by MrBoe](https://github.com/MrBoe/Bambilight) and therefore (and to met the MIT licence) a big thank you goes to [MrBoe](https://github.com/MrBoe)
* More thanks goes to [jasonpong](https://github.com/jasonpang) for his [sample code for the Desktop Duplication API](https://github.com/jasonpang/desktop-duplication-net)