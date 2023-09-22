using Assets.Packages.Engine.Game;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField]
    GameObjects objects;

    [SerializeField]
    GameRules rules;

    Rigidbody2D rb;

    int scorePlayer1;
    int scorePlayer2;

    public int Round => 1 + scorePlayer1 + scorePlayer2;

    private GameObject ball => objects.ball;

    public GameRules Rules => rules;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        rb = ball.GetComponent<Rigidbody2D>();

        ServeBall();
        UpdateScoreUI();
    }


    private void FixedUpdate()
    {
        KeepVelocity();
    }


    void KeepVelocity()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity = rb.velocity.normalized * rules.velocity / rb.mass;
        }
    }


    public void ServeBall()
    {
        int r = Round % 2 == 1 ? Round + 1 : Round;

        if ((r / 2) % 2 == 0)
        {
            ServeBall(rules.SecondSide);
        }
        else
        {
            ServeBall(rules.startSide);
        }
    }


    void ServeBall(OutSide side)
    {
        rb.velocity = Vector2.zero;

        switch (side)
        {
            case OutSide.left:
                ball.transform.position = objects.leftServe.position;
                rb.AddForce(Vector2.right * rules.velocity, ForceMode2D.Impulse);
                break;
            case OutSide.right:
                ball.transform.position = objects.rightServe.position;
                rb.AddForce(Vector2.left * rules.velocity, ForceMode2D.Impulse);
                break;
        }
    }


    public void Score(OutSide side)
    {
        switch (side)
        {
            case OutSide.left:
                scorePlayer2++;
                break;
            case OutSide.right:
                scorePlayer1++;
                break;
        }

        UpdateScoreUI();

        if (scorePlayer1 >= rules.winScore || scorePlayer2 >= rules.winScore)
        {
            WinSequence();
            return;
        }

        AudioController.Instance.PlayScore();
    }

    
    public void ScoreClear()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;

        UpdateScoreUI();
    }


    public void UpdateScoreUI()
    {
        GameUI.Instance.PlayerScore(OutSide.left, scorePlayer1);
        GameUI.Instance.PlayerScore(OutSide.right, scorePlayer2);
    }


    void WinSequence()
    {
        AudioController.Instance.PlayWin();
    }
}
