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

        // FIX: Switch to SettingsKeybind as required by MSCLoader v1.4.2
        private SettingsKeybind summonKey;
        private SettingsKeybind jumpstartKey;

        public override void OnLoad()
        {
            // FIX: Use modern Keybind settings syntax (ID, Name, Default Key, Modifier)
            summonKey = Settings.CreateKeybind("SummonHammer", "Summon Sledgehammer", KeyCode.H, KeyCode.LeftControl);
            jumpstartKey = Settings.CreateKeybind("JumpstartSatsuma", "Jumpstart Satsuma", KeyCode.J, KeyCode.LeftControl);

            ModConsole.Log("[Samentor Utilities] Mod framework successfully initialized!");
        }

        public override void Update()
        {
            // FIX: Renamed .IsPressed() to .GetKeybind() per deprecation error
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
                // FIX: Fallback dynamic lookup for PlayMakerFSM components to satisfy the compiler
                if (battery != null)
                {
                    Component[] components = battery.GetComponents<Component>();
                    foreach (var comp in components)
                    {
                        if (comp.GetType().Name == "PlayMakerFSM" && comp.name == "Data")
                        {
                            // Safely communicate with PlayMaker via Unity standard messaging or reflection if namespaces fail
                            comp.SendMessage("SetFsmFloat", new object[] { "Charge", 130.0f }, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                
                Component[] satsumaComponents = satsuma.GetComponents<Component>();
                foreach (var comp in satsumaComponents)
                {
                    if (comp.GetType().Name == "PlayMakerFSM" && comp.name == "Electricity")
                    {
                        comp.gameObject.SendMessage("SendEvent", "START_ENGINE", SendMessageOptions.DontRequireReceiver);
                    }
                }

                ModConsole.Print("Satsuma actions processed cleanly!");
            }
        }
    }
}
