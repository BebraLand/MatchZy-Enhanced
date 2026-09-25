# Optional Map Advertisement integration

MatchZy exposes the current series map through the CounterStrikeSharp capability
`matchzy:current_map_number:v1`. It returns a one-based map number for BO1, BO3
and BO5, or `0` while there is no active series, during a transition, practice,
sleep or after the series ends.

The capability is optional: MatchZy does not depend on the advertisement plugin,
and consumers must handle the provider being absent. The companion advertisement
plugin queries it only when its own integration setting is enabled.

MatchZy also exposes its current `matchzy_chat_prefix` through
`matchzy:chat_prefix:v1`. Companion plugins can reuse the tournament's live chat
branding; this capability is optional and consumers must provide a fallback.
