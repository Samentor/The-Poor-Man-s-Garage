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

        // Standard MSCLoader initialization method
        public override void OnLoad()
        {
            ModConsole.Log("[Samentor Utilities] Mod framework successfully initialized!");
        }

        // Standard MSCLoader frame update method
        public override void Update()
        {
            // Using native Unity Input handles keybinds reliably without breaking across loader updates
            if (Input.GetKey(KeyCode.LeftControl))
            {
                // Left Ctrl + H -> Summon Sledgehammer
                if (Input.GetKeyDown(KeyCode.H))
                {
                    TriggerHammerSummon();
                }

                // Left Ctrl + J -> Jumpstart Satsuma
                if (Input.GetKeyDown(KeyCode.J))
                {
                    TriggerSatsumaJumpstart();
                }
            }
        }

        private void TriggerHammerSummon()
        {
            GameObject player = GameObject.Find("PLAYER");
            GameObject hammer = GameObject.Find("sledgehammer(itemx)");

            if (player != null && hammer != null)
            {
                // Teleport safely in front of player
                hammer.transform.position = player.transform.position + (player.transform.forward * 0.5f) + (player.transform.up * 0.2f);
                
                // Kill physical momentum so it doesn't clip out of world bounds
                Rigidbody rb = hammer.GetComponent<Rigidbody>();
                if (rb != null) 
                { 
                    rb.velocity = Vector3.zero; 
                    rb.angularVelocity = Vector3.zero; 
                }
                ModConsole.Print("Sledgehammer pulled to your location!");
            }
        }

        private void TriggerSatsumaJumpstart()
        {
            GameObject satsuma = GameObject.Find("SATSUMA(557hp)");
            GameObject battery = GameObject.Find("battery(itemx)");

            if (satsuma != null)
            {
                // Dynamic PlayMaker FSM reflection lookup to avoid project dependency errors
                if (battery != null)
                {
                    Component fsm = battery.GetComponent("PlayMakerFSM");
                    if (fsm != null && fsm.name == "Data")
                    {
                        try
                        {
                            object fsmVars = fsm.GetType().GetProperty("FsmVariables")?.GetValue(fsm, null);
                            object[] findFloatArgs = { "Charge" };
                            object fsmFloat = fsmVars?.GetType().GetMethod("FindFsmFloat", new[] { typeof(string) })?.Invoke(fsmVars, findFloatArgs);
                            fsmFloat?.GetType().GetProperty("Value")?.SetValue(fsmFloat, 130.0f, null);
                        }
                        catch { /* Fail-safe fallback */ }
                    }
                }
                
                Component engineFsm = satsuma.GetComponent("PlayMakerFSM");
                if (engineFsm != null && engineFsm.name == "Electricity")
                {
                    engineFsm.GetType().GetMethod("SendEvent", new[] { typeof(string) })?.Invoke(engineFsm, new object[] { "START_ENGINE" });
                }

                ModConsole.Print("Satsuma jumpstarted successfully!");
            }
        }
    }
}
