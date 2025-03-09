using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAttack : MonoBehaviour
{
    private static CardAttack select;
    [SerializeField] private GameObject cards;
    [SerializeField] private string joueurTag;
    [SerializeField] private Manager manager;

    private void Awake()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    public void OnClick()
    {
        if (manager.currentPhase == 2)
        {
            string namePlayer = GetPlayerName(manager.currentPlayer);
            Transform playerDeck = GameObject.FindWithTag(namePlayer).transform;

            bool isMyCard = transform.IsChildOf(playerDeck);

            if (!isMyCard && select == null) return;
            if (select == this) Deselect();

            else
            {
                if (isMyCard)
                {
                    select?.Deselect();
                    Select();
                }
                else
                {
                    Attack(select, this);
                    select.Deselect();
                }
            }
        }
    }

    // pour avoir nom du joueur (qui est aussi le tag)
    public string GetPlayerName(int currentPlayer)
    {
        switch (currentPlayer)
        {
            case 1: return "Joueur1";
            case 2: return "Joueur2";
            case 3: return "Joueur3";
            case 4: return "Joueur4";
            default: return "";
        }
    }

    // attaquer un joueur
    void Attack(CardAttack Attaquant, CardAttack Enemie)
    {

        // je r�cup�re les stats des joueurs
        TMPro.TextMeshProUGUI[] enemiePlayerStats = Enemie.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI[] attaquantPlayerStats = Attaquant.GetComponentsInChildren<TMPro.TextMeshProUGUI>();

        // je fais la soustraction 
        int enemyHP = int.Parse(enemiePlayerStats[1].text);
        int attackerDamage = int.Parse(attaquantPlayerStats[0].text);

        enemyHP -= attackerDamage;

        enemiePlayerStats[1].text = enemyHP.ToString();

        // si hp <= 0 je supprime la carte et je d�cr�mente le countcard de l'�nemie ainsi que les modifs de gold
        if (enemyHP <= 0)
        {
            int enemyPlayerNumber = GetPlayerNumber(Enemie.transform.parent.name);

            Destroy(Enemie.gameObject);

            // retirer gold au joueur attaqu�
            manager.AddGold(-5, enemyPlayerNumber);

            // ajouter gold au joueur qui a attaaqu�
            manager.AddGold(5, manager.currentPlayer);
            
            // d�cr�menter soncompteur
            manager.DecrementationCountCard(enemyPlayerNumber);
        }
        // Une seule attaque et c'est au tour du joueur suivant
        manager.EndRound();
    }


    int GetPlayerNumber(string playerName)
    {
        switch (playerName)
        {
            case "DeckJoueur1": return 1;
            case "DeckJoueur2": return 2;
            case "DeckJoueur3": return 3;
            case "DeckJoueur4": return 4;
            default: return 0; // Ou g�rer autrement si c'est un cas invalide
        }
    }

    private void Update()
    {
        if (select == null) cards.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

    }

    void Deselect()
    {
        cards.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
        select = null;
    }
    void Select()
    {
        cards.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        select = this;
    }
}
