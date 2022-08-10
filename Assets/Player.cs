using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb;
    public float jumph = 5;
    private bool isgrounded = false;

    private Animator anim;
    private Vector3 rotation;

    private CoinManager m;
    public GameObject panel;

    public GameObject kamera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.eulerAngles;

        //finde objekt mit tag text, in komponente coinmanager zugreifen
        m = GameObject.FindGameObjectWithTag("Text").GetComponent<CoinManager>();

    }

    // Update is called once per frame
    void Update()
    {
        float richtung = Input.GetAxis("Horizontal");

//laufen und laufanimationen mit Richtung rechts links
        if(richtung != 0)
        {
            anim.SetBool("IsRunning", true);
        } else
        {
            anim.SetBool("IsRunning", false);
        }

        if(richtung < 0)
        {
            transform.eulerAngles = rotation - new Vector3(0,180,0);
            transform.Translate(Vector2.right * speed * -richtung * Time.deltaTime);

        }
        if(richtung > 0)
        {
            transform.eulerAngles = rotation;
            transform.Translate(Vector2.right * speed * richtung * Time.deltaTime);
        }

        if(isgrounded == false)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

//springen
        if(Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            rb.AddForce(Vector2.up * jumph, ForceMode2D.Impulse);
            isgrounded = false;
        }


        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            kamera.transform.position = new Vector3(transform.position.x, -6, -10);
        }
        else
       {
            kamera.transform.position = new Vector3(transform.position.x, -1, -10);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "ground")
        {
            isgrounded = true;
        }
//Gegner
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            panel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Wasted");

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin")
        {
            m.Addmoney();
            Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("Coin");
        }

        if(other.gameObject.tag == "Spike")
        {
            Destroy(gameObject);
            panel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Wasted");
        }

        if(other.gameObject.tag == "End")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            FindObjectOfType<AudioManager>().Play("Win");
        }
    }
}
