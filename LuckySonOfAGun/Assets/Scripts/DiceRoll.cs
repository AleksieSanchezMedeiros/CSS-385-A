using UnityEngine;

//making it a singleton?, i have no clue how to do this in c# so its straight outta google more or less
public class DiceRoll
{
    private static DiceRoll privInstance;
    public static DiceRoll Instance
    {
        get
        {
            if (privInstance == null)
                privInstance = new DiceRoll();
            return privInstance;
        }
    }

    public int currentRoll = 0;

    private DiceRoll() { } // Prevent direct instantiation

    public int RollDie(int sides)
    {
        currentRoll = Random.Range(1, sides + 1);
        return currentRoll;
    }
}