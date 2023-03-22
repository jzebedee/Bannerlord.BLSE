﻿using Bannerlord.BLSE.Features.AssemblyResolver;
using Bannerlord.BLSE.Features.Commands;
using Bannerlord.BLSE.Features.ContinueSaveFile;
using Bannerlord.BLSE.Features.Interceptor;
using Bannerlord.BLSE.Features.Xbox;
using Bannerlord.BLSE.Shared.NoExceptions;
using Bannerlord.LauncherEx;

using HarmonyLib;

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Bannerlord.BLSE.Shared;

public static class LauncherEx
{
    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    private static readonly Harmony _featureHarmony = new("bannerlord.blse.features");

    public static void Launch(string[] args)
    {
        if (Environment.OSVersion.Version.Major >= 6)
            SetProcessDPIAware();

        InterceptorFeature.Enable(_featureHarmony);
        AssemblyResolverFeature.Enable(_featureHarmony);
        ContinueSaveFileFeature.Enable(_featureHarmony);
        CommandsFeature.Enable(_featureHarmony);
        XboxFeature.Enable(_featureHarmony);

        Manager.Initialize();
        Manager.Enable();

        ModuleInitializer.Disable();
        if (args.Contains("/noexceptions"))
        {
            ProgramEx.Main(args);
        }
        else
        {
            TaleWorlds.MountAndBlade.Launcher.Library.Program.Main(args);
        }
    }
}