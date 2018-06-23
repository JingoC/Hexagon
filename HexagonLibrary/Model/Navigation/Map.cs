using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexagonLibrary.Model.Navigation
{
    using HexagonLibrary.Entity.Players;

    using Entity.GameObjects;
    using WinSystem.Controls;

    using Microsoft.Xna.Framework;

    public class Map : Container
    {
        private List<Player> players;
        Random r = new Random((int) DateTime.Now.Ticks);
        public List<List<HexagonObject>> Rows { get; private set; } = new List<List<HexagonObject>>();
        public List<List<HexagonObject>> Columns { get; private set; } = new List<List<HexagonObject>>();

        public int Column { get; private set; }
        public int Row { get; private set; }

        public Map(List<Player> players) : this(players, 10, 10)
        {
            
        }

        public Map(List<Player> players, int row, int column)
        {
            this.players = players;
            this.Row = row;
            this.Column = column;
            
            for(int i = 0; i < this.Row; i++)
            {
                this.Rows.Add(new List<HexagonObject>());
                for (int j = 0; j < this.Column; j++)
                {
                    if (i == 0)
                    {
                        this.Columns.Add(new List<HexagonObject>());
                    }

                    this.AddItem(new HexagonObject(-1) { Visible = false }, i, j);
                }
            }
        }

        public void SetItem(HexagonObject item, int row, int column)
        {
            int x = item.Width * column + ((row % 2 == 0) ? 0 : (int)(24 * item.Scale));
            int y = item.Height * row + row * (int)(-12 * item.Scale);
            item.Position = new Vector2(x, y);
            item.SectorId = row * this.Column + column;
            this.Items[row * this.Column + column] = item;
            this.Rows[row][column] = item;
            this.Columns[column][row] = item;
        }

        private void AddItem(HexagonObject item, int row, int column)
        {
            this.Items.Add(item);
            this.Rows[row].Add(item);
            this.Columns[column].Add(item);
        }

        public GameObjectPositionInfo GetPositionInfo(HexagonObject hObj)
        {
            int row = hObj.SectorId / this.Column;
            int column = hObj.SectorId - (row * this.Column);
            return this.GetPositionInfo(row, column);
        }

        public bool IsLinkedObjects(HexagonObject o1, HexagonObject o2)
        {
            return this.GetPositionInfo(o1).AroundObjects.Exists((x)=>x.Equals(o2));
        }

        public GameObjectPositionInfo GetPositionInfo(int row, int column)
        {
            GameObjectPositionInfo pi = new GameObjectPositionInfo();
            pi.Current = this.Rows[row][column];

            bool isRowTop = row == 0;
            bool isRowBotton = row == this.Row - 1;
            bool isColumnLeft = column == 0;
            bool isColumnRight = column == this.Column - 1;

            var empty = new HexagonObject(-1) { Type = TypeHexagon.Blocked, Life = 1000 };
            if ((row % 2) == 0)
            {
                pi.AroundObjects.Add(!isRowTop && !isColumnLeft ? this.Rows[row - 1][column - 1] : empty);
                pi.AroundObjects.Add(!isRowTop ? this.Rows[row - 1][column] : empty);
                pi.AroundObjects.Add(!isColumnRight ? this.Rows[row][column + 1] : empty);
                pi.AroundObjects.Add(!isRowBotton ? this.Rows[row + 1][column] : empty); 
                pi.AroundObjects.Add(!isRowBotton && !isColumnLeft ? this.Rows[row + 1][column - 1] : empty);
                pi.AroundObjects.Add(!isColumnLeft ? this.Rows[row][column - 1] : empty);
            }
            else
            {
                pi.AroundObjects.Add(!isRowTop ? this.Rows[row - 1][column] : empty);
                pi.AroundObjects.Add(!isRowTop && !isColumnRight ? this.Rows[row - 1][column + 1] : empty);
                pi.AroundObjects.Add(!isColumnRight ? this.Rows[row][column + 1] : empty);
                pi.AroundObjects.Add(!isRowBotton && !isColumnRight ? this.Rows[row + 1][column + 1] : empty);
                pi.AroundObjects.Add(!isRowBotton ? this.Rows[row + 1][column] : empty);
                pi.AroundObjects.Add(!isColumnLeft ? this.Rows[row][column - 1] : empty);
            }
            
            return pi;
        }

        public bool Attack(HexagonObject src, HexagonObject dst)
        {
            if (((src == null) || (dst == null)) ||
                ((src.SectorId < 0) || (dst.SectorId < 0))
                )
                return false;

            if (src.Life < 1)
                return false;

            int[] percentsPositive = { 30, 60, 80, 100 };
            int[] percentsNegative = { 25, 15, 5, 0 };

            bool isSrcLifeLarge = src.Life >= dst.Life;
            int diff = Math.Abs(src.Life - dst.Life);
            diff = diff > 3 ? 3 : diff;

            bool isHold = isSrcLifeLarge ?
                r.Next(101) < percentsPositive[diff] :
                r.Next(101) < percentsNegative[diff];

            void ActionHold(HexagonObject obj)
            {
                obj.BelongUser = src.BelongUser;
                obj.SetDefaultTexture((TypeTexture)(TypeTexture.UserIdle0 + src.BelongUser));
                obj.Type = TypeHexagon.Enemy;
                obj.Visible = true;
            }

            void ActionBlocked(HexagonObject obj)
            {
                obj.BelongUser = -1;
                obj.Type = TypeHexagon.Blocked;
                obj.Bonus = TypeHexagonBonus.None;
                obj.Visible = false;
            }
            
            void AttackFree()
            {
                if (isHold)
                {
                    switch (dst.Bonus)
                    {
                        case TypeHexagonBonus.None:
                        {
                            dst.Life = isSrcLifeLarge ? src.Life - (dst.Life == 0 ? 1 : dst.Life) : 0;
                            src.Life = 0;
                            ActionHold(dst);
                        }
                        break;
                        case TypeHexagonBonus.Bomb:
                        {
                            void RecursiveBomb(HexagonObject obj)
                            {
                                obj.Life = 0;
                                ActionBlocked(obj);

                                var pi = this.GetPositionInfo(obj);

                                foreach (var h in pi.AroundObjects)
                                {
                                    if (h != null)
                                    {
                                        if (h.Bonus == TypeHexagonBonus.Bomb) RecursiveBomb(h);
                                        else h.Life = h.Life > obj.Loot ? h.Life - obj.Loot : 0;
                                    }
                                }
                            }

                            RecursiveBomb(dst);
                        }
                        break;
                        default: break;
                    }
                }
                else
                {
                    dst.Life = isSrcLifeLarge ? 0 : dst.Life - src.Life;
                    src.Life = 0;
                }
            }
            
            switch (dst.Type)
            {
                case TypeHexagon.Enemy: AttackFree(); break;
                case TypeHexagon.Free: AttackFree(); break;
                default: break;
            }

            return isHold;
        }

        public void Create(Player p, HexagonObject h)
        {
            h.Life = 0;
            h.BelongUser = p.ID;
            h.SetDefaultTexture((TypeTexture)(TypeTexture.UserIdle0 + p.ID));
            h.Type = TypeHexagon.Enemy;
            h.Bonus = TypeHexagonBonus.None;
            h.Visible = true;
            
            p.LootPoints -= Player.LootPointForCreate;
        }
    }
}
