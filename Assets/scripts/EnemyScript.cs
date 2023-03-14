using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject expPrefab;
    public GameObject exppartPrefab;
    public int points;

    

    private void die()
    {
        GetComponentInParent<GameControllerScript>().addScore(points);
        GameObject exp = Instantiate(expPrefab);
        exp.transform.position = transform.position;
        GameObject exp_p = Instantiate(exppartPrefab);
        exp_p.transform.position = transform.position;
        Destroy(exp, 1.1f);
        Destroy(exp_p, 1.1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullets") 
        {
            Destroy(collision.gameObject);
            die();
        }
    }
}
