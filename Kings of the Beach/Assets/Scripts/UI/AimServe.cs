using UnityEngine;
using KotB.Match;
using RoboRyanTron.Unite2017.Variables;

public class AimServe : MonoBehaviour
{
    [SerializeField] private Vector3 startPos = new Vector3(0, 3.5f, 0);
    [SerializeField] private Vector3Variable serveAimPosition;
    [SerializeField] private MatchInfoSO matchInfo;

    private float hideDelay = 0.5f;

    private void Start() {
        MoveToPos(startPos);
        RotateTowardSide(matchInfo.Server.CourtSide);
    }

    private void Update() {
        MoveToPos(serveAimPosition.Value);
    }

    public void MoveToPos(Vector3 newPos) {
        transform.position = newPos;
    }

    public void RotateTowardSide(float courtSide) {
        transform.eulerAngles = new Vector3(0, 90 * courtSide, 0);
    }

    public void OnHideServeAim() {
        Destroy(this.gameObject, hideDelay);        
    }
}
