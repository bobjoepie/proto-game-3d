using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class BG_CardManager : MonoBehaviour
{
    public static BG_CardManager Instance { get; private set; }

    public BG_DeckData deckData;
    public List<BG_CardController> deck = new List<BG_CardController>();
    public List<BG_CardController> hand = new List<BG_CardController>();
    public List<BG_CardController> discardPile = new List<BG_CardController>();
    public int startingHandSize = 4;

    private BG_StateManager stateManager;
    private BG_UIDocManager uiDocManager;
    private Camera mainCamera;

    public BG_CardManager()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        stateManager = BG_StateManager.Instance;
        uiDocManager = BG_UIDocManager.Instance;
        mainCamera = Camera.main;

        ConvertDeckData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ConvertDeckData()
    {
        foreach (var cardData in deckData.deck)
        {
            var card = new GameObject($"Card - {cardData.name}").AddComponent<BG_CardController>();
            cardData.ConvertData(card);
            deck.Add(card);
        }
    }

    public void DrawCardFromDeck(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!deck.Any())
            {
                RefreshDeckFromDiscardPile();
            }
            var index = Random.Range(0, deck.Count - 1);
            var card = deck[index];
            deck.RemoveAt(index);
            hand.Add(card);
        }

        RenderCardsInHand();
    }

    private void RenderCardsInHand()
    {
        UniTask.Action(async () =>
        {
            foreach (var card in hand)
            {
                uiDocManager.hudOverlay.InitCard(card, () => PlayCard(card));
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f / hand.Count));
            }
        }).Invoke();
    }

    public void DrawHand()
    {
        DrawCardFromDeck(startingHandSize);
    }

    public void PlayCard(BG_CardController card)
    {
        foreach (var behavior in card.castBehaviors)
        {
            switch (behavior)
            {
                case BG_CardSummon cardSummon:
                    Action<Vector3> summon = pos =>
                    {
                        var entity = cardSummon.unitData.entityGameObject;
                        cardSummon.unitData.ConvertData(entity);
                        Instantiate(entity, pos, Quaternion.identity);
                        DiscardCard(card);
                    };
                    stateManager.WaitForConfirmAction(summon);
                    break;
            }
        }
    }

    public void DiscardCard(BG_CardController card)
    {
        card.RemoveFromHud();
        hand.Remove(card);
        discardPile.Add(card);
    }

    public void AddCardToHand(BG_CardController card)
    {
        hand.Add(card);
    }

    public void AddCardToDeck(BG_CardController card)
    {
        deck.Add(card);
    }

    public void AddCardToDiscardPile(BG_CardController card)
    {
        discardPile.Add(card);
    }

    public void DiscardHand()
    {
        discardPile.AddRange(hand.ToList());
        UniTask.Action(async () =>
        {
            foreach (var card in hand)
            {
                uiDocManager.hudOverlay.RemoveCard(card);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f / hand.Count));
            }
            hand.Clear();
        }).Invoke();
    }

    public void RefreshDeckFromDiscardPile()
    {
        deck = discardPile.ToList();
        discardPile.Clear();
    }
}
