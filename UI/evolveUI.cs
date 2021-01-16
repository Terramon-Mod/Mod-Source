using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Localisation;
using Terramon.Items.Evolution;
using Terramon.Items.MiscItems.LinkCables;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Network.Catching;
using Terramon.Pokemon;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terramon.UI.SidebarParty
{
    // ExampleUIs visibility is toggled by typing "/coin" in chat. (See CoinCommand.cs)
    // ExampleUI is a simple UI example showing how to use UIPanel, UIImageButton, and even a custom UIElement.
    public class EvolveUI : UIState
    {
        private DragableUIPanel mainPanel;
        public static bool Visible;
        public bool lightmode = true;

        public Texture2D test;

        private UIText PokemonGoesHere;
        private UIText RareCandiesGoHere;

        private UIHoverImageButton SaveButton;

        public VanillaItemSlotWrapper partyslot1;
        public VanillaItemSlotWrapper partyslot2;

        public ILocalisedBindableString placeMonString =
            TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("evolveUI.placemon", "Place a Pokémon in the first slot.")));
        public ILocalisedBindableString placeCandyText = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("evolveUI.placeItem", "Place {0} {1} in the second slot.")));
        public ILocalisedBindableString pressEvolveText = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("evolveUI.pressEvolve", "Great! Press the evolve button!")));
        public ILocalisedBindableString cannotEvolveText = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("evolveUI.cannotEvolve", "This Pokémon cannot evolve!")));
        public ILocalisedBindableString rareCandyText = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("rareCandy", "Rare Candy")));
        public ILocalisedBindableString linkCableText = TerramonMod.Localisation.GetLocalisedString(new LocalisedString(("linkCable","Link Cable")));


        // In OnInitialize, we place various UIElements onto our UIState (this class).
        // UIState classes have width and height equal to the full screen, because of this, usually we first define a UIElement that will act as the container for our UI.
        // We then place various other UIElement onto that container UIElement positioned relative to the container UIElement.
        public override void OnInitialize()
        {
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.

            //pokemon icons


            // Next, we create another UIElement that we will place. Since we will be calling `mainPanel.Append(playButton);`, Left and Top are relative to the top left of the mainPanel UIElement. 
            // By properly nesting UIElements, we can position things relatively to each other easily.
            mainPanel = new DragableUIPanel();
            mainPanel.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            mainPanel.HAlign = 0.4f;
            mainPanel.VAlign = 0.65f;
            mainPanel.Width.Set(180, 0f);
            mainPanel.Height.Set(70f, 0f);

            partyslot1 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot1.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot1.HAlign = 0.15f;
            partyslot1.VAlign = 0.5f;
            partyslot1.ValidItemFunc = item => item.IsAir || TerramonMod.PokeballFactory.GetEnum(item.modItem) !=
                                               TerramonMod.PokeballFactory.Pokebals.Nothing;
            mainPanel.Append(partyslot1);

            partyslot2 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem);
            partyslot2.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(mainPanel);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            partyslot2.HAlign = 0.65f;
            partyslot2.VAlign = 0.5f;
            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is RareCandy;
            mainPanel.Append(partyslot2);

            PokemonGoesHere = new UIText("0/0");
            PokemonGoesHere.HAlign = 0.5f;
            PokemonGoesHere.VAlign = 1.5f;
            //PokemonGoesHere.SetText("Place a Pokémon in the first slot.");
            PokemonGoesHere.SetText(placeMonString.Value);
            mainPanel.Append(PokemonGoesHere);

            RareCandiesGoHere = new UIText("0/0");
            RareCandiesGoHere.HAlign = 0.5f;
            RareCandiesGoHere.VAlign = 1.7f;
            RareCandiesGoHere.SetText("");
            mainPanel.Append(RareCandiesGoHere);

            Texture2D buttonSaveTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            SaveButton = new UIHoverImageButton(buttonSaveTexture, "Evolve!"); // Localized text for "Close"
            SaveButton.HAlign = 0.95f;
            SaveButton.VAlign = 0.53f;
            SaveButton.Width.Set(30, 0f);
            SaveButton.Height.Set(30, 0f);
            SaveButton.OnClick += EvolveButtonClicked;

            //Texture2D buttonSaveTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            //UIHoverImageButton SaveButton = new UIHoverImageButton(buttonSaveTexture, "Evolve"); // Localized text for "Close"
            //SaveButton.Left.Set(33, 0f);
            //SaveButton.Top.Set(7, 0f);
            //SaveButton.Width.Set(30, 0f);
            //SaveButton.Height.Set(30, 0f);
            //SaveButton.OnClick += new MouseEvent();
            //mainPanel.Append(SaveButton);

            Append(mainPanel);


            // As a recap, ExampleUI is a UIState, meaning it covers the whole screen. We attach mainPanel to ExampleUI some distance from the top left corner.
            // We then place playButton, closeButton, and moneyDiplay onto mainPanel so we can easily place these UIElements relative to mainPanel.
            // Since mainPanel will move, this proper organization will move playButton, closeButton, and moneyDiplay properly when mainPanel moves.
        }

        public int eeveelution = 0;
        public int i;

        public override void Update(GameTime gameTime)
        {
            // Don't delete this or the UIElements attached to this UIState will cease to function.
            base.Update(gameTime);

            if (partyslot1.Item.IsAir)
            {
                //PokemonGoesHere.SetText("Place a Pokémon in the first slot.");
                PokemonGoesHere.SetText(placeMonString.Value);
                PokemonGoesHere.TextColor = Color.White;
                mainPanel.RemoveChild(partyslot2);
                i = 0;
            }
            else
            {
                if (partyslot1.Item.modItem is BaseCaughtClass pokeball)
                {
                    var mon = TerramonMod.GetPokemon(pokeball.CapturedPokemon);
                    if (mon != null && mon.CanEvolve)
                    {
                        if (mon.EvolveItem == EvolveItem.RareCandy)
                        {
                            //Set preference to RareCandy
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is RareCandy;
                            //PokemonGoesHere.SetText($"Place {mon.EvolveCost} Rare Candies in the second slot.");
                            placeCandyText.Args = new object[] {mon.EvolveCost, rareCandyText.Value};
                            PokemonGoesHere.SetText(placeCandyText.Value);
                            PokemonGoesHere.TextColor = Color.White;
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is RareCandy &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.LinkCable)
                        {
                            //Set preference to LinkCable
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is LinkCable;

                            PokemonGoesHere.SetText("Place a Link Cable in the second slot.");
                            PokemonGoesHere.TextColor = new Color(60, 171, 70);
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is LinkCable &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.FireStone)
                        {
                            //Set preference to FireStone
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is FireStone;

                            PokemonGoesHere.SetText("Place a Fire Stone in the second slot.");
                            PokemonGoesHere.TextColor = new Color(255, 202, 99);
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is FireStone &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.ThunderStone)
                        {
                            //Set preference to ThunderStone
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is ThunderStone;

                            PokemonGoesHere.SetText("Place a Thunder Stone in the second slot.");
                            PokemonGoesHere.TextColor = new Color(97, 255, 69);
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is ThunderStone &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.WaterStone)
                        {
                            //Set preference to WaterStone
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is WaterStone;

                            PokemonGoesHere.SetText("Place a Water Stone in the second slot.");
                            PokemonGoesHere.TextColor = new Color(10, 120, 255);
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is WaterStone &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.LeafStone)
                        {
                            //Set preference to LeafStone
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is LeafStone;

                            PokemonGoesHere.SetText("Place a Leaf Stone in the second slot.");
                            PokemonGoesHere.TextColor = new Color(55, 176, 115);
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is LeafStone &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.MoonStone)
                        {
                            //Set preference to MoonStone
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is MoonStone;

                            PokemonGoesHere.SetText("Place a Moon Stone in the second slot.");
                            PokemonGoesHere.TextColor = new Color(64, 67, 71);
                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is MoonStone &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                //PokemonGoesHere.SetText("Great! Press the evolve button!");
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                        else if (mon.EvolveItem == EvolveItem.Eeveelution)
                        {
                            //This a special evolve condition applying to only Eevee with custom logic
                            partyslot2.ValidItemFunc = item => item.IsAir || item.modItem is FireStone || item.modItem is ThunderStone || item.modItem is WaterStone;

                            PokemonGoesHere.SetText("Place an evolution stone in the second slot.");
                            PokemonGoesHere.TextColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);

                            mainPanel.Append(partyslot2);
                            if (!partyslot2.Item.IsAir && partyslot2.Item.modItem is FireStone || partyslot2.Item.modItem is ThunderStone || partyslot2.Item.modItem is WaterStone &&
                                partyslot2.Item.stack == mon.EvolveCost)
                            {
                                if (partyslot2.Item.modItem is FireStone)
                                {
                                    eeveelution = 1; // Evolve to Flareon
                                } else if (partyslot2.Item.modItem is ThunderStone)
                                {
                                    eeveelution = 2; // Evolve to Jolteon
                                } else
                                {
                                    eeveelution = 3; // Evolve to Vaporeon
                                }
                                PokemonGoesHere.SetText(pressEvolveText.Value);
                                PokemonGoesHere.TextColor = Color.White;

                                mainPanel.Append(SaveButton);
                                i = 0;
                            }
                            else
                            {
                                mainPanel.RemoveChild(SaveButton);
                            }
                        }
                    }
                    else
                    {
                        PokemonGoesHere.SetText(cannotEvolveText.Value);
                        PokemonGoesHere.TextColor = Color.White;
                    }
                }
            }
        }

        private void EvolveButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            int whicheverballtype = TerramonMod.PokeballFactory.GetPokeballType(partyslot1.Item.modItem);

            // stuff break
            if (partyslot1.Item.modItem is BaseCaughtClass pokeball)
            {
                var mon = TerramonMod.GetPokemon(pokeball.CapturedPokemon);
                if (mon == null)
                {
                    Main.NewText(cannotEvolveText.Value);
                    return;
                }

                var evolved = TerramonMod.GetPokemon(mon.EvolveTo.Name);
                string evolvedname = evolved.GetType().Name;

                if (evolved.GetType().Name == "Flareon")
                {
                    if (eeveelution == 1)
                    {
                        evolvedname = "Flareon";
                    } else if (eeveelution == 2)
                    {
                        evolvedname = "Jolteon";
                    } else
                    {
                        evolvedname = "Vaporeon";
                    }
                }

                if (evolved == null)
                {
                    Main.NewText(cannotEvolveText.Value);
                    return;
                }

                pokeball.PokeData.Pokemon = evolved.GetType().Name;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    BaseCatchPacket packet = new BaseCatchPacket();
                    packet.Send(TerramonMod.Instance, evolved.GetType().Name, evolved.GetType().Name,
                        Main.LocalPlayer.getRect(), whicheverballtype, pokeball.PokeData);
                }
                else
                {
                    int index = Item.NewItem(Main.LocalPlayer.getRect(), whicheverballtype);
                    if (index >= 400)
                        return;
                    if (Main.item[index].modItem is BaseCaughtClass item)
                    {
                        item.PokemonName = evolved.GetType().Name;
                        item.CapturedPokemon = evolved.GetType().Name;
                        item.PokeData = pokeball.PokeData;
                    }
                }

                eeveelution = 0;
            }

            Visible = false;
            partyslot1.Item.TurnToAir();
            partyslot2.Item.TurnToAir();
            mainPanel.RemoveChild(SaveButton);
            Main.LocalPlayer.talkNPC = 0;
        }
    }
}
