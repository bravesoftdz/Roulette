using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Roulette
{
    public enum OddEven { Odd, Even};
    public enum OutsideDozens { FirstTwelve, SecondTwelve, ThirdTwelve };
    public enum OutsideColumns { FirstColumn, SecondColumn, ThirdColumn }; 
    
    public class RouletteNumber
    {
        public string Number { get; set; }
        public Color? Color { get; set; }
        public OddEven? OddEven 
        {
            get
            {
                if (!IsZero())
                {
                    int num = Convert.ToInt32(Number);
                    if (num % 2 == 0)
                        return Roulette.OddEven.Even;
                    else
                        return Roulette.OddEven.Odd;
                }
                else
                    return null;
            }            
        }

        public bool IsZero()
        {
            return Convert.ToInt32(Number) == 0;
        }

        public OutsideDozens? IsInOutsizeDozen
        {
            get
            {        
                if (!IsZero())
                {
                    int num = Convert.ToInt32(Number);
                    if (num <= 12)
                        return Roulette.OutsideDozens.FirstTwelve;
                    if (num <= 24)
                        return Roulette.OutsideDozens.SecondTwelve;
                    if (num <= 36)
                        return Roulette.OutsideDozens.ThirdTwelve;
                }

                return null;
            }
        }

        public OutsideColumns? IsInOutsideColumn
        {
            get
            {
                if (!IsZero())
                {
                    double num = Convert.ToDouble(Number) / 3.0;
                    num = (Math.Round(num, 2) - (Math.Truncate(num)))  * 100;
                    if (num == 0)
                        return Roulette.OutsideColumns.ThirdColumn;
                    if (num >= 66)
                        return Roulette.OutsideColumns.SecondColumn;
                    if (num >= 33)
                        return Roulette.OutsideColumns.FirstColumn;                                                          
                }

                return null;
            }
        }

        public bool IsInLowerHalf
        {
            get
            {
                var num = Convert.ToInt32(Number);
                if (num <= 18)
                    return true;
                return false;
            }
        }

        public bool IsInUpperHalf
        {
            get
            {
                var num = Convert.ToInt32(Number);
                if (num >= 19)
                    return true;
                return false;
            }
        }

    }

    public class RouletteNumberList : List<RouletteNumber>
    {
        public RouletteNumberList()
        {
            Initialize();   
        }

        public RouletteNumber GetRouletteNumber(string number)
        {
            var q = (from x in this
                     where x.Number == number
                     select x).SingleOrDefault();
            return q;
        }

        protected void Initialize()
        {
            
            // add the zeros
            Add(new RouletteNumber()
            {
                Number = "0",
                Color = null                
            });

            Add(new RouletteNumber()
            {
                Number = "00",
                Color = null
            });

            // create numbers           
            for (int i = 1; i <= 36; i++)
            {
                var r = new RouletteNumber();
                r.Number = i.ToString();

                // first 10 alternate color, red first
                if (i < 11)
                {
                    if (i % 2 == 0)
                        r.Color = Color.Red;
                    else
                        r.Color = Color.Black;
                }
                else if (i < 29)
                {
                    // alternate black first
                    if (i % 2 == 0)
                        r.Color = Color.Black;
                    else
                        r.Color = Color.Red;
                }
                else
                {
                    if (i % 2 == 0)
                        r.Color = Color.Red;
                    else
                        r.Color = Color.Black;
                }

                Add(r);
            }
        }
    }

    public class SpinResult
    {
        public SpinResult(RouletteNumber rouletteNumber)
        {
            RouletteNumber = rouletteNumber;
        }

        public RouletteNumber RouletteNumber { get; set; }
    }

    public class RouletteGame
    {
        public RouletteGame()
        {
            
        }


        public void Reset()
        {
            Bets.Clear();
            Winners.Clear();
            SpinHistory.Clear();
        }
        
        public SpinResult Spin()
        {
            Winners.Clear();            
            var rouletteNumber = RandomizeResult();
            var spinResult = new SpinResult(rouletteNumber);
            spinResult.RouletteNumber = rouletteNumber;
            LastSpinResult = spinResult;

            ProcessResult(spinResult);

            SpinHistory.Add(spinResult);

            return spinResult;
        }

        public SpinResult LastSpinResult { get; set; }

        private void ProcessResult(SpinResult result)
        {
            foreach (var bet in Bets)
            {                
                bet.WinAmount = 0;
                if (bet.Color.HasValue)
                {
                    if (bet.Color == result.RouletteNumber.Color)
                    {
                        bet.WinAmount += bet.BetAmount * 2;                                                
                    }
                }

                if (bet.OutsideDozens.HasValue)
                {
                    if (bet.OutsideDozens == result.RouletteNumber.IsInOutsizeDozen)
                    {
                        bet.WinAmount += bet.BetAmount*3;
                    }                   
                }

                foreach(var r in bet.RouletteNumbers)
                {
                    if (r.Number == "0" || r.Number == "00")
                    {
                        bet.WinAmount += bet.BetAmount*17;
                    }
                    else
                    if (r.Number == result.RouletteNumber.Number)
                    {
                        if (bet.RouletteNumbers.Count == 4)
                            bet.WinAmount += bet.BetAmount*8;
                        if (bet.RouletteNumbers.Count == 1)
                            bet.WinAmount += bet.BetAmount*35;
                        if (bet.RouletteNumbers.Count == 2)
                            bet.WinAmount += bet.BetAmount*16;
                    }

                }

                if (bet.WinAmount > 0)
                    Winners.Add(bet);
            }
        }

        private Random randomizer = new Random();
        protected RouletteNumber RandomizeResult()
        {

            int i = randomizer.Next(0, RouletteNumberList.Count - 1);
            return RouletteNumberList[i];
        }

        public List<SpinResult> SpinHistory = new List<SpinResult>();
        public RouletteNumberList RouletteNumberList = new RouletteNumberList();
        
        public List<Bet> Bets = new List<Bet>();
        public List<Bet> Winners = new List<Bet>();
    }

    public class Bet
    {
        public Bet()
        {
            WinAmount = 0;
            BetAmount = 1;
            this.RouletteNumbers = new List<RouletteNumber>();
        }
        public List<RouletteNumber> RouletteNumbers { get; set; }
        public int BetAmount { get; set; }
        public OddEven? OddEven { get; set; }
        public OutsideDozens? OutsideDozens { get; set; }
        public OutsideColumns? OutsideColumns { get; set; }
        public bool? LowerHalf { get; set; }
        public bool? UpperHalf { get; set; }
        public Color? Color { get; set; }
        public int WinAmount { get; set; }
    }

    public class Player
    {
        public Decimal Capital { get; set; }
    }
    
}
