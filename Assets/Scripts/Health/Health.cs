using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float staringHealth;
    public float currentHealth { get; private set; }
    private Animator  anim;
    private bool dead;


    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invuInerable;


    private void Awake()
    {
        currentHealth = staringHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, staringHealth);

        if (currentHealth > 0) 
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        
        }
        else 
        {
            if (!dead)
            {
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                dead = true;
                
            }
           

        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, staringHealth);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(staringHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());
        foreach (Behaviour component in components)
            component.enabled = true;

    }
    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < numberOfFlashes; i++) 
        {
            spriteRend.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(iFramesDuration / ( numberOfFlashes * 2 ));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / ( numberOfFlashes * 2 ));

        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

   

}
