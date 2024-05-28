using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using access_supplement;
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
using AccessSupplement;

namespace Freetravel {
    public class FreeTravelPanel : IClickableMenu
    {
        public List<string> PointIDS;
        public int PointIndex;
        private ModConfig Config;
        private CallbackDelegate CallbackMethod;

        public FreeTravelPanel(CallbackDelegate callback, List<string> ps, ModConfig config) : base((int)GetMenuPosition().X, (int)GetMenuPosition().Y, 600 + IClickableMenu.borderWidth * 2, 100 + IClickableMenu.borderWidth * 2 + 64)
        {
            this.CallbackMethod = callback;
            this.PointIDS = ps;
            this.PointIndex = 0;
            this.Config = config;
        }

        public override void receiveKeyPress(Keys key)
        {
            if(this.PointIDS.Count > 0)
            {
                if(this.Config.FreeTravel_TravelPointUpRolling.JustPressed())
                {
                    this.PointIndex--;
if(this.PointIndex < 0)
                    {
                        this.PointIndex = this.PointIDS.Count - 1;
                    }
                    Game1.playSound("shwip");
                    this.CallbackMethod(this.PointIDS[this.PointIndex]);
                }
                if (this.Config.FreeTravel_TravelPointDownRolling.JustPressed())
                {
                    this.PointIndex++;
                    this.PointIndex = this.PointIndex % this.PointIDS.Count;
                    Game1.playSound("shwip");
                    this.CallbackMethod(this.PointIDS[this.PointIndex]);
                }
                if (this.Config.FreeTravel_SkipUp8.JustPressed())
                {
if(this.PointIndex -8 < 0)
                    {
                        this.PointIndex = 0;
                    } else
                    {
                        this.PointIndex -= 8;
                    }
                    Game1.playSound("shwip");
                    this.CallbackMethod(this.PointIDS[this.PointIndex]);
                }
                if (this.Config.FreeTravel_SkipDown8.JustPressed())
                {
                    if (this.PointIndex + 8 > this.PointIDS.Count -1)
                    {
                        this.PointIndex = this.PointIDS.Count - 1;
                    }
                    else
                    {
                        this.PointIndex += 8;
                    }
                    Game1.playSound("shwip");
                    this.CallbackMethod(this.PointIDS[this.PointIndex]);
                }


                if (Config.CloseMenu.JustPressed())
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
            }
            else
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
