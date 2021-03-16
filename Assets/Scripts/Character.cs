using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class handles all the data relating to the characters stats 
/// - Generate Physical Stats for our character 
/// - Calculate the dancing stats based on our physical stats 
/// - SetPercentageValue based on the decimal value coming in -> Normalize into a % 
/// - ReturnDancePowerLevel return a power level based on our dancing stats
/// - AddXP based on the experience coming in, add some xp points. 
/// - LevelUp increase our level as well as increase the threshold for levelling up.
/// - DistributePhysicalStatsOnLevelUp increase each of our physical stats by a value, and recalculate the characters dancing stats
/// </summary>
public class Character : MonoBehaviour
{

    public const string styled = "---";

    public CharacterName charName; // This is a reference to an instance of the characters name script.
    public string character_name;

    [Range(0.0f, 1.0f)]
    public float mojoRemaining = 1; // This is the dancers hp float 0-100 but is normalised to 0.0 - 1.0;

    [Header(styled + " Character Levelling Stats " + styled)]
    public int level;
    public int currentXp;
    public int xpThreshold = 10;
    public int maxLevel = 99;
    public int experienceBase = 50;
    public bool hasReachedMilestone = false;
    public int previousThreshold; 

    [Header(styled + " Scaling Factors " + styled)]
    public int skillPointScaling = 1;
    public int luckScalingFactor = 1;
    public float levelScalingFactor = 1.2f;
    public int baseSkillPoints = 5;
    public float addHealthMinimum = 0.25f;
    public float addHealthMaximum = 1.0f;


    /// <summary>
    /// Our variables used to determine our fighting power.
    /// Dance Stats (Calculated from Character Attrs)
    /// </summary>
    [Header(styled + " Dancing Stats " + styled)]
    public int style;
    public int luck;
    public int rhythm;

    /// <summary>
    /// Our physical stats that determine our dancing stats.
    /// Character Attributes 
    /// </summary>
    [Header(styled + " Basic Character Stats " + styled)]
    public int agility = 2;
    public int intelligence = 1;
    public int strength = 2;

    /// <summary>
    /// Used to determine the conversion of 1 physical stat, to 1 dancing stat.
    /// Modifiers 
    /// </summary>
    [Header(styled + " Modifiers " + styled)]
    public float agilityMultiplier = 0.5f;
    public float strengthMultiplier = 1f;
    public float intelligenceMultiplier = 2f;

    /// <summary>
    /// A float used to display what the chance of winning the current fight is.
    /// </summary>
    public float percentageChanceToWin; // Percentage chance of winning a round 
    public int numberOfAttributes; // Total amount of attributes 
    public int totalPowerLevelPoints; // Total amount of power level points (Debugging)



    [Header(styled + " Character Settings (Other) " + styled)]
    public DanceTeam myTeam; // This holds a reference to the characters current dance team instance they are assigned to.
    public bool isSelected; // This is used for determining if this character is selected in a battle.
    [SerializeField]
    protected TMPro.TextMeshPro nickText; // This is just a piece of text in Unity,  to display the characters name.
    public AnimationController animController; // A reference to the animationController, is used to switch dancing states.

    // This is called once BEFORE start, this then calls Initial Stats function
    void Awake()
    {
        animController = GetComponent<AnimationController>(); // Animation Controller Reference 
        GeneratePhysicalStatsStats(); // Generate a characters physical stats 
    }


    private void Start()
	{
        // If the charactes level is less than or equal to zero 
        if (level <= 0)
		{
            // Set the starting characters level to 1.
            level = 1;
		}
	}

    /// <summary>
    /// This function should set our starting stats of Agility, Strength and Intelligence
    /// to some default RANDOM values.
    /// </summary>
    public void GeneratePhysicalStatsStats()
    {
        // Debug.LogWarning("Generate Physical Stats has been called");

        // Let's set up agility, intelligence and strength to some default Random values.
        agility = Random.Range(1, 4);  // random agility value between 1 minimum inclusive and 4 exclusive.
        strength = Random.Range(1, 4);  // random strength value between 1 minimum inclusive and 4 exclusive.
        intelligence = Random.Range(1, 3); // random intelligence value between 1 minimum inclusive and 3 exclusive.

        Debug.Log("[GeneratePhysicalStatsStats]: " + " Agility: " + agility + " Strength: " + strength + " Intelligence: " + intelligence);

        // Calculate the dancing stats for our character based on the characters 
        // agility, strength and intelligence. 
        CalculateDancingStats(agility, strength, intelligence);
    }

