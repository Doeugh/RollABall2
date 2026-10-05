using UnityEngine;
using System.Collections;

public class MainMenuCamera : MonoBehaviour
{
    public Transform startingPoint;
    public Transform mainStoryWaypoint;
    public Transform RLGLWaypoint;
    public Transform dalognaWaypoint;
    public Transform tugOFWarWaypoint;
    public Transform jumpRopeWaypoint;
    public Transform MingleWaypoint;
    public Transform SquidGameWaypoint;

    public float moveDuration = 2f;
    public MainMenuUIManager uiManager;

    void Start()
    {
        transform.localPosition = startingPoint.localPosition;
    }

    public void GoToMainStory()
    {
        uiManager.HideAllUI();

        StartCoroutine(MoveToWaypoint(mainStoryWaypoint));
    }

    public void GoToStartingPoint()
    {
        uiManager.HideAllUI();

        StartCoroutine(MoveToWaypoint(startingPoint));
    }

    public void GoToRLGL()
    {
        uiManager.HideAllUI();

        StartCoroutine(MoveToWaypoint(RLGLWaypoint));
    }

    public void GoToDalgona()
    {
        uiManager.HideAllUI();

        StartCoroutine(MoveToWaypoint(dalognaWaypoint));
    }

    IEnumerator MoveToWaypoint(Transform waypoint)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float time = elapsedTime / moveDuration;

            transform.position = Vector3.Lerp(startPosition, waypoint.position, time);

            transform.rotation = Quaternion.Slerp(startRotation, waypoint.rotation, time);

            yield return null;
        }

        transform.position = waypoint.position;
        transform.rotation = waypoint.rotation;

        ShowUIForWaypoint(waypoint);
    }

    void ShowUIForWaypoint(Transform waypoint)
    {
        if (waypoint == mainStoryWaypoint)
        {
            uiManager.ShowMainStoryUI();
        }
        else if (waypoint == startingPoint)
        {
            uiManager.ShowMainButtons();
        }
        else if (waypoint == RLGLWaypoint)
        {
            uiManager.ShowRLGLUI();
        }
        else if (waypoint == dalognaWaypoint)
        {
            uiManager.ShowDalgonaUI();
        }

    }
}