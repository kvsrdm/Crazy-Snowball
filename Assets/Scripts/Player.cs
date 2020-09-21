using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float scaleSpeed = 2;

    public int mainCount = 100;
    public TextMeshProUGUI countText;

    enum Lane { LEFT, MIDDLE, RIGHT }

    private void Awake()
    {
        playerCollision = GetComponent<PlayerCollision>();
        playerCollision.onLevelFinished += OnLevelFinishedListener;
        playerCollision.onScaleUp += OnScaleUp;
        playerCollision.onScaleDown += OnScaleDown;
        playerCollision.onCount += OnCount;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        playerCollision.onLevelFinished -= OnLevelFinishedListener;
        playerCollision.onScaleUp -= OnScaleUp;
        playerCollision.onScaleDown -= OnScaleDown;
        playerCollision.onCount -= OnCount;
    }
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        SetCountText();
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
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        Debug.Log(System.Math.Round(transform.localScale.x, 1));

        float step = speed * Time.deltaTime;
        if (transform.position.z <= target.position.z && GameManager.instance.isGameStarted)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, target.position.z), step);
        }



        if (transform.localScale.x < playerScale)
        {
            transform.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
        }

        if (transform.localScale.x > playerScale)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
        }

        transform.Rotate(Vector3.right, Space.World);

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
    }
    private void OnScaleDown(float amount)
    {
        playerScale -= amount;
    }

    private void SetCountText()
    {
        countText.text = "Score: " + mainCount.ToString();
    }

    private void OnCount(int count)
    {
        mainCount += count;
        Debug.Log("Count: " + mainCount);
        SetCountText();
    }

    Coroutine coroutine = null;

    public void MoveLeft()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        StopAllCoroutines();

        if (currentLane == Lane.LEFT && transform.position.x != leftLaneX)
        {

            coroutine = StartCoroutine(MovePlayer(moveDuration, leftLaneX));
            currentLane = Lane.LEFT;
        }
        else if (currentLane == Lane.MIDDLE)
        {
            coroutine = StartCoroutine(MovePlayer(moveDuration, leftLaneX));
            currentLane = Lane.LEFT;
        }
        else if (currentLane == Lane.RIGHT)
        {
            coroutine = StartCoroutine(MovePlayer(moveDuration, midLaneX));
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
            coroutine = StartCoroutine(MovePlayer(moveDuration, midLaneX));
            currentLane = Lane.MIDDLE;
        }
        else if (currentLane == Lane.MIDDLE)
        {
            coroutine = StartCoroutine(MovePlayer(moveDuration, rightLaneX));
            currentLane = Lane.RIGHT;
        }
        else if (currentLane == Lane.RIGHT && transform.position.x != rightLaneX)
        {
            coroutine = StartCoroutine(MovePlayer(moveDuration, rightLaneX));
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
