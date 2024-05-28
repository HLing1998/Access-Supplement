using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace AccessSupplement
{
    public class GameInfo
    {
        private string petShopText = "";



        //阅读当前技能经验值
        public void ReadXP(IModHelper helper, API api)
        {
            //获取技能页和当前鼠标停留的技能名称
            List<IClickableMenu> pages = helper.Reflection.GetField<List<IClickableMenu>>(Game1.activeClickableMenu, "pages").GetValue();
            StardewValley.Menus.SkillsPage page = (StardewValley.Menus.SkillsPage)pages[1];
            int x = Game1.getMouseX(true), y = Game1.getMouseY(true);
            string skillname = "";
foreach (var area in page.skillAreas)
            {
                if (!area.containsPoint(x, y)) continue;
                skillname = Farmer.getSkillNameFromIndex(Convert.ToInt32(area.name));
            }
//根据技能名称查找对应技能索引和等级
            List<int> levels = this.GetLevelAndIndex(skillname);
            if (levels[0] != -1)
            {
                int exp = Game1.player.experiencePoints[levels[0]];
                int max_exp = 100;
                if (levels[1] < 10)
                {
                    max_exp = Farmer.getBaseExperienceForLevel(levels[1] + 1);
                }
                else
                {
                    max_exp = Farmer.getBaseExperienceForLevel(levels[1]);
                }
                    api.Say($"经验值： {exp} / {max_exp}", true);
            }
        }

        //获取等级和索引
        private List<int> GetLevelAndIndex(string name)
        {
            List<int> return_val = new List<int>();
                switch(name)
                {
                    case "Farming":
                        return_val.Add(0);
                        return_val.Add(Game1.player.FarmingLevel);
                        break;
                    case "Mining":
                        return_val.Add(3);
                        return_val.Add(Game1.player.MiningLevel);
                        break;
                    case "Foraging":
                        return_val.Add(2);
                        return_val.Add(Game1.player.ForagingLevel);
                        break;
                    case "Fishing":
                        return_val.Add(1);
                        return_val.Add(Game1.player.FishingLevel);
                        break;
                case "Combat":
                        return_val.Add(4);
                        return_val.Add(Game1.player.CombatLevel);
                        break;
                default:
                    return_val.Add(-1);
                    return_val.Add(-1);
                    break;
                }
            return return_val;
        }

        //阅读当前友好度
        public void ReadFriendship(IModHelper helper, API api)
        {
            List<IClickableMenu> pages = helper.Reflection.GetField<List<IClickableMenu>>(Game1.activeClickableMenu, "pages").GetValue();
            StardewValley.Menus.SocialPage page = (StardewValley.Menus.SocialPage)pages[2];
            int x = Game1.getMouseX(true), y = Game1.getMouseY(true);
            for (int i = 0; i < page.characterSlots.Count; i++)
            {
                if (!page.characterSlots[i].bounds.Contains(x, y)) continue;
                SocialPage.SocialEntry entry = page.GetSocialEntry(i);
                if (entry.Friendship != null)
                {
                    api.Say($"友好度：{entry.Friendship.Points}", true);
                } else
                {
                    api.Say("你还未见过此人，哪来的友好度！", true);
                }
                break;
            }
        }

        //阅读动物友好度
        public void ReadFriendshipAtAnimal(IModHelper helper, API api)
        {
            List<IClickableMenu> pages = helper.Reflection.GetField<List<IClickableMenu>>(Game1.activeClickableMenu, "pages").GetValue();
            StardewValley.Menus.AnimalPage page = (StardewValley.Menus.AnimalPage)pages[5];
            int x = Game1.getMouseX(true), y = Game1.getMouseY(true);
            for (int i = 0; i < page.characterSlots.Count; i++)
            {
                if (!page.characterSlots[i].bounds.Contains(x, y)) continue;
                AnimalPage.AnimalEntry entry = page.GetSocialEntry(i);
                if (entry.FriendshipLevel != null)
                {
                    api.Say($"友好度：{entry.FriendshipLevel}", true);
                }
                else
                {
                    api.Say("暂无数据", true);
                }
                break;
            }
        }

        //阅读玩家运气
        public void ReadLuck(IModHelper helper, API api)
        {
            double dayLuck = Game1.player.DailyLuck;
            int addLuck = Game1.player.LuckLevel;
            double jrluck = dayLuck * 100;
            double allLuck = jrluck + addLuck;
            api.Say($"今日幸运：{jrluck.ToString("n1")}%；幸运加成：{addLuck}%；目前幸运：{allLuck.ToString("n1")}%", true);
        }


    }
}