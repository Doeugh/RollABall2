using UnityEngine;

public class AttemptObjectController : MonoBehaviour
{
    [SerializeField] private DalgonaController controller;

    [SerializeField] private GameObject twoAttemptsObject;
    [SerializeField] private GameObject oneAttemptObject;
    [SerializeField] private GameObject zeroAttemptsObject;

    private int previousAttempts = -1;

    private void Start()
    {
        twoAttemptsObject.SetActive(false);
        oneAttemptObject.SetActive(false);
        zeroAttemptsObject.SetActive(false);

        UpdateObjects();
    }

    private void Update()
    {
        if (controller == null || controller.attempts == previousAttempts)
        {
            return;
        }

        UpdateObjects();
    }

    private void UpdateObjects()
    {
        if (controller == null)
        {
            return;
        }

        int attempts = controller.attempts;

        previousAttempts = attempts;

        twoAttemptsObject.SetActive(attempts <= 2);
        oneAttemptObject.SetActive(attempts <= 1);
        zeroAttemptsObject.SetActive(attempts <= 0);
    }
}
