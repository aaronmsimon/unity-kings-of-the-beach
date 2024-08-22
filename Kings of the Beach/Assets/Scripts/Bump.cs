using UnityEngine;

public class Bump : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Transform target;
    [SerializeField] private float height;
    [SerializeField] private float duration;
    [SerializeField] private InputReader inputReader;

    //Adds listeners for events being triggered in the InputReader script
    private void OnEnable()
    {
        inputReader.testCanceledEvent += OnBumpCanceled;
    }
    
    //Removes all listeners to the events coming from the InputReader script
    private void OnDisable()
    {
        inputReader.testCanceledEvent -= OnBumpCanceled;
    }

    private void PerformBump() {
        ball.Bump(target.position, height, duration);
    }

    //---- EVENT LISTENERS ----

    private void OnBumpCanceled()
    {
        PerformBump();
    }
}
