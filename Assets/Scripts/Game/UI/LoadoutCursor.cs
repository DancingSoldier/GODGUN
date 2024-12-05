using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadoutCursor : MonoBehaviour
{

    public TextMeshProUGUI amountLeft;
    SelectionScreen manager;
    void ChangeNumber()
    {
        int MaxNumber = 5;
        int selected = manager.chosenPickups.Count;
        int left = MaxNumber - selected;
        amountLeft.text = left.ToString();

    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        manager = GameObject.FindGameObjectWithTag("SelectionManager").GetComponent<SelectionScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
        ChangeNumber();
    }
}
