using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalMovement : CharacterMovement
{
        //Time before the navmesh agent can move again
    [SerializeField] float cooldownTime;
    float cooldownTimer;
    protected override void Start()
    {
        base.Start();
        cooldownTimer = Random.Range(0, cooldownTime);
    }



    // Update is called once per frame
    void Update()
    {
        Wander();
    }
    
    void Wander()
    {
        if (!agent.enabled) return;
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {

            //Generate a random direction within a certain radius
            Vector3 randomDirection = Random.insideUnitSphere * 10f;

            //Offset the random direction by the current position
            randomDirection += transform.position;

            NavMeshHit hit;
            //Sample the nearest point on the NavMesh to ensure the animal moves on the surface
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
            Debug.Log($"Sampled position: {hit.position}");
            //Get the final destination
            Vector3 targetPos = hit.position;
            agent.SetDestination(targetPos);
            cooldownTimer = cooldownTime;
        }
    }

}
