using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonLibrary.Model
{
    using Entity.GameObjects;

    public class Map
    {
        public List<HexagonObject> Items { get; private set; } = new List<HexagonObject>();
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
            item.SectorId = row * column + column;
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
    }
}
