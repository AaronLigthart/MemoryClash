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
    public GameObject camera;

    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> grabbedCardsList = new List<GameObject>();
    public List<GameObject> selectedCards = new List<GameObject>();

    public TextMeshProUGUI soldierText;
    public TextMeshProUGUI mageText;
    public TextMeshProUGUI tankText;
    public TextMeshProUGUI archerText;

    public TextMeshProUGUI timerText;

    private int soldierCount = 0;
    private int mageCount = 0;
    private int tankCount = 0;
    private int archerCount = 0;
    public List<string> playerList = new List<string>();
    public List<string> enemyList = new List<string>();

    public int score = 0;

    private int currentTime;



    void Start()
    {
        CreateDeck(20);
    }
    
    /************************
     * Handle Deck creation *
     ************************/
    void CreateDeck(int units)
    {
        for (int i = 0; i < units; i++)
        {
            CreateCard(card, i);
            CreateCard(card, i);
        }
        GrabCards(24);
    }
    GameObject CreateCard(GameObject card, int type)
    {
        GameObject card1 = Instantiate(card);
        card1.GetComponent<Card>().SetType(type);
        deck.Add(card1);
        return card1;
    }
    void GrabCards(int grabAmount)
    {
        for (int i = 0; i < grabAmount; i += 2)
        {
            GrabCard(Random.Range(0, deck.Count));
        }
        placeCards(Shuffle(grabbedCardsList));
    }
    void GrabCard(int index)
    {
        if (index % 2 == 0)
        {
            grabbedCardsList.Add(deck[index]);
            grabbedCardsList.Add(deck[index + 1]);
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
    void placeCards(List<GameObject> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.SetParent(grid.transform);
        }
        ResetTimer();
        StartCoroutine(StartTimer());
    }
    /*****************************
    ** Handle Memory game Phase **
    ******************************/
    public IEnumerator StartTimer()
    {
        while (currentTime != 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            timerText.text = "Time untill attack commences: " + this.currentTime;
        }
        StartCoroutine(RemoveCards());
        StopCoroutine(StartTimer());
    }

    private void ResetTimer(int setTime = 30)
    {
        currentTime = setTime;
    }
    public void CheckCards(GameObject card)
    {
        selectedCards.Add(card);

        if (selectedCards.Count == 2)
        {
           if(selectedCards[0].GetComponent<Card>().unitType == selectedCards[1].GetComponent<Card>().unitType)
           {
                score += 1;
                AddUnit(selectedCards[0].GetComponent<Card>().unitType);
                selectedCards.Clear();
            }
            else {

                for (int i = 0; i < selectedCards.Count;i++)
                {
                    selectedCards[i].GetComponent<Card>().Reset();

                }
                selectedCards.Clear();
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


    /****************************
    *** Handle War game Phase ***
    *****************************/

    public void CreatePlayerArmy()
    {
        playerList.Add("soldier");
        playerList.Add("tank");
        playerList.Add("archer");
        playerList.Add("mage");
    }
    public void CreateEnemyArmy()
    {
        enemyList.Add("soldier");
        enemyList.Add("tank");
        enemyList.Add("archer");
        enemyList.Add("mage");
    }
    public void StartWar()
    {
        CreatePlayerArmy();
        CreateEnemyArmy();
        unitSpawner.Spawn(playerList, enemyList);
    }

    public IEnumerator RemoveCards()
    {
        float removetime = 0.3f;
        for (int i = 0; i < deck.Count; i++)
        {
            Destroy(deck[i]);
        }

        for (int i = 0; i < grabbedCardsList.Count; i++ )
        {
            
            Destroy(grabbedCardsList[i]);
            yield return new WaitForSeconds(removetime *=0.9f);
        }
        StartCoroutine(TransitionCamera(new Vector3(0, 14.5f, -10), 1.2f));

    }
    public IEnumerator TransitionCamera(Vector3 endPos, float duration)
    {
        float elapsedTime = 0.0f;
        Vector3 startingPos = camera.transform.position;
        while (elapsedTime < duration)
        {
            print(camera.transform.position);
            camera.transform.position = Vector3.Lerp(startingPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime/duration)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartWar();
        yield return 0;
    }
}
