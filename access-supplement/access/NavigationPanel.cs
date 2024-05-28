using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using access_supplement;
using AccessSupplement;
using Freetravel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using static StardewValley.Menus.SocialPage;

namespace access_supplement
{
    public class NavigationPanel : IClickableMenu
    {
        private ModConfig Config;

        public NavigationPanel(ModConfig config) : base((int)GetMenuPosition().X, (int)GetMenuPosition().Y, 600 + IClickableMenu.borderWidth * 2, 100 + IClickableMenu.borderWidth * 2 + 64)
        {
            this.Config = config;
        }

        public override void receiveKeyPress(Keys key)
        {
            if (Config.CloseMenu.JustPressed())
            {
                Game1.exitActiveMenu();
                Game1.playSound("bigDeSelect");
            }
        }

        public static Vector2 GetMenuPosition()
        {
            Vector2 result = new Vector2(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (100 + IClickableMenu.borderWidth * 2 + 64) / 2);
            if (result.X + (float)(600 + IClickableMenu.borderWidth * 2) > (float)Game1.viewport.Width)
            {
                result.X = 0f;
            }

            if (result.Y + (float)(100 + IClickableMenu.borderWidth * 2 + 64) > (float)Game1.viewport.Height)
            {
                result.Y = 0f;
            }

            return result;
        }

    }
}