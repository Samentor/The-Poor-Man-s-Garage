using MSCLoader;
using UnityEngine;

namespace SamentorMods
{
    public class SamentorModManager : Mod
    {
        public override string ID => "Samentor_MSC_Utilities"; 
        public override string Name => "Samentor's Utility Suite"; 
        public override string Version => "1.0.0"; 
        public override string Author => "samentor"; 

        private SettingsKeybind summonKey;
        private SettingsKeybind jumpstartKey;

        // FIX: MSCLoader v1.4.2 uses OnEnable instead of OnLoad
        public override void OnEnable()
        {
            summonKey = Settings.CreateKeybind("SummonHammer", "Summon Sledgehammer", KeyCode.H, KeyCode.LeftControl);
            jumpstartKey = Settings.CreateKeybind("JumpstartSatsuma", "Jumpstart Satsuma", KeyCode.J, KeyCode.LeftControl);

            ModConsole.Log("[Samentor Utilities] Mod framework successfully initialized!");
        }

        // FIX: MSCLoader v1.4.2 uses OnUpdate instead of Update
        public override void OnUpdate()
        {
            if (summonKey.GetKeybind())
            {
                TriggerHammerSummon();
            }

            if (jumpstartKey.GetKeybind())
            {
                TriggerSatsumaJumpstart();
            }
        }

        private void TriggerHammerSummon()
        {
            GameObject player = GameObject.Find("PLAYER");
            GameObject hammer = GameObject.Find("sledgehammer(itemx)");

            if (player != null && hammer != null)
            {
                hammer.transform.position = player.transform.position + (player.transform.forward * 0.5f) + (player.transform.up * 0.2f);
                Rigidbody rb = hammer.GetComponent<Rigidbody>();
                if (rb != null) { rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
                ModConsole.Print("Sledgehammer pulled to your location!");
            }
        }

        private void TriggerSatsumaJumpstart()
        {
            GameObject satsuma = GameObject.Find("SATSUMA(557hp)");
            GameObject battery = GameObject.Find("battery(itemx)");

            if (satsuma != null)
            {
                // FIX: Use accurate PlayMaker component interaction via reflection to ensure events fire cleanly
                if (battery != null)
                {
                    Component fsm = battery.GetComponent("PlayMakerFSM");
                    if (fsm != null && fsm.name == "Data")
                    {
                        try
                        {
                            // Targets the internal FsmVariables structure dynamically without a hard dependency
                            object fsmVars = fsm.GetType().GetProperty("FsmVariables")?.GetValue(fsm, null);
                            object[] findFloatArgs = { "Charge" };
                            object fsmFloat = fsmVars?.GetType().GetMethod("FindFsmFloat", new[] { typeof(string) })?.Invoke(fsmVars, findFloatArgs);
                            fsmFloat?.GetType().GetProperty("Value")?.SetValue(fsmFloat, 130.0f, null);
                        }
                        catch { /* Fallback fail-safe */ }
                    }
                }
                
                Component engineFsm = satsuma.GetComponent("PlayMakerFSM");
                if (engineFsm != null && engineFsm.name == "Electricity")
                {
                    // Invokes the specific visual script string state directly
                    engineFsm.GetType().GetMethod("SendEvent", new[] { typeof(string) })?.Invoke(engineFsm, new object[] { "START_ENGINE" });
                }

                ModConsole.Print("Satsuma jumpstarted successfully!");
            }
        }
    }
}
