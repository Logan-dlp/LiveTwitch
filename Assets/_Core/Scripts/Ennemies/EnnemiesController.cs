using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnnemiesController : MonoBehaviour
{
    /*
     * (2 modes)
     * Tableau de point que l'ennemie vas suivre (Quand il ne voit pas le joueur) *
     * effet de surprise quand il voit un joueur (le voit directement s'il a la lumière allumer)
     * il suit le joueur jusqu'a un certain rayon
     * infflige des degats au joueur
     * option : si le joueur atteint un certain rayon l'ennemis arrete de le suivre
     */

    private Animator animator;
    private NavMeshAgent agent;
    public Transform[] Destination;
    private int increment;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovementPNJ();
    }

    void MovementPNJ()
    {
        agent.speed = 2.5f;
        animator.SetBool("Walking", true);
        int _maxValue = Destination.Length;
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(Destination[increment].position.x, Destination[increment].position.z)) <= 2)
        {
            increment ++;
            if (increment >= _maxValue) increment = 0;
            agent.destination = Destination[increment].position;
        }
        else if (increment == 0)
        {
            agent.destination = Destination[increment].position;
        }
    }
}
