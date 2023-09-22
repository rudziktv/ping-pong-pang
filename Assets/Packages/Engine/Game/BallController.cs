using UnityEngine;

namespace Assets.Packages.Engine.Game
{
    public class BallController : MonoBehaviour
    {
        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        AudioClip clip;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
