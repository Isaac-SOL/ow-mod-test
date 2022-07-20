using OWML.Common;
using OWML.ModHelper;

namespace ModTest
{
    public class ModTest : ModBehaviour
    {
        public static ModTest Instance { get; set; }

        private ShipLogController _controller;

        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        private void Start()
        {
            Instance = this;

            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(ModTest)} is loaded!", MessageType.Success);

            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                var playerBody = FindObjectOfType<PlayerBody>();
                ModHelper.Console.WriteLine($"Found player body, and it's called {playerBody.name}!",
                    MessageType.Success);

                _controller = FindObjectOfType<ShipLogController>();

                GlobalMessenger.AddListener("EnterShipComputer", EnterHoloScreen);
                GlobalMessenger.AddListener("ExitShipComputer", ExitHoloScreen);

                //ModHelper.HarmonyHelper.AddPostfix<ShipLogController>("EnterShipComputer", typeof(ModTest), "EnterHoloScreen");
                //ModHelper.HarmonyHelper.AddPostfix<ShipLogController>("ExitShipComputer", typeof(ModTest), "ExitHoloScreen");

                _controller._attachPoint.transform.localPosition += new UnityEngine.Vector3(-0.5f, -0.3f, -3);
                _controller._shipLogCanvas.transform.localPosition += new UnityEngine.Vector3(-0.5f, 0, -1.5f);

                _controller._shipLogCanvas.transform.localRotation = UnityEngine.Quaternion.identity;
            };
        }

        private static void EnterHoloScreen()
        {
            Instance.ModHelper.Console.WriteLine("Entered computer");
            Instance._controller._canvasAnimator.AnimateTo(1, UnityEngine.Vector3.one * 0.0025f, 0.5f, null);
            var camera = Locator.GetPlayerCamera().GetComponent<PlayerCameraController>();
            camera.SnapToDegrees(0f, 0f, 100f, true);
            camera._playerCamera.farClipPlane = 10f;
        }

        private static void ExitHoloScreen()
        {
            Instance.ModHelper.Console.WriteLine("Exited computer");
        }
    }
}
