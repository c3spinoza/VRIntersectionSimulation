using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectAnimation : MonoBehaviour {
    // Components
    public Animator animator;
    public NavMeshAgent agent;
    
    // Parameters
    public float speed;
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    void Start() {
        // Get components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        // Set initial speed
        speed = agent.velocity.magnitude;
        animator.SetFloat(Speed, speed);
    }
    
    void Update() {
        // Update Speed every frame
        speed = agent.velocity.magnitude;
        animator.SetFloat(Speed, speed);
    }
}
