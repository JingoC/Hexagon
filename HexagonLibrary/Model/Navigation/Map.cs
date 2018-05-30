using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexagonLibrary.Model.Navigation
{
    using Entity.GameObjects;
    using WinSystem.Controls;

    public class Map : Container
    {
        public List<List<HexagonObject>> Rows { get; private set; } = new List<List<HexagonObject>>();
        public List<List<HexagonObject>> Columns { get; private set; } = new List<List<HexagonObject>>();

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map() : this(10, 10)
        {

        }

        public Map(int row, int column)
        {
            this.Width = row;
            this.Height = column;

            for(int i = 0; i < row; i++)
            {
                this.Rows.Add(new List<HexagonObject>());
                for(int j = 0; j < column; j++)
                {
                    if (i == 0)
                    {
                        this.Columns.Add(new List<HexagonObject>());
                    }
                    this.AddItem(HexagonObject.Empty, i, j);
                }
            }
        }

        public void SetItem(HexagonObject item, int row, int column)
        {
            //49,38
            int x = item.Width * column + ((row % 2 == 0) ? 0 : 24);
            int y = item.Height * row + row * (-12);
            item.Position = new Microsoft.Xna.Framework.Vector2(x, y);
            item.SectorId = row * this.Height + column;
            item.Font = GameObject.GetFont(TypeFonts.TextHexagon);
            this.Items[row * this.Height + column] = item;
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
            int row = hObj.SectorId / this.Width;
            int column = hObj.SectorId - (row * this.Height);
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
            bool isRowBotton = row == this.Height - 1;
            bool isColumnLeft = column == 0;
            bool isColumnRight = column == this.Width - 1;

            var empty = new HexagonObject(-1);
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
    }
}
