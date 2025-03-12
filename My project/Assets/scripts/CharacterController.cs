using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck; // Transform para verificar el suelo
    public LayerMask groundLayer; // Capa designada para el suelo
    public Rigidbody2D rb; // Rigidbody del personaje
    public int health = 3; // Vida del jugador
    public float knockbackForce = 5f; // Fuerza con la que el enemigo empuja al jugador
    private bool isGrounded;
    private bool isKnockbackActive = false; // Bandera para controlar el knockback

    void Update()
    {
        if (!isKnockbackActive) // Bloquear movimiento y salto durante el knockback
        {
            Move();
            Jump();
        }
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Aplicar fuerza de salto solo si está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
            Knockback(collision.transform.position);
        }
    }

    void TakeDamage()
    {
        health--;
        Debug.Log("Vida restante: " + health);
        if (health <= 0)
        {
            Debug.Log("¡Jugador derrotado!");
            gameObject.SetActive(false); // Desactiva el personaje
        }
    }

    void Knockback(Vector3 enemyPosition)
    {
        // Calcular dirección del knockback
        Vector2 knockbackDirection = (transform.position - enemyPosition).normalized;
        
        // Aplicar knockback
        rb.linearVelocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackDirection.y * knockbackForce);

        // Activar la bandera para evitar entrada de usuario durante el knockback
        isKnockbackActive = true;

        // Desactivar el knockback después de un corto periodo de tiempo
        Invoke("EndKnockback", 0.2f);
    }

    void EndKnockback()
    {
        isKnockbackActive = false; // Permitir nuevamente el movimiento y salto
    }
}