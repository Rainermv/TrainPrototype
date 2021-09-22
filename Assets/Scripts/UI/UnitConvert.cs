internal static class UnitConvert
{
    // 1 Unit = 2 Meters
    private const float UnitToM = 2f;

    private const float MsToKh = 3.6f; // Meters/Second To Km/Hour
    private static float UnitToKm => UnitToM * 0.001f;


    public static float UnitsToKm(float units)
    {
        return units * UnitToKm;
    }


    public static float UnitSpeedToKmHour(float unitsPerSecond)
    {
        // unit / second => km / hour
        // 1 unit / second = 5 meters / second
        var metersPerSecond = unitsPerSecond * UnitToM;
        return metersPerSecond * MsToKh;

    }
}