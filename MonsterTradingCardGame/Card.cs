using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame
{

    public enum Element
    {
        Water,
        Fire,
        Ground
    }

    public enum Type
    {
        Spell,
        Elf,
        Dragon, 
        Goblin,
        Knight,
        Kraken,
        Ork,
        Wizard
    }

    public class Card
    {
        public string Id { set; get;}
        public string Name { get; set; }
        public double Damage { set; get; }
        public Element Element { set; get; }
        public Type Type { set; get; }

        public Card(string pId, string pName, double pDamage, Element pElement, Type pType)
        {
            Id = pId;
            Name = pName;
            Damage = pDamage;
            Element = pElement;
            Type = pType;
        }

        public string ShowCard()
        {
            return $"Name: {Name} Damage: {Damage} Type: {Element} Type: {Type} \n";
        }

    }
}
