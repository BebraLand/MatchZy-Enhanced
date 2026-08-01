using System.Text.Json.Serialization;

namespace MatchZy;
public class MatchZyEvent
{
    public MatchZyEvent(string eventName)
    {
        EventName = eventName;
    }

    [JsonPropertyName("event")]
    public string EventName { get; }
}

public class MatchZyMatchEvent : MatchZyEvent
{
    [JsonPropertyName("matchid")]
    public required long MatchId { get; init; }

    protected MatchZyMatchEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyMatchTeamEvent : MatchZyMatchEvent
{
    [JsonPropertyName("team")]
    public required string Team { get; init; }

    protected MatchZyMatchTeamEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyMapEvent : MatchZyMatchEvent
{
    [JsonPropertyName("map_number")]
    public required int MapNumber { get; init; }

    protected MatchZyMapEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyMapTeamEvent : MatchZyMapEvent
{
    [JsonPropertyName("team_int")]
    public required int TeamNumber { get; init; }

    protected MatchZyMapTeamEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyRoundEvent : MatchZyMapEvent
{
    [JsonPropertyName("round_number")]
    public required int RoundNumber { get; init; }

    protected MatchZyRoundEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyTimedRoundEvent : MatchZyRoundEvent
{
    [JsonPropertyName("round_time")]
    public required int RoundTime { get; init; }

    protected MatchZyTimedRoundEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyPlayerRoundEvent : MatchZyRoundEvent
{

    [JsonPropertyName("player")]
    public required int Player { get; init; }

    protected MatchZyPlayerRoundEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyPlayerTimedRoundEvent : MatchZyTimedRoundEvent
{
    [JsonPropertyName("player")]
    public required int Player { get; init; }

    protected MatchZyPlayerTimedRoundEvent(string eventName) : base(eventName)
    {
    }
}

public class MatchZyPlayerConnectedEvent : MatchZyMatchEvent
{
    [JsonPropertyName("player")]
    public required MatchZyPlayerInfo Player { get; init; }

    public MatchZyPlayerConnectedEvent() : base("player_connect")
    {
    }
}

public class MatchZyPlayerDisconnectedEvent : MatchZyMatchEvent
{
    [JsonPropertyName("player")]
    public required MatchZyPlayerInfo Player { get; init; }

    public MatchZyPlayerDisconnectedEvent() : base("player_disconnect")
    {
    }
}

public class MatchZyBackupLoadedEvent : MatchZyMapEvent
{
    [JsonPropertyName("round_number")]
    public required int RoundNumber { get; init; }

    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    public MatchZyBackupLoadedEvent() : base("backup_loaded")
    {
    }
}

public class MatchZySeriesStartedEvent : MatchZyMatchEvent
{
    [JsonPropertyName("team1")]
    public required MatchZyTeamWrapper Team1 { get; init; }

    [JsonPropertyName("team2")]
    public required MatchZyTeamWrapper Team2 { get; init; }

    [JsonPropertyName("num_maps")]
    public required int NumberOfMaps { get; init; }

    public MatchZySeriesStartedEvent() : base("series_start")
    {
    }
}

public class MatchZySeriesResultEvent : MatchZyMatchEvent
{
    [JsonPropertyName("time_until_restore")]
    public required int TimeUntilRestore { get; init; }

    [JsonPropertyName("winner")]
    public required Winner Winner { get; init; }

    [JsonPropertyName("team1_series_score")]
    public required int Team1SeriesScore { get; init; }

    [JsonPropertyName("team2_series_score")]
    public required int Team2SeriesScore { get; init; }

    public MatchZySeriesResultEvent() : base("series_end")
    {
    }
}

public class GoingLiveEvent : MatchZyMapEvent
{
    [JsonPropertyName("map_name")]
    public required string MapName { get; init; }

    [JsonPropertyName("team1_score")]
    public required int Team1Score { get; init; }

    [JsonPropertyName("team2_score")]
    public required int Team2Score { get; init; }

    [JsonPropertyName("team1_series_score")]
    public required int Team1SeriesScore { get; init; }

    [JsonPropertyName("team2_series_score")]
    public required int Team2SeriesScore { get; init; }

    public GoingLiveEvent() : base("going_live")
    {
    }
}

public class MatchZyRoundEndedEvent : MatchZyTimedRoundEvent
{

    [JsonPropertyName("reason")]
    public required int Reason { get; init; }

    [JsonPropertyName("winner")]
    public required Winner Winner { get; init; }

    [JsonPropertyName("team1")]
    public required MatchZyStatsTeam StatsTeam1 { get; init; }

    [JsonPropertyName("team2")]
    public required MatchZyStatsTeam StatsTeam2 { get; init; }

    public MatchZyRoundEndedEvent() : base("round_end")
    {
    }
}

public class MapResultEvent : MatchZyMapEvent
{
    [JsonPropertyName("winner")]
    public required Winner Winner { get; init; }

    [JsonPropertyName("team1")]
    public required MatchZyStatsTeam StatsTeam1 { get; init; }

    [JsonPropertyName("team2")]
    public required MatchZyStatsTeam StatsTeam2 { get; init; }

    public MapResultEvent() : base("map_result")
    {
    }
}

public class MatchZyDemoUploadedEvent : MatchZyMatchEvent
{
    [JsonPropertyName("map_number")]
    public required int MapNumber { get; init; }

    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    public MatchZyDemoUploadedEvent() : base("demo_upload_ended")
    {
    }
}

// ============================================================================
// Demo lifecycle events (recording + upload progress)
// ============================================================================

public class MatchZyDemoRecordingStartedEvent : MatchZyMapEvent
{
    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    public MatchZyDemoRecordingStartedEvent() : base("demo_recording_start")
    {
    }
}

public class MatchZyDemoRecordingStoppedEvent : MatchZyMapEvent
{
    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    public MatchZyDemoRecordingStoppedEvent() : base("demo_recording_stop")
    {
    }
}

public class MatchZyDemoUploadStartedEvent : MatchZyMapEvent
{
    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    [JsonPropertyName("size_mb")]
    public required double SizeMB { get; init; }

    public MatchZyDemoUploadStartedEvent() : base("demo_upload_start")
    {
    }
}

public class MatchZyDemoUploadSuccessEvent : MatchZyMapEvent
{
    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    [JsonPropertyName("size_mb")]
    public required double SizeMB { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    public MatchZyDemoUploadSuccessEvent() : base("demo_upload_success")
    {
    }
}

public class MatchZyDemoUploadFailEvent : MatchZyMapEvent
{
    [JsonPropertyName("filename")]
    public required string FileName { get; init; }

    [JsonPropertyName("size_mb")]
    public double? SizeMB { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("reason")]
    public required string Reason { get; init; }

    public MatchZyDemoUploadFailEvent() : base("demo_upload_fail")
    {
    }
}

// Player Ready System Events
public class MatchZyPlayerReadyEvent : MatchZyMatchEvent
{
    [JsonPropertyName("player")]
    public required MatchZyPlayerInfo Player { get; init; }

    [JsonPropertyName("team")]
    public required string Team { get; init; }

    [JsonPropertyName("ready_count_team1")]
    public required int ReadyCountTeam1 { get; init; }

    [JsonPropertyName("ready_count_team2")]
    public required int ReadyCountTeam2 { get; init; }

    [JsonPropertyName("total_ready")]
    public required int TotalReady { get; init; }

    [JsonPropertyName("expected_total")]
    public required int ExpectedTotal { get; init; }

    public MatchZyPlayerReadyEvent() : base("player_ready")
    {
    }
}

public class MatchZyPlayerUnreadyEvent : MatchZyMatchEvent
{
    [JsonPropertyName("player")]
    public required MatchZyPlayerInfo Player { get; init; }

    [JsonPropertyName("team")]
    public required string Team { get; init; }

    [JsonPropertyName("ready_count_team1")]
    public required int ReadyCountTeam1 { get; init; }

    [JsonPropertyName("ready_count_team2")]
    public required int ReadyCountTeam2 { get; init; }

    [JsonPropertyName("total_ready")]
    public required int TotalReady { get; init; }

    [JsonPropertyName("expected_total")]
    public required int ExpectedTotal { get; init; }

    public MatchZyPlayerUnreadyEvent() : base("player_unready")
    {
    }
}

public class MatchZyTeamReadyEvent : MatchZyMatchEvent
{
    [JsonPropertyName("team")]
    public required string Team { get; init; }

    [JsonPropertyName("ready_count")]
    public required int ReadyCount { get; init; }

    [JsonPropertyName("total_ready")]
    public required int TotalReady { get; init; }

    [JsonPropertyName("expected_total")]
    public required int ExpectedTotal { get; init; }

    public MatchZyTeamReadyEvent() : base("team_ready")
    {
    }
}

public class MatchZyAllPlayersReadyEvent : MatchZyMatchEvent
{
    [JsonPropertyName("ready_count_team1")]
    public required int ReadyCountTeam1 { get; init; }

    [JsonPropertyName("ready_count_team2")]
    public required int ReadyCountTeam2 { get; init; }

    [JsonPropertyName("total_ready")]
    public required int TotalReady { get; init; }

    [JsonPropertyName("countdown_started")]
    public required bool CountdownStarted { get; init; }

    public MatchZyAllPlayersReadyEvent() : base("all_players_ready")
    {
    }
}

// Match Phase Change Events
public class MatchZyWarmupEndedEvent : MatchZyMapEvent
{
    public MatchZyWarmupEndedEvent() : base("warmup_ended")
    {
    }
}

public class MatchZyKnifeRoundStartedEvent : MatchZyMapEvent
{
    public MatchZyKnifeRoundStartedEvent() : base("knife_round_started")
    {
    }
}

public class MatchZyKnifeRoundEndedEvent : MatchZyMapEvent
{
    [JsonPropertyName("winner")]
    public required string Winner { get; init; }

    public MatchZyKnifeRoundEndedEvent() : base("knife_round_ended")
    {
    }
}

// Pause System Events
public class MatchZyPauseRequestedEvent : MatchZyMapEvent
{
    [JsonPropertyName("requested_by")]
    public required MatchZyPlayerInfo RequestedBy { get; init; }

    [JsonPropertyName("is_tactical")]
    public required bool IsTactical { get; init; }

    [JsonPropertyName("is_admin")]
    public required bool IsAdmin { get; init; }

    public MatchZyPauseRequestedEvent() : base("pause_requested")
    {
    }
}

public class MatchZyMatchPausedEvent : MatchZyMapEvent
{
    [JsonPropertyName("paused_by")]
    public required MatchZyPlayerInfo PausedBy { get; init; }

    [JsonPropertyName("is_tactical")]
    public required bool IsTactical { get; init; }

    [JsonPropertyName("is_admin")]
    public required bool IsAdmin { get; init; }

    [JsonPropertyName("pause_time")]
    public required long PauseTime { get; init; }

    public MatchZyMatchPausedEvent() : base("match_paused")
    {
    }
}

public class MatchZyUnpauseRequestedEvent : MatchZyMapEvent
{
    [JsonPropertyName("team")]
    public required string Team { get; init; }

    [JsonPropertyName("teams_ready")]
    public required int TeamsReady { get; init; }

    [JsonPropertyName("teams_needed")]
    public required int TeamsNeeded { get; init; }

    public MatchZyUnpauseRequestedEvent() : base("unpause_requested")
    {
    }
}

public class MatchZyMatchUnpausedEvent : MatchZyMapEvent
{
    [JsonPropertyName("pause_duration")]
    public required long PauseDuration { get; init; }

    public MatchZyMatchUnpausedEvent() : base("match_unpaused")
    {
    }
}

// Round and Game State Events
public class MatchZyRoundStartedEvent : MatchZyRoundEvent
{
    [JsonPropertyName("team1_score")]
    public required int Team1Score { get; init; }

    [JsonPropertyName("team2_score")]
    public required int Team2Score { get; init; }

    public MatchZyRoundStartedEvent() : base("round_started")
    {
    }
}

public class MatchZyHalftimeStartedEvent : MatchZyMapEvent
{
    [JsonPropertyName("team1_score")]
    public required int Team1Score { get; init; }

    [JsonPropertyName("team2_score")]
    public required int Team2Score { get; init; }

    public MatchZyHalftimeStartedEvent() : base("halftime_started")
    {
    }
}

public class MatchZyOvertimeStartedEvent : MatchZyMapEvent
{
    [JsonPropertyName("overtime_number")]
    public required int OvertimeNumber { get; init; }

    public MatchZyOvertimeStartedEvent() : base("overtime_started")
    {
    }
}

public class MatchZySideSwapEvent : MatchZyMapEvent
{
    [JsonPropertyName("team1_side")]
    public required string Team1Side { get; init; }

    [JsonPropertyName("team2_side")]
    public required string Team2Side { get; init; }

    public MatchZySideSwapEvent() : base("side_swap")
    {
    }
}

// Test Event
public class MatchZyServerConfiguredEvent : MatchZyEvent
{
    [JsonPropertyName("server_id")]
    public required string ServerId { get; init; }

    [JsonPropertyName("hostname")]
    public required string Hostname { get; init; }

    [JsonPropertyName("plugin_version")]
    public required string PluginVersion { get; init; }

    [JsonPropertyName("remote_log_url")]
    public required string RemoteLogUrl { get; init; }

    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; init; }

    [JsonPropertyName("configured_by")]
    public required string ConfiguredBy { get; init; }

    public MatchZyServerConfiguredEvent() : base("server_configured")
    {
    }
}

public class MatchZyServerHealthEvent : MatchZyEvent
{
    [JsonPropertyName("server_id")]
    public required string ServerId { get; init; }

    [JsonPropertyName("plugin_version")]
    public required string PluginVersion { get; init; }

    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; init; }

    [JsonPropertyName("db_ok")]
    public required bool DbOk { get; init; }

    [JsonPropertyName("db_type")]
    public required string DbType { get; init; } // "sqlite" | "mysql"

    [JsonPropertyName("db_error")]
    public string? DbError { get; init; }

    [JsonPropertyName("reason")]
    public string? Reason { get; init; } // "startup" | "periodic" | "change"

    public MatchZyServerHealthEvent() : base("server_health")
    {
    }
}

public class MatchZyTestEvent : MatchZyMatchEvent
{
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; init; }

    [JsonPropertyName("triggered_by")]
    public required string TriggeredBy { get; init; }

    // Optional metadata to help external controllers attribute connectivity checks
    // cleanly per server and per logical match slug.
    [JsonPropertyName("server_id")]
    public string? ServerId { get; init; }

    [JsonPropertyName("match_slug")]
    public string? MatchSlug { get; init; }

    public MatchZyTestEvent() : base("test_event")
    {
    }
}

// CS2 update status events (server-level)
public class MatchZyCs2UpdateRequiredEvent : MatchZyMatchEvent
{
    [JsonPropertyName("server_id")]
    public required string ServerId { get; init; }

    [JsonPropertyName("required_version")]
    public required int RequiredVersion { get; init; }

    // Best-effort marker so the API/admin UI can distinguish "update found"
    // vs "about to quit now".
    [JsonPropertyName("phase")]
    public string? Phase { get; init; }

    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; init; }

    public MatchZyCs2UpdateRequiredEvent() : base("cs2_update_required")
    {
    }
}