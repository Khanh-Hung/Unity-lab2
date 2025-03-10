using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float normalSpeed = 20f;
    [SerializeField] float boostSpeed = 40f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer; // Đã được chỉnh thành "Ground" trong Inspector

    Rigidbody2D rb2d;
    public GameObject gameObject;
    SurfaceEffector2D surfaceEffector2D;
    bool canMove = true;
    bool isGrounded = false; 
    [SerializeField] Transform groundCheck; 
    [SerializeField] float groundCheckRadius = 0.3f;
    public int score = 0;
    public TextMeshProUGUI textMeshProUGUI;
    

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
    }

    void Update()
    {
        if (score >= 40 && normalSpeed == 20f)
        {
            StartCoroutine(BoostSpeedForSeconds(5f)); 
        } else if(score >= 20 && normalSpeed == 20f)
        {
            StartCoroutine(BoostSpeedForSeconds(3f));
        } 
        if (canMove)
        {
            RotatePlayer();
            RespondToBoost();
            Jump(); 
        }
        CheckGrounded(); // Kiểm tra chạm đất
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed!");
        }
        void CheckGrounded()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            Debug.Log("Is Grounded: " + isGrounded);
        }
       
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    IEnumerator BoostSpeedForSeconds(float duration)
    {
        normalSpeed = 25f; 
        yield return new WaitForSeconds(duration); 
        normalSpeed = 20f; 
    }

    public void DisableControls()
    {
        canMove = false;
    }

    public void AddScore(int amount)
    {
        score += amount;
        textMeshProUGUI.SetText("Score: " + score);
    }

    void RespondToBoost()
    {
        if (Input.GetKey(KeyCode.W))
        {
            surfaceEffector2D.speed = boostSpeed;
        }
        else
        {
            surfaceEffector2D.speed = normalSpeed;
        }
    }

    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb2d.AddTorque(-torqueAmount);
        }
    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jumping!");
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("finish"))
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
        
    }


}
