using UnityEngine;

public class WaterRising : MonoBehaviour
{
    [Header("Water Rising Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, 0f);  // The amount by which the water will rise

    [Header("Water Rising Speed")]
    [SerializeField] private float speed = 2f;

    [Header("Water Position")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 endPosition;

    private bool isRising = true;

    private void Awake()
    {
       transform.localPosition = startPosition;
       currentPosition = startPosition;
       endPosition = startPosition + offset;
    }

    private void Update()
    {
        if (!isRising) return;

        currentPosition = Vector3.MoveTowards(currentPosition, endPosition, speed * Time.deltaTime);
        transform.localPosition = currentPosition;

        if (currentPosition == endPosition)
        {
            isRising = false;
        }
    }

}


   

