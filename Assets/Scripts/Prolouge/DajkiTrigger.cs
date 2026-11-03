using UnityEngine;

public class DajkiTrigger : MonoBehaviour
{
    public static bool IsInsideDakji = false;

    private void Start()
    {
        IsInsideDakji = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInsideDakji = true;
            Debug.Log("IsInsideDakji: True");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInsideDakji = false;
            Debug.Log("IsInsideDakji: False");
        }
    }
}
