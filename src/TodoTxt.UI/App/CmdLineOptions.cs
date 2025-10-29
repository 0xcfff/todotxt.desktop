using System.Diagnostics;
using System.Reflection.Metadata;
using CommandLine;

namespace TodoTxt.UI.App;

public class CmdLineOptions
{
    [Option('d', "debug", HelpText = "Enable debug mode", Default = false)]
    public bool IsDebug { get; set; } = false;

    [Option('l', "logfile", HelpText = "Log file name", Default = null)]
    public string? LogFileName { get; set; } = null;


    public CmdLineOptions(string[] args)
    {
        Parser.Default.ParseArguments<CmdLineOptions>(() => this, args);
    }

    public static CmdLineOptions Parse(string[] args) {
        return new CmdLineOptions(args);
    }
    public static CmdLineOptions Parse() {
        var args = Environment.GetCommandLineArgs();
        return new CmdLineOptions(args.Length >= 1 ? args[1..] : args);
    }

}