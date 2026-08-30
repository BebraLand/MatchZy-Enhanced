using CounterStrikeSharp.API.Core;

namespace MatchZy;

internal readonly record struct MatchStatsParticipant(string Id, int Team);

internal sealed class TrackedPlayerStats
{
    public int FlashAssists { get; set; }
    public int TeamKills { get; set; }
    public int Suicides { get; set; }
    public int FriendliesFlashed { get; set; }
    public int KnifeKills { get; set; }
    public int RoundsPlayed { get; set; }
    public int BombDefuses { get; set; }
    public int BombPlants { get; set; }
    public int Kills1 { get; set; }
    public int Kills2 { get; set; }
    public int Kills3 { get; set; }
    public int Kills4 { get; set; }
    public int Kills5 { get; set; }
    public int OneV1s { get; set; }
    public int OneV2s { get; set; }
    public int OneV3s { get; set; }
    public int OneV4s { get; set; }
    public int OneV5s { get; set; }
    public int FirstKillsT { get; set; }
    public int FirstKillsCT { get; set; }
    public int FirstDeathsT { get; set; }
    public int FirstDeathsCT { get; set; }
    public int TradeKills { get; set; }
    public int KastRounds { get; set; }
    public double Kast => RoundsPlayed == 0
        ? 0
        : Math.Round(KastRounds * 100.0 / RoundsPlayed, 1, MidpointRounding.AwayFromZero);
}

internal sealed class MatchStatsTracker
{
    private const double TradeWindowSeconds = 5;
    private readonly Dictionary<string, TrackedPlayerStats> totals = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, RoundPlayerStats> round = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<int, ClutchCandidate> clutchCandidates = new();
    private readonly List<RecentEnemyKill> recentEnemyKills = new();
    private bool openingKillRecorded;

    public void Reset()
    {
        totals.Clear();
        round.Clear();
        clutchCandidates.Clear();
        recentEnemyKills.Clear();
        openingKillRecorded = false;
    }

    public void BeginRound(IEnumerable<MatchStatsParticipant> participants)
    {
        round.Clear();
        clutchCandidates.Clear();
        recentEnemyKills.Clear();
        openingKillRecorded = false;

        foreach (var participant in participants)
        {
            if (!string.IsNullOrWhiteSpace(participant.Id) && participant.Team is 2 or 3)
            {
                round[participant.Id] = new RoundPlayerStats(participant.Team);
            }
        }

        TryStartClutch(2);
        TryStartClutch(3);
    }

    public void RecordBlind(string attackerId, int attackerTeam, string victimId, int victimTeam)
    {
        if (attackerTeam == victimTeam && !attackerId.Equals(victimId, StringComparison.OrdinalIgnoreCase))
        {
            GetRoundPlayer(attackerId, attackerTeam).FriendliesFlashed++;
        }
    }

    public void RecordBombPlant(string playerId, int team) => GetRoundPlayer(playerId, team).BombPlants++;

    public void RecordBombDefuse(string playerId, int team) => GetRoundPlayer(playerId, team).BombDefuses++;

