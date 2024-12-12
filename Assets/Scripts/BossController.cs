using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject ToxicArea;
    [SerializeField] private PlayerController Player;

    public float currentHealth;
    private float maxHealth = 200;
    public GameObject Projectile;
    public Transform LaunchOffSet;
    public float forceAmount = 7f;

    State currentState;
    Dictionary<States, State> statesDict = new Dictionary<States, State>();

    // ready
    void Start() 
    {
        // inicializar datos boss
        currentHealth = maxHealth;
        
        // inicializar estados:
        //      definir estado inicial
        currentState = new FollowState(this);
        currentState.Entry();
        //      crear lista de estados
        statesDict.Add(States.Follow, currentState);
        statesDict.Add(States.Rage, new RageState(this));
        statesDict.Add(States.Spit, new SpitState(this));
        statesDict.Add(States.Burp, new BurpState(this));
        statesDict.Add(States.Recovery, new RecoveryState(this));
        
        //      preparar sistema de eventos
    }

    // process
    void Update()
    {
        // llamar update del estado actual
        currentState.Update();
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public void ChangeStateKey(States newState)
    {
        if(statesDict.ContainsKey(newState))
        {
            ChangeState(statesDict[newState]);
        }
        else
        {
            Debug.LogWarning("State not in list.");
        }
    }
    
    void ChangeState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Entry();
    }

    public void Shoot()
    {
        GameObject projectileInstance = Instantiate(Projectile, LaunchOffSet.position, transform.rotation);
        Rigidbody2D rigidBody = projectileInstance.GetComponent<Rigidbody2D>();

        if (rigidBody)
        {
            rigidBody.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
        }
    }

}

public enum States
{
    Follow, Spit, Burp, Recovery, Rage,
}

