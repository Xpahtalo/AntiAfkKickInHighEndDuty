using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace AntiAfkKickInHighEndDuty;

internal class Svc
{
    [PluginService] internal static DalamudPluginInterface PluginInterface { get; set; } = null!;
    [PluginService] internal static IClientState           ClientState     { get; set; } = null!;
    [PluginService] internal static IDataManager           DataManager     { get; set; } = null!;
    [PluginService] internal static IFramework             Framework       { get; set; } = null!;
    [PluginService] internal static ISigScanner            SigScanner      { get; set; } = null!;
    [PluginService] internal static IGameInteropProvider   Hook            { get; set; } = null!;
    [PluginService] internal static IPluginLog             Log             { get; set; } = null!;
}
