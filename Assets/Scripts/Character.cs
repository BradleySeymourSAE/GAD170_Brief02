using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Functions to complete:
/// - Initial Stats
/// - Return Battle Points
/// - Deal Damage
/// </summary>
public class Character : MonoBehaviour
{

    public const string styled = "---";

    public CharacterName charName; // This is a reference to an instance of the characters name script.

    [Range(0.0f, 1.0f)]
    public float mojoRemaining = 1; // This is the characters hp this is a float 0-100 but is normalised to 0.0 - 1.0;

    [Header(styled + " Character Stats " + styled)]
    public int level;
    public int currentXp;
    public int xpThreshold = 10;
    public int skillPointScaling = 1;

    int previousThreshold;
    public int experienceBase = 50;
    public float levelScaling = 1.2f;


    /// <summary>
    /// Our variables used to determine our fighting power.
    /// Dance Stats (Calculated from Character Attrs)
    /// </summary>
    public int style;
    public int luck;
    public int rhythm;

    /// <summary>
    /// Our physical stats that determine our dancing stats.
    /// Character Attributes 
    /// </summary>
    public int agility = 2;
    public int intelligence = 1;
    public int strength = 2;

    /// <summary>
    /// Used to determine the conversion of 1 physical stat, to 1 dancing stat.
    /// Modifiers 
    /// </summary>
    public float agilityMultiplier = 0.5f;
    public float strengthMultiplier = 1f;
    public float intelligenceMultiplier = 2f;

    /// <summary>
    /// A float used to display what the chance of winning the current fight is.
    /// </summary>
    public float percentageChanceToWin;


    [Header(styled + " Character Settings (Other) " + styled)]
    public DanceTeam myTeam; // This holds a reference to the characters current dance team instance they are assigned to.
    public bool isSelected; // This is used for determining if this character is selected in a battle.
    [SerializeField]
    protected TMPro.TextMeshPro nickText; // This is just a piece of text in Unity,  to display the characters name.
    public AnimationController animController; // A reference to the animationController, is used to switch dancing states.

    // This is called once, this then calls Initial Stats function
    void Awake()
    {
        animController = GetComponent<AnimationController>();
        GeneratePhysicalStatsStats(); // we want to generate some physical stats.
    }

    /// <summary>
    /// This function should set our starting stats of Agility, Strength and Intelligence
    /// to some default RANDOM values.
    /// </summary>
    public void GeneratePhysicalStatsStats()
    {
        Debug.LogWarning("Generate Physical Stats has been called");

        // Let's set up agility, intelligence and strength to some default Random values.
        agility = Random.Range(1, 4);
        strength = Random.Range(1, 4);
        intelligence = Random.Range(1, 3);


        CalculateDancingStats(agility, strength, intelligence);
    }

    /// <summary>
    /// This function should set our style, luck and ryhtmn to values
    /// based on our currrent agility,intelligence and strength.
    /// </summary>
    public void CalculateDancingStats(int _agility, int _strength, int _intelligence)
    {
        Debug.LogWarning("Generate Calculate Dancing Stats has been called");
        // take our physical stats and translate them into dancing stats,
        // Style = Agility * .5f
        // Rhythm = Strength * 1.0f 
        // Luck = intelligence * 2.0f
        // Convert integer value to a float so that we can modify the value
        // Then return that value back to an integer 

        if (agilityMultiplier <= 0.0f) agilityMultiplier = 0.5f;
        if (strengthMultiplier <= 0.0f) strengthMultiplier = 1f;
        if (intelligenceMultiplier <= 0.0f) intelligenceMultiplier = 2f;

        // Parse the Characters Dance Attrs (Style, Dance, Rhythm) to float 

        float _style = _agility * agilityMultiplier;
        float _rhythm = _strength * strengthMultiplier;
        float _luck = _intelligence * Random.Range(0.0f, intelligenceMultiplier);


        style = (int)_style;
        rhythm = (int)_rhythm;
        luck = (int)_luck;

        // Debugging : Return the Data Type (Float, Int) 
        // Debug.Log("Style is type: " + style.GetType() + " Rhythm is type: " + rhythm.GetType() + " Luck's Type: " + luck.GetType());
    
    }


    /// <summary>
    /// This is takes in a normalised value i.e. 0.0f - 1.0f, and is used to display our % chance to win.
    /// </summary>
    /// <param name="normalisedValue"></param>
    public void SetPercentageValue(float normalisedValue)
    {
      
        // Convert float's normalised value into a whole number 

        normalisedValue = Mathf.Round(normalisedValue);

        percentageChanceToWin = normalisedValue;
    }

    /// <summary>
    /// We probably want to use this to remove some hp (mojo) from our character.....
    /// Then we probably want to check to see if they are dead or not from that damage and return true or false.
    /// </summary>
    public void DealDamage(float amount)
    {

        // Check to see whether the character is dead or not 

        // If the character is dead
            // Remove the character from their teams dancer list 

        // If the character is not dead
            // Remove some hp from our character 
                // Check if the character is dead
                    // Return true 
                    // Return false
    }

