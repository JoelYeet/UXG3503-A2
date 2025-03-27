using UnityEngine;
using UnityEngine.Splines; // Ensure Unity's Spline package is imported
using System.Collections;

public class MultiSplineControllerWithTransition : MonoBehaviour
{
    // An array of splines (SplineContainers) to follow consecutively.
    public SplineContainer[] splines;

    // Speed at which the object moves along the spline (in normalized units per second).
    public float speed = 0.2f;

    // Duration of the lerp transition to the next spline's start.
    public float transitionDuration = 1f;

    // The index of the current active spline.
    private int currentSplineIndex = 0;

    // Linear progress along the current spline (from 0 to 1).
    private float t = 0f;

    // Flag to determine if the object is transitioning between splines.
    private bool isTransitioning = false;

    void Update()
    {
        // Skip normal spline movement while transitioning.
        if (isTransitioning)
            return;

        // Ensure there is at least one spline.
        if (splines == null || splines.Length == 0)
            return;

        // Increment progress along the current spline.
        t += speed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        // Apply easing to create a smooth ease-in/ease-out effect.
        float easedT = Mathf.SmoothStep(0f, 1f, t);

        // Get the current spline.
        SplineContainer currentSpline = splines[currentSplineIndex];

        // Evaluate position along the spline using the eased progress.
        Vector3 pos = currentSpline.EvaluatePosition(easedT);

        // Option 2: Compute rotation manually using the tangent and up vector.
        Vector3 tangent = currentSpline.EvaluateTangent(easedT);
        // If your spline does not provide an up vector, you can simply use Vector3.up.
        Vector3 upVector = currentSpline.EvaluateUpVector(easedT);
        Quaternion rot = Quaternion.LookRotation(tangent, upVector);

        // Update the object's position and rotation.
        transform.SetPositionAndRotation(pos, rot);

        // When reaching the end of the current spline...
        if (t >= 1f)
        {
            if (currentSplineIndex < splines.Length - 1)
            {
                // Transition to the next spline.
                StartCoroutine(LerpToNextSplineStart());
            }
            else
            {
                // Final spline completed.
                OnAllSplinesCompleted();
            }
        }
    }

    // Coroutine to smoothly move the object to the start of the next spline.
    private IEnumerator LerpToNextSplineStart()
    {
        isTransitioning = true;

        // Determine the next spline.
        SplineContainer nextSpline = splines[currentSplineIndex + 1];

        // Target position and rotation at the start of the next spline (t = 0).
        Vector3 targetPos = nextSpline.EvaluatePosition(0f);
        Vector3 targetTangent = nextSpline.EvaluateTangent(0f);
        Vector3 targetUpVector = nextSpline.EvaluateUpVector(0f); // Or use Vector3.up if needed
        Quaternion targetRot = Quaternion.LookRotation(targetTangent, targetUpVector);

        // Capture current position and rotation.
        Vector3 initialPos = transform.position;
        Quaternion initialRot = transform.rotation;

        float elapsed = 0f;

        // Lerp over the specified transition duration.
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float lerpT = Mathf.Clamp01(elapsed / transitionDuration);

            transform.position = Vector3.Lerp(initialPos, targetPos, lerpT);
            transform.rotation = Quaternion.Lerp(initialRot, targetRot, lerpT);

            yield return null;
        }

        // Ensure the object is exactly at the target position and rotation.
        transform.position = targetPos;
        transform.rotation = targetRot;

        // Advance to the next spline.
        currentSplineIndex++;
        t = 0f; // Reset progress for the new spline.
        isTransitioning = false;
    }

    // Called when all splines have been traversed.
    void OnAllSplinesCompleted()
    {
        Debug.Log("All splines completed!");
        // Insert any additional end-of-sequence logic here.
    }
}
