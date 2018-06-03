using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.Controls
{
    public interface IControl : IDrawable
    {
        Vector2 Position { get; set; }

        event EventHandler OnClick;
        event EventHandler OnPressed;

        void Designer();
        void CheckEntry(float x, float y);
        void CheckEntryPressed(float x, float y);
    }
}
