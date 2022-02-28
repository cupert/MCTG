using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardGame;
using Type = System.Type;

namespace MonsterTradingCardGame
{
    public class Game
    {

        private DB db = new DB();

        public void StartFight(User Player1, User Player2)
        {
            int GoblinsDragons,
                OrksWizard,
                WaterspellKnight,
                KrakenSpell,
                FireElvesDragon,
                Round = 1,
                rndPlayer1,
                rndPlayer2;

            bool Roundover;

            double P1Mult, P2Mult, P1Dmg, P2Dmg;

            Card PlayerCard1, PlayerCard2;

            Random rnd = new Random();

            List<Card> Player1BattleDeck = db.SelectBattleDeck(Player1.Username);
            List<Card> Player2BattleDeck = db.SelectBattleDeck(Player2.Username);

            Console.WriteLine($"Game: {Player1.Username} vs {Player2.Username} \n Fight!\n");

            while (Player1BattleDeck.Any() && Player2BattleDeck.Any() && Round < 101)
            {
                Console.WriteLine($"Round: {Round}");

                rndPlayer1 = rnd.Next(Player1BattleDeck.Count);
                rndPlayer2 = rnd.Next(Player2BattleDeck.Count);

                Console.WriteLine($"{Player1BattleDeck[rndPlayer1].Name} vs {Player2BattleDeck[rndPlayer2].Name}");

                PlayerCard1 = Player2BattleDeck[rndPlayer1];
                PlayerCard2 = Player2BattleDeck[rndPlayer2];

                Roundover = false;

                //1 - P1 wins 2 - P2 wins 0 - Event not valid

                //MonsterEvents
                GoblinsDragons = GoblinsDragonsEvent(PlayerCard1, PlayerCard2);

                OrksWizard = OrksWizardEvent(PlayerCard1, PlayerCard2);

                //Mixed

                WaterspellKnight = WaterspellKnightEvent(PlayerCard1, PlayerCard2);

                KrakenSpell = KrakenSpellEvent(PlayerCard1, PlayerCard2);

                FireElvesDragon = FireElvesDragonEvent(PlayerCard1, PlayerCard2);

                //Console.WriteLine($"Numbers of Events: {GoblinsDragons} {OrksWizard} {WaterspellKnight} {KrakenSpell} {FireElvesDragon}\n");

                //check if Special event occurs
                if (GoblinsDragons > 0 || OrksWizard > 0 || WaterspellKnight > 0 || KrakenSpell > 0 ||
                    FireElvesDragon > 0)
                {
                    if (GoblinsDragons == 1 || OrksWizard == 1 || WaterspellKnight == 1 || KrakenSpell == 1 ||
                        FireElvesDragon == 1)
                    {
                        //Player 1 Wins
                        Console.WriteLine("Player 1 Wins - Specialevent");
                        Player1BattleDeck.Add(PlayerCard2);
                        Player2BattleDeck.RemoveAt(rndPlayer2);
                    }
                    else
                    {
                        //Player 2 Wins
                        Console.WriteLine("Player 2 Wins - Specialevent");
                        Player2BattleDeck.Add(PlayerCard1);
                        Player1BattleDeck.RemoveAt(rndPlayer1);
                    }

                    Roundover = true;
                }

                //check if pure Monsterfight
                if (PlayerCard1.Type != Type.Spell &&
                    PlayerCard2.Type != Type.Spell && !Roundover)
                {
                    if (PlayerCard1.Damage > PlayerCard2.Damage)
                    {
                        //Player 1 Wins
                        Console.WriteLine("Player 1 Wins - pure Monsterfight");
                        Player1BattleDeck.Add(PlayerCard2);
                        Player2BattleDeck.RemoveAt(rndPlayer2);
                    }
                    else
                    {
                        //Player 2 Wins
                        Console.WriteLine("Player 2 Wins - pure Monsterfight");
                        Player2BattleDeck.Add(PlayerCard1);
                        Player1BattleDeck.RemoveAt(rndPlayer1);
                    }

                    Roundover = true;

                }

                P1Mult = GetMult(PlayerCard1.Element, PlayerCard2.Element);
                P2Mult = GetMult(PlayerCard2.Element, PlayerCard1.Element);

                P1Dmg = P1Mult * PlayerCard1.Damage;
                P2Dmg = P2Mult * PlayerCard2.Damage;

                if (P1Dmg > P2Dmg && !Roundover)
                {
                    //Player 1 Wins
                    Console.WriteLine("Player 1 Wins - Normal fight");
                    Player1BattleDeck.Add(PlayerCard2);
                    Player2BattleDeck.RemoveAt(rndPlayer2);
                }
                else if (P1Dmg < P2Dmg && !Roundover)
                {
                    //Player 2 Wins
                    Console.WriteLine("Player 2 Wins - Normal fight");
                    Player2BattleDeck.Add(PlayerCard1);
                    Player1BattleDeck.RemoveAt(rndPlayer1);
                }

                Round++;


            }

            if (Player2BattleDeck.Count == 0)
            {
                Console.WriteLine("Player 1 Won the Game!");
                Player1.Elo += 3;
                Player1.Wins++;
                Player2.Elo -= 5;
                Player2.Loss++;
            }
            else if (Player1BattleDeck.Count == 0)
            {
                Console.WriteLine("Player 2 Won the Game!");
                Player2.Elo += 3;
                Player2.Wins++;
                Player1.Elo -= 5;
                Player1.Loss++;
            }
            else
            {
                Console.WriteLine("Draw, nobody won the Game!");
                Player2.Elo--;
                Player2.Draws++;
                Player1.Elo--;
                Player1.Draws++;

            }

            Console.WriteLine(db.UpdateStats(Player1));
            Console.WriteLine(db.UpdateStats(Player2));
        }

