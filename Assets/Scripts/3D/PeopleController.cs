using DG.Tweening;
using UnityEngine;

public class PeopleController : MonoBehaviour
{
    public int moveDistance = 8;

    private void FixedUpdate()
    {
        if (GameController.Instance.isMovingCalled)
        {
            MoveForward(moveDistance);
            GameController.Instance.isMovingCalled = false;
        }
    }

    private void MoveForward(float distance)
    {
        // transform.Translate(new Vector3(0,0,distance), Space.Self);
        // transform.DOLocalMove(new Vector3(transform.forward.x, transform.forward.y, transform.forward.z*distance), 2f);
        transform.DOMoveX(transform.position.x + distance, 2f);
    }
}