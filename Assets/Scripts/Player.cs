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

    private PlayerCollision playerCollision;

    private float playerScale = 1;

    enum Lane { LEFT, MIDDLE, RIGHT }

    private void Awake()
    {
        playerCollision = GetComponent<PlayerCollision>();
        playerCollision.onLevelFinished += OnLevelFinishedListener;
        playerCollision.onScaleUp += OnScaleUp;
        playerCollision.onScaleDown += OnScaleDown;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        playerCollision.onLevelFinished -= OnLevelFinishedListener;
        playerCollision.onScaleUp -= OnScaleUp;
        playerCollision.onScaleDown -= OnScaleDown;
    }
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

        if (transform.localScale.x < playerScale)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
        }
    }

    private void OnLevelFinishedListener()
    {
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        rigidBody.angularVelocity = Vector3.zero;
        Debug.Log("Level Finished");
    }

    private void OnScaleUp(float amount)
    {
        playerScale += amount;
        //transform.localScale = Vector3.one * playerScale;

    }
    private void OnScaleDown(float amount)
    {
        playerScale -= amount;
        //transform.localScale = Vector3.one * playerScale;
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
