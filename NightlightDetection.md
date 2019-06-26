# How does Adrilight detect the nightlight state of Windows?

Windows has no API to explicitly ask it about the night light state. [At least the Internet does not know one](https://stackoverflow.com/questions/43340619/get-status-of-night-light-mode-in-windows-10/43953978#43953978).

Depending on your version of Windows, it stores the current state of the night light in the registry either in:

```text
older Windows 10 editons:
\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.bluelightreduction.bluelightreductionstate\Current

new Windows 10 editions:
Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate
```

Either way, this is a huge blob of bytes without any description for decoding it. It may be some kind of a compressed format because even the number of bytes differs a lot.

## Adrilight uses ML.net to solve it

I compiled a list of [observed values with the known state](https://github.com/fabsenet/adrilight/blob/dev/Tools/NighlightDetectionModelGenerator/NighlightDetectionModelGenerator/Data/data.csv). I used [ML.net](https://github.com/dotnet/machinelearning) to train a model and eventually got it working.

If you select `Auto Detect` mode for alternate white balance, adrilight reads the registry values every other second and lets the ML.net model predict the state. This works very well on my test machines, but the data itself are from these machines as well.

## If it does NOT work

### Quick fix

Choose one of the other two modes of `Forced On` or `Forced Off`.

### Provide sample data to smarten up adrilight!

To let adrilight improve over time, more data is needed. If you observe adrilight not correctly detecting the current state of the night light (this can very well be only once!) you should provide the data to the project so we can train the model on them as well to make the next adrilight version even smarter.

What you need to do:

1. Find the `adrilight.log.nightlight.yyyyMMdd.txt` log file(s). Look in the folder `%localappdata%\adrilightApp\app-2.0.9\logs` or the highest version number you have. The real path may look like `c:\Users\fancyUsername\AppData\Local\adrilightApp\app-2.0.9\logs`.
1. [Open up an issue on adrilight](https://github.com/fabsenet/adrilight/issues/new) and upload this file as well (drag and drop on the text field works!)
1. **It is essential** that you provide information whether the night light was **ON or OFF** when the detection failed.
