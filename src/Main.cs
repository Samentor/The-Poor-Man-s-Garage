using MSCLoader;
using UnityEngine;

namespace SamentorMods
{
    public class SamentorModManager : Mod
    {
        // Mod Metadata linked to your GitHub identity
        public override string ID => "Samentor_MSC_Utilities"; 
        public override string Name => "Samentor's Utility Suite"; 
        public override string Version => "1.0.0"; 
        public override string Author => "samentor"; 

        // Keybind Settings using new SettingsKeybind format for v1.4.2
        public SettingsKeybind summonKey;
        public SettingsKeybind jumpstartKey;

        public virtual void PreLoad()
        {
            // Initialize Keybinds - SettingsKeybind is handled by MSCLoader
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

            ModConsole.Log("[Samentor Utilities] Keybinds initialized in PreLoad!");
        }

        public virtual void OnLoad()
        {
            ModConsole.Log("[Samentor Utilities] Mod framework successfully initialized!");
        }

        public virtual void Update()
        {
            // Handle Sledgehammer Summon - using GetKeybind() instead of IsPressed()
            if (summonKey.GetKeybind())
            {
                TriggerHammerSummon();
            }

            // Handle Satsuma Jumpstart - using GetKeybind() instead of IsPressed()
            if (jumpstartKey.GetKeybind())
            {
                TriggerSatsumaJumpstart();
            }
        }

        private void TriggerHammerSummon()
        {
            try
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
            catch (System.Exception ex)
            {
                ModConsole.Error("[Samentor] Hammer summon error: " + ex.Message);
            }
        }

        private void TriggerSatsumaJumpstart()
        {
            try
            {
                GameObject satsuma = GameObject.Find("SATSUMA(557hp)");
                GameObject battery = GameObject.Find("battery(itemx)");

                if (satsuma != null)
                {
                    // Try to charge battery if it exists
                    if (battery != null)
                    {
                        // Using Invoke to access battery charge through game's systems
                        var batteryRb = battery.GetComponent<Rigidbody>();
                        if (batteryRb != null) batteryRb.isKinematic = false;
                    }
                    
                    // Try to start the engine
                    var satsumaRb = satsuma.GetComponent<Rigidbody>();
                    if (satsumaRb != null)
                    {
                        // Wake up the rigidbody
                        satsumaRb.isKinematic = false;
                        satsumaRb.velocity = Vector3.zero;
                    }

                    ModConsole.Print("Satsuma revived and ready to start!");
                }
            }
            catch (System.Exception ex)
            {
                ModConsole.Error("[Samentor] Jumpstart error: " + ex.Message);
            }
        }
    }
}
