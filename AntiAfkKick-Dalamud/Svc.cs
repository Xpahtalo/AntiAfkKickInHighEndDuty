using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace AntiAfkKick;

internal class Svc
{
    [PluginService] internal static DalamudPluginInterface PluginInterface { get; }
    [PluginService] internal static IBuddyList             Buddies         { get; }
    [PluginService] internal static IChatGui               Chat            { get; }
    [PluginService] internal static IClientState           ClientState     { get; }
    [PluginService] internal static ICommandManager        Commands        { get; }
    [PluginService] internal static ICondition             Condition       { get; }
    [PluginService] internal static IDataManager           Data            { get; }
    [PluginService] internal static IFateTable             Fates           { get; }
    [PluginService] internal static IFlyTextGui            FlyText         { get; }
    [PluginService] internal static IFramework             Framework       { get; }
    [PluginService] internal static IGameGui               GameGui         { get; }
    [PluginService] internal static IGameNetwork           GameNetwork     { get; }
    [PluginService] internal static IJobGauges             Gauges          { get; }
    [PluginService] internal static IKeyState              KeyState        { get; }
    [PluginService] internal static ILibcFunction          LibcFunction    { get; }
    [PluginService] internal static IObjectTable           Objects         { get; }
    [PluginService] internal static IPartyFinderGui        PfGui           { get; }
    [PluginService] internal static IPartyList             Party           { get; }
    [PluginService] internal static ISigScanner            SigScanner      { get; }
    [PluginService] internal static ITargetManager         Targets         { get; }
    [PluginService] internal static IToastGui              Toasts          { get; }
    [PluginService] internal static IGameInteropProvider   Hook            { get; }
    [PluginService] internal static IPluginLog             Log             { get; }
}
