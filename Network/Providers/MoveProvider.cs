using Razorwing.RPC.Factory;
using System;
using Terramon.Pokemon.Moves;

namespace Terramon.Network.Providers
{
    public class MoveProvider : IIdentityProvider
    {
        public Type[] workingTypes => new[] {typeof(BaseMove)};
        public Identity GetIdentity(object input)
        {
            var move = (BaseMove) input;

            return new Identity(nameof(BaseMove), nameof(TerramonMod), move.GetType().Name);
        }

        public object GetObject(Identity identity)
        {
            return TerramonMod.GetMove(identity.GetString("val"));
        }
    }
}
