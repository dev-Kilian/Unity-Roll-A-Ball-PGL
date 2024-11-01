using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    private int lvlCollectables;
    public float jumpForce = 5f;
    private bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0; 
        winTextObject.SetActive(false);
        lvlCollectables = 9; // la cantidad de items (PickUp) que el jugador recoge en el nivel. está hardcodeado y lo suyo sería dedectarlo de alguna manera para automatizarlo.
        SetCountText();
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 movement = inputValue.Get<Vector2>(); // el tutorial oficial dice que esto sería Vector2 movementVector = movementValue.Get<Vector2>(); - pero con esto parece funcionar igual. 

        movementX = movement.x;
        movementY = movement.y;
    }

       void SetCountText() 
   {
       countText.text =  "Count: " + count.ToString();
       if (count >= lvlCollectables) {
        winTextObject.SetActive(true);
        Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        Invoke("RestartLevel", 3f);
       }
   }

    // Update is called once per frame
        void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

   void OnTriggerEnter (Collider other) 
   {
       if (other.gameObject.CompareTag("PickUp")) 
       {
           other.gameObject.SetActive(false);
           count = count + 1;
           SetCountText();
       }
   }

   private void OnCollisionEnter(Collision collision)
    {
    if (collision.gameObject.CompareTag("Enemy"))
    {
        // Destroy the current object
        Destroy(gameObject); 
        // Update the winText to display "You Lose!"
        winTextObject.gameObject.SetActive(true);
        winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        Invoke("RestartLevel", 3f);
    }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnJump() 
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
        void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
