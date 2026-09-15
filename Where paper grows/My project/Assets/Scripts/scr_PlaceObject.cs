using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class scr_PlaceObject : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navmesh;
    [SerializeField] private AudioData placeObjectSound;
    [SerializeField] private Material hologramMaterial;
    public GameObject PreviewBox;
    

    private bool IsPlacingObject = false;
    private Camera MainCamera;
    public GameObject ObjectToPlace;
    private GameObject objectPreview;
    private GameObject placedObject;
    private bool hasSelectedPosition;
    private scr_InputManager inputManager;
    public int TreeCount = 0;
    public int BushCount = 0;
    public int FlowerCount = 0;
    private string ObjectName;
    private bool MusicOn = false;

    void Start()
    {
        inputManager = GameManager.s_Instance.GetComponent<scr_InputManager>();
        MainCamera = GameManager.s_Instance != null ? GameManager.s_Instance.MainCamera : null;
        if (MainCamera == null)
            MainCamera = Camera.main;
    }

    void Update()
    {
        if (IsPlacingObject && !hasSelectedPosition)
        {
            ObjectPreview(ObjectToPlace);
        }

        if (inputManager.interactAction.WasPerformedThisFrame() && IsPlacingObject && !hasSelectedPosition)
        {
            EditObject(ObjectToPlace);
            MusicOn = !MusicOn;
            EventManager.RaiseOnMusicStart(MusicOn);
        }
    }

    public void TargetPosition(GameObject ObjectToPlace)
    {
        DestroyPreview();
        this.ObjectToPlace = ObjectToPlace;
        IsPlacingObject = true;
        hasSelectedPosition = false;
        placedObject = null;
        objectPreview = Instantiate(ObjectToPlace);
        objectPreview.name = ObjectToPlace.name + "_Hologram";
        ConfigurePreview(objectPreview);
        objectPreview.SetActive(false);

    }

    public void PlaceObject() //called from confirm button in UI
    {
        if (!hasSelectedPosition || placedObject == null)
        {
            return;
        }

        ObjectName = ObjectToPlace.name;
        GameManager.s_Instance.UI.GetComponent<scr_UIHandler>().RoofViewButtonOn = true;
        switch (ObjectName)
        {
            case "pre_Tree":
                TreeCount += 1;
                break;
            case "pre_Bush":
                BushCount += 1;
                break;
            case "pre_Flower":
                FlowerCount += 1;
                break;
            default:
                return;
        }
        PreviewBox.SetActive(false);
        IsPlacingObject = false;
        hasSelectedPosition = false;
        AudioManager.Instance.PlayAtPosition(placeObjectSound, placedObject.transform.position);
        placedObject = null;
    }

        public void ObjectPreview(GameObject ObjectToPlace)
        {
            if (objectPreview == null || !TryGetPlacementPosition(out Vector3 placementPosition))
            {
                if (objectPreview != null)
                {
                    objectPreview.SetActive(false);
                }
                return;
            }

            objectPreview.transform.SetPositionAndRotation(placementPosition, Quaternion.identity);
            objectPreview.SetActive(true);
        }

        private bool TryGetPlacementPosition(out Vector3 placementPosition)
        {
            placementPosition = default;
            if (MainCamera == null)
            {
                MainCamera = Camera.main;
                if (MainCamera == null) return false;
            }

            Ray ray = MainCamera.ScreenPointToRay(inputManager.lookValue);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f) && hit.collider.CompareTag("Floor"))
            {
                placementPosition = hit.point;
                return true;
            }

            return false;
        }

        private void ConfigurePreview(GameObject preview)
        {
            foreach (Collider collider in preview.GetComponentsInChildren<Collider>())
            {
                Destroy(collider);
            }

            foreach (MonoBehaviour component in preview.GetComponentsInChildren<MonoBehaviour>())
            {
                component.enabled = false;
            }

            Material previewMaterial = hologramMaterial != null
                ? hologramMaterial
                : CreateDefaultHologramMaterial();

            foreach (Renderer renderer in preview.GetComponentsInChildren<Renderer>())
            {
                Material[] materials = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = new Material(previewMaterial);
                }
                renderer.materials = materials;
            }
        }

        private Material CreateDefaultHologramMaterial() //create a hologram material
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            Material material = new Material(shader);
            Color hologramColor = new Color(0.1f, 0.9f, 1f, 0.45f); //object preview color
            material.color = hologramColor;
            //set shader properties for transparency and blending
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", hologramColor);
            if (material.HasProperty("_Surface")) material.SetFloat("_Surface", 1f);
            if (material.HasProperty("_Blend")) material.SetFloat("_Blend", 0f);
            if (material.HasProperty("_SrcBlend")) material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (material.HasProperty("_DstBlend")) material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (material.HasProperty("_ZWrite")) material.SetFloat("_ZWrite", 0f);
            material.renderQueue = 3000;
            return material;
        }
        //removes the hologram
        private void DestroyPreview()
        {
            if (objectPreview != null)
            {
                Destroy(objectPreview);
                objectPreview = null;
            }

        }

        //starts edit mode
        public void EditObject(GameObject ObjectToPlace)
        {
            if (!IsPlacingObject || !TryGetPlacementPosition(out Vector3 placementPosition))
            {
                return;
            }

            placedObject = Instantiate(ObjectToPlace, placementPosition, Quaternion.identity);
            hasSelectedPosition = true;
            DestroyPreview();
            ActivatePreviewBox();
        }


        //enables the edit box
        private void ActivatePreviewBox()
        {
            if (PreviewBox == null || placedObject == null)
            {
                return;
            }
            Vector3 previewPosition = PreviewBoxDimensionCheck();
            PreviewBox.SetActive(true);
            RectTransform uiTransform = PreviewBox.GetComponent<RectTransform>();
            Canvas canvas = PreviewBox.GetComponentInParent<Canvas>();

            if (uiTransform == null || canvas == null || canvas.renderMode == RenderMode.WorldSpace)
            {
                PreviewBox.transform.position = previewPosition;
                return;
            }

            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(MainCamera, previewPosition);
            RectTransform canvasTransform = canvas.transform as RectTransform;
            Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, screenPosition, canvasCamera, out Vector2 localPosition))
            {
                uiTransform.anchoredPosition = localPosition;
            }
        }

        //resizes box to "fit" the object
        private Vector3 PreviewBoxDimensionCheck()
        {
            if (placedObject.transform.childCount == 0)
            {
                return placedObject.transform.position;
            }

            Transform placedObjectChild = placedObject.transform.GetChild(0);
            float previewScale = Mathf.Max(placedObjectChild.localScale.x, placedObjectChild.localScale.z);
            float previewPositionY = placedObject.transform.position.y + placedObjectChild.localPosition.y;
            Debug.Log($"PreviewBox scale: {previewScale}, position Y: {previewPositionY}");
            PreviewBox.transform.localScale = new Vector3(previewScale, 1f, previewScale);
            return new Vector3(placedObject.transform.position.x, previewPositionY, placedObject.transform.position.z);
        }


        //Scrap button calls here, destroys object
        public void CancelPlacement()
        {
            if (placedObject != null)
            {
                Destroy(placedObject);
                placedObject = null;
            }
            DestroyPreview();
            IsPlacingObject = false;
            hasSelectedPosition = false;
            PreviewBox.SetActive(false);
        }
    }

