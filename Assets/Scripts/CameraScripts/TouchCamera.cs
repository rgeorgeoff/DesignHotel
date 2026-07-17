using System;
using System.Collections;
using System.Linq;
using Managers;
using UnityEngine;

namespace CameraScripts
{
    public class TouchCamera : MonoBehaviour
    {
        private Vector3 lerpTotal;
        private float lerpTotalRotation;
        float lerpDrag = .1f;
        float lerpRotationDrag = .1f;
        float killLerpValue = .04f;
        bool cameraMovementInputEnabled = true;
        private Vector3 lastFrameCamPos = Vector3.zero;
        private float panSlower = .6f;
        private bool lateralMovementIsRotation = true;
        private Transform transformRotationParent;
        float rotationScale = 2f;
        public Vector3 dtm;
        private Camera mainCamera;
        public Transform EditCameraTransform;
        public Transform RegularCameraTransform;

        private Vector2 currentMousePositon = Vector2.zero;
        private Vector2 lastMousePositon = Vector2.zero;

        private float screenTransitionTime = 0.5f;

        private float zoomInMin = 8;
        private float zoomOutMax = 20;
        private float zoomOutMinHorizontal = 8;
        private float zoomOutMaxHorizontal = 20;
        private float zoomOutMinVertical = 15;
        private float zoomOutMaxVertical = 40;
        private float scrollSpeed = 0.01f;
        private float scrollSpeedMouse = 10f;
        private float scrollSpeedHorizontal = .01f;
        private float scrollSpeedVertical = .010f;

//        private float zoomForEdit = 15;
//        private float zoomForEditHorizontal = 12;
//        private float zoomForEditVertical = 20;
//        
//        private float zoomForRegular = 16;
//        private float zoomForRegularHorizontal = 16;
//        private float zoomForRegularVertical = 30;

        private bool lockedHorizontalMovement;
        private bool lockedVerticalMovement;
        


        private void Awake()
        {
            GameMainManager.Instance.EventManager.StartListening(
                EventManager.EventName.DisableCameraMovement.ToString(),
                DisableCamera);
            GameMainManager.Instance.EventManager.StartListening(EventManager.EventName.EnableCameraMovement.ToString(),
                EnableCamera);
            GameMainManager.Instance.EventManager.StartListening(
                EventManager.EventName.RoomSelectedForEditing.ToString(),
                ZoomToRoomEdit);
            GameMainManager.Instance.EventManager.StartListening(
                EventManager.EventName.RoomDeselected.ToString(),
                ZoomToNormal);
            transformRotationParent = transform.parent;
            Screen.autorotateToPortrait = true;
            mainCamera = GetComponent<Camera>();
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.orientation = ScreenOrientation.AutoRotation;

            UpdateCameraLandscapeOrVertical();
            //get camera in right position to start

            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomInMin, zoomOutMax);
        }

        void Update()
        {
            UpdateCameraLandscapeOrVertical();

            if (cameraMovementInputEnabled)
                MoveCameraFromInput();
            //if the camera has moved at all, send out the event that the camera is moving.
            if (lastFrameCamPos != transform.position)
                GameMainManager.Instance.EventManager.TriggerEvent(this, EventManager.EventName.CameraMoved.ToString(),
                    new CameraMovedEventArgs(transform.position));
            lastFrameCamPos = transform.position;
        }

        void UpdateCameraLandscapeOrVertical()
        {
            if (new[] {ScreenOrientation.Landscape, ScreenOrientation.LandscapeLeft, ScreenOrientation.LandscapeLeft}
                    .Contains(Screen.orientation) || Screen.width > Screen.height)
            {
                zoomOutMax = zoomOutMaxHorizontal;
                zoomInMin = zoomOutMinHorizontal;
                scrollSpeed = scrollSpeedHorizontal;
//                zoomForEdit = zoomForEditHorizontal;
            }
            else if (new[] {ScreenOrientation.Portrait, ScreenOrientation.PortraitUpsideDown}.Contains(
                Screen.orientation) || Screen.width < Screen.height)
            {
                zoomOutMax = zoomOutMaxVertical;
                zoomInMin = zoomOutMinVertical;
                scrollSpeed = scrollSpeedVertical;
//                zoomForEdit = zoomForEditVertical;
            }
        }

