using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{

    GameObject player;
    Shooting shooting;
    Image crosshairImage;
    RectTransform crosshair;
    public Vector2 originalSize;
    public Vector2 targetSize;
    public float animationDuration;
    private float animationTime = 0f;
    public float upAnimationSpeed;
    public float downAnimationSpeed;
    void ChangeCursor()
    {
        
        if(shooting.shooting && shooting.readyToShoot)
        {
            animationTime += Time.deltaTime;
            float progress = animationTime / animationDuration;
            crosshair.sizeDelta = Vector2.Lerp(crosshair.sizeDelta, targetSize, Time.deltaTime * upAnimationSpeed);
        }
        else
        {
            crosshair.sizeDelta = Vector2.Lerp(crosshair.sizeDelta, originalSize, Time.deltaTime * downAnimationSpeed); ;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        crosshair = GetComponent<RectTransform>();
        player = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
        shooting = player.GetComponent<Shooting>();
        crosshair.sizeDelta = originalSize;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
        ChangeCursor();
    }
}
