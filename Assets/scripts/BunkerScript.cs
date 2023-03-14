using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerScript : MonoBehaviour
{
    
    public int lives;
    PlayerControllerScript bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lives = 10;
        GetComponent<PlayerControllerScript>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemyBullet"))
        {
            lives--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullets")
        {
            Destroy(bulletPrefab);
            lives--;
            
        }
    }


    // Update is called once per frame
    void Update()
    {
        if( lives == 0)
        {
            Destroy(gameObject);
        }
    }
}
