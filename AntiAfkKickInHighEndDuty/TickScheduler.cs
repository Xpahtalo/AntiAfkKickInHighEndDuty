using System;
using Dalamud.Plugin.Services;
using Action = System.Action;

namespace AntiAfkKickInHighEndDuty;

internal class TickScheduler : IDisposable
{
    private readonly long       executeAt;
    private readonly Action     function;
    private readonly IFramework framework;


    public TickScheduler(Action function, IFramework framework, long delayMS = 0)
    {
        executeAt        =  Environment.TickCount64 + delayMS;
        this.function    =  function;
        this.framework   =  framework;
        framework.Update += Execute;
    }



    public void Dispose()
    {
        framework.Update -= Execute; 
    }

    private void Execute(object _)
    {
        if (Environment.TickCount64 < executeAt) return;
        try {
            function();
        } catch (Exception e) {
            Svc.Log.Error(e.Message + "\n" + e.StackTrace ?? "");
        }

        Dispose();
    }
}