    public void RecordDeath(
        string victimId,
        int victimTeam,
        string? attackerId,
        int attackerTeam,
        string? assisterId,
        int assisterTeam,
        bool assistedFlash,
        string weapon,
        DateTime occurredAt)
    {
        var victim = GetRoundPlayer(victimId, victimTeam);
        victim.Dead = true;

        bool suicide = string.IsNullOrWhiteSpace(attackerId) ||
            attackerId.Equals(victimId, StringComparison.OrdinalIgnoreCase);
        bool teamKill = !suicide && attackerTeam == victimTeam;
        bool enemyKill = !suicide && !teamKill && attackerTeam is 2 or 3;

        if (suicide)
        {
            victim.Suicides++;
        }
        else
        {
            var attacker = GetRoundPlayer(attackerId!, attackerTeam);
            if (teamKill)
            {
                attacker.TeamKills++;
            }
            else if (enemyKill)
            {
                attacker.EnemyKills++;
                attacker.Kill = true;
                if (IsKnife(weapon)) attacker.KnifeKills++;

                if (!openingKillRecorded)
                {
                    if (attackerTeam == 2) attacker.FirstKillsT++;
                    else attacker.FirstKillsCT++;

                    if (victimTeam == 2) victim.FirstDeathsT++;
                    else victim.FirstDeathsCT++;
                    openingKillRecorded = true;
                }

                bool traded = false;
                foreach (var recentKill in recentEnemyKills)
                {
                    double elapsedSeconds = (occurredAt - recentKill.OccurredAt).TotalSeconds;
                    if (!recentKill.Traded &&
                        recentKill.KillerId.Equals(victimId, StringComparison.OrdinalIgnoreCase) &&
                        recentKill.VictimTeam == attackerTeam &&
                        elapsedSeconds is >= 0 and <= TradeWindowSeconds)
                    {
                        if (round.TryGetValue(recentKill.VictimId, out var tradedVictim))
                        {
                            tradedVictim.Traded = true;
                        }
                        recentKill.Traded = true;
                        traded = true;
                    }
                }
                if (traded) attacker.TradeKills++;

                recentEnemyKills.RemoveAll(kill =>
                    (occurredAt - kill.OccurredAt).TotalSeconds > TradeWindowSeconds);
                recentEnemyKills.Add(new RecentEnemyKill(attackerId!, victimId, victimTeam, occurredAt));

                if (!string.IsNullOrWhiteSpace(assisterId) &&
                    !assisterId.Equals(attackerId, StringComparison.OrdinalIgnoreCase) &&
                    assisterTeam == attackerTeam)
                {
                    var assister = GetRoundPlayer(assisterId, assisterTeam);
                    assister.Assist = true;
                    if (assistedFlash) assister.FlashAssists++;
                }
            }
        }

        TryStartClutch(2);
        TryStartClutch(3);
    }

    public void EndRound(int winnerTeam)
    {
        foreach (var (playerId, roundStats) in round)
        {
            var player = GetTotals(playerId);
            player.RoundsPlayed++;
            player.FlashAssists += roundStats.FlashAssists;
            player.TeamKills += roundStats.TeamKills;
            player.Suicides += roundStats.Suicides;
            player.FriendliesFlashed += roundStats.FriendliesFlashed;
            player.KnifeKills += roundStats.KnifeKills;
            player.BombDefuses += roundStats.BombDefuses;
            player.BombPlants += roundStats.BombPlants;
            player.FirstKillsT += roundStats.FirstKillsT;
            player.FirstKillsCT += roundStats.FirstKillsCT;
            player.FirstDeathsT += roundStats.FirstDeathsT;
            player.FirstDeathsCT += roundStats.FirstDeathsCT;
            player.TradeKills += roundStats.TradeKills;

            switch (roundStats.EnemyKills)
            {
                case 1: player.Kills1++; break;
                case 2: player.Kills2++; break;
                case 3: player.Kills3++; break;
                case 4: player.Kills4++; break;
                case >= 5: player.Kills5++; break;
            }

            if (roundStats.Kill || roundStats.Assist || !roundStats.Dead || roundStats.Traded)
            {
                player.KastRounds++;
            }
        }

        if (clutchCandidates.TryGetValue(winnerTeam, out var clutch))
        {
            var player = GetTotals(clutch.PlayerId);
            switch (clutch.Opponents)
            {
                case 1: player.OneV1s++; break;
                case 2: player.OneV2s++; break;
                case 3: player.OneV3s++; break;
                case 4: player.OneV4s++; break;
                case 5: player.OneV5s++; break;
            }
        }

        round.Clear();
        clutchCandidates.Clear();
        recentEnemyKills.Clear();
    }

