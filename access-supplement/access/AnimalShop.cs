using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace AccessSupplement
{
    public class AnimalShop
    {
        private int AnimalshopBuildIndex = 0;

        public void OnKeyPressed(IModHelper helper, ModConfig config, API api)
        {
            if(helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "onFarm").GetValue() && !helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "namingAnimal").GetValue())
            {
                //按下上一个建筑
                if(config.AnimalShopBuildUp.JustPressed())
                {
                    this.BuildIndexUp(api);
                }
                //按下下一个建筑
                if(config.AnimalShopBuildDown.JustPressed())
                {
                    this.BuildIndexDown(api);
                }
                //按下购买
                if(config.AnimalShopPurchase.JustPressed())
                {
                    this.Purchase(helper, api);
                }
            } else
            {
                this.AnimalshopBuildIndex = 0;
            }

        }

        //朗读建筑名称
        private void ReadBuildName(API api)
        {
            string name = Game1.locations[0].buildings[this.AnimalshopBuildIndex].GetData().Name;
            MatchCollection match = Regex.Matches(name, @"(?<=\s)(\S+)(?=\])");
            string text = Game1.content.LoadString(match[0].Value);
            api.Say($"{this.AnimalshopBuildIndex}：{text}", true);
        }

        private void Purchase(IModHelper helper, API api)
        {
            int x = (Game1.locations[0].buildings[this.AnimalshopBuildIndex].tileX.Value * Game1.tileSize) - Game1.viewport.X;
            int y = (Game1.locations[0].buildings[this.AnimalshopBuildIndex].tileY.Value * Game1.tileSize) - Game1.viewport.Y;
            Game1.activeClickableMenu.receiveLeftClick(x, y);
        }

        //建筑索引上移
        private void BuildIndexUp(API api)
        {
            if (Game1.locations[0].buildings.Count <= 1) return;
            this.AnimalshopBuildIndex--;
            if (this.AnimalshopBuildIndex < 1) this.AnimalshopBuildIndex = Game1.locations[0].buildings.Count - 1;
            this.ReadBuildName(api);
            Game1.playSound("shwip");
        }

        //建筑索引下移
        private void BuildIndexDown(API api)
        {
            if (Game1.locations[0].buildings.Count <= 1) return;
            this.AnimalshopBuildIndex++;
            if (this.AnimalshopBuildIndex >= Game1.locations[0].buildings.Count) this.AnimalshopBuildIndex = 1;
            this.ReadBuildName(api);
            Game1.playSound("shwip");
        }




    }
}