    /// <summary>
    ///     This function should set our Style, Rhythm & Luck to values based on our currrent characters Agility, Strength and Intelligence Values.
    /// <param name="_agility">The characters agility level</param>
    /// <param name="_strength">The characters strength level</param>
    /// <param name="_intelligence">The characters intelligence level</param>
    /// <returns></returns>
    /// </summary>
    public void CalculateDancingStats(int _agility, int _strength, int _intelligence)
    {
        Debug.Log("[CalculateDancingStats]: " + "Character Stats for Agility: " + _agility + " Strength: " + _strength + " Intelligence: " + _intelligence);

        // Style is calculated based on the characters 'Agility' * agilityMultiplier (0.5f)
        // Rhythm is calculated based on the characters 'Strength' * strengthMultiplier (1f)
        // Luck is calculated based on the characters 'Intelligence' Stat * intelligenceMultiplier (2f)


        // We want to multiply agility value by its multiplier and cast the value from a float to an integer. 
        style = (int)(_agility * agilityMultiplier);
        rhythm = (int)(_strength * strengthMultiplier);
        luck = (int)(_intelligence * Random.Range(0f, intelligenceMultiplier));

        // Extra Debugging Methods
        // Debug.Log("[CalculateDancingStats]: " + " Style Type: " + style.GetType() + " Rhythm Type: " + rhythm.GetType() + " Luck Type: " + luck.GetType());

        Debug.Log("[CalculateDancingStats]: " + "Style: " + style + " Rhythm: " + rhythm + " Luck: " + luck);

        // Store global var for total power level points.
        totalPowerLevelPoints = agility + strength + intelligence + style + rhythm;
    }


    /// <summary>
    /// This is takes in a normalised value i.e. 0.0f - 1.0f, and is used to display our % chance to win.
    /// </summary>
    /// <param name="normalisedValue"></param>
    public void SetPercentageValue(float normalisedValue)
    {
        // Set Percentage to be a normalised value.
        Debug.Log("[SetPercentageValue]: " + " Setting percentage value for " + normalisedValue);

        // Round float value to the nearest whole decimal number
        normalisedValue = Mathf.Round(normalisedValue);

        // Sets the percentage chance of winning for a characters Stats
        percentageChanceToWin = normalisedValue;

        Debug.Log("[SetPercentageValue]: " + " Percentage: " + percentageChanceToWin + "%");
    }

    /// <summary>
    /// Deals damage to our character from a float value 
    /// Then returns a true or false value to determine whether the character is dead or not 
    /// <param name="amount">The amount of damage to incur on the player</param>
    /// </summary>
    public bool DealDamage(float amount)
    {

        Debug.Log("[DealDamage]: " + character_name + " is has been dealt " + amount);
        // Deduct an amount from the current mojo remaining for the character
        mojoRemaining -= amount;



        // Check if the current health is less than or equal to 0.0f
        // list if they are 'dead'
        if (mojoRemaining <= 0)
        {
            // Characer is dead
            Debug.Log("[DealDamage]: " + character_name + " has been SLAIN! Health remaining is now " + mojoRemaining + ". Removing from active dancers list!");

            // Reset the characters mojo - Good practice but not neccesarily required.
            mojoRemaining = 0;
            
            // Remove dancer from active dancer list.
            myTeam.RemoveDancerFromActive(this);
            return true;
        }
            else
        {
            // Character is still alive
            Debug.Log("[DealDamage]: " + character_name + " has been hit with " + amount + " but is still standing! Health remaining: " + mojoRemaining);
            return false;
        }
    }

    /// <summary>
    ///  Used to return a characters dance power level, which is used in combat! 
    /// </summary>
    /// <returns></returns>
    public int ReturnDancePowerLevel()
    {
        // Create a characters dance power level based off the characters overall stats & level.

        // Luckiness is the modifier in this case that balances the fight between characters
        // Calculated using a range of 1 and luck + adjustable luck scaling factor 
        int _luckiness = Random.Range(1, luck + luckScalingFactor);
    
        numberOfAttributes = 5;// Total number of character attributes (Which is 5 if you dont count luck)
        totalPowerLevelPoints = agility + strength + intelligence + rhythm + style; // Example Base Points: 4 + 5 + 8 + 5 + 2 + 16

        // Debugging - Debug.Log("Power Level: " + powerLevel);
        // int powerLevel = level + (totalPowerLevelPoints + numberOfAttributes) * luckiness;

        // Debug log power level to the console.
        Debug.Log("[ReturnDancePowerLevel]: " + " Power Level: " + level + (totalPowerLevelPoints + numberOfAttributes) * _luckiness);

       // Return the current characters level + (totalPowerLevelPoints + attributesTotal) * luckiness;
        return level + (totalPowerLevelPoints + numberOfAttributes) * _luckiness;
    }

