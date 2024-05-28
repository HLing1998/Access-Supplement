using AccessSupplement;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace access_supplement
{
    public class Club21D
    {
                private string endTitle21D = "";
        private string endMessage21D = "";

        //俱乐部21点游戏键盘监听
        public void OnKeyPressed(object sender, ButtonPressedEventArgs e, IModHelper helper, ModConfig config, API api)
        {
            //按下阅读玩家牌面
            if (config.ReadPlayerCards21D.JustPressed() && api != null)
            {
                this.ReadPlayerCards21D(helper,api);
            }
            //按下阅读庄家牌面
            if (config.ReadDealerCards21D.JustPressed() && api != null)
            {
                this.ReadDealerCards21D(helper,api);
            }
            //按下阅读赌注
            if (config.ReadBet21D.JustPressed() && api != null)
            {
                this.ReadBet21D(helper,api);
            }
            //按下阅读齐币
            if(config.ReadClubCoins21D.JustPressed() && api != null)
            {
                this.ReadClubCoins21D(api);
            }
            //按下加牌
            if (config.Hit21D.JustPressed() && api != null)
            {
                this.PlayerHit21D(helper);
            }
            //按下停止加牌
            if (config.Cease21D.JustPressed() && api != null)
            {
                this.PlayerCease21D(helper);
            }
            //按下新游戏
            if (config.NewGame21D.JustPressed() && api != null)
            {
                this.PlayerNewGame21D(helper);
            }
            //加倍游戏
            if (config.doubleOrNothing21D.JustPressed() && api != null)
            {
                this.PlayerDoubleGame21D(helper);
            }
            //结束游戏
            if (config.EndGame21D.JustPressed() && api != null)
            {
                this.PlayerEndGame21D(helper);
            }
        }


        //循环监听结束标题与信息
        public void OnGameLoop(API api)
        {
            if (Game1.currentMinigame is StardewValley.Minigames.CalicoJack m)
            {
                string endTitle = (string)Game1.currentMinigame.GetType().GetField("endTitle", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Game1.currentMinigame);
                string endMessage = (string)Game1.currentMinigame.GetType().GetField("endMessage", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Game1.currentMinigame);
                if (this.endTitle21D != endTitle || this.endMessage21D != endMessage)
                {
                    this.endTitle21D = endTitle;
                    this.endMessage21D = endMessage;
                    if (this.endTitle21D != "" && this.endMessage21D != "")
                    {
                        api.Say($"{this.endTitle21D}： {this.endMessage21D}", true);
                    }
            }
            }
        }

        //播报玩家牌面及点数
        private void ReadPlayerCards21D(IModHelper helper, API api)
        {
            List<int> cardslist = new List<int>();
            int point = 0;
            foreach (int[] n in helper.Reflection.GetField<List<int[]>>(Game1.currentMinigame, "playerCards").GetValue())
            {
                cardslist.Add(n[0]);
                point += n[0];
            }
            string cards = new string($"{Game1.player.name}： 点数{point}； 牌面：{string.Join(",", cardslist)}");
            api.Say(cards, true);
        }

        //播报庄家牌面及点数
        private void ReadDealerCards21D(IModHelper helper, API api)
        {
            List<int> cardslist = new List<int>();
            int point = 0;
            foreach (int[] n in helper.Reflection.GetField<List<int[]>>(Game1.currentMinigame, "dealerCards").GetValue())
            {
                cardslist.Add(n[0]);
                point += n[0];
            }
            string cards = new string($"庄家： 点数{point}； 牌面：{string.Join(",", cardslist)}");
            api.Say(cards, true);
        }

        //阅读赌注
        private void ReadBet21D(IModHelper helper, API api)
        {
            api.Say($"赌注：{Game1.currentMinigame.GetType().GetField("currentBet", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Game1.currentMinigame)}", true);
        }

        //阅读齐币
        private void ReadClubCoins21D(API api)
        {
            api.Say($"齐币：{Game1.player.clubCoins}", true);
        }

        //加牌
        private void PlayerHit21D(IModHelper helper)
        {
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)((float)Game1.graphics.GraphicsDevice.Viewport.Width / Game1.options.zoomLevel - 128f - (float)SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924"))) + 5, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 64 + 5, true);
        }

        //停止加牌
        private void PlayerCease21D(IModHelper helper)
        {
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)((float)Game1.graphics.GraphicsDevice.Viewport.Width / Game1.options.zoomLevel - 128f - (float)SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927"))) + 5, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + 32 + 5, true);
        }

        //新游戏
        private void PlayerNewGame21D(IModHelper helper)
        {
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) / Game1.options.zoomLevel) - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933")) / 2 + 5, (int)((float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2) / Game1.options.zoomLevel) + 64 + 16 + 5, true);
        }

        //21点加倍游戏
        private void PlayerDoubleGame21D(IModHelper helper)
        {
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) / Game1.options.zoomLevel) - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930")) / 2 + 5, (int)((float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2) / Game1.options.zoomLevel) + 5, true);
        }

        //21点退出游戏
        private void PlayerEndGame21D(IModHelper helper)
        {
            helper.Reflection.GetMethod(Game1.currentMinigame, "receiveLeftClick").Invoke((int)((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) / Game1.options.zoomLevel) - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936")) / 2 + 5, (int)((float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2) / Game1.options.zoomLevel) + 64 + 96 + 5, true);
        }

    }
}