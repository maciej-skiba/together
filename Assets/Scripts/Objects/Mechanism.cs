using UnityEngine;

abstract public class Mechanism : MonoBehaviour
{
    abstract public bool IsAnimating { protected set; get; }
    abstract public bool IsOpening { protected set; get; } // TO CHANGE: not generic, but gate-specified

    abstract public void On();

    abstract public void Off();
}