    /// <summary>
    /// A function called when the battle is completed and some xp is to be awarded.
    /// <param name="exp">The amount of experience to be awarded</param>
    /// </summary>
    public void AddXP(int exp)
    {
        if (exp == 0)
        {
            Debug.LogWarning("[AddXP]: " + "This character needs some experience added. Experience added is currently " + exp);
        }

        // Add experience to the current players XP.
        currentXp += exp;

        // Check if the characters current experience has reached the threshold required to level up.
        if (currentXp >= xpThreshold)
        {
            previousThreshold = xpThreshold; // Store the previous experience threshold for the character.
            LevelUp(currentXp, previousThreshold); // Level up the player using the experience gained.
        }
    }

    /// <summary>
    /// A function used to add health to the current dancer if they have won a round 
    /// <param name="health">The amount of health to add to the dancer</param>
    /// </summary>
    /// 
    public void AddHealth(float health)
	{
        // Get the current characters health 
        float s_currentHealth = mojoRemaining += health;

        Debug.Log("[AddHealth]: " + character_name + " new health added to the characters remaining health is " + s_currentHealth);

        // Check if the health value (Once added) is greater than 1f if it is, just make the health 1f.
        if (s_currentHealth > 1f)
        {
            s_currentHealth = 1.0f;
        }
        else
		{
            // Otherwise if the current health value is calculated to be less than or equal to 0f 
            if (s_currentHealth <= 0f)
			{
                // Set a random value to be added to the characters health.
                s_currentHealth = Random.Range(addHealthMinimum, addHealthMaximum);
            }
		}

        mojoRemaining = s_currentHealth;
     
      
        Debug.Log("[AddHealth]: " + "Adding " + health + " to " + character_name + " current health " + mojoRemaining);
        // Then we want to set the characters health. 
        mojoRemaining = s_currentHealth;

        Debug.Log("[AddHealth]: " + character_name + " new health is " + mojoRemaining);
	}

    /// <summary>
    ///     Handle the leveling up logic for a character
    /// </summary>
    /// <param name="p_currentXP">Current experience of the character</param>
    /// <param name="p_previousThreshold">Previous level up threshold</param>
    private void LevelUp(int p_currentXP, int p_previousThreshold)
    {
        Debug.LogWarning("[LevelUp]" + "Level up character function has been called with Experience Points: " + p_currentXP + " and previous threshold of " + p_previousThreshold);
        int currentLevel = level; // Current dancers level 
        int levelCap = maxLevel; // the max level allowed 
        baseSkillPoints = 5; // base amount of points to award 
        int intelligencePoint = 0;



        // Check to see whether the players current experience is greater than or equal to the previous threshold.
        // If the current level of the character is not greater than or equal to the max level 
        if (p_currentXP >= xpThreshold && !(currentLevel >= levelCap))
		{
            // Increase the characters level 
            level += 1;
		}

        // Simple Experience Scaling - Increase the threshold for when the character should level up based on: 
        // the previous threshold & power function. 
        xpThreshold = (int)(p_previousThreshold + Mathf.Pow(experienceBase * level, levelScalingFactor));
        Debug.Log("[LevelUp]: " + character_name + " new experience threshold is " + xpThreshold);

        // Determine the amount of points to assign to the characters stats 
        // Don't assign points to the intelligence value, as this increases the luck and makes the game unbalanced.
        // Edit: Moved based points to top of script. 

        // Check to see whether the player has reached a milestone 
        // Edit: Removing for loop and just using the if statement to check for milestone 
        // after the milestone has been reached just increase the intelligence point by 1 
        // plus its scaling factor.


        if (
           level % 10 == 0 ||
           level % 25 == 0 ||
           level % 50 == 0 ||
           level % 75 == 0 ||
           level % 99 == 0
          )
        {
            // Has reached a level milestone 
            hasReachedMilestone = true;
		}
        else
		{
            // Has not reached a level milestone 
            hasReachedMilestone = false;
		}

        // Debuging - Check for milestone 
        Debug.Log("[LevelUp]: " + "Has character reached milestone: " + hasReachedMilestone);

        /*
        for (int i = 1; i <= level; i++)
		{
            // Check if the value is equal to 10, 25, 50, 75, 99
            if (level % i == 10 || level % i == 25 || level % i == 50 || level % i == 75 || level % i == 99)
            {
                // If it is,  add an intelligence point 
                intelligencePoint += 1;
            }
        }


        Debug.Log("[LevelUp]: " + "Checking for milestone... " + (intelligencePoint == 0 ? "Milestone Achieved: " + intelligencePoint : "Milestone not achieved." + intelligencePoint));
*/

        // IF the character has reached a milestone 
        if (hasReachedMilestone == true)
		{
           // Add scaling factor to intelligence point 
           intelligencePoint += (1 + skillPointScaling);
		}

        // Update the characters physical stats. Add intelligence points & reachedMilestone value to be handled by stats on level up.
        DistributePhysicalStatsOnLevelUp(baseSkillPoints, intelligencePoint);
    }

