namespace Pokdle.Models
{
    public class Guess
    {
        // Three possible values: 1 = correct, 0 = answer is greater than, -1 = answer is less than
        public int Gen { get; set; }
        // Three possible values: 1 = correct, 0 = partially correct, -1 = incorrect
        public int Type { get; set; }
        public int EvolutionCount { get; set; }
        // Three possible values: 1 = correct, 0 = partially correct, -1 = incorrect
        public int Abilities { get; set; }
        public int Top2BaseStats { get; set; }
        public int Bottom2BaseStats { get; set; }
        public int EvolutionStage { get; set; }
        public bool IsCorrect { get; set; }
    }
}
