using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonLibrary.Entity.SystemObjects
{
    using Entity.GameObjects;

    public class Button : MonoObject
    {
        public event EventHandler Click;

        public Button()
        {

        }
    }
}