    public TrackedPlayerStats? Get(string playerId) => totals.GetValueOrDefault(playerId);

    private RoundPlayerStats GetRoundPlayer(string playerId, int team)
    {
        if (!round.TryGetValue(playerId, out var player))
        {
            player = new RoundPlayerStats(team);
            round[playerId] = player;
        }
        return player;
    }

    private TrackedPlayerStats GetTotals(string playerId)
    {
        if (!totals.TryGetValue(playerId, out var player))
        {
            player = new TrackedPlayerStats();
            totals[playerId] = player;
        }
        return player;
    }

    private void TryStartClutch(int team)
    {
        if (clutchCandidates.ContainsKey(team)) return;

        var aliveTeammates = round.Where(entry => entry.Value.Team == team && !entry.Value.Dead).ToList();
        int aliveOpponents = round.Count(entry => entry.Value.Team != team && !entry.Value.Dead);
        if (aliveTeammates.Count == 1 && aliveOpponents is >= 1 and <= 5)
        {
            clutchCandidates[team] = new ClutchCandidate(aliveTeammates[0].Key, aliveOpponents);
        }
    }

    private static bool IsKnife(string weapon) =>
        weapon.Contains("knife", StringComparison.OrdinalIgnoreCase) ||
        weapon.Equals("bayonet", StringComparison.OrdinalIgnoreCase);

    internal static void SelfCheck()
    {
        var tracker = new MatchStatsTracker();
        var now = DateTime.UtcNow;
        tracker.BeginRound([new("a", 2), new("b", 2), new("x", 3), new("y", 3)]);
        tracker.RecordBlind("a", 2, "b", 2);
        tracker.RecordBombPlant("a", 2);
        tracker.RecordDeath("b", 2, "x", 3, null, 0, false, "ak47", now);
        tracker.RecordDeath("x", 3, "a", 2, "b", 2, true, "ak47", now.AddSeconds(2));
        tracker.RecordDeath("y", 3, "a", 2, null, 0, false, "knife", now.AddSeconds(3));
        tracker.EndRound(2);

        tracker.BeginRound([new("a", 2), new("b", 2), new("x", 3), new("y", 3)]);
        tracker.RecordBombDefuse("x", 3);
        tracker.RecordDeath("b", 2, "a", 2, null, 0, false, "ak47", now.AddMinutes(1));
        tracker.RecordDeath("y", 3, "y", 3, null, 0, false, "world", now.AddMinutes(1));
        tracker.RecordDeath("a", 2, "x", 3, null, 0, false, "ak47", now.AddMinutes(1));
        tracker.EndRound(3);

        var a = tracker.Get("a")!;
        var b = tracker.Get("b")!;
        var x = tracker.Get("x")!;
        var y = tracker.Get("y")!;
        if (a.TradeKills != 1 || a.KnifeKills != 1 || a.Kills2 != 1 || a.Kast != 50 ||
            a.FriendliesFlashed != 1 || a.BombPlants != 1 || a.TeamKills != 1 || a.OneV2s != 1 ||
            b.FlashAssists != 1 || b.FirstDeathsT != 1 || b.Kast != 50 ||
            x.FirstKillsCT != 2 || x.BombDefuses != 1 || x.OneV1s != 1 || x.Kast != 100 ||
            y.Suicides != 1 || y.Kast != 0)
        {
            throw new InvalidOperationException("MatchStatsTracker self-check failed.");
        }
    }

    private sealed class RoundPlayerStats(int team)
    {
        public int Team { get; } = team;
        public int FlashAssists { get; set; }
        public int TeamKills { get; set; }
        public int Suicides { get; set; }
        public int FriendliesFlashed { get; set; }
        public int KnifeKills { get; set; }
        public int BombDefuses { get; set; }
        public int BombPlants { get; set; }
        public int EnemyKills { get; set; }
        public int FirstKillsT { get; set; }
        public int FirstKillsCT { get; set; }
        public int FirstDeathsT { get; set; }
        public int FirstDeathsCT { get; set; }
        public int TradeKills { get; set; }
        public bool Kill { get; set; }
        public bool Assist { get; set; }
        public bool Dead { get; set; }
        public bool Traded { get; set; }
    }

