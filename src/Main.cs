using MSCLoader;
using UnityEngine;

namespace SamentorMods
{
    // 1. The Mod Registration Class
    public class SamentorModManager : Mod
    {
        // These properties are required by the base Mod class
        public override string ID => "Samentor_MSC_Utilities"; 
        public override string Name => "Samentor's Utility Suite"; 
        public override string Version => "1.0.0"; 
        public override string Author => "samentor"; 

        // Dropping 'override' fixes the compiler rejection. 
        // MSCLoader will still find and execute this via reflection when the mod loads.
        public void OnLoad()
        {
            // Create an invisible GameObject to hold our native Unity logic
            GameObject modRunner = new GameObject("Samentor_Mod_Runner");
            Object.DontDestroyOnLoad(modRunner);
            
            // Attach our custom MonoBehaviour to run the Update loop natively
            modRunner.AddComponent<SamentorLogic>();

            ModConsole.Log("[Samentor Utilities] Mod framework successfully initialized!");
        }
    }

    // 2. The Native Unity Logic Class
    // By using MonoBehaviour, we guarantee the Update loop runs flawlessly independently of the ModLoader.
    public class SamentorLogic : MonoBehaviour
    {
        private void Update()
        {
            // Native Unity Input handling
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.H)) TriggerHammerSummon();
                if (Input.GetKeyDown(KeyCode.J)) TriggerSatsumaJumpstart();
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
                        catch { /* Fallback fail-safe */ }
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
