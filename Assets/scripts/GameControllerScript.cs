using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameControllerScript : MonoBehaviour
{

    EnemyScript[] enemies;
    EnemyScript randomEnemy;

    public float shootingInterval = 3f;
    public float shootSpeed = 2f;
    public GameObject enemyBulletPrefab;
    private float shootTimer;

    public float moveInterval = 0.42857639f;
    public float moveDistance = 0.1f;
    public float hLimit = 4f;
    private float movingDir = 1;
    private float movingTimer;
    private float rightMostEnemyPos = 0;
    private float leftMostEnemyPos = 0;


    public float maxMoveInterval = 0.42857639f;
    public float minMoveInterval = 0.05f;
    private int maxEnemies;

    private int score;
    private int lives;
    private int wave;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI livesText;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            score = PlayerPrefs.GetInt("score");
            wave = PlayerPrefs.GetInt("wave");
            lives = PlayerPrefs.GetInt("lives");
        }
        else
        {
            score = 0;
            wave = 1;
            lives = 3;
        }


        update_Ui();

        shootTimer = shootingInterval;
        movingTimer = maxMoveInterval;
        enemies = GetComponentsInChildren<EnemyScript>();
        maxEnemies = enemies.Length;
    }

    public void player_die()
    {
        lives--;
        update_Ui();
        if (lives < 1)
        {
            PlayerPrefs.DeleteKey("score");
            PlayerPrefs.DeleteKey("wave");
            PlayerPrefs.DeleteKey("lives");
            SceneManager.LoadScene("Game Over Credits");
        }
    }

    public void addScore(int points)
    {
        score += points;
        update_Ui();
    }

    private void update_Ui()
    {
        scoreText.text = "Score\n" + score.ToString();
        waveText.text = "Wave\n" + wave.ToString();
        livesText.text = "lives\n" + lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GetComponentsInChildren<EnemyScript>();
        if (enemies.Length >= 1)
        {
            movingTimer -= Time.deltaTime;
            if (movingTimer <= 0)
            {
                int curEnemieCount = enemies.Length;
                float diffsetting = 1f - (float)curEnemieCount / maxEnemies;
                moveInterval = maxMoveInterval - (maxMoveInterval - minMoveInterval) * diffsetting;

                movingTimer = moveInterval;
                transform.position = new Vector3(transform.position.x + (moveDistance * movingDir), transform.position.y, 0);

            }
            if (movingDir == 1)
            {

                foreach (EnemyScript enemy in enemies)
                {
                    if (enemy.transform.position.x > rightMostEnemyPos)
                    {
                        rightMostEnemyPos = enemy.transform.position.x;
                    }

                }

                if (rightMostEnemyPos >= hLimit)
                {
                    movingDir *= -1;
                    transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance, 0);
                    rightMostEnemyPos = 0;
                    leftMostEnemyPos = 0;

                }
            }
            else
            {
                foreach (EnemyScript enemy in enemies)
                {
                    if (enemy.transform.position.x < leftMostEnemyPos)
                    {
                        leftMostEnemyPos = enemy.transform.position.x;
                    }
                }

                if (leftMostEnemyPos <= -hLimit)
                {
                    movingDir *= -1;
                    transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance, 0);
                    rightMostEnemyPos = 0;
                    leftMostEnemyPos = 0;

                }
            }


            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0 && enemies.Length > 0)
            {
                shootingInterval = Random.Range(2, 6);
                shootSpeed = Random.Range(2, 8);
                shootTimer = shootingInterval;
                randomEnemy = enemies[Random.Range(0, enemies.Length)];
                GameObject enemyBullet = Instantiate(enemyBulletPrefab);
                enemyBullet.transform.position = randomEnemy.transform.position;
                enemyBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shootSpeed);
                Destroy(enemyBullet, 6f);

            }
        }
        else
        {
            wave++;
            saveData();

            SceneManager.LoadScene("Game");
        }
    }

    public void saveData()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("wave", wave);
        PlayerPrefs.SetInt("lives", lives);
    }

    public void clearData()
    {
        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.DeleteKey("wave");
        PlayerPrefs.DeleteKey("lives");
    }

    
}

