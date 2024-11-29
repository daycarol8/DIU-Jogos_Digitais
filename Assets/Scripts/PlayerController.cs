using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float radius;

    private bool isInvunerable = false;

    private void Update()
    {

        if (isDashing)
        {
            return;
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
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        isInvunerable = false;
    }

    public void Attack(){
        
        isInvunerable = true;
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemiesLayer);

        foreach (Collider2D enemyGameObject in enemy){
            Debug.Log("Hit");
            enemyGameObject.GetComponent<EnemyPatrol>().health -= 1;
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
        }
    }
}