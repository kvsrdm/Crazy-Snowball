using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public delegate void OnLevelFinished();
    public OnLevelFinished onLevelFinished;

    public delegate void OnScaleUp(float amount);
    public OnScaleUp onScaleUp;

    public delegate void OnScaleDown(float amount);
    public OnScaleDown onScaleDown;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Finish(other);
        CollectTree(other);
        CollectRock(other);
        CollectFire(other);
    }

    void Finish(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            onLevelFinished();
           
        }
    }

    void CollectTree(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            onScaleUp(0.3f);
            Destroy(other.gameObject);
        }
    }

    void CollectRock(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            onScaleUp(0.1f);
            Destroy(other.gameObject);
        }
    }

    void CollectFire(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            onScaleDown(0.3f);
            Destroy(other.gameObject);
        }
    }
}
