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

    int setPlayer1;
    int setPlayer2;

    float roundTimer;

    float Acceleration => GameRules.accelerationOverTime ?
        roundTimer * GameRules.accelerationRate / 60f
        : 0f;

    public int Round => 1 + scorePlayer1 + scorePlayer2;
    public int Set => 1 + scorePlayer1 + setPlayer2;

    public GameObject Ball => objects.ball;
    public Rigidbody2D BallRb => rb;

    public float IntentionalVelocity => Acceleration + GameRules.velocity / rb.mass;

    bool reseting;


    private void Awake()
    {
        Instance = this;
        rb = Ball.GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        OutInitialize();
        GameRestart();
    }

    private void OutInitialize()
    {
        foreach (var item in objects.outs)
        {
            item.outDetected += Score;
        }
    }

    private void FixedUpdate()
    {
        AccelerationTimer();
        KeepVelocity();
    }

    private void AccelerationTimer()
    {
        if (GameRules.accelerationOverTime && !reseting)
        {
            roundTimer += Time.fixedDeltaTime;
        }
    }

    void KeepVelocity()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity = rb.velocity.normalized * GameRules.velocity / rb.mass
                + rb.velocity.normalized * Acceleration;
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
                Ball.transform.position = objects.leftServe.position;
                rb.AddForce(Vector2.right * GameRules.velocity, ForceMode2D.Impulse);
                break;
            case OutSide.Right:
                Ball.transform.position = objects.rightServe.position;
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

        roundTimer = 0f;

        if (setPlayer1 >= GameRules.sets || setPlayer2 >= GameRules.sets)
        {
            WinSequence();
            return;
        }
        else if (scorePlayer1 >= GameRules.winScore || scorePlayer2 >= GameRules.winScore)
        {

            AudioController.Instance.PlayWin();
            StartCoroutine(nameof(ResetBallPosition));
            return;
        }
        else
        {
            StartCoroutine(nameof(ResetBallPosition));
        }

        AudioController.Instance.PlayScore();
    }


    IEnumerator ResetBallPosition()
    {
        reseting = true;
        yield return new WaitForSeconds(2);
        RecenterPlayers();
        ServeBall();
        roundTimer = 0f;
        reseting = false;
    }


    public void GameRestart(bool changeSide = false)
    {
        if (changeSide)
            GameRules.startSide = GameRules.SecondSide;

        RecenterPlayers();
        ScoreClear();
        ServeBall();

        Time.timeScale = 0f;

        StartCoroutine(nameof(UnfreezeGame));
    }

    private void RecenterPlayers()
    {
        objects.player1.CenterPlayer();
        objects.player2.CenterPlayer();
    }

    IEnumerator UnfreezeGame()
    {
        yield return new WaitForSecondsRealtime(2);
        if (!GameUI.Instance.GamePaused)
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