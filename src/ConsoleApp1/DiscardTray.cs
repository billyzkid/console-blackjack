using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class DiscardTray
    {
        public DiscardTray()
        {
            Cards = new List<Card>();
        }

        public IList<Card> Cards { get; }
    }
}
