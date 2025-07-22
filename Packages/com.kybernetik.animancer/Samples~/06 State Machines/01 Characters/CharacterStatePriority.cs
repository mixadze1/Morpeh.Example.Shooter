// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

namespace Animancer.Samples.StateMachines
{
    /// <summary>Levels of importance for <see cref="CharacterState"/>s.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/characters">
    /// Characters</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/CharacterStatePriority
    /// 
    public enum CharacterStatePriority
    {
        // Enums are ints starting at 0 by default.
        // This means you can compare them with numerical operators like < and >.

        Low,// Could specify "Low = 0," if we want to be explicit or change the order.
        Medium,// Medium = 1,
        High,// High = 2,
    }
}
