using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
    private PlayerCastBehaviour.Spell leftSpell, rightSpell;
    private MessageData currentLevel;
    public List<MessageData> levels = null;
    private Queue<MessageData> levelSQueue;

	// Use this for initialization
	void Awake () {
        if (levels != null)
        {
            levelSQueue = new Queue<MessageData>(levels);
        } else
        {
            levelSQueue = new Queue<MessageData>();
        }
        if (SceneManager.GetActiveScene().name.Equals("Startup"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public MessageData NextLevel
    {
        get
        {
            if (levels != null && levelSQueue.ToArray().Length > 0)
            {
                return levelSQueue.Dequeue();
            } else
            {
                MessageData messageData = new MessageData();
                messageData.levelName = "Ending";
                return messageData;
            }
        }
    }

	
    public PlayerCastBehaviour.Spell GetLeftSpell()
    {
        return leftSpell;
    }
    public PlayerCastBehaviour.Spell GetRightSpell()
    {
        return rightSpell;
    }

    public void SetLeftSpell(PlayerCastBehaviour.Spell spell)
    {
        leftSpell = spell;
    }
    public void SetRightSpell(PlayerCastBehaviour.Spell spell)
    {
        rightSpell = spell;
    }

    public void SkipLevel()
    {
        currentLevel = NextLevel;
    }

    public MessageData GetCurrentLevel()
    {
        if (currentLevel == null)
        {
            SkipLevel();
        }
        return currentLevel;
    }

    public void GoToSkillSelection()
    {
        if (currentLevel.levelName == "Ending")
        {
            SceneManager.LoadScene("Ending");
        } else
        {
            SceneManager.LoadScene("SkillSelection");
        }
    }
    public void GoToProceduralLevel()
    {
        SceneManager.LoadScene("ProceduralMap");
    }
    public void GoToTutoLevel()
    {
        SceneManager.LoadScene("LD_Scene");
    }

}
