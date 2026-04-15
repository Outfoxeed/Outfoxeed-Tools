namespace OutfoxeedTools
{
    /// <summary>
    /// Triggered when the World at the origin of a World loading is finally loaded
    /// </summary>
    public readonly struct RequestedWorldLoadedEvent
    {
        public readonly WorldConfig LoadedWorld;
        
        public RequestedWorldLoadedEvent(WorldConfig loadedWorld)
        {
            LoadedWorld = loadedWorld;
        }
    }
    
    /// <summary>
    /// Triggered when a World is loaded. The World can be a dependency to another.
    /// </summary>
    public readonly struct WorldLoadedEvents
    {
        public readonly WorldConfig LoadedWorld;

        public WorldLoadedEvents(WorldConfig loadedWorld)
        {
            LoadedWorld = loadedWorld;
        }
    };
}