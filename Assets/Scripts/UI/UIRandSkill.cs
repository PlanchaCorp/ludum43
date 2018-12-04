using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRandSkill : MonoBehaviour {


    private Sprite[] skillImages;
    [SerializeField] Image[] skillButtonImages;
    private int[] skillButtonIndex;
    [SerializeField] Image[] twoSkillImages;
    private string[] twoSkillNames;

    private bool leftSkillOccupied = false;
    private bool rightSkillOccupied = false;
    
    void Start()
    {
        FetchCurrentSpells();
        FetchOpenSpells();
        RandomizePickingSlots();
	}

    /// <summary>
    /// Fetch the current player spells
    /// </summary>
    void FetchCurrentSpells()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            Manager manager = gameController.GetComponent<Manager>();
            twoSkillImages[0].sprite = Resources.Load<Sprite>("Sprites/" + manager.GetLeftSpell().ToString());
            twoSkillImages[1].sprite = Resources.Load<Sprite>("Sprites/" + manager.GetRightSpell().ToString());
        }
    }

    /// <summary>
    /// Fetch the spells available and their sprite
    /// </summary>
    void FetchOpenSpells()
    {
        skillImages = new Sprite[System.Enum.GetNames(typeof(PlayerCastBehaviour.Spell)).Length - 1];
        int j = 0;
        foreach (var spell in System.Enum.GetNames(typeof(PlayerCastBehaviour.Spell)))
        {
            if (spell != PlayerCastBehaviour.Spell.EMPTY.ToString())
            {
                skillImages[j] = Resources.Load<Sprite>("Sprites/" + spell.ToString());
                j++;
            }
        }
    }

    /// <summary>
    /// Randomize picking slots
    /// </summary>
    void RandomizePickingSlots()
    {
        skillButtonIndex = new int[3];
        List<Sprite> alreadyInUseSprites = new List<Sprite>();
        twoSkillNames = new string[2];
        for (int i = 0; i < skillButtonImages.Length; i++)
        {
            Sprite skillImage = null;
            int randomNumber;
            do
            {
                randomNumber = Random.Range(0, skillImages.Length);
                skillImage = skillImages[randomNumber];
            } while (alreadyInUseSprites.Contains(skillImage));
            alreadyInUseSprites.Add(skillImage);
            skillButtonIndex[i] = randomNumber;
            skillButtonImages[i].sprite = skillImage;
        }
    }

    /// <summary>
    /// Pick a skill from the slots available and put it on hotbar
    /// </summary>
    /// <param name="slotNumber">Slot the player clicked on</param>
    public void PickSkill(int slotNumber)
    {
        if (!leftSkillOccupied)
        {
            twoSkillImages[0].sprite = skillImages[skillButtonIndex[slotNumber]];
            leftSkillOccupied = true;
        } else if (!rightSkillOccupied)
        {
            twoSkillImages[1].sprite = skillImages[skillButtonIndex[slotNumber]];
            rightSkillOccupied = true;
            Debug.Log(twoSkillImages[1].mainTexture);
        } else
        {
            int slot = Random.Range(0, 2);
            twoSkillImages[slot].sprite = skillImages[skillButtonIndex[slotNumber]];
        }
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (leftSkillOccupied && gameController != null)
        {
            gameController.GetComponent<Manager>().SetLeftSpell((PlayerCastBehaviour.Spell)System.Enum.Parse(typeof(PlayerCastBehaviour.Spell), twoSkillImages[0].mainTexture.name, true));
        }
        if (rightSkillOccupied && gameController != null)
        {
            gameController.GetComponent<Manager>().SetRightSpell((PlayerCastBehaviour.Spell)System.Enum.Parse(typeof(PlayerCastBehaviour.Spell), twoSkillImages[1].mainTexture.name, true));
        }
    }

    public void GoToNextLevel()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            Manager manager = gameController.GetComponent<Manager>();
            gameController.GetComponent<Manager>().GoToProceduralLevel();
        }
    }
}
