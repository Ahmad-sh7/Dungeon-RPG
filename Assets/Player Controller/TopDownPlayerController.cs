using UnityEngine;
using UnityEngine.Tilemaps;

public class TopDownPlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float runMultiplier = 1.5f;
    

    //[SerializeField] private Tilemap collisionTilemap;
    //[SerializeField] private string collisionLayerName;
    


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door1"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(-7.749203f, -7.414156f, 0f);
        }
        if (collision.gameObject.CompareTag("Door3"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(-0.5699998f, -3.31f, 0f);
        }
        if (collision.gameObject.CompareTag("Door2"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(13.74919f, 2.6414481f, 0f);
        }
        if (collision.gameObject.CompareTag("Door4"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(13.57595f, -15.28938f, 0f);
        }
        if (collision.gameObject.CompareTag("Door5"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(6.066393f, 2.836445f, 0f);
        }
        if (collision.gameObject.CompareTag("Door6"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(27.24244f, -7.389411f, 0f);
        }
        if (collision.gameObject.CompareTag("Door7"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(28.23417f, -2.243476f, 0f);
        }
        if (collision.gameObject.CompareTag("Door8"))
        {
            // Trigger the event or action you desire
            transform.position = new Vector3(6.165025f, -14.22836f, 0f);
        }
    }
}