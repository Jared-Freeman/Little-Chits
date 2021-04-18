using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: Interaction system raycasts every frame update to determine if 
 * player is looking at a interactable object in the scene.
 */

public class InteractionSystem : MonoBehaviour
{
    #region members

    [Range(1, 20)]
    public float maxInteractionDistance = 10;

    public Sprite crosshairSprite;
    [Range(5, 100)]
    public int crosshairSize = 10;

    public GameObject focusUIObject;
    private Text focusUIText;
    //public GameObject player;

    public Interactable focusedInteractable;
    public Transform focusedTransform;

    private Image crosshairImage;
    private RectTransform crosshairRectTransform;
    private int interactableLayerMask = 1 << 10;
    private Camera camera; // for raycasting

    private bool newInteractableFound;

    #endregion

    #region events

    #endregion

    #region event subscriptions
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    #endregion

    #region event handlers
    #endregion

    #region init
    private void Awake()
    {
        focusUIText = focusUIObject.GetComponentInChildren<Text>();

        if (camera == null)
        {
            camera = Camera.main;
        }

        if (crosshairSprite != null)
        {
            GameObject canvasObj = new GameObject();
            canvasObj.name = "Crosshair Canvas";
            canvasObj.transform.parent = transform;

            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

            GameObject imageObj = new GameObject();
            imageObj.name = "Crosshair Image";
            imageObj.transform.parent = canvasObj.transform;

            crosshairImage = imageObj.AddComponent<Image>();
            crosshairImage.sprite = crosshairSprite;

            crosshairRectTransform = imageObj.GetComponent<RectTransform>();
            crosshairRectTransform.localPosition = Vector3.zero;
            crosshairRectTransform.sizeDelta = new Vector2(crosshairSize, crosshairSize);
        }
    }
    #endregion

    #region trivial methods

    public void UpdateCrosshair(Sprite newSprite, int newSize)
    {
        crosshairSprite = newSprite;
        crosshairImage.sprite = crosshairSprite;

        crosshairSize = newSize;
        crosshairRectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    public void StartFocus(Transform trans)
    {
        focusedInteractable = trans.gameObject.GetComponent<Interactable>();
        focusedTransform = trans;
        focusedInteractable.OnFocus();
        focusUIText.text = focusedInteractable.focusText;
        focusUIObject.gameObject.SetActive(true);
    }

    public void EndFocus()
    {
        focusedInteractable.OnFocusLost();
        focusedInteractable = null;
        focusedTransform = null;
        focusUIObject.gameObject.SetActive(false);
    }

    // Checks for interactables in camera view around crosshair region.
    public void CheckIfInteractableInCrosshair()
    {
        // make sure to ignore currently held interactable
        newInteractableFound = true;

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxInteractionDistance, interactableLayerMask))
        {
            if (hit.transform != focusedTransform)
            {
                StartFocus(hit.collider.transform);
            }
            else
            {
                newInteractableFound = false;
            }
        } else
        {
            if (focusedInteractable == null)
            {
                newInteractableFound = false;
            } else
            {
                EndFocus();
            }
        }
    }

    void PointInteractionUITowardsCamera()
    {
        focusUIObject.transform.position = focusedInteractable.transform.position + new Vector3(0,.5f,0);
        focusUIObject.transform.LookAt(Camera.main.transform);
    }

    void ContinueFocusing()
    {
        PointInteractionUITowardsCamera();

        if (Input.GetButtonDown("Interact"))
        {
            if (focusedInteractable != null)
            {
                focusedInteractable.OnInteract(transform.parent.gameObject);
            }
        }
    }

    void Update()
    {
        CheckIfInteractableInCrosshair();

        if (focusedInteractable != null) // same interactable
        {
            ContinueFocusing();
        }

    }

    #endregion




}
