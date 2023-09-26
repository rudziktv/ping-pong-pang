using UnityEngine;

public class OutController : MonoBehaviour
{
    public delegate void OutArgs(OutSide side);
    public event OutArgs outDetected;

    [SerializeField]
    OutSide side;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            outDetected?.Invoke(side);
        }
    }
}


public enum OutSide
{
    Left = 0,
    Right = 1,
}