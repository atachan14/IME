using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IME
{
    internal class MainButton
    {
        private int id;
        private string name;

        public MainButton(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