        void DisableCamera(object sender, EventArgs e)
        {
            cameraMovementInputEnabled = false;
        }

        void EnableCamera(object sender, EventArgs e)
        {
            cameraMovementInputEnabled = true;
        }

        private void MoveCameraFromInput()
        {
            currentMousePositon = Input.mousePosition;
            if (Input.touches.Length == 0 && !Input.GetMouseButton(0))
            {
                if (Math.Abs(lerpTotal.magnitude) > killLerpValue)
                    LerpEnd(lerpTotal);
                if (Math.Abs(lerpTotalRotation) > killLerpValue)
                    LerpEndRotation(lerpTotalRotation);
            }
            else if (Input.touches.Length == 1)
            {
                Touch touchZero = Input.GetTouch(0);
                HandleOneFingerDrag(touchZero.deltaPosition);
            }
            else if (Input.GetMouseButton(0) && Input.touches.Length == 0)
            {
                Vector2 deltaMousePositon = currentMousePositon - lastMousePositon;
                HandleOneFingerDrag(deltaMousePositon);
            }
            else
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                if (mainCamera.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    mainCamera.orthographicSize += deltaMagnitudeDiff * scrollSpeed;

                    // Make sure the orthographic size never drops below zero.
                    mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, zoomInMin);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    mainCamera.fieldOfView += deltaMagnitudeDiff * scrollSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, zoomInMin, zoomOutMax);
                }
            }

            if (Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > .001)
                zoom(Input.GetAxis("Mouse ScrollWheel") * scrollSpeedMouse);
            lastMousePositon = currentMousePositon;
        }

        private void HandleOneFingerDrag(Vector2 touchDelta)
        {
            // ReSharper disable once PossibleInvalidOperationException
            Vector3 localVector = (Vector3) (-touchDelta *
                                             mainCamera.fieldOfView /
                                             mainCamera.pixelHeight * 2f) * panSlower;
            Vector3 distanceToMove = transform.TransformDirection(localVector);
            dtm = distanceToMove;
            if (lateralMovementIsRotation)
            {
                //translate position on Y axis
                if (!lockedVerticalMovement)
                {
                    lerpTotal = Vector3.Scale(distanceToMove, Vector3.up);
                    transformRotationParent.position += lerpTotal;
                }
                //rotate from parent root
                if (!lockedHorizontalMovement)
                {
                    transformRotationParent.Rotate(Vector3.down, localVector.x * rotationScale);
                    lerpTotalRotation = localVector.x * rotationScale;
                }
            }
            else
            {
                lerpTotal = distanceToMove;
                transformRotationParent.position += distanceToMove;
            }
        }

        void zoom(float increment)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment, zoomInMin, zoomOutMax);
        }

        void LerpEnd(Vector3 distanctToMoveInput)
        {
            transformRotationParent.position += distanctToMoveInput;
            lerpTotal = lerpTotal * (1 - lerpDrag);
        }

        void LerpEndRotation(float rotation)
        {
            transformRotationParent.Rotate(Vector3.down, rotation * rotationScale);
            lerpTotalRotation = lerpTotalRotation * (1 - lerpRotationDrag);
        }

        public void ZoomToRoomEdit(object sender, EventArgs e)
        {
            lockedVerticalMovement = true;
            float targetHeight = ((RoomSelectedEventArgs) e).RoomController.GetWorldPosition().y;
            Debug.Log(targetHeight);
//            StartCoroutine(ZoomToMin(zoomForEdit, screenTransitionTime, true));
            ZoomToHeight(targetHeight, screenTransitionTime, true);
            ZoomToTarget(mainCamera.transform, EditCameraTransform, screenTransitionTime, true);
        }

        public void ZoomToNormal(object sender, EventArgs e)
        {
            lockedVerticalMovement = false;
//            StartCoroutine(ZoomToMin(zoomForRegular, time, interuptable));
            ZoomToTarget(mainCamera.transform, RegularCameraTransform, screenTransitionTime, true);
        }
        
        public void ZoomToTarget(Transform transformToMove, Transform goalTransform, float time, bool interuptable)
        {
            //StartCoroutine(ZoomToMin(zoomForRegular, time, interuptable));
            StartCoroutine(ZoomToRotation(transformToMove, goalTransform.localRotation, time, interuptable));
            StartCoroutine(ZoomToLocalPosition(transformToMove, goalTransform.localPosition, time, interuptable));
        }

        public void ZoomToHeight(float targetHeight, float screenTransitionTime, bool interuptable)
        {
            StartCoroutine(MoveParentToHeight(targetHeight, screenTransitionTime, interuptable));
        }

        public IEnumerable PauseCameraControlForTime(float seconds, bool canBeInterupted)
        {
            cameraMovementInputEnabled = false;
            float t = 0;
            if (canBeInterupted)
            {
                while (!HasCameraInteruption() && t <= 1)
                {
                    t += Time.deltaTime / seconds;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                yield return new WaitForSeconds(seconds);
            }            
            cameraMovementInputEnabled = true;
        }

        public IEnumerator MoveParentToHeight(float targetHeight, float seconds, bool canBeInterupted)
        {
            float startheight = transformRotationParent.position.y;
            float t = 0;
            while ((!HasCameraInteruption() || !canBeInterupted) && t <= 1)
            {
                t += Time.deltaTime / seconds;
                transformRotationParent.position = new Vector3(transformRotationParent.position.x, Mathf.Lerp(startheight, targetHeight, Mathf.SmoothStep(0.0f, 1.0f, t)), transformRotationParent.position.z);
                yield return new WaitForEndOfFrame();
            }
        }
        
        public IEnumerator ZoomToMin(float goalZoom, float seconds, bool canBeInterupted)
        {
            float startZoom = mainCamera.fieldOfView;
            float t = 0;
            while ((!HasCameraInteruption() || !canBeInterupted) && t <= 1)
            {
                t += Time.deltaTime / seconds;
                mainCamera.fieldOfView = Mathf.Lerp(startZoom, goalZoom, Mathf.SmoothStep(0.0f, 1.0f, t));
                yield return new WaitForEndOfFrame();
            }
        }
        
        public IEnumerator ZoomToRotation(Transform objToTransform, Quaternion goalRotation, float seconds, bool canBeInterupted)
        {
            Quaternion startRotation = objToTransform.localRotation;
            float t = 0;
            while ((!HasCameraInteruption() || !canBeInterupted) && t <= 1)
            {
                t += Time.deltaTime / seconds;
                objToTransform.transform.localRotation = Quaternion.Lerp(startRotation, goalRotation, Mathf.SmoothStep(0.0f, 1.0f, t));
                yield return new WaitForEndOfFrame();
            }
        }
        
        public IEnumerator ZoomToLocalPosition(Transform objToTransform, Vector3 goalPosition, float seconds, bool canBeInterupted)
        {
            Vector3 startPos = objToTransform.transform.localPosition;
            float t = 0;
            while ((!HasCameraInteruption() || !canBeInterupted) && t <= 1)
            {
                t += Time.deltaTime / seconds;
                objToTransform.transform.localPosition = Vector3.Lerp(startPos, goalPosition, Mathf.SmoothStep(0.0f, 1.0f, t));
                yield return new WaitForEndOfFrame();
            }
        }

        public bool HasCameraInteruption()
        {
            return Input.touchCount != 0 || Input.anyKey || Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.001f;
        }

        public void ToggleHorizontalMovement(bool lockMovement)
        {
            lockedHorizontalMovement = lockMovement;
        }

//    public IEnumerator RotateToRoom(Transform roomTrans, float seconds)
//    {
//        Vector3 startPos = this.transform.position;
//        float t = 0;
//        while (!Input.anyKey && t <= 1)
//        {
//            t += Time.deltaTime/seconds;
//            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0, 1.0, t));
//            yield;
//            yield return new WaitForEndOfFrame();
//        }
//    }
    }
}
