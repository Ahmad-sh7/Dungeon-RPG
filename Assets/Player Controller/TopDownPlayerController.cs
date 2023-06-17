using UnityEngine;

public class TopDownPlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float runMultiplier = 1.5f;

    Rigidbody2D rb;
    Collider2D col;
    Animator anim;

    Vector2 moveVector;
    bool isGod = false;
    bool isSprinting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() => rb.velocity = speed * moveVector;

    private void Update()
    {
        GetInput();
        SetAnimations();
    }

    private void GetInput()
    {
        if (Input.GetButtonDown("Debug Previous"))
        {
            isGod = !isGod;
            col.enabled = !isGod;            
        }

        isSprinting = Input.GetButton("Fire3");
      
        moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (isGod) moveVector *= 5f;
        if (isSprinting) moveVector *= runMultiplier;

    }

    private void SetAnimations()
    { 
        // If the player is moving
        if (moveVector != Vector2.zero)
        {
            // Trigger transition to moving state
            anim.SetBool("IsMoving", true);

            // Set X and Y values for Blend Tree
            anim.SetFloat("MoveX", moveVector.x);
            anim.SetFloat("MoveY", moveVector.y);
        }
        else
            anim.SetBool("IsMoving", false);
    }
}