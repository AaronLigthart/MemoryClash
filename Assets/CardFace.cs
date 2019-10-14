using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardFace : MonoBehaviour
{

    [Header("units")]
    public Sprite[] units;
    public Image image;

    // Start is called before the first frame update
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void Show()
    {

    }

    public void Reset()
    {
        
    }
}
