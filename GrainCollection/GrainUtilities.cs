namespace GrainCollection
{
    public static class GrainUtilities
    {
        static GrainUtilities()
        {
            GrainResolutionPrefix = typeof(FooGrain).Namespace;
        }

        public static string GrainResolutionPrefix { get; set; }
    }
}
