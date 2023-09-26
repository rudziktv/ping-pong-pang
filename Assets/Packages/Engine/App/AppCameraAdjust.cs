using UnityEngine;

namespace Assets.Packages.Engine.App
{
    public class AppCameraAdjust : MonoBehaviour
    {
        [SerializeField]
        float standardSize = 6.8f;

        [SerializeField]
        float triggerRatio = 1.4f;

        [SerializeField]
        float additionalSize = 3.55f;

        float currentRatio;

        private void FixedUpdate()
        {
            GetCurrentRaio();

            if (triggerRatio > currentRatio)
            {
                Camera.main.orthographicSize = standardSize +
                    Mathf.Pow((triggerRatio - currentRatio) * additionalSize, 2);
            }
        }

        private void GetCurrentRaio()
        {
            currentRatio = (float)((float)Screen.width / (float)Screen.height);
        }
    }
}
