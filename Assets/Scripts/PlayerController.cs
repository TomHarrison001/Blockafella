using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, movesText, gameOverScore;
    [SerializeField] private GameObject cam;
    [SerializeField] private ActivateMenu gameOverMenu;
    private AudioManager audioManager;
    private GameController controller;
    private LevelGenerator levelGen;
    private Animator anim;
    private SpriteRenderer rend;
    private int score, moves, dir, newMove, newDash;
    private float speed = 1.2f;
    private bool moving, dashing;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        controller = FindObjectOfType<GameController>();
        levelGen = FindObjectOfType<LevelGenerator>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        Restart();
    }

    private void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("up")) Move(0);
        else if (Input.GetKey("s") || Input.GetKey("down")) Move(1);
        else if (Input.GetKey("a") || Input.GetKey("left")) Move(2);
        else if (Input.GetKey("d") || Input.GetKey("right")) Move(3);
    }

    private void Restart()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        moves = 10;
        movesText.text = "Moves: " + moves;
        transform.position = new Vector3(0, 4.8f, 0);
        SelectColour();
        cam.transform.position = new Vector3(0, 0, -1f);
        levelGen.GenerateNewLevel();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "extraMoves") moves += 6;
        if (other.gameObject.name == "destroyLine") StartCoroutine(Dash());
        if (other.gameObject.name == "bonusScore") { score += 4; controller.Coins += 4; }
        if (other.gameObject.name == "doubleMoves") moves *= 2;
        audioManager.Play(other.gameObject.name == "rock" ? "block" : "powerup");
        Destroy(other.gameObject);
        score++;
        controller.Coins++;
        scoreText.text = "Score: " + score;
        movesText.text = "Moves: " + moves;
        StartCoroutine(Spin());
    }

    public void Move(int newDir)
    {
        if (moves <= 0 || moving || dashing) return;
        switch (newDir)
        {
            case 0: if (transform.position.y >= cam.transform.position.y + 3.5) return; break;
            case 2: if (transform.position.x <= -2.1) return; break;
            case 3: if (transform.position.x >= 2.1) return; break;
        }
        dir = newDir;
        moves--;
        movesText.text = "Moves: " + moves;
        newMove = 0;
        StartCoroutine(NewMove());
        if (moves == 0) StartCoroutine(EndGame());
    }

    private IEnumerator NewMove()
    {
        moving = true;
        if (dir == 0) transform.position += transform.up * speed / 20;
        if (dir == 1) transform.position -= transform.up * speed / 20;
        if (dir == 2) transform.position -= transform.right * speed / 20;
        if (dir == 3) transform.position += transform.right * speed / 20;
        if (transform.position.y < cam.transform.position.y - 2.4)
            cam.transform.position -= cam.transform.up * speed / 20;
        if (cam.transform.position.y < levelGen.Frame * -10.8f - 1.3f)
        {
            levelGen.ShowRocks();
            levelGen.ShowPowerUps();
        }
        newMove++;
        yield return new WaitForSeconds(0.025f);
        if (newMove < 20) StartCoroutine(NewMove());
        else moving = false;
    }

    private IEnumerator Dash()
    {
        dashing = true;
        newDash = 0;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(NewDash());
    }

    private IEnumerator NewDash()
    {
        dashing = true;
        if (newDash < 15 || newDash > 44) transform.position -= transform.right * speed / 3;
        else transform.position += transform.right * speed / 3;
        newDash++;
        yield return new WaitForSeconds(0.005f);
        if (newDash < 60) StartCoroutine(NewDash());
        else dashing = false;
    }

    private IEnumerator Spin()
    {
        anim.SetBool("Block", true);
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Block", false);
    }

    private void ChangeColour(Color skin)
    {
        rend.color = skin;
    }

    private void SelectColour()
    {
        if (controller.Skin == 0) return;
        switch (controller.Skin)
        {
            case 1:
                rend.color = new Color(0.4f, 0.9019608f, 0.3529412f, 1);
                break;
            case 2:
                rend.color = new Color(0.3960784f, 0.8980392f, 1, 1);
                break;
            case 3:
                rend.color = new Color(1, 0.7137255f, 1, 1);
                break;
            case 4:
                rend.color = new Color(1, 0.3686275f, 0.3411765f, 1);
                break;
            case 5:
                rend.color = new Color(0.9803922f, 0.5882353f, 0, 1);
                break;
            case 6:
                rend.color = new Color(1, 0.937255f, 0, 1);
                break;
        }
    }

    public void PlayButtonAudio()
    {
        audioManager.Play("select");
    }

    public void LoadMainMenu()
    {
        controller.LoadLevel(0);
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        if (moves == 0)
        {
            gameOverMenu.EnableMenu();
            gameOverScore.text = "Score: " + score;
            if (score > controller.Highscore) controller.Highscore = score;
            controller.SaveData();
        }
    }
}
