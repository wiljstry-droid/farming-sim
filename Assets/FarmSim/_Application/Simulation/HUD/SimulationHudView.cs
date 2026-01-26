using UnityEngine;
using TMPro;

namespace FarmSim.Application.Simulation.HUD
{
    public sealed class SimulationHudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text hudText;

        private string simTimeLine = "SIM TIME: (pending)";
        private string soilLine = "SOIL BUCKET: (pending)";

        public void SetSimTime(string value)
        {
            simTimeLine = value ?? "SIM TIME: (null)";
            Refresh();
        }

        public void SetSoilBucket(string value)
        {
            soilLine = value ?? "SOIL BUCKET: (null)";
            Refresh();
        }

        private void Refresh()
        {
            if (hudText == null)
                return;

            hudText.text = simTimeLine + "\n" + soilLine;
        }
    }
}
