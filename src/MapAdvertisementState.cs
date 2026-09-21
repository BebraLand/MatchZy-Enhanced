using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;

namespace MatchZy;

public partial class MatchZy
{
    public static readonly PluginCapability<int> CurrentMapNumberCapability =
        new("matchzy:current_map_number:v1");
    private static MatchZy? instance;
    private static bool capabilityRegistered;
    private bool advertSeriesEnded;
    private bool advertMapChanging;

    private void StartMapAdvertisementCapability()
    {
        RegisterListener<Listeners.OnMapStart>(_ => advertMapChanging = false);
        RegisterListener<Listeners.OnMapEnd>(() => advertMapChanging = true);
        instance = this;
        if (!capabilityRegistered)
        {
            Capabilities.RegisterPluginCapability(CurrentMapNumberCapability,
                () => instance?.GetCurrentMapNumber() ?? 0);
            capabilityRegistered = true;
        }
    }

    private int GetCurrentMapNumber()
    {
        var active = isMatchSetup && !advertSeriesEnded && !advertMapChanging
            && !isPractice && !isSleep && !awaitingOperatorNextMap
            && nextMapTransitionTimer == null;
        return MapAdvertisementRules.Select(active, matchConfig.CurrentMapNumber,
            matchConfig.NumMaps, matchConfig.Maplist, Server.MapName);
    }

    public override void Unload(bool hotReload)
    {
        advertSeriesEnded = true;
        if (ReferenceEquals(instance, this)) instance = null;
    }
}
