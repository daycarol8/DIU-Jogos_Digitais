using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float horizontal;

    [SerializeField]
    private float speed = 8f;

    [SerializeField]
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;

    [SerializeField]
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer, enemiesLayer;

    [SerializeField] private Animator anim;

    [SerializeField] private GameObject attackPoint, gameManager;
    [SerializeField] private float radius;

    private bool isInvunerable = false;

    [SerializeField] private float health;
    [SerializeField] private Image healthBar;
    [SerializeField] private float dashAmount;
    [SerializeField] private Image dashBar;

    private void Start() {
        health = 100;
        dashAmount = 100;
        Time.timeScale = 1;
    }

    private void Update()
    {

        if (isDashing)
        {
            return;
        }

        if (dashAmount <=0){
            canDash = false;
        } else{
            canDash = true;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(horizontal));

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.X)){
            anim.SetBool("isAttacking", true);
        }

        Flip();

        if(health <= 0){
          //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          gameManager.GetComponent<GameManager>().GameOver();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        dashAmount-= 20;
        dashBar.fillAmount = dashAmount / 100f;
        Physics2D.IgnoreLayerCollision(8,7, true);
        isInvunerable = true;
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        anim.SetBool("isDashing", true);
        yield return new WaitForSeconds(dashingTime);
        anim.SetBool("isDashing", false);
        rb.gravityScale = originalGravity;
        isDashing = false;
        Physics2D.IgnoreLayerCollision(8,7, false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        isInvunerable = false;
    }

    public void Attack(){
        
        isInvunerable = true;
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemiesLayer);

        foreach (Collider2D enemyGameObject in enemy){
            if(enemyGameObject.GetComponent<EnemyController>() != null){
                enemyGameObject.GetComponent<EnemyController>().health -= 1;
            } else {
                enemyGameObject.gameObject.GetComponent<EnemyFollow>().health -= 1;
                Destroy(enemyGameObject.gameObject);
            }
            
        }
    }

    public void EndAttack(){
        anim.SetBool("isAttacking", false);
        isInvunerable = false;
    }


    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isInvunerable == false && other.gameObject.CompareTag("Enemy")){
            anim.SetTrigger("Damage");
            health-= 20;
            healthBar.fillAmount = health / 100f;
        }
        if(other.gameObject.CompareTag("Hollow")){
            anim.SetTrigger("Damage");
            health-= health;
            healthBar.fillAmount = health / 100f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Potion") && dashAmount < 100){
            dashAmount += 20;
            dashAmount = Mathf.Clamp(dashAmount, 0, 100);
            dashBar.fillAmount = dashAmount/100;
            other.gameObject.SetActive(false);
        }
    }
}