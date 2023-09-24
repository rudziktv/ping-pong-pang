using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Packages.Engine.Game
{
    public class TurbulanceController : MonoBehaviour
    {
        [Tooltip("If 0, then its random direction.")]
        [SerializeField]
        Vector2 turbulanceDirection;

        [SerializeField]
        float turbulanceForce;

        [SerializeField]
        float turbulanceTriggerTime;


        bool ballInArea;
        float timer;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ColliderIsBallEffect(collision, () =>
            {
                timer = 0f;
                ballInArea = true;
            });
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            ColliderIsBallEffect(collision, () => ballInArea = false);
        }

        void ColliderIsBallEffect(Collider2D collision, Action callback)
        {
            if (collision.gameObject.CompareTag("Ball"))
                callback.Invoke();
        }


        private void FixedUpdate()
        {
            TimerTick();
            Turbulance();
        }

        private void Turbulance()
        {
            if (timer > turbulanceTriggerTime && ballInArea)
            {
                timer = 0f;
                if (turbulanceDirection != Vector2.zero)
                    GameController.Instance.BallRb
                        .AddForce(turbulanceDirection.normalized * turbulanceForce);
                else
                    GameController.Instance.BallRb
                        .AddForce(RandomVector2().normalized * turbulanceForce);
            }
        }

        private void TimerTick()
        {
            if (ballInArea)
            {
                timer += Time.fixedDeltaTime;
            }
        }


        Vector2 RandomVector2()
        {
            return new(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
    }
}
