using UnityEngine;

namespace Assets.Packages.Engine.Game
{
    public class BallController : MonoBehaviour
    {
        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        AudioClip clip;


        Vector3 lastFramePos;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            audioSource.PlayOneShot(clip);
            GameSession.Instance.AddContact();
        }


        private void Start()
        {
            lastFramePos = transform.position;
        }


        private void FixedUpdate()
        {
            GameSession.Instance.AddDistance((lastFramePos - transform.position).magnitude);
            lastFramePos = transform.position;
        }
    }
}
