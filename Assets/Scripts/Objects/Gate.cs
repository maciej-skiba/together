using UnityEngine;
using System.Collections;

public class Gate : Mechanism
{
    public override bool IsAnimating { protected set; get; }
    public override bool IsClosing { protected set; get; }

    protected Vector3 deltaVector;
    protected Vector3 deltaHorizontalVector;

    [SerializeField] private bool horizontal;
    [SerializeField] private bool invertMechanism;
    private readonly float openCloseTimeInSeconds = 1.0f;
    private readonly float coroutineDeltaTime = 0.01f;
    private int invertFactor = 1;
    private WaitForSeconds coroutineWaitForSecs;
    private Vector3 closedGatePosition;
    private Vector3 openedGatePosition;
    private Vector3 openedHorizontalGatePosition;
    private bool gateIsOpen;
    private bool gateIsOpening;

    private void Awake()
    {
        coroutineWaitForSecs = new WaitForSeconds(coroutineDeltaTime);

        if (invertMechanism)
        {
            invertFactor *= -1;
        }

        float openCloseGateDistanceDelta = openCloseTimeInSeconds / coroutineDeltaTime;
        deltaVector = new Vector3(0, GetComponent<Renderer>().bounds.size.y / openCloseGateDistanceDelta, 0) * invertFactor;
        deltaHorizontalVector = new Vector3(GetComponent<Renderer>().bounds.size.x / openCloseGateDistanceDelta, 0, 0) * invertFactor;

        closedGatePosition = transform.position;
        openedGatePosition = transform.position + new Vector3(0, GetComponent<Renderer>().bounds.size.y, 0) * invertFactor;
        openedHorizontalGatePosition = transform.position + new Vector3(GetComponent<Renderer>().bounds.size.x, 0, 0) * invertFactor;
    }

    public override void On()
    {
        if (!gateIsOpen || IsClosing)
        {
            StopAllCoroutines();
            CoOpenGate(this.horizontal);
        };
    }

    public override void Off()
    {
        if (gateIsOpen || gateIsOpening)
        {
            StopAllCoroutines();
            CoCloseGate(this.horizontal);
        }
    }

    private void CoOpenGate(bool horizontal = false)
    {
        if (horizontal)
        {
            StartCoroutine(OpenHorizontalGate());
        }
        else
        {
            StartCoroutine(OpenVerticalGate());
        }
    }

    private void CoCloseGate(bool horizontal = false)
    {
        if (horizontal)
        {
            StartCoroutine(CloseHorizontalGate());
        }
        else
        {
            StartCoroutine(CloseVerticalGate());
        }
    }

    private IEnumerator OpenHorizontalGate()
    {
        float timePassed = 0;
        gateIsOpening = true;

        if (invertMechanism)
        {
            while (transform.position.x > openedHorizontalGatePosition.x)
            {
                transform.position += deltaHorizontalVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }
        else
        {
            while (transform.position.x < openedHorizontalGatePosition.x)
            {
                transform.position += deltaHorizontalVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }

        transform.position = openedHorizontalGatePosition;
        gateIsOpen = true;
        gateIsOpening = false;
    }

    private IEnumerator OpenVerticalGate()
    {
        float timePassed = 0;
        gateIsOpening = true;

        if (invertMechanism)
        {
            while (transform.position.y > openedGatePosition.y)
            {
                transform.position += deltaVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }
        else
        {
            while (transform.position.y < openedGatePosition.y)
            {
                transform.position += deltaVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }

        transform.position = openedGatePosition;
        gateIsOpen = true;
        gateIsOpening = false;
    }

    private IEnumerator CloseHorizontalGate()
    {
        float timePassed = 0;
        IsClosing = true;

        if (invertMechanism)
        {
            while (transform.position.x < closedGatePosition.x)
            {
                transform.position -= deltaHorizontalVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }
        else
        {
            while (transform.position.x > closedGatePosition.x)
            {
                transform.position -= deltaHorizontalVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }

        transform.position = closedGatePosition;

        gateIsOpen = false;
        IsClosing = false;
    }

    private IEnumerator CloseVerticalGate()
    {
        float timePassed = 0;
        IsClosing = true;

        if (invertMechanism)
        {
            while (transform.position.y < closedGatePosition.y)
            {
                transform.position -= deltaVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }
        else
        {
            while (transform.position.y > closedGatePosition.y)
            {
                transform.position -= deltaVector;

                yield return coroutineWaitForSecs;
                timePassed += coroutineDeltaTime;
            }
        }

        transform.position = closedGatePosition;

        gateIsOpen = false;
        IsClosing = false;
    }
}
