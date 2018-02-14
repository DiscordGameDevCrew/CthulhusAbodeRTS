using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour
{

    Vector3 previous;
    [SerializeField]
    float velocity;
    Vector3 velocityVec;
    [SerializeField]
    Vector3 ahead;
    Vector3 ahead2;
    Vector3 steering;

    [SerializeField]
    private Transform[] obstacles;

    // How many units ahead should we check?
    float MAX_SEE_AHEAD = 5;
    // How intensely should we avoid an obstacle?
    float MAX_AVOID_FORCE = 3;
    // What's the maximum force with which we can turn?
    float MAX_FORCE = 2;
    // What is our unit mass?
    float UNIT_MASS = 10;
    // What is our max speed?
    float MAX_SPEED = 5;
    // At what velocity is our collision checking at maximum distance?
    float MAX_VELOCITY = 1f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        steering = Vector3.zero;
        steering += Seek();
        steering += CollisionAvoidance();
        steering = Vector3.ClampMagnitude(steering, MAX_FORCE);
        steering /= UNIT_MASS;

        velocityVec = Vector3.ClampMagnitude(velocityVec + steering, MAX_SPEED);

        transform.Translate(velocityVec * Time.deltaTime);
    }

    private Vector3 Seek()
    {
        return new Vector3(0, 0, 1);
    }

    #region Collision Avoidance
    private float DistanceEuclidean(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
    }

    private bool LineIntersectsCircle(Vector3 ahead, Vector3 ahead2, Transform obstacle)
    {
        return Vector3.Distance(obstacle.position, ahead) <= obstacle.GetComponent<Obstacle>().radius || Vector3.Distance(obstacle.position, ahead2) <= obstacle.GetComponent<Obstacle>().radius;
    }

    private Vector3 CollisionAvoidance()
    {
        float dynamic_length = velocityVec.magnitude / MAX_VELOCITY;
        ahead = transform.position + Vector3.Normalize(velocityVec) * dynamic_length;
        ahead2 = transform.position + Vector3.Normalize(velocityVec) * dynamic_length * 0.5f;

        Transform mostThreatening = FindMostThreateningObstacle();
        Vector3 avoidance = new Vector3(0, 0, 0);

        if (mostThreatening != null)
        {
            avoidance.x = ahead.x - mostThreatening.position.x;
            avoidance.z = ahead.z - mostThreatening.position.z;

            avoidance = Vector3.Normalize(avoidance);
            avoidance *= MAX_AVOID_FORCE;
        }
        else
        {
            avoidance *= 0;
        }

        return avoidance;
    }

    private Transform FindMostThreateningObstacle()
    {
        Transform mostThreatening = null;

        for (int i = 0; i < obstacles.Length; i++)
        {
            Transform obs = obstacles[i];
            bool collision = LineIntersectsCircle(ahead, ahead2, obs);

            // If we're already on a collision course AND there isn't anything closer to us that we should be avoiding.
            if (collision && (mostThreatening == null || DistanceEuclidean(transform.position, obs.position) < DistanceEuclidean(transform.position, mostThreatening.position)))
            {
                mostThreatening = obs;
            }
        }

        return mostThreatening;
    }

    #endregion
}
