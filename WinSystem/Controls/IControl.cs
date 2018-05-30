using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSystem.Controls
{
    public interface IControl
    {
        void Draw();
        void CheckEntry(float x, float y);
    }
}
