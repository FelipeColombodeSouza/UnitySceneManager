using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    // Inicialização
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask Tiles;

    // Movimento
    private float velIni = 7;
    public float velocidade = 7f;
    public float velocidadePulo = 11f;
    public float accel = 2f;
    private float jumpWait = 0;

    private int contadorPulo = 1;

    // Polimento
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public float fallMultiplier = 2.5f;
    public float initialGravity = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
       Pular();
    }

    private void FixedUpdate()
    {
        
        Andar();
         
    }

    private void Andar()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 direcao = new Vector2(x, y);


        if (direcao.x != 0)
        {
            if(velIni < velocidade)
            {
                velIni += accel * Time.deltaTime;
            }
            rb.velocity = new Vector2(direcao.x * velIni, rb.velocity.y);
        }
        else
        {
            velIni = 7;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Pular()
    {
        // Coyote / Pulo duplo
        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            contadorPulo = 1;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        // Pula caso esteja dentro do coyote timer ou do jump buffer
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        { 
            rb.velocity = new Vector2(rb.velocity.x, velocidadePulo);
            jumpBufferCounter = 0f;
            jumpWait = 0.2f;
        } 

        if(jumpWait > 0)
        {
            jumpWait -= Time.deltaTime;
        }

        // Pula caso tenha um contador de pulo duplo, esteja fora do chão e fora do coyote timer
        if(Input.GetButtonDown("Jump") && contadorPulo > 0 && !isGrounded() && coyoteTimeCounter <= 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, velocidadePulo);
            contadorPulo -= 1;
        }

        // Distância de pulo relativo ao soltar o botão de pulo
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        // Aumentar gravidade enquanto estiver caindo
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, Tiles);
    }
}
