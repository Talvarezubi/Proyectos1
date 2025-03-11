using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck; // Objeto para verificar el suelo
    public Collider2D groundCollider; // Collider que detecta el suelo
    public LayerMask groundLayer; // Capa designada para el suelo
    public Rigidbody2D rb; // Rigidbody del personaje
    private bool isGrounded;

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Cambiar la orientación del personaje según la dirección
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Jump()
    {
        // Verificar si el personaje está en el suelo
        isGrounded = groundCollider.IsTouchingLayers(groundLayer);

        // Aplicar fuerza de salto solo si está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}