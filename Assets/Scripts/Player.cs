using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    Rigidbody rigidBody;

    public Transform target;
    private Lane currentLane = Lane.MIDDLE;
    private float moveDuration = 10f;

    public float rightLaneX;
    public float midLaneX;
    public float leftLaneX;

    enum Lane { LEFT, MIDDLE, RIGHT }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        if (transform.position.z <= target.position.z && GameManager.instance.isGameStarted)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, target.position.z), step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.angularVelocity = Vector3.zero;
            Debug.Log("Merhaba");
        }
    }

    public void MoveLeft()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        StopAllCoroutines();
        if (currentLane == Lane.LEFT && transform.position.x != leftLaneX)
        {
            StartCoroutine(MovePlayer(moveDuration, leftLaneX));
            currentLane = Lane.LEFT;
        }
        else if (currentLane == Lane.MIDDLE)
        {
            StartCoroutine(MovePlayer(moveDuration, leftLaneX));
            currentLane = Lane.LEFT;
        }
        else if (currentLane == Lane.RIGHT)
        {
            StartCoroutine(MovePlayer(moveDuration, midLaneX));
            currentLane = Lane.MIDDLE;
        }
    }

    public void MoveRight()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        StopAllCoroutines();
        if (currentLane == Lane.LEFT)
        {
            StartCoroutine(MovePlayer(moveDuration, midLaneX));
            currentLane = Lane.MIDDLE;
        }
        else if (currentLane == Lane.MIDDLE)
        {
            StartCoroutine(MovePlayer(moveDuration, rightLaneX));
            currentLane = Lane.RIGHT;
        }
        else if (currentLane == Lane.RIGHT && transform.position.x != rightLaneX)
        {
            StartCoroutine(MovePlayer(moveDuration, rightLaneX));
            currentLane = Lane.RIGHT;
        }
    }


    IEnumerator MovePlayer(float duration, float posX)
    {
        float journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
            transform.position = Vector3.Lerp(transform.position, new Vector3(posX, transform.position.y, transform.position.z), percent * 30f);
            yield return null;
        }
    }

}
