using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletScript : MonoBehaviour
{
    public float speed = 1.0f;
    public BossController boss;
    float lifetimeTimer = 0.0f;
    public float bulletLifetime = 2.5f;
    Vector3 direction;
    float rayLength = 0.03f;
    public LayerMask groundLayer = 6;

    bool moving = false;
    public bool fired = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void setDirection(Vector2 dir)
    {
        boss = this.transform.parent.gameObject.GetComponent<BossController>();
        this.direction = dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        this.lifetimeTimer = 0;
        this.moving = true;
        fired = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState != GameState.GS_GAME) { return; }
        if (moving)
        {
            this.transform.position = this.transform.position + (speed * direction * Time.deltaTime);
            this.lifetimeTimer += Time.deltaTime;
            if (lifetimeTimer >= bulletLifetime) {Explode(); }
            if (collidedWithWall()) { Explode(); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player")){
            Explode();
        }else if (other.CompareTag("PlayerBullet")){
            Explode();
        }else if (other.CompareTag("PlayerAttack"))
        {
            Explode();
        }

    }

    bool collidedWithWall()
    {
        Debug.DrawRay(this.transform.position, this.direction*rayLength, Color.white, 0);
        return (Physics2D.Raycast(this.transform.position, this.direction, rayLength, groundLayer.value));
    }

    void Explode()
    {
        // Debug.Log("Explode Called");
        animator.SetTrigger("Explode");
        moving = false;
    }

    void Hide()
    {
        gameObject.SetActive(false);
        boss.DestroyBullet(this.gameObject);
        fired = false;
    }

}
