using System;
using System.Linq;
using System.Threading;
using AntiAfkKick;
using Dalamud.Hooking;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using Lumina.Excel.GeneratedSheets;
using static AntiAfkKick.Native.Keypress;

namespace AntiAfkKick_Dalamud;

internal unsafe class AntiAfkKickinHighEndDuty : IDalamudPlugin
{
    //long NextKeyPress = 0;
    private           IntPtr        BaseAddress = IntPtr.Zero;
    private           float*        AfkTimer;
    private           float*        AfkTimer2;
    private           float*        AfkTimer3;
    private readonly  Hook<UnkFunc> UnkFuncHook;
    internal volatile bool          running = true;
    public            string        Name => "AntiAfkKickinHighEndDuty";

    private delegate long UnkFunc(IntPtr a1, float a2);

    public void Dispose()
    {
        if (!UnkFuncHook.IsDisposed) {
            if (UnkFuncHook.IsEnabled) UnkFuncHook.Disable();
            UnkFuncHook.Dispose();
        }

        running                          =  false;
        Svc.ClientState.TerritoryChanged -= OnTerritoryChanged;
    }

    public AntiAfkKickinHighEndDuty(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Svc>();
        UnkFuncHook = Svc.Hook.HookFromAddress<UnkFunc>(Svc.SigScanner.ScanText("48 8B C4 48 89 58 18 48 89 70 20 55 57 41 54 41 56 41 57 48 8D 68 A1"), UnkFunc_Dtr);
        UnkFuncHook.Enable();
        try {
            Svc.Log.Information($"RaptureAtkModule: {(IntPtr)Framework.Instance()->GetUiModule()->GetRaptureAtkModule():X16}");
        } catch (Exception e) {
            Svc.Log.Error(e.Message + "\n" + e.StackTrace ?? "");
        }
        Svc.ClientState.TerritoryChanged += OnTerritoryChanged;
    }

    private void BeginWork()
    {
        AfkTimer  = (float*)(BaseAddress + 20);
        AfkTimer2 = (float*)(BaseAddress + 24);
        AfkTimer3 = (float*)(BaseAddress + 28);
        new Thread((ThreadStart)delegate
        {
            while (running)
                try {
                    Svc.Log.Verbose($"Afk timers: {*AfkTimer}/{*AfkTimer2}/{*AfkTimer3}");
                    if (Max(*AfkTimer, *AfkTimer2, *AfkTimer3) > 2f * 60f) {
                        if (Native.TryFindGameWindow(out var mwh)) {
                            Svc.Log.Verbose($"Afk timer before: {*AfkTimer}/{*AfkTimer2}/{*AfkTimer3}");
                            Svc.Log.Verbose($"Sending anti-afk keypress: {mwh:X16}");
                            new TickScheduler(delegate
                            {
                                SendMessage(mwh, WM_KEYDOWN, LControlKey, 0);
                                new TickScheduler(delegate
                                {
                                    SendMessage(mwh, WM_KEYUP, LControlKey, 0);
                                    Svc.Log.Verbose($"Afk timer after: {*AfkTimer}/{*AfkTimer2}/{*AfkTimer3}");
                                }, Svc.Framework, 200);
                            }, Svc.Framework);
                        } else {
                            Svc.Log.Error("Could not find game window");
                        }
                    }

                    Thread.Sleep(10000);
                } catch (Exception e) {
                    Svc.Log.Error(e.Message + "\n" + e.StackTrace ?? "");
                }

            Svc.Log.Debug("Thread has stopped");
        }).Start();
    }

    private long UnkFunc_Dtr(IntPtr a1, float a2)
    {
        BaseAddress = a1;
        Svc.Log.Information($"Obtained base address: {BaseAddress:X16}");
        new TickScheduler(delegate
        {
            if (!UnkFuncHook.IsDisposed) {
                if (UnkFuncHook.IsEnabled) UnkFuncHook.Disable();
                UnkFuncHook.Dispose();
                Svc.Log.Debug("Hook disposed");
            }
            
        }, Svc.Framework);
        return UnkFuncHook.Original(a1, a2);
    }

    public static float Max(params float[] values) => values.Max();
    
    private void OnTerritoryChanged(ushort territoryId)
    {
        if( Svc.DataManager.Excel.GetSheet<TerritoryType>()?.GetRow(territoryId)?.ContentFinderCondition?.Value?.HighEndDuty == true) {
            running = true;
            BeginWork();
        }
        else
            running = false;
        Svc.Log.Information("Territory changed. Running: " + running);
    }
}
