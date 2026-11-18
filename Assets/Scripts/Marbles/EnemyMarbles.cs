using UnityEngine;

public class EnemyMarbles : MonoBehaviour
{
    [Range(1, 19)]
    public int startingMarbles = 10;

    [SerializeField]
    private int currentMarbles;

    public int CurrentMarbles => currentMarbles;

    private void Awake()
    {
        currentMarbles = Mathf.Clamp(startingMarbles, 1, 19);
    }

    public void AddMarbles(int amount)
    {
        if (amount <= 0)
            return;

        currentMarbles += amount;
    }

    public void RemoveMarbles(int amount)
    {
        if (amount <= 0)
            return;

        currentMarbles = Mathf.Max(0, currentMarbles - amount);
    }

    public int GetHandAmount()
    {
        if (currentMarbles <= 0)
            return 0;

        return Random.Range(1, currentMarbles + 1);
    }
}
