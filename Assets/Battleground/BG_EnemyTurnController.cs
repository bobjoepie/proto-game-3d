using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BG_EnemyTurnController : BG_TurnController
{
    private BG_UIDocManager uiDocManager;

    public BG_DeckData deckData;
    public List<BG_CardController> deck = new List<BG_CardController>();

    void Start()
    {
        uiDocManager = BG_UIDocManager.Instance;

        ConvertDeckData();
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

    public override void StartTurn()
    {
        uiDocManager.hudOverlay.SetStateLabel("Enemy");
        UniTask.Action(async () =>
        {
            var spawner = entityManager.GetAllOwnObjectives(faction).FirstOrDefault();
            if (spawner != null)
            {
                var card = deck.FirstOrDefault();
                if (card != null)
                {
                    PlayCard(card, spawner.transform.position);
                }
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            EndTurn();

        }).Invoke();
    }

    public override void EndTurn()
    {
        stateManager.PassTurnOwner();
    }

    public void PlayCard(BG_CardController card, Vector3 pos)
    {
        foreach (var behavior in card.castBehaviors)
        {
            switch (behavior)
            {
                case BG_CardSummon cardSummon:
                    var tags = BG_EntityTags.FactionEnemy;
                    tags |= BG_EntityTags.ObjectiveSeeker;
                    cardSummon.Summon(pos, tags, "EnemyCollider", "EnemyActionRadius");
                    break;
            }
        }
    }
}
