public static class DataPrefs {
    private const string LEVEL_DATA_KEY = "Level";

    public static string GenerateLevelKey(int level) {
        return $"{LEVEL_DATA_KEY}{level}";
    }
}