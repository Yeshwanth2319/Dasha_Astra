using UnityEngine;

public class DivineFlowController : MonoBehaviour
{
    public ParticleSystem flow;

    public void Drop()
    {
        flow.Emit(1);
    }
}