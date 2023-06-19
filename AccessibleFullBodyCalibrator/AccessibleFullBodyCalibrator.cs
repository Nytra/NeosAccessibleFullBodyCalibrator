using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using System.Reflection;
using BaseX;

namespace AccessibleFullBodyCalibrator
{
    public class AccessibleFullBodyCalibrator : NeosMod
    {
        public override string Name => "AccessibleFullBodyCalibrator";
        public override string Author => "Nytra";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/Nytra/NeosFullBodyCalibratorMod";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.username.Template");
            harmony.PatchAll();
        }
		
        [HarmonyPatch(typeof(FullBodyCalibratorDialog), "OnStartCalibration")]
        class AccessibleFullBodyCalibratorPatch
        {
            public static void Postfix(FullBodyCalibratorDialog __instance, SyncRef<FullBodyCalibrator> ____calibrator)
            {
                Slot s = __instance.Slot.Parent.AddSlot("Button");
                s.AttachComponent<Grabbable>().Scalable.Value = true;
                NeosButton b = s.AttachComponent<NeosButton>();
                b.LocalPressed += (btn, data) => 
                {
                    typeof(FullBodyCalibrator).GetMethod("set_CalibratingPose", BindingFlags.Public | BindingFlags.Instance).Invoke(____calibrator.Target, new object[] { false });
                    typeof(FullBodyCalibrator).GetMethod("MapTrackers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(____calibrator.Target, new object[] { });
                    s.Destroy();
                };
                b.LabelText = "Confirm Tracker Mapping";
                s.PositionInFrontOfUser(float3.Backward, distance: 0.5f);
                __instance.Slot.AttachComponent<DestroyProxy>().DestroyTarget.Value = s.ReferenceID;
            }
        }
    }
}