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
    public class Renovate
    {

        public void OnKeyPressed(IModHelper helper, ModConfig config, API api)
        {
            if(config.RenovateCursorUp.JustPressed())
            {
                this.CursorUp(api);
            } else if(config.RenovateCursorDown.JustPressed())
            {
                this.CursorDown(api);
            } else if(config.RenovateCursorLeft.JustPressed())
            {
                this.CursorLeft(api);
            } else if(config.RenovateCursorRight.JustPressed())
            {
                this.CursorRight(api);
            }

        }

        //光标向上
        private void CursorUp(API api)
        {
            int y = (int)(Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64;
            Game1.viewport.Y = (int)((y - 1) * 64) - Game1.getOldMouseY(ui_scale: false);
            int x = (int)(Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64;
            y = (int)(Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64;
            string typename = this.GetTileType(x, y);
            api.Say($"{x}， {y}， {typename}", true);
        }

        //光标向下
        private void CursorDown(API api)
        {
            int y = (int)(Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64;
            Game1.viewport.Y = (int)((y + 1) * 64) - Game1.getOldMouseY(ui_scale: false);
            int x = (int)(Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64;
            y = (int)(Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64;
            string typename = this.GetTileType(x, y);
            api.Say($"{x}， {y}， {typename}", true);
        }

        //光标向左
        private void CursorLeft(API api)
        {
            int x = (int)(Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64;
            Game1.viewport.X = (int)((x - 1) * 64) - Game1.getOldMouseX(ui_scale: false);
            x = (int)(Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64;
            int y = (int)(Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64;
            string typename = this.GetTileType(x, y);
            api.Say($"{x}， {y}， {typename}", true);
        }

        //光标向右
        private void CursorRight(API api)
        {
            int x = (int)(Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64;
            Game1.viewport.X = (int)((x + 1) * 64) - Game1.getOldMouseX(ui_scale: false);
            x = (int)(Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64;
            int y = (int)(Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64;
            string typename = this.GetTileType(x, y);
            api.Say($"{x}， {y}， {typename}", true);
        }

        //获取坐标是否是堵塞或者门口
        private string GetTileType(int x, int y)
        {
            string typename = "空";
            foreach(Warp w in Game1.locations[1].warps)
            {
                if (w.X == x && w.Y == y) return w.TargetName +"入口";
            }
            if (Game1.locations[1].isObjectAtTile(x, y))
            {
                return Game1.locations[1].getObjectAtTile(x, y).DisplayName;
            }
            if (Game1.locations[1].IsTileBlockedBy(new Vector2(x, y))) return"墙壁";
            if (Game1.locations[1].isTilePassable(new Vector2(x, y))) return "空";


            return typename;
        }


    }
}