    /// <summary>
    /// A function used to assign a random amount of points ot each of our skills.
    /// <param name="statPoints">Default amount of points to distribute to the characters skills</param>
    /// <param name="p_intelligencePoints">Increase intelligence value by this value. </param>
    /// </summary>
    public void DistributePhysicalStatsOnLevelUp(int p_PointsPool, int p_intelligencePoints)
    {
        Debug.LogWarning("[DistributePhysicalStatsOnLevelUp]: " + " Points: " + p_PointsPool + " Increase Intelligence: " + p_intelligencePoints);
        // We need the current player's level
        int newStrength, // local var for storing new strength points 
            newAgility; // local var for storing new agility points

        // Removed duplicate check for intelligence points. 


        // Check to see whether the characters agility is greater than strength 
        if (agility > strength)
        {
            Debug.Log("[DistributePhysicalStatsOnLevelUp]: " + " Agility " + agility + " is greater than Strength " + strength);
            // Random integer value calculated for strength local var 
            newStrength = Random.Range(1, p_PointsPool);

            // Remove points from the skill points pool. 
            // Fixed error, was calculating strength here for some reason? 
            p_PointsPool -= newStrength;

            // Add points to the characters strength and agility. 
            strength += newStrength;
            agility += p_PointsPool;
        }
        // Otherwise if the characters strength skill is greater than its agility skill
        else if (strength > agility)
        {
            Debug.Log("[DistributePhysicalStatsOnLevelUp]: " + "Strength " + strength + " is greater than agility " + agility);
            // Random integer value calculated for agility local var 
            newAgility = Random.Range(1, p_PointsPool);

            // Remove points from the skill points pool.
            p_PointsPool -= newAgility;

            // Add points to the characters agility, then assign the rest to the characters strength.
            agility += newAgility;
            strength += p_PointsPool;
        }
        else
        {
            // Otherwise - If the characters strength & agility skills are the same value. 
            if (strength == agility)
            {
                Debug.Log("[DistributePhysicalStatsOnLevelUp]: " + "Strength" + strength + " and Agility " + agility + " are equal!");

                // Random integer value calculated for agility local var 
                newStrength = Random.Range(1, p_PointsPool + 1);

                // Remove points from the skill points pool.
                p_PointsPool -= newStrength;

                // Assign points randomly between both.
                strength += newStrength;
                agility += p_PointsPool;
            }
            // It would have to be one of those values, or an error would occur. Which would be handled here.
        }

        // Check to see if intelligence point value is greater than or equal to 1
        // Intelligence point can be more than one as it has a scaling factor. 
        // If a Leveling Milestone has been reached, increase the characters intelligence, agility and strength values by random.

        // Check whether the character has reached a milestone 
        if (p_intelligencePoints >= 1 && hasReachedMilestone == true)
        {
            // Add Agility & Strength random value between 1 inclusive, 3 exclusive. 
            agility += Random.Range(1, 3);
            strength += Random.Range(1, 3);

            // Increase intelligence by point.
            intelligence += p_intelligencePoints;

            Debug.Log("[DistributePhysicalStatsOnLevelUp]: " + "Reached milestone: " + hasReachedMilestone + " Adding Agility: " + agility + " Strength: " + strength + " Intelligence: " + intelligence);

            // Reset the milestone reached boolean to default value or it will be called every level up.
            hasReachedMilestone = false;
        }


        Debug.Log("[DistributePhysicalStatsOnLevelUp]: " + "Reached milestone: " + hasReachedMilestone);
        // After calculating players physical stats, recalculate dancing stats again
        CalculateDancingStats(agility, strength, intelligence);
    }

    /// <summary>
    ///     Called from DanceTeam Script - used to assign a dance characters name
    /// </summary>
    /// <param name="characterName">The name to assign to a character</param>
    public void AssignName(CharacterName _character)
    {
        charName = _character;
        
        if (nickText != null)
		{
            nickText.text = charName.firstName + " the " + charName.descriptor;
            nickText.transform.LookAt(Camera.main.transform.position);

            nickText.transform.Rotate(0, 180, 0);
		
            character_name = nickText.text;
        }
    }
}
