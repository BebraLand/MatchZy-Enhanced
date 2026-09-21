namespace MatchZy;

public static class MapAdvertisementRules
{
    // MatchZy uses zero-based indexes. The integration publishes one-based labels.
    public static int Select(bool active, int index, int numMaps, IReadOnlyList<string> maps, string currentMap)
    {
        if (!active || numMaps is not (1 or 3 or 5) || index < 0 || index >= numMaps || index >= maps.Count) return 0;
        // Workshop IDs are resolved by CS2; transition flags in the caller gate those loads.
        if (!long.TryParse(maps[index], out _) && !string.Equals(Path.GetFileName(maps[index]),
                Path.GetFileName(currentMap), StringComparison.OrdinalIgnoreCase)) return 0;
        return index + 1;
    }
}
