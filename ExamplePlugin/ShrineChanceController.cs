

namespace ExamplePlugin
{
    public static class ShrineChanceController
    {
        public static bool IsEnabled { get { return enabled; } }

        public static void ToggleEnabled() => enabled = !enabled;

        public static void Enable() => enabled = true;

        public static void Disable() => enabled = false;

        public static void MakeShrineImpossible()
            => failureChance = SHRINE_CHANCE_IMPOSSIBLE;

        public static void MakeShrineGuaranteed()
            => failureChance = SHRINE_CHANCE_GUARANTEED;

        public static float GetDesiredFailureChance()
            => failureChance;

        private static bool enabled = false;

        private static float failureChance;

        private static readonly float SHRINE_CHANCE_IMPOSSIBLE = 1.0f;

        private static readonly float SHRINE_CHANCE_GUARANTEED = 0.0f;
    }

}