    /// <summary>
    /// Used to generate a number of battle points that is used in combat.
    /// </summary>
    /// <returns></returns>
    public int ReturnDancePowerLevel()
    {
        int _luckiness = Random.Range(1, luck);
        int totalAttributes = 6;
        int s_currentLevel = level;
        int basePoints = agility + strength + intelligence + rhythm + style;

        int powerLevel = s_currentLevel + (basePoints + totalAttributes) * _luckiness;


        if (powerLevel != 0)
            return powerLevel;
        else
		{
            Debug.LogWarning("Return Battle Points has been called and the power level returned was equal to zero.");
            powerLevel = 0;
            return powerLevel;
		}
    }

    /// <summary>
    /// A function called when the battle is completed and some xp is to be awarded.
    /// The amount of xp gained is coming into this function
    /// </summary>
    public void AddXP(int exp)
    {
        if (exp == 0)
            Debug.LogWarning("This character needs some xp to be given, the xpGained from the fight was: " + exp);

        // Check to see if the player has leveled up
        currentXp += exp;

        if (currentXp >= xpThreshold)
        {
            // Level up!
            // Store last xp threshold in a local variable 
            previousThreshold = xpThreshold;
            LevelUp(currentXp, previousThreshold);
        }


    }

    /// <summary>
    /// A function used to handle actions associated with levelling up.
    /// </summary>
    private void LevelUp(int p_currentXP, int p_previousThreshold)
    {
        Debug.LogWarning("Level up has been called");
        int s_currentLevel = level;
        int maxLevel = 99;
        int basePoints = 5;
        int addIntelligence = 0;
        bool hasReachedMilestone = false;

        Debug.Log("Current XP: " + p_currentXP + " Threshold: " + xpThreshold + " Not Level greater than or equal to maxLevel: " + !(level >= maxLevel));
        if (p_currentXP >= xpThreshold && !(s_currentLevel >= maxLevel))
            level += 1;

        // Simple Experience Scaling 
        float s_newThreshold = p_previousThreshold + Mathf.Pow(experienceBase * level, levelScaling);

        // Convert threshold back to int
        xpThreshold = (int)s_newThreshold;

        // Determin the amount of points to assign to the characters stats 
        // Don't add points to the intelligence value, as this increases the luck and makes the game fairly unbalanced.
        // Use milestones 

        switch (level)
		{
            case 10:
            case 25:
            case 50:
            case 75:
            case 99:
                hasReachedMilestone = true;
                break;
            default:
                break;
		}

        if (hasReachedMilestone)
		{
            if (skillPointScaling == 0)
                addIntelligence = 1;
            else
                addIntelligence += skillPointScaling;
		}

        DistributePhysicalStatsOnLevelUp(basePoints, addIntelligence);
    }

    /// <summary>
    /// A function used to assign a random amount of points ot each of our skills.
    /// </summary>
    public void DistributePhysicalStatsOnLevelUp(int p_pointsPool, int addedIntelligencePoint)
    {
        Debug.LogWarning("DistributePhysicalStatsOnLevelUp has been called " + p_pointsPool);
       
        int s_currentLevel = level;
        int remainder = p_pointsPool;
        int _strengthPoints, _agilityPoints;


        if (addedIntelligencePoint == 1)
            intelligence += addedIntelligencePoint;

        if (agility > strength)
		{
            _strengthPoints = Random.Range(1, (p_pointsPool - 2));
            remainder = p_pointsPool - _strengthPoints;
            strength += _strengthPoints;
            agility += remainder;
		}
        else if (strength > agility)
		{
            _agilityPoints = Random.Range(1, (p_pointsPool - 2));
            remainder = p_pointsPool - _agilityPoints;
            agility += _agilityPoints;
            strength += remainder;
		}
        else
		{
            if (strength == agility)
			{
                _strengthPoints = Random.Range(1, (p_pointsPool + 1));
                _agilityPoints = p_pointsPool - _strengthPoints;
                agility += _agilityPoints;
                strength += _strengthPoints;
			}
		}

        // Check whether the character has met a level milestone 
        // If the player has met a milestone (Level 10, 25, 50, 75, 99)
        // Add Two Extra Points to Intelligence Value (Which in turn gives them more luck)

        switch (s_currentLevel)
		{
            case 10:
            case 25:
            case 50:
            case 75:
            case 99:

                if (agility < strength)
                    agility += 2;
                if (strength < agility)
                    strength += 2;

                intelligence += 1;
                break;
            default:
                break;
		}



        // After calculating the characters physical stats we need to recalculate 
        // the characters dancing stats using the updated Strength, Agility and Intelligence Values 

        Debug.Log("Intelligence: " + intelligence + " Agility: " + agility + " Strength: " + strength);

        CalculateDancingStats(agility, strength, intelligence);
    }



    /// <summary>
    /// Is called inside of our DanceTeam.cs is used to set the characters name!
    /// </summary>
    /// <param name="characterName"></param>
    public void AssignName(CharacterName characterName)
    {
        charName = characterName;
        if(nickText != null)
        {
            nickText.text = charName.nickname;
            nickText.transform.LookAt(Camera.main.transform.position);
            //text faces the wrong way so
            nickText.transform.Rotate(0, 180, 0);
        }
    }
}
