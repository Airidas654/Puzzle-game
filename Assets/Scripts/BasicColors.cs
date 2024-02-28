using UnityEngine;

public enum BasicColorsenum : short { 
    Uncolored = 0,
    Green = 1,
    Blue = 2,
    Red = 3,
    Yellow = 4,
    Orange = 5
}

public static class BasicColors
{
    public static Color GetColorFromEnum(BasicColorsenum colorEnum)
    {
        switch (colorEnum)
        {
            case BasicColorsenum.Uncolored:
                return Color.white;
            case BasicColorsenum.Green:
                return Color.green;
            case BasicColorsenum.Blue:
                return Color.blue;
            case BasicColorsenum.Red:
                return Color.red;
            case BasicColorsenum.Yellow:
                return Color.yellow;
            case BasicColorsenum.Orange:
                return new Color(1, (float)102/255, 0);
        }
        return Color.white;
    }
}