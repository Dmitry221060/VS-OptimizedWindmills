namespace OptimizedWindmills {
    class ModConfig {
        [Newtonsoft.Json.JsonIgnore]
        public static ModConfig Loaded { get; set; } = new ModConfig();
        public float windmillEfficiency { get; set; } = 0.4f;
    }
}
