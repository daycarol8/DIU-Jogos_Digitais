using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject pointA, pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    [SerializeField] private float speed;

    public float health;
    public float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        currentHealth = health;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform){
            rb.linearVelocity = new Vector2(speed, 0);
        } else{
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform){
            Flip();
            currentPoint = pointA.transform;
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform){
            Flip();
            currentPoint = pointB.transform;
        }

        if(health < currentHealth){
            currentHealth = health;
            anim.SetTrigger("Attacked");
        }

        if(health <= 0){
          anim.SetBool("isDead", true); 
        }
    }

    private void Flip(){
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Dead(){
        this.gameObject.SetActive(false);
    }
}
