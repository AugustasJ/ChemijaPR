using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    GameObject ARMenu;
    
    private Transform infoBtn1;
    private Transform infoBtn2;
    private Transform infoBtn3;
    private Transform infoBtn4;
    private Transform infoBtn5;
    private Transform infoBtn6;

    [SerializeField]
    private Text scoreText;
    
    [SerializeField]
    private Text imageTrackedText;
    private Transform prefabNameContainer;

    private Dictionary<string, string> translator = new Dictionary<string, string>();

    private List<string> discoveredElements = new List<string>();
    
    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        foreach (GameObject prefab in placablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }

        translator.Add("Hydrogen", "Vandenilis");
        translator.Add("Oxygen", "Deguonis");
        translator.Add("Phosphorus", "Fosforas");
        translator.Add("Sulfur", "Siera");
        translator.Add("H2SO4", "Sieros rūgštis");
        translator.Add("H2O", "Vanduo");
        translator.Add("Unknown", "");
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        
        List<ARTrackedImage> imagesOnScreen = new List<ARTrackedImage>();

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                imagesOnScreen.Add(trackedImage);
            }
        }

        UpdateImage(imagesOnScreen);

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 initPosition = trackedImage.transform.position;
        var position = new Vector3(initPosition.x, initPosition.y, initPosition.z - 0.05f);

        if (placablePrefabs != null)
        {
            foreach (GameObject pref in spawnedPrefabs.Values)
            {
                pref.SetActive(false);
                pref.transform.position = position;
            }

            GameObject prefab = spawnedPrefabs[name];
            prefab.SetActive(true);
            prefab.transform.position = position;
        }

        SceneActions(name);
    }
    
    private void UpdateImage(List<ARTrackedImage> imagesOnScreen)
    {
        foreach (GameObject pref in spawnedPrefabs.Values)
        {
            pref.SetActive(false);
        }

        string name = "Unknown";

        if (imagesOnScreen.Count == 1)
        {
            var currentlyTrackedImg = imagesOnScreen[0];
            name = currentlyTrackedImg.referenceImage.name;
            Vector3 initPosition = currentlyTrackedImg.transform.position;
            var position = new Vector3(initPosition.x, initPosition.y, initPosition.z - 0.05f);

            if (placablePrefabs != null)
            {
                GameObject prefab = spawnedPrefabs[name];
                prefab.SetActive(true);
                prefab.transform.position = position;
            }

            SceneActions(name);
        }
        else
        {
            foreach (var trackedImage in imagesOnScreen)
            {
                bool isHydrogen = imagesOnScreen.Exists(x => x.referenceImage.name.Equals("Hydrogen"));
                bool isOxygen = imagesOnScreen.Exists(x => x.referenceImage.name.Equals("Oxygen"));
                bool isSulfur = imagesOnScreen.Exists(x => x.referenceImage.name.Equals("Sulfur"));

                if (isHydrogen && isOxygen && !isSulfur)
                {
                    name = "H2O";

                    Vector3 position0 = imagesOnScreen[0].transform.position;
                    Vector3 position1 = imagesOnScreen[1].transform.position;
                    Vector3 position = new Vector3((position0.x + position1.x) / 2, (position0.y + position1.y) / 2, position0.z - 0.05f);

                    if (placablePrefabs != null)
                    {
                        GameObject prefab = spawnedPrefabs[name];
                        prefab.SetActive(true);
                        prefab.transform.position = position;
                    }

                    SceneActions(name);
                }
                else if(isHydrogen && isOxygen && isSulfur)
                {
                    name = "H2SO4";
                    
                    Vector3 position0 = imagesOnScreen[0].transform.position;
                    Vector3 position1 = imagesOnScreen[1].transform.position;
                    Vector3 position2 = imagesOnScreen[2].transform.position;

                    Vector3 position = new Vector3((position0.x + position1.x + position2.x) / 3, (position0.y + position1.y + position2.y) / 3, position0.z - 0.05f);

                    if (placablePrefabs != null)
                    {
                        GameObject prefab = spawnedPrefabs[name];
                        prefab.SetActive(true);
                        prefab.transform.position = position;
                    }

                    SceneActions(name);
                }
                else
                {
                    Vector3 initPosition = trackedImage.transform.position;
                    var position = new Vector3(initPosition.x, initPosition.y, initPosition.z - 0.05f);

                    if (placablePrefabs != null)
                    {
                        GameObject prefab = spawnedPrefabs[name];
                        prefab.SetActive(true);
                        prefab.transform.position = position;
                    }
                }

                SceneActions(name);
            }
        }
    }

    private void SceneActions(string name)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "AR")
        {
            Debug.Log(sceneName);
            InfoBtn(name);
            ShowName(name);
        }
        else if (sceneName == "ARChallenge")
        {
            Debug.Log(sceneName);
            SetScore(name);
        }
    }

    private void ShowName(string name)
    {
        GameObject Canvas = GameObject.Find("Canvas");
        prefabNameContainer = Canvas.transform.Find("PrefabNameContainer");

        if(name != "Unknown" || name != null)
        {
            Debug.Log(name);
            prefabNameContainer.gameObject.SetActive(true);
            imageTrackedText.text = translator[name];
        }
        else
        {
            Debug.Log(name);
            prefabNameContainer.gameObject.SetActive(false);
        }
    }

    private void SetScore(string name)
    {
        bool hasH2SO4 = discoveredElements.Exists(x => x == name);
        bool hasH2O = discoveredElements.Exists(x => x == name);
        
        if (name == "H2SO4" && !hasH2SO4)
        {
            discoveredElements.Add(name);
        }
        else if (name == "H2O" && !hasH2O)
        {
            discoveredElements.Add(name);
        }

        string numberOfElementsDiscovered = discoveredElements.Count.ToString();

        scoreText.text = numberOfElementsDiscovered;
    }

    public void InfoBtn(string name)
    {
        ARMenu = GameObject.Find("ARMenu");
        infoBtn1 = ARMenu.transform.Find("ButtonInfo1");
        infoBtn2 = ARMenu.transform.Find("ButtonInfo2");
        infoBtn3 = ARMenu.transform.Find("ButtonInfo3");
        infoBtn4 = ARMenu.transform.Find("ButtonInfo4");
        infoBtn5 = ARMenu.transform.Find("ButtonInfo5");
        infoBtn6 = ARMenu.transform.Find("ButtonInfo6");

        switch (name)
        {
            case "H2O":
                infoBtn1.gameObject.SetActive(true);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(false);
                break;
            case "H2SO4":
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(true);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(false);
                break;
            case "Hydrogen":
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(true);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(false);
                break;
            case "Oxygen":
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(true);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(false);
                break;
            case "Phosphorus":
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(true);
                infoBtn6.gameObject.SetActive(false);
                break;
            case "Sulfur":
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(true);
                break;
            case "Unknown":
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(false);
                break;
            default:
                infoBtn1.gameObject.SetActive(false);
                infoBtn2.gameObject.SetActive(false);
                infoBtn3.gameObject.SetActive(false);
                infoBtn4.gameObject.SetActive(false);
                infoBtn5.gameObject.SetActive(false);
                infoBtn6.gameObject.SetActive(false);
                break;
        }
    }
}