    private sealed record ClutchCandidate(string PlayerId, int Opponents);

    private sealed class RecentEnemyKill(string killerId, string victimId, int victimTeam, DateTime occurredAt)
    {
        public string KillerId { get; } = killerId;
        public string VictimId { get; } = victimId;
        public int VictimTeam { get; } = victimTeam;
        public DateTime OccurredAt { get; } = occurredAt;
        public bool Traded { get; set; }
    }
}

public partial class MatchZy
{
    private readonly MatchStatsTracker matchStatsTracker = new();

    private string GetMatchStatsPlayerId(CCSPlayerController player)
    {
        if (isSimulationMode && player.UserId.HasValue &&
            simulationPlayersByUserId.TryGetValue(player.UserId.Value, out var identity))
        {
            return identity.ConfigSteamId;
        }
        return player.SteamID.ToString();
    }

    private bool IsTrackedMatchPlayer(CCSPlayerController? player) =>
        IsPlayerValid(player) && player!.TeamNum is 2 or 3 &&
        !matchzyTeam1.coach.Contains(player) && !matchzyTeam2.coach.Contains(player);

    private void BeginMatchStatsRound()
    {
        var participants = playerData.Values
            .Where(IsTrackedMatchPlayer)
            .Select(player => new MatchStatsParticipant(GetMatchStatsPlayerId(player), player.TeamNum));
        matchStatsTracker.BeginRound(participants);
    }

    private void TrackMatchStatsDeath(EventPlayerDeath @event)
    {
        if (!isMatchLive || !IsTrackedMatchPlayer(@event.Userid)) return;

        var victim = @event.Userid!;
        var attacker = IsTrackedMatchPlayer(@event.Attacker) ? @event.Attacker : null;
        var assister = IsTrackedMatchPlayer(@event.Assister) ? @event.Assister : null;
        matchStatsTracker.RecordDeath(
            GetMatchStatsPlayerId(victim),
            victim.TeamNum,
            attacker == null ? null : GetMatchStatsPlayerId(attacker),
            attacker?.TeamNum ?? 0,
            assister == null ? null : GetMatchStatsPlayerId(assister),
            assister?.TeamNum ?? 0,
            @event.Assistedflash,
            @event.Weapon,
            DateTime.UtcNow);
    }

    private void TrackMatchStatsBlind(EventPlayerBlind @event)
    {
        if (!isMatchLive || !IsTrackedMatchPlayer(@event.Attacker) || !IsTrackedMatchPlayer(@event.Userid)) return;
        matchStatsTracker.RecordBlind(
            GetMatchStatsPlayerId(@event.Attacker!),
            @event.Attacker!.TeamNum,
            GetMatchStatsPlayerId(@event.Userid!),
            @event.Userid!.TeamNum);
    }

    private HookResult EventBombPlantedStatsHandler(EventBombPlanted @event, GameEventInfo info)
    {
        if (isMatchLive && IsTrackedMatchPlayer(@event.Userid))
        {
            matchStatsTracker.RecordBombPlant(GetMatchStatsPlayerId(@event.Userid!), @event.Userid!.TeamNum);
        }
        return HookResult.Continue;
    }

    private HookResult EventBombDefusedStatsHandler(EventBombDefused @event, GameEventInfo info)
    {
        if (isMatchLive && IsTrackedMatchPlayer(@event.Userid))
        {
            matchStatsTracker.RecordBombDefuse(GetMatchStatsPlayerId(@event.Userid!), @event.Userid!.TeamNum);
        }
        return HookResult.Continue;
    }
}
