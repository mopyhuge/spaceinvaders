using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public bool fired;
    public float bulletSpeed;
    public float moveSpeed;
    public Rigidbody2D rig;
    public float limit;

    private float coolDown;
    private float coolDownDur;




    private Vector3 startPos;
    private Vector3 hidePos;
    public GameObject expPrefab;
    public GameObject exppartPrefab;
    public float hidetime;
    public float hideDur;
    private bool dead;

    void Start()
    {
        dead = false;
        hideDur = 3f;
        hidetime = hideDur;

        startPos = transform.position;
        hidePos = startPos + Vector3.up * 1000;


        fired = false;
        rig = GetComponent<Rigidbody2D>();
        coolDownDur = .1f;
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0);
        if (transform.position.x > limit)
        {
            transform.position = new Vector3(-limit, transform.position.y, transform.position.x);
        }
        if (transform.position.x < -limit)
        {
            transform.position = new Vector3(limit, transform.position.y, transform.position.x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            hidetime -= Time.deltaTime;
            if(hidetime <= 0)
            {
                dead = false;
                hidetime = hideDur;
                transform.position = startPos;
            }
        }




        coolDown -= Time.deltaTime;
        if (Input.GetAxis("Jump") == 1f)
        {
            if(coolDown <= 0 && fired == false)
            {
                coolDown = coolDownDur;
                fired = true;
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetParent(transform.parent);
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
                    0,
                    bulletSpeed
                    );
                Destroy(bullet, 2f);
            }
        }
        else
        {
            fired = false;
        }
        
    }

    private void die()
    {
        GameObject exp = Instantiate(expPrefab);
        exp.transform.position = transform.position;
        GameObject exp_p = Instantiate(exppartPrefab);
        exp_p.transform.position = transform.position;

        dead = true;
        transform.position = hidePos;

        Destroy(exp, 1.1f);
        Destroy(exp_p, 1.1f);
        GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControllerScript>().player_die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemyBullet")
        {
            die();
        }
    }
}
