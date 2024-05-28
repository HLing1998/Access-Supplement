using AccessSupplement;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using access_supplement;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using Freetravel;
using xTile.Tiles;
using StardewValley.GameData.WorldMaps;
using StardewValley.GameData.Locations;

namespace access_supplement
{
    public class ClubSlots
    {

        //俱乐部21点游戏键盘监听
        public void OnKeyPressed(object sender, ButtonPressedEventArgs e, IModHelper helper, ModConfig config, API api)
        {
            //10点赌注
            if (config.SlotsBet10.JustPressed() && api != null)
            {
                this.PressedSlotsBet10(helper);
            }
            //100点赌注
            if (config.SlotsBet100.JustPressed() && api != null)
            {
                this.PressedSlotsBet100(helper);
            }
            //阅读赌注
            if (config.SlotsReadBet.JustPressed() && api != null)
            {
                this.PressedSlotsReadBet(helper, api);
            }
                //阅读结果
                if (config.SlotsReadResult.JustPressed() && api != null)
            {
                this.PressedSlotsResult(helper, api);
            }
                //阅读齐币
                if(config.SlotsReadClubCoin.JustPressed())
            {
                this.readClubCoin(api);
            }

        }

        //按下10点赌注
        private void PressedSlotsBet10(IModHelper helper)
        {
            Vector2 v = Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport, 104, 52, -16, 32);
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)v.X +10, (int)v.Y +10, true);
        }

        //按下100点赌注
        private void PressedSlotsBet100(IModHelper helper)
        {
            Vector2 v = Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport, 124, 52, -16, 96);
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)v.X +10, (int)v.Y +10, true);
                }

        //阅读赌注
        private void PressedSlotsReadBet(IModHelper helper, API api)
        {
            int bet = helper.Reflection.GetField<int>(Game1.currentMinigame, "currentBet").GetValue();
            api.Say($"当前赌注：{bet}", true);
        }

        //阅读结果
        private void PressedSlotsResult(IModHelper helper, API api)
        {
            List<float> result = helper.Reflection.GetField<List<float>>(Game1.currentMinigame, "slotResults").GetValue();
            List<string> results = new List<string>();
            foreach(float f in result)
            {
                results.Add(this.GetObjectName((int)f));
            }
            api.Say($"结果：{string.Join(",", results)}", true);
        }

        //朗读齐币
        private void readClubCoin(API api)
        {
            api.Say($"齐币：{Game1.player.clubCoins}", true);
        }

        //获取物品名称
        private string GetObjectName(int index)
        {
            switch(index)
            {
                case 0:
                    return "防风草";
                case 1:
                    return "大壶牛奶";
                case 2:
                    return "虹鳟鱼";
                case 3:
                    return "鹦鹉螺";
                case 4:
                    return "甜瓜";
                case 5:
                    return "星之果实";
                case 6:
                    return "钻石";
                case 7:
                    return "樱桃";
                default:
                    return "防风草";
            }
        }

    }
}