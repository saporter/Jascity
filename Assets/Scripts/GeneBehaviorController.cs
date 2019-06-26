using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneBehaviorController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform BulletAperture;
    public float RateOfFire = 0.6f;
    public float MoveSpeed = 7f;

    private Genes behaviorGenes;
    private BoxCollider radar;
    private List<Transform> encounters;  // The predators, food or warbles encountered on the track

    private void Awake()
    {
        behaviorGenes = GetComponent<Genes>();
        radar = GetComponent<BoxCollider>();
        encounters = new List<Transform>(8);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Predator" || other.tag == "Food" || other.tag == "Warble") && !encounters.Contains(other.gameObject.transform))
        {
            encounters.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (encounters.Contains(other.gameObject.transform))
        {
            encounters.Remove(other.gameObject.transform);
        }
    }

    private void OnEnable()
    {
        radar.enabled = true;
    }

    private void OnDisable()
    {
        encounters.Clear();
        radar.enabled = false;
    }

    private void Update()
    {
        Transform closest = GetClosest(encounters);
        if (closest == null)
            return;

        int behaviorGeneIndex = closest.tag == "Predator" ? 0 : closest.tag == "Food" ? 1 : 2;
        Gene gene = behaviorGenes.genes[behaviorGeneIndex];

        bool atLeastOneDominant = false;
        if((int)gene.oneHalf > 2)
        {
            atLeastOneDominant = true;
            ExecuteBehavior((int)gene.oneHalf % 3, closest);
        }
        if ((int)gene.otherHalf > 2 && ((int)gene.oneHalf % 3 != (int)gene.otherHalf % 3 || !atLeastOneDominant))
        {
            atLeastOneDominant = true;
            ExecuteBehavior((int)gene.otherHalf % 3, closest);
        }
        else if(!atLeastOneDominant)
        {
            if ((int)gene.oneHalf % 3 != (int)gene.otherHalf % 3)
            {
                ExecuteBehavior((int)gene.oneHalf % 3, closest);
                ExecuteBehavior((int)gene.otherHalf % 3, closest);
            }
            else
            {
                ExecuteBehavior((int)gene.oneHalf % 3, closest);
            }
        }
    }

    Transform GetClosest(List<Transform> targets)
    {
        if (targets.Count <= 0)
            return null;

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void ExecuteBehavior(int behavior, Transform target)
    {
        switch((Allele)behavior)
        {
            case Allele.shoot:
                Shoot();
                break;
            case Allele.avoid:
                Avoid(target);
                break;
            case Allele.tackle:
                Tackle(target);
                break;
        }
    }

    float lastShot = 0f;
    private void Shoot()
    {
        if (Time.time - lastShot > RateOfFire)
        {
            lastShot = Time.time;
            GameObject.Instantiate(BulletPrefab, BulletAperture.position, BulletAperture.rotation, transform);
        }
    }

    private void Avoid(Transform target)
    {
        float delta = transform.localPosition.x - target.localPosition.x;
        float move = delta > 0f ? MoveSpeed : -MoveSpeed;
        //move = Mathf.Abs(delta) > MoveSpeed ? move : delta;
        transform.localPosition += Vector3.right * move * Time.deltaTime;
    }

    private void Tackle(Transform target)
    {
        float delta = target.localPosition.x - transform.localPosition.x;
        float move = delta > 0f ? MoveSpeed : -MoveSpeed;
        //move = Mathf.Abs(delta) > MoveSpeed ? move : delta;
        transform.localPosition += Vector3.right * move * Time.deltaTime;
    }
}
