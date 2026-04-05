using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    
    public UI uiManager;
    public string playerTag = "Player";
    public static bool isWin = false;
    

    void Reset()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UI>();
    }

    void Start()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UI>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            Win();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            Win();
        //t = Time.time - TimeManager.instance.timeCount;
        //Debug.Log("Time: " + t);
    }

    void Win()
    {
        isWin = true;
       

        if (uiManager == null)
            uiManager = FindObjectOfType<UI>();

        // Stop/record the timer before showing win UI
        var timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.StopTimer();

        if (uiManager != null)
            uiManager.ShowWin();
       
    }
}
