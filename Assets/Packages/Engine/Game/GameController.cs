using Assets.Packages.Engine.Game;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField]
    GameObjects objects;

    Rigidbody2D rb;

    int scorePlayer1;
    int scorePlayer2;

    public int Round => 1 + scorePlayer1 + scorePlayer2;

    private GameObject ball => objects.ball;


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
            rb.velocity = rb.velocity.normalized * GameRules.velocity / rb.mass;
        }
    }


    public void ServeBall()
    {
        int r = Round % 2 == 1 ? Round + 1 : Round;

        if ((r / 2) % 2 == 0)
        {
            ServeBall(GameRules.SecondSide);
        }
        else
        {
            ServeBall(GameRules.startSide);
        }
    }


    void ServeBall(OutSide side)
    {
        rb.velocity = Vector2.zero;

        switch (side)
        {
            case OutSide.Left:
                ball.transform.position = objects.leftServe.position;
                rb.AddForce(Vector2.right * GameRules.velocity, ForceMode2D.Impulse);
                break;
            case OutSide.Right:
                ball.transform.position = objects.rightServe.position;
                rb.AddForce(Vector2.left * GameRules.velocity, ForceMode2D.Impulse);
                break;
        }
    }


    public void Score(OutSide side)
    {
        switch (side)
        {
            case OutSide.Left:
                scorePlayer2++;
                break;
            case OutSide.Right:
                scorePlayer1++;
                break;
        }

        UpdateScoreUI();

        if (scorePlayer1 >= GameRules.winScore || scorePlayer2 >= GameRules.winScore)
        {
            WinSequence();
            return;
        }

        AudioController.Instance.PlayScore();
    }


    public void GameRestart(bool changeSide = false)
    {
        Time.timeScale = 1f;

        ScoreClear();

        objects.player1.CenterPlayer();
        objects.player2.CenterPlayer();

        if (changeSide)
            GameRules.startSide = GameRules.SecondSide;

        StartCoroutine(nameof(UnfreezeGame));
    }


    IEnumerator UnfreezeGame()
    {
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1f;
    }

    
    public void ScoreClear()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;

        UpdateScoreUI();
    }


    public void UpdateScoreUI()
    {
        GameUI.Instance.PlayerScore(OutSide.Left, scorePlayer1);
        GameUI.Instance.PlayerScore(OutSide.Right, scorePlayer2);
    }


    void WinSequence()
    {
        AudioController.Instance.PlayWin();

        Time.timeScale = 0f;

        GameUI.Instance.WinScreen();
    }


    public Score GetScore => new(scorePlayer1, scorePlayer2);
}


public class Score
{
    public int p1;
    public int p2;

    public Score(int p1, int p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
}