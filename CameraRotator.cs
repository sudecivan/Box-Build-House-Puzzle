using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float rotationAngle = 90f;
    private int currentViewIndex = 0;

    private void Start()
    {
        SetView(currentViewIndex); // Start at initial angle
    }

    public void RotateLeft()
    {
        currentViewIndex = (currentViewIndex + 3) % 4; // Equivalent to -1 mod 4
        SetView(currentViewIndex);
    }

    public void RotateRight()
    {
        currentViewIndex = (currentViewIndex + 1) % 4;
        SetView(currentViewIndex);
    }

    private void SetView(int index)
    {
        float angle = index * rotationAngle;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}

