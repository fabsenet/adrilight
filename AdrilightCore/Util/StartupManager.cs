using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Principal;

public class StartUpManager
{
    private const string ApplicationName = "adrilight";

    public static void AddApplicationToCurrentUserStartup()
    {
        using var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        key?.SetValue(ApplicationName, $"\"{GetExePath()}\"");
    }

    private static string GetExePath()
    {
        //via the current process
        var adrilightExe = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
        if(adrilightExe != null) return adrilightExe;

        //alternative
        adrilightExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
        if (adrilightExe.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            adrilightExe = Path.ChangeExtension(adrilightExe, ".exe");
        }
        return adrilightExe;
    }

    public static void AddApplicationToAllUserStartup()
    {
        using var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        key?.SetValue(ApplicationName, $"\"{GetExePath()}\"");
    }

    public static void RemoveApplicationFromCurrentUserStartup()
    {
        using var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        key?.DeleteValue(ApplicationName, false);
    }

    public static void RemoveApplicationFromAllUserStartup()
    {
        using var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        key?.DeleteValue(ApplicationName, false);
    }

    public static bool IsUserAdministrator()
    {
        //bool value to hold our return value
        bool isAdmin;
        try
        {
            //get the currently logged in user
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch (UnauthorizedAccessException)
        {
            isAdmin = false;
        }
        catch (Exception)
        {
            isAdmin = false;
        }
        return isAdmin;
    }
}