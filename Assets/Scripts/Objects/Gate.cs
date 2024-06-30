using UnityEngine;
using System.Collections;

public class Gate : Mechanism
{
    public override bool IsAnimating { protected set; get; }
    public override bool IsOpening { protected set; get; }

    protected Vector3 deltaVector;

    private readonly float openCloseTimeInSeconds = 1.0f;
    private readonly float coroutineDeltaTime = 0.02f;
    private WaitForSeconds coroutineWaitForSecs;
    private Vector3 closedGatePosition;
    private Vector3 openedGatePosition;
    private bool gateIsOpen;
    private bool gateIsOpening;

    private void Awake()
    {
        coroutineWaitForSecs = new WaitForSeconds(coroutineDeltaTime);

        float openCloseGateDistanceDelta = openCloseTimeInSeconds / coroutineDeltaTime;
        deltaVector = new Vector3(0, GetComponent<Renderer>().bounds.size.y / openCloseGateDistanceDelta, 0);

        closedGatePosition = transform.position;
        openedGatePosition = transform.position + new Vector3(0, GetComponent<Renderer>().bounds.size.y, 0);
    }

    public override void On()
    {
        if (!gateIsOpen || IsOpening)
        {
            StopAllCoroutines();
            StartCoroutine(CoOpenGate());
        };
    }

    public override void Off()
    {
        if (gateIsOpen || gateIsOpening)
        {
            StopAllCoroutines();
            StartCoroutine(CoCloseGate());
        }
    }

    private IEnumerator CoOpenGate()
    {
        gateIsOpening = true;

        float timePassed = 0;

        while (transform.position.y < openedGatePosition.y)
        {
            transform.position += deltaVector;

            yield return coroutineWaitForSecs;
            timePassed += coroutineDeltaTime;
        }

        transform.position = openedGatePosition;

        gateIsOpen = true;
        gateIsOpening = false;
    }

    private IEnumerator CoCloseGate()
    {
        IsOpening = true;

        float timePassed = 0;

        while (transform.position.y > closedGatePosition.y)
        {
            transform.position -= deltaVector;

            yield return coroutineWaitForSecs;
            timePassed += coroutineDeltaTime;
        }

        transform.position = closedGatePosition;

        gateIsOpen = false;
        IsOpening = false;
    }
}
