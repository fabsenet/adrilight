using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NighlightDetectionModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //var r0egkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate");
            //var r1egkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.bluelightreduction.settings\Current", false);
            //var r2egkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.bluelightreduction.bluelightreductionstate\Current", false);

            var tks = new CancellationTokenSource();
            var nl = new NightLightStatusWatcher(tks.Token);

            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.KeyChar == '+') { nl.AddLast(true); }
                else if (key.KeyChar == '-') { nl.AddLast(false); }
                else if (key.KeyChar == 'w') { nl.WriteToFile(); }
                else if (key.KeyChar == 't') { nl.TrainAndGuess(); }
                else if (key.Key == ConsoleKey.Enter) { return; }
            }
        }

    }


}
