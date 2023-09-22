using System.Collections;
using UnityEngine;

public class OutController : MonoBehaviour
{
    [SerializeField]
    OutSide side;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameController.Instance.Score(side);
            StartCoroutine(nameof(OutCoroutine));
        }
    }


    IEnumerator OutCoroutine()
    {
        yield return new WaitForSeconds(2);
        GameController.Instance.ServeBall();
    }
}


public enum OutSide
{
    left = 0,
    right = 1,
}