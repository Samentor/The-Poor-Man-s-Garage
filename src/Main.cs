using MSCLoader;
using UnityEngine;
using PlayMaker;

namespace SamentorMods
{
    public class SamentorModManager : Mod
    {
        // Mod Metadata linked to your GitHub identity
        public override string ID => "Samentor_MSC_Utilities"; 
        public override string Name => "Samentor's Utility Suite"; 
        public override string Version => "1.0.0"; 
        public override string Author => "samentor"; 

        // Keybind Settings using new SettingsKeybind format
        public SettingsKeybind summonKey;
        public SettingsKeybind jumpstartKey;

        public virtual void PreLoad()
        {
            // Initialize Keybinds using new settings format
            summonKey = new SettingsKeybind()
            {
                Name = "Summon Sledgehammer",
                Key = KeyCode.H,
                Modifier = KeyCode.LeftControl
            };

            jumpstartKey = new SettingsKeybind()
            {
                Name = "Jumpstart Satsuma",
                Key = KeyCode.J,
                Modifier = KeyCode.LeftControl
            };

            ModConsole.Log("[Samentor Utilities] Keybinds initialized!");
        }

        public virtual void OnLoad()
        {
            ModConsole.Log("[Samentor Utilities] Mod framework successfully initialized!");
        }

        public virtual void Update()
        {
            // Handle Sledgehammer Summon
            if (summonKey.GetKeybind())
            {
                TriggerHammerSummon();
            }

            // Handle Satsuma Jumpstart
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
                if (battery != null)
                {
                    PlayMakerFSM batteryFSM = PlayMakerFSM.FindFsmOnGameObject(battery, "Data");
                    if (batteryFSM != null) batteryFSM.FsmVariables.GetFsmFloat("Charge").Value = 130.0f;
                }
                
                PlayMakerFSM engineFSM = PlayMakerFSM.FindFsmOnGameObject(satsuma, "Electricity");
                if (engineFSM != null) engineFSM.SendEvent("START_ENGINE");

                ModConsole.Print("Satsuma revived and fully charged!");
            }
        }
    }
}
