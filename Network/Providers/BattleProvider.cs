using Razorwing.RPC.Factory;
using System;
using Terramon.Pokemon;
using Terraria.ModLoader;

namespace Terramon.Network.Providers
{
    public class BattleProvider : IIdentityProvider
    {
        public Type[] workingTypes => new[] {typeof(BattleModeV2)};
        public Identity GetIdentity(object input)
        {
            var battle = (BattleModeV2)input;

            return new Identity(nameof(BattleModeV2), nameof(TerramonMod), battle.BattleID)
            {
                ["st"] = (byte)battle.State,
            };

            throw new InvalidOperationException($"Can't handle {input.GetType().Name} in {nameof(BattleProvider)}");
        }

        public object GetObject(Identity identity)
        {
            switch (identity.Type)
            {
                case nameof(BattleModeV2):
                    var id = identity.GetString("val");
                    var battle = ModContent.GetInstance<TerramonWorld>().Battles[id];
                    battle.State = (BattleModeV2.BattleState)identity.GetByte("st");
                    return battle;
            }
            throw new InvalidOperationException($"Can't handle {identity.Type} in {nameof(BattleProvider)}");
        }
    }
}
