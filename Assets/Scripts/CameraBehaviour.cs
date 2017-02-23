using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Creates a an invisible box that the player pushes around to control the camera.
 *  Behaves somewhat like the camera in Super Mario World
 */

public class CameraBehaviour : MonoBehaviour {

    [SerializeField] CharacterController target;
    [SerializeField] Vector2 focusAreaSize  = new Vector2(2.5f, 8.0f);
    [SerializeField] float lookAheadDistX   = 3.0f;
    [SerializeField] float smoothTimeX      = 0.2f;
    [SerializeField] float verticalOffset   = 1.5f;

    FocusArea focusArea;

    float currentLookAheadX     = 0;
    float targetLookAheadX      = 0;
    float lookAheadDirX         = 0;
    float smoothLookVelocityX   = 0;

    void Start()
    {
        if (!target)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        focusArea = new FocusArea(target.bounds, focusAreaSize);
    }

    // Update camera after most of the game logic and user input
    void LateUpdate()
    {
        focusArea.Update(target.bounds);

        if (focusArea.velocity.x != 0)  // Mathf.Sign(0) == 1
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);

        targetLookAheadX = lookAheadDirX * lookAheadDistX;
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, smoothTimeX);

        Vector3 cameraFocusPosition = new Vector3(focusArea.centre.x,
                                                  focusArea.centre.y,
                                                  transform.position.z);
        cameraFocusPosition += Vector3.up * verticalOffset;
        cameraFocusPosition += Vector3.right * currentLookAheadX;

        this.transform.position = cameraFocusPosition;
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;

        float left, right;
        float top, bottom;

        public FocusArea(Bounds bounds, Vector2 size)
        {
            left = bounds.center.x - (size.x / 2.0f);
            right = bounds.center.x + (size.x / 2.0f);

            bottom = bounds.min.y;
            top = bounds.min.y + size.y;

            centre = new Vector2((left + right) / 2.0f, (top + bottom) / 2.0f);
            velocity = new Vector2();
        }

        public void Update(Bounds targetBounds)
        {
            float dx = .0f;

            if (targetBounds.min.x < left)
                dx = targetBounds.min.x - left;
            else if (targetBounds.max.x > right)
                dx = targetBounds.max.x - right;

            left += dx;
            right += dx;

            float dy = .0f;

            if (targetBounds.min.y < bottom)
                dy = targetBounds.min.y - bottom;
            else if (targetBounds.max.y > top)
                dy = targetBounds.max.y - top;

            bottom += dy;
            top += dy;

            centre.x = (left + right) / 2.0f;
            centre.y = (top + bottom) / 2.0f;

            velocity.x = dx;
            velocity.y = dy;
        }
    }
}
