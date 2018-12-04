using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCastBehaviour : MonoBehaviour
{
    /// <summary>
    /// Enumeration of spells
    /// </summary>
    public enum Spell { EMPTY, STUN, TELEPORT, SPRINT, INVISIBILITY, FEARAREA };
    Manager manager;
    private GameObject leftCooldownSkill, rightCooldownSkill;
    /// <summary>
    /// Cooldowns for spells
    /// </summary>
    private Dictionary<Spell, float> COOLDOWN = new Dictionary<Spell, float>()
    {
        { Spell.STUN, 5.0f },
        { Spell.TELEPORT, 15.0f },
        { Spell.SPRINT, 8.0f },
        { Spell.INVISIBILITY, 10.0f },
        { Spell.FEARAREA, 1f }
    };
    /// <summary>
    /// Binded spells ; true = left click, false = right click
    /// </summary>
    private Dictionary<bool, Spell> bindedSpells = new Dictionary<bool, Spell>()
    {
        { false, Spell.EMPTY },
        { true, Spell.EMPTY }
    };
    /// <summary>
    /// Availability of spells (used when in cooldown)
    /// </summary>
    private Dictionary<string, bool> spellAvailability = new Dictionary<string, bool>();
    /// <summary>
    /// Cooldown currently elapsed for spells
    /// </summary>
    private Dictionary<string, float> spellActualElapsedCooldown = new Dictionary<string, float>();

    private void Start()
    {
        GameObject[] cooldownSkills = GameObject.FindGameObjectsWithTag("SkillCooldown");
        if (cooldownSkills.Length >= 2)
        {
            leftCooldownSkill = cooldownSkills[0];
            rightCooldownSkill = cooldownSkills[1];
        }
        foreach (var spell in System.Enum.GetNames(typeof(Spell)))
        {
            spellAvailability[spell] = true;
            spellActualElapsedCooldown[spell] = 0;
        }
        RefreshSpells();
    }

    public void RefreshSpells()
    {
        // Load spells from game controller/manager
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            manager = gameController.GetComponent<Manager>();
            bindedSpells[true] = manager.GetLeftSpell();
            bindedSpells[false] = manager.GetRightSpell();
        } else
        {
            GameObject managerGameObject = Instantiate(new GameObject());
            managerGameObject.tag = "GameController";
            manager = managerGameObject.AddComponent<Manager>();
            DontDestroyOnLoad(manager);
            bindedSpells[true] = manager.GetLeftSpell();
            bindedSpells[false] = manager.GetRightSpell();
        }
        // Load spell images
        GameObject[] skillSlots = GameObject.FindGameObjectsWithTag("SkillSlot");
        if (skillSlots.Length >= 2)
        {
            skillSlots[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + manager.GetLeftSpell().ToString());
            skillSlots[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + manager.GetRightSpell().ToString());
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Spell leftClickSpell = bindedSpells[true];
        Spell rightClickSpell = bindedSpells[false];
        //  Editing cooldown bar
        if (leftCooldownSkill != null && rightCooldownSkill != null)
        {
            if (!spellAvailability[leftClickSpell.ToString()])
            {
                spellActualElapsedCooldown[manager.GetLeftSpell().ToString()] += Time.deltaTime;
                leftCooldownSkill.GetComponent<Image>().fillAmount = 1 - spellActualElapsedCooldown[manager.GetLeftSpell().ToString()] / COOLDOWN[manager.GetLeftSpell()];
            }
            else
            {
                spellActualElapsedCooldown[manager.GetLeftSpell().ToString()] = 0;
                leftCooldownSkill.GetComponent<Image>().fillAmount = 0;
            }
            if (!spellAvailability[rightClickSpell.ToString()])
            {
                spellActualElapsedCooldown[manager.GetRightSpell().ToString()] += Time.deltaTime;
                rightCooldownSkill.GetComponent<Image>().fillAmount = 1 - spellActualElapsedCooldown[manager.GetRightSpell().ToString()] / COOLDOWN[manager.GetRightSpell()];
            }
            else
            {
                spellActualElapsedCooldown[manager.GetRightSpell().ToString()] = 0;
                rightCooldownSkill.GetComponent<Image>().fillAmount = 0;
            }
        }
        //  Try to cast spell
        if (Input.GetMouseButtonDown(0))
        {
            if (spellAvailability[leftClickSpell.ToString()])
            {
                CastSpell(leftClickSpell);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (spellAvailability[rightClickSpell.ToString()])
            {
                CastSpell(rightClickSpell);
            }
        }
    }

    /// <summary>
    /// Cast the given spell
    /// </summary>
    /// <param name="spell">Spell to cast</param>
    void CastSpell(Spell spell)
    {
        switch (spell)
        {
            case Spell.TELEPORT:
                gameObject.AddComponent<Teleport>();
                SetCooldown(Spell.TELEPORT);
                break;
            case Spell.STUN:
                gameObject.AddComponent<Stun>();
                SetCooldown(Spell.STUN);
                break;
            case Spell.SPRINT:
                gameObject.AddComponent<Sprint>();
                SetCooldown(Spell.SPRINT);
                break;
            case Spell.INVISIBILITY:
                gameObject.AddComponent<Invisibility>();
                SetCooldown(Spell.INVISIBILITY);
                break;
            case Spell.FEARAREA:
                gameObject.AddComponent<FearArea>();
                SetCooldown(Spell.FEARAREA);
                break;
        }
    }

    /// <summary>
    /// Set a spell unavailability and trigger the cooldown restauration for later
    /// </summary>
    /// <param name="spell">Spell to launch cooldown</param>
    void SetCooldown(Spell spell)
    {
        spellAvailability[spell.ToString()] = false;
        StartCoroutine(RestaureCooldown(spell));
    }

    /// <summary>
    /// Restaure the spell availability
    /// </summary>
    /// <param name="spell">Spell that must be available</param>
    /// <returns>Yielding some time</returns>
    IEnumerator RestaureCooldown(Spell spell)
    {
        yield return new WaitForSeconds(COOLDOWN[spell]);
        spellAvailability[spell.ToString()] = true;
    }
}
