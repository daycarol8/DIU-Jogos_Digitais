using System.Collections;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;

    private Transform target;
    public float health;
    public float currentHealth;
    private float distance;

    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        transform.position = Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);

        if(health < currentHealth){
            currentHealth = health;
            anim.SetTrigger("Attacked");
        }

        if(health <= 0){
          anim.SetBool("isDead", true); 
        }
    }
}
