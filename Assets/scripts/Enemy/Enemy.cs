using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float runSpeed;
    public float normalSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;
    [HideInInspector] public Transform attacker;
    [Header("state")]
    public bool isHurt;
    public bool isDie;
    [Header("findCheck")]
    public Vector2 centreOffset;
    public Vector2 checkSize;
    public LayerMask attackLayer;
    public float checkDistance;

    [Header("Timer")]
    public float waitTime;
    [HideInInspector] public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;
    public bool isLost;


    private BaseState currentState;
    protected BaseState walkState;
    protected BaseState chaseState;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
        lostTimeCounter = lostTime;
    }
    private void OnEnable()
    {
        currentState = walkState;
        currentState.OnEnter(this);
    }
    private void OnDisable()
    {
        currentState.OnExit();
    }
    private void Update()
    {
        faceDir = new Vector3(-rb.transform.localScale.x, 0, 0);

        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDie && !wait)
            Move();
        currentState.PhysicsUpdate();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if (FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
        // else
        // {
        //     lostTimeCounter = lostTime;
        // }
    }
    public void OnTakeDamage(Transform attacktrans)
    {
        attacker = attacktrans;
        waitTimeCounter = -0.1f;
        //turn
        if (attacktrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attacktrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        isHurt = true;
        anim.SetTrigger("attack");
        Vector2 dir = new Vector2(transform.position.x - attacktrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }
    public bool FoundPlayer()
    {
        var temp = Physics2D.BoxCast((Vector2)transform.position + centreOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
        return temp;
    }
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Walk => walkState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    public IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }
    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDie = true;
    }
    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + centreOffset + new Vector2(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
