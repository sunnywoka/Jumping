using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;//Unity free asset

public class PlayerControl : MonoBehaviour
{
    //Player rigidbody
    private Rigidbody m_rigidbody;

    //Time between pressing space and releasing space (Keyboard)
    private float m_spaceTime;

    //The factor of distance of charator moving
    public float Factor = 3.0f;

    //GameObject Cube
    public GameObject Cube;

    //The cube which player stand on right now
    private GameObject m_currentCube;
    private Collider m_lastCollider;

    //The maximum distance for clonning the new cube
    public float m_distance = 2.0f;

    //Show player's score
    public Text Score;
    private int m_score = 0;
    
    //Animator
    private Animator playerAnim;

    //Particle to display the power accumulation
    public ParticleSystem dirtParticle;

    //First new cube direction
    Vector3 m_initDirection = new Vector3(1, 0, 0);

    //Sound of jumping and touching ground
    public AudioClip jumpSound;
    public AudioClip touchGround;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {

        m_rigidbody = GetComponent<Rigidbody>();

        playerAnim = GetComponent<Animator>();

        playerAudio = GetComponent<AudioSource>();


        //Change the center of mass of charator to its feet
        m_rigidbody.centerOfMass = Vector3.zero;

        //The cube which player stand on right now
        m_currentCube = Cube;
        m_lastCollider = m_currentCube.GetComponent<Collider>();

        //Call SpawnCube() function to create a new cube
        SpawnCube();
        dirtParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //Record the time between pressing space and releasing space (Keyboard)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_spaceTime = Time.time;
            //display particle
            dirtParticle.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var m_time = Time.time - m_spaceTime;
            //Call Onjump() function to calculate the charator jumping
            OnJumping(m_time);
            //Play jump sound
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //Simple animation that displaying the cube be pressed
            m_currentCube.transform.localScale += new Vector3(0, -1, 0) * 0.15f * Time.deltaTime;
            m_currentCube.transform.localPosition += new Vector3(0, -1, 0) * 0.15f * Time.deltaTime;
            
        }
    }

    //Function to change the new direction randomly
    void RandomDirection()
    {
        //two random variable
        var seed = Random.Range(0, 2);

        //give a new direction randomly
        if (seed == 0)
        {
            m_initDirection = new Vector3(1, 0, 0);
        }
        else
        {
            m_initDirection = new Vector3(0, 0, 1);
        }
    }

    //Funtion to calculate the charator jumping distance by the time between pressing and releasing keyboard space
    void OnJumping(float time)
    {
        //changing the direction of player jumping and jumping to next cube
        m_rigidbody.AddForce((new Vector3(0, 1, 0) + m_initDirection) * time * Factor, ForceMode.Impulse);
        //Play animation when player jump on air
        playerAnim.SetTrigger("Jump_trig");
        //stop particle when player jump on air
        dirtParticle.Stop();
        //Simple animation that restoring the cube from pressing
        m_currentCube.transform.DOLocalMoveY(0.25f, 0.2f);
        m_currentCube.transform.DOScale(new Vector3(1, 0.5f, 1), 0.2f);
        
    }

    //Function to create a new cube for next jumping
    void SpawnCube()
    {
        //instantiste a new cube with random direction and size
        var cube = Instantiate(Cube);
        cube.transform.position = m_currentCube.transform.position + m_initDirection * Random.Range(1.1f, m_distance);

        //player rotate itself for next jumping
        transform.rotation = Quaternion.LookRotation(m_initDirection);

        //random colour to cube
        var randomScale = Random.Range(0.5f, 1);
        Cube.transform.localScale = new Vector3(randomScale, 0.5f, randomScale);
        cube.GetComponent<Renderer>().material.color = new Color(Random.Range(0.01f, 1), Random.Range(0.01f, 1), Random.Range(0.01f, 1));
    }

    //Function to check where the player collises, cube or ground
    void OnCollisionEnter(Collision collision)
    {
        //player collises the cube
        if (collision.gameObject.name.Contains("Cube") && collision.collider != m_lastCollider)
        {
            m_lastCollider = collision.collider;
            m_currentCube = collision.gameObject;
            //Call this function to change the direction randomly
            RandomDirection();
            //Create a new cube
            SpawnCube();
            //update the score
            m_score++;
            //display the score on canvas
            Score.text = "Score: " + m_score.ToString();
        }

        //player collises the ground
        if (collision.gameObject.name == "Ground")
        {
            //play the sound when player collises the fround
            playerAudio.PlayOneShot(touchGround, 1.0f);
            //reload the scene and re-start the game
            SceneManager.LoadScene("GameScenes");

        }
    }
}
