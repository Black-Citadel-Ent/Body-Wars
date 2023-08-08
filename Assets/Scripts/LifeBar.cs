using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private GameObject visibleObject;
    [SerializeField] private Transform lifeBar;

    public void UpdateLife(int max, int current)
    {
        lifeBar.localScale = new Vector3(current / (float)max, 1, 1);
        visibleObject.SetActive(current != max);
    }
}