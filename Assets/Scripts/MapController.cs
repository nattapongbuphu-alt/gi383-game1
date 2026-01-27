using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("UI")]
    public GameObject mapUI; // Panel ของแมพ
    public MapRenderer mapRenderer; // ตัววาดแมพ


    [Header("Optional")]
    public MonoBehaviour playerController; // สคริปต์ควบคุมผู้เล่น (ถ้ามี)


    private bool isMapOpen = false;


    void Start()
    {
        mapUI.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }


    void ToggleMap()
    {
        isMapOpen = !isMapOpen;
        mapUI.SetActive(isMapOpen);


        // วาดแมพครั้งแรกตอนเปิด
        if (isMapOpen && mapRenderer != null)
        {
            mapRenderer.GenerateMap();
        }


        // (ถ้าอยากให้ตัวละครหยุดเดินตอนเปิดแมพ)
        if (playerController != null)
        {
            playerController.enabled = !isMapOpen;
        }
    }
}
