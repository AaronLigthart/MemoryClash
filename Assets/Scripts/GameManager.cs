using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject card;
    public GameObject grid;
    public UnitSpawner unitSpawner;

    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> grabbedCardsList = new List<GameObject>();
    public List<GameObject> selectedCards = new List<GameObject>();

    public TextMeshProUGUI soldierText;
    public TextMeshProUGUI mageText;
    public TextMeshProUGUI tankText;
    public TextMeshProUGUI archerText;
    private int soldierCount = 0;
    private int mageCount = 0;
    private int tankCount = 0;
    private int archerCount = 0;
    public int[] array = new int[] { 1, 2, 3 };


    public int score = 0;




    /*
     * DONE Step 1: Create a deck with all the possible cards that can be played
     * Step 2: Grab a pair and put them in a grabbedCardsList
     * step 3: Shuffle the deck
     * step 4: place the deck
     * step 5: when a pair is matched add it to a pairedCardsList
     * step 6: when the timer has ran out.
     */
    void Start()
    {
   
        CreateDeck(20);
        unitSpawner.Spawn(array, array);
    }

    public void CheckCards(GameObject card)
    {
        print("checking card");
        selectedCards.Add(card);

        if (selectedCards.Count == 2)
        {
            Debug.LogWarning("we got 2!");
           if(selectedCards[0].GetComponent<Card>().unitType == selectedCards[1].GetComponent<Card>().unitType)
           {
                Debug.LogWarning("GOOD JOB!");

                score += 1;
                AddUnit(selectedCards[0].GetComponent<Card>().unitType);
                selectedCards.Clear();

            }
            else {
                Debug.LogWarning("resetting");

                for (int i = 0; i < selectedCards.Count;i++)
                {
                    selectedCards[i].GetComponent<Card>().Reset();

                }
                selectedCards.Clear();

                print(selectedCards.Count);
            }
        }
        
    }

    void AddUnit(int unitType)
    {
        if (unitType >= 0 && unitType < 5)
        {
            archerCount++;
            archerText.text = "x " + archerCount;
        } else if (unitType >= 5 && unitType < 10)
        {
            mageCount++;
            mageText.text = "x " + mageCount;
        } else if (unitType >= 10 && unitType < 15)
        {
            soldierCount++;
            soldierText.text = "x " + soldierCount;
        } else if (unitType >= 15 && unitType < 20)
        {
            tankCount++;
            tankText.text = "x " + tankCount;
        } else Debug.LogError("Unit out of range");
    }
    void CreateDeck(int units)
    {
        for(int i = 0; i < units; i++)
        {
            CreateCard(card, i);
            CreateCard(card, i);
        }
        GrabCards(24);
    }

    void GrabCards(int grabAmount)
    {
        for(int i = 0; i < grabAmount; i += 2)
        {
            GrabCard(Random.Range(0, deck.Count));
        }
        placeCards(Shuffle(grabbedCardsList));
    }

    void GrabCard(int index)
    {
        if(index%2 == 0){
            grabbedCardsList.Add(deck[index]);
            grabbedCardsList.Add(deck[index+1]);
            deck.Remove(deck[index + 1]);
            deck.Remove(deck[index]);
        }
        else
        {
            GrabCard(Random.Range(0, deck.Count));
        }
    }

    List<GameObject> Shuffle(List<GameObject> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rnd = Random.Range(0, deck.Count);
            GameObject tempGO = deck[rnd];
            deck[rnd] = deck[i];
            deck[i] = tempGO;
        }
        return deck;
    }

    GameObject CreateCard(GameObject card, int type)
    {
        GameObject card1 = Instantiate(card);
        card1.GetComponent<Card>().SetType(type);
        deck.Add(card1);
        return card1;
    }

    void placeCards(List<GameObject> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.SetParent(grid.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
