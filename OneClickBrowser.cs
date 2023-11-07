using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Reflection;
using FrooxEngine;
using Elements.Core;
using FrooxEngine.UIX;
using System.Security.Cryptography.X509Certificates;

namespace OneClickBrowser;

public class OneClickBrowser : ResoniteMod
{
    public override string Name => "One Click Browser";
    public override string Author => "EuphieEuphoria";
    public override string Version => "0.0.1";
    public override string Link => "https://github.com/EuphieEuphoria/One-Click-Browser";
    public static ModConfiguration? Config;

    [AutoRegisterConfigKey]

    public static ModConfigurationKey<bool> OneClickBrowserEnabled = new("OneClickBrowserEnabled", "Enable One Click Browser", () => true);

    public override void OnEngineInit()
    {
        Harmony harmony = new("net.You.OneClickBrowser");
        Config = GetConfiguration();
        Config?.Save(true);
        harmony.PatchAll();
    }

    [HarmonyPatch(typeof(ComponentSelector), "BuildUI")]

    class ComponentSelectorButtonPatcher {

        [HarmonyPostfix]
        static void EditButtons(SyncRef<Slot> ____uiRoot) {
            if (!Config!.GetValue(OneClickBrowserEnabled)) {
                return;
            }
            for (int i=0; i<____uiRoot.Target.ChildrenCount; i++) {
                Slot targetSlot = ____uiRoot.Target[i];
                Button targetButton = targetSlot.GetComponent<Button>();
                ButtonRelay<string> targetButtonRelay = targetSlot.GetComponent<ButtonRelay<string>>();
                if (targetButton != null && targetButtonRelay != null) {
                    targetButton.RequireLockInToPress.Value = true;
                    targetButtonRelay.DoublePressDelay.Value = 0f;
                }
            }
        }
    }
}
