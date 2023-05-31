using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce = 10f;
    private float gravityModifier = 1.5f;
    private float limitTop = 16f;
    
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        //Impide darle impulso al globo si esta en y = 15
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < limitTop && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }

        //Impide que el globo salga por arriba
        if (transform.position.y > limitTop)
        {
            transform.position = new Vector3(transform.position.x, limitTop);
        }

        //Cambiamos a 0 la velocidad del globo cuando llega arriba, si no el globo mantiene la velocidad
        //y se queda en el techo hasta que la velocidad baja a 0
        if (transform.position.y == limitTop)
        {
            playerRb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        //Si el globo choca contra el suelo y no hay GameOver explota y desaparece + GameOver
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Invoke("Destroy", 1f);
        }

        //Si ya hay GameOver solo desaparece
        else if (other.gameObject.CompareTag("Ground") && gameOver)
        {
            Invoke("Destroy", 1f);
        }

    }

    void Destroy()
    {
        Destroy(playerRb.gameObject);
    }
}
