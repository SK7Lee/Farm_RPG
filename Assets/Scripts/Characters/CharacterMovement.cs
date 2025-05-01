using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    protected NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void ToggleMovement(bool enabled)
    {
        agent.enabled = enabled;
    }

    public bool IsMoving()
    {
        //if the agent is disabled, return false
        if (!agent.enabled) return false;
        float v = agent.velocity.sqrMagnitude;
        return v > 0.25;

    }

    public void MoveTo(NPCLocationState locationState)
    {
        SceneTransitionManager.Location locationToMoveTo = locationState.location;
        SceneTransitionManager.Location currentLocation = SceneTransitionManager.Instance.currentLocation;

        //check if location is the same as the current location
        if (locationToMoveTo == currentLocation)
        {
            // check if the coord is the same
            NavMeshHit hit;
            //sample the nearest valid position on the Navmesh
            NavMesh.SamplePosition(locationState.coord, out hit, 10f, NavMesh.AllAreas);
            //if the npc is already where he should be just carry on
            if (Vector3.Distance(transform.position,hit.position) < 1)
            {
                return;
            }
            agent.SetDestination(hit.position);
            return;
        }

        SceneTransitionManager.Location nextLocation = LocationManager.GetNextLocation(currentLocation, locationToMoveTo);

        //find the exit point
        Vector3 destination = LocationManager.Instance.GetExitPosition(nextLocation).position;
        agent.SetDestination(destination);
    }

    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