        public double GetMult(Element element1, Element element2)
        {
            if (element1 == Element.Fire && element2 == Element.Ground || element1 == Element.Ground && element2 == Element.Water || element1 == Element.Water && element2 == Element.Fire)
            {
                return 2;
            }
            else if (element1 == Element.Fire && element2 == Element.Water ||
                     element1 == Element.Water && element2 == Element.Ground ||
                     element1 == Element.Ground && element2 == Element.Fire)
            {
                return 0.5;
            }
            else
            {
                return 1;
            }
        }

        public int GoblinsDragonsEvent(Card Player1, Card Player2)
        {
            if (Player1.Name.Contains("Goblin") && Player2.Name.Contains("Dragon"))
            {
                //Player 2 Wins
                return 2;
            }
            else if (Player2.Name.Contains("Goblin") && Player1.Name.Contains("Dragon"))
            {
                //Player 1 Wins
                return 1;
            }
            else
            {
                //event does not occur
                return 0;
            }
        }

        public int OrksWizardEvent(Card Player1, Card Player2)
        {
            if (Player1.Name.Contains("Ork") && Player2.Name.Contains("Wizard"))
            {
                //Player 2 Wins
                return 2;
            }
            else if (Player2.Name.Contains("Ork") && Player1.Name.Contains("Wizard"))
            {
                //Player 1 Wins
                return 1;
            }
            else
            {
                //event does not occur
                return 0;
            }
        }

        public int WaterspellKnightEvent(Card Player1, Card Player2)
        {
            if (Player1.Element == Element.Water && Player1.Type == Type.Spell &&
                Player2.Name.Contains("Knight"))
            {
                //Player 1 Wins
                return 1;
            }
            else if (Player2.Element == Element.Water && Player2.Type == Type.Spell &&
                     Player1.Name.Contains("Knight"))
            {
                //Player 2 Wins
                return 2;
            }
            else
            {
                //event does not occur
                return 0;
            }
        }

        public int KrakenSpellEvent(Card Player1, Card Player2)
        {
            if (Player1.Name.Contains("Kraken") && Player2.Type == Type.Spell)
            {
                //Player 1 Wins
                return 1;
            }
            else if (Player2.Name.Contains("Kraken") && Player1.Type == Type.Spell)
            {
                //Player 2 Wins
                return 2;
            }
            else
            {
                //event does not occur
                return 0;
            }
        }

        public int FireElvesDragonEvent(Card Player1, Card Player2)
        {
            if (Player1.Element == Element.Fire && Player1.Name.Contains("Elve") &&
                Player2.Name.Contains("Dragon"))
            {
                //Player 1 Wins
                return 1;
            }
            else if (Player1.Element == Element.Fire && Player1.Name.Contains("Elve") &&
                     Player2.Name.Contains("Dragon"))
            {
                //Player 2 Wins
                return 2;
            }
            else
            {
                //event does not occur
                return 0;
            }
        }
    }
}
