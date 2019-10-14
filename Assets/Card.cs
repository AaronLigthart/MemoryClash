using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sprite currentFront;
    private Sprite currentBack;
    private Image image;
    private Animator animator;
    private GameManager gameManager;
    public GameObject cardFace;
    public int unitType;

    public bool isOver = false;
    public bool isSelected = false;



    public enum cardDeck { pink, red, blue, orange };
    public cardDeck currentDeck;

    [Header("Pink cards")]
    public Sprite pinkFront;
    public Sprite pinkBack;

    [Header("Red cards")]
    public Sprite redFront;
    public Sprite redBack;

    [Header("Blue cards")]
    public Sprite blueFront;
    public Sprite blueBack;

    [Header("Orange cards")]
    public Sprite orangeFront;
    public Sprite orangeBack;

    public void Reset()
    {
        animator.SetTrigger("Reset");
    }

    // Start is called before the first frame update
    void Start()
    {

        image = GetComponent<Image>();
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        SetTheme(currentDeck);
        image.sprite = currentBack;


    }
 

    public void SetType(int type)
    {
        unitType = type;
        cardFace.GetComponent<Image>().sprite = cardFace.GetComponent<CardFace>().units[type];
        cardFace.GetComponent<Image>().SetNativeSize();
        cardFace.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.y > 0.5)
        {
            image.sprite = currentFront;
            cardFace.GetComponent<Image>().enabled = true;

        }
        else if (transform.rotation.y < 0.5)
        {
            image.sprite = currentBack;
            cardFace.GetComponent<Image>().enabled = false;
        }
    }

    void SetTheme(cardDeck currentDeck)
    {
        switch (currentDeck)
        {
            case cardDeck.pink:
                currentFront = pinkFront;
                currentBack = pinkBack;
                break;
            case cardDeck.red:
                currentFront = redFront;
                currentBack = redBack;
                break;
            case cardDeck.blue:
                currentFront = blueFront;
                currentBack = blueBack;
                break;
            case cardDeck.orange:
                currentFront = orangeFront;
                currentBack = orangeBack;
                break;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
        if (!isSelected)
        {
            animator.SetTrigger("Highlighted");
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = true;
        animator.SetTrigger("Pressed");
        gameManager.CheckCards(this.gameObject);

    }


    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
        if (!isSelected) { 
            animator.SetTrigger("Normal");
          }
    }
}
