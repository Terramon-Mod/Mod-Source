using Microsoft.Xna.Framework;
using Razorwing.Framework.Allocation;
using Razorwing.Framework.Timing;

namespace Razorwing.Framework.Graphics.Transforms
{
    public interface ITransformable
    {
        InvokeOnDisposal BeginDelayedSequence(double delay, bool recursive = false);

        InvokeOnDisposal BeginAbsoluteSequence(double newTransformStartTime, bool recursive = false);

        /// <summary>
        /// The current frame's time as observed by this class's <see cref="Transform"/>s.
        /// </summary>
        GameTime Time { get; }

        double TransformStartTime { get; }

        void AddTransform(Transform transform, ulong? customTransformID = null);

        void RemoveTransform(Transform toRemove);
    }
}
