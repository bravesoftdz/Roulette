using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Roulette.Strategies
{

    public class GameParams
    {
        public decimal Capital;
        public int PlayHours;
        public int SpinsPerHour;
        public int SingleChipValue;
    }

    public abstract class StrategyBase
    {
        public StrategyBase(RouletteGame game, GameParams gameParam)
        {
            GameParams = gameParam;
            Game = game;
        }
        public abstract void Execute();
        public GameParams GameParams { get; set; }
        public RouletteGame Game { get; set; }
        public Dictionary<int, Decimal> CapitalHistory = new Dictionary<int, decimal>();
        public int WinCount {get; set;}
        public int LoseCount { get; set; }
    }

    public class OneThreeTwoFour : StrategyBase
    {
        public OneThreeTwoFour(RouletteGame game, GameParams gameParam, Color color)
            : base(game, gameParam)
        {
            Color = color;
        }

        public Color Color { get; set; }
        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = Color;

            int winCount = 0;
            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            GameParams.Capital -= bet.BetAmount*GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);
            for (int i = 0; i < numSpins; i++)
            {
                Game.Bets.Clear();
                Game.Bets.Add(bet);
                Game.Spin();

                var betValue = bet.WinAmount * GameParams.SingleChipValue;
                if (Game.Winners.Count > 0)
                {
                    GameParams.Capital += betValue;
                    winCount++;
                    WinCount++;
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
                    winCount = 0;
                    LoseCount++;
                }
                CapitalHistory.Add(i + 1, GameParams.Capital);
                switch(winCount)
                {
                    case 1: 
                        bet.BetAmount = 3;                        
                        break;
                    case 2:
                        bet.BetAmount = 2;                        
                        break;
                    case 3:
                        bet.BetAmount = 4;                        
                        break;
                    default:
                        bet.BetAmount = 1;                        
                        break;
                }

                betValue = bet.BetAmount * GameParams.SingleChipValue;
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
            }
        }
    }

    public class Martingale : StrategyBase
    {
        public Martingale(RouletteGame game, GameParams gameParam)
            : base(game, gameParam)
        {
        }

        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour*GameParams.PlayHours;
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = Color.Black;

            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            int winCtr = 0;
            GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);
            for(int i = 0; i < numSpins; i++)
            {
                Game.Bets.Clear();
                Game.Bets.Add(bet);
                Game.Spin();

                if (Game.Winners.Count > 0)
                {
                    var winValue = bet.WinAmount * GameParams.SingleChipValue;
                    GameParams.Capital += winValue;
                    if (bet.Color == Color.Black)
                        bet.Color = Color.Red;
                    else if (bet.Color == Color.Red)
                        bet.Color = Color.Black;                                        
                    winCtr++;
                    WinCount++;
                    switch(winCtr)
                    {
                        case 1:
                            bet.BetAmount = 2;
                            break;
                        case 2:
                            bet.BetAmount = 3;
                            break;
                        default:
                            bet.BetAmount = 1;
                            break;
                    }
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
                    bet.BetAmount = bet.BetAmount*2;
                    winCtr = 0;
                    LoseCount++;
                }
                CapitalHistory.Add(i + 1, GameParams.Capital);
                var betValue = bet.BetAmount*GameParams.SingleChipValue;                
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;

            }
        }
    }

    public class KamoteStrategy : StrategyBase
    {
        public KamoteStrategy(RouletteGame game, GameParams gameParam)
            : base(game, gameParam)
        {
            
        }

        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;            

            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            
            for (int i = 0; i < numSpins; i++)
            {                
                Game.Bets.Clear();
                var bet1 = new Bet();
                var bet2 = new Bet();
                var bet3 = new Bet();
                var bet4 = new Bet();
                var bet5 = new Bet();
                var bet6 = new Bet();
                var bet7 = new Bet();
                var bet8 = new Bet();
                var bet9 = new Bet();
                var bet10 = new Bet();
                var bet11 = new Bet();
                var bet12 = new Bet();
                var bet13 = new Bet();
                var bet14 = new Bet();
                var bet15 = new Bet();
                var bet16 = new Bet();

                bet1.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("3"));
                bet2.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("5"));
                
                bet3.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("1"));
                bet3.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("2"));
                bet3.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("4"));
                bet3.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("5"));

                bet4.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("7"));
                bet4.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("8"));
                bet4.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("11"));
                bet4.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("10"));

                bet5.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("17"));
                bet6.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("20"));
                
                bet7.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("13"));
                bet7.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("14"));
                bet7.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("17"));
                bet7.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("16"));

                bet8.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("20"));
                bet8.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("21"));
                bet8.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("24"));
                bet8.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("23"));

                bet9.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("26"));
                bet10.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("35"));

                bet11.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("28"));
                bet11.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("31"));
                bet11.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("29"));
                bet11.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("32"));

                bet12.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("30"));
                bet12.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("29"));
                bet12.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("33"));
                bet12.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("32"));

                bet13.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("36"));
                bet13.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("32"));
                bet13.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("33"));
                bet13.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("35"));

                var lastNumber = Game.LastSpinResult.RouletteNumber.Number;
                bet14.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber(lastNumber));
                bet14.BetAmount = 3;

                bet15.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("6"));
                bet15.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("9"));
                bet16.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("22"));
                bet16.RouletteNumbers.Add(Game.RouletteNumberList.GetRouletteNumber("25"));

                var betValue = bet1.BetAmount + bet2.BetAmount + bet3.BetAmount +
                               bet4.BetAmount + bet5.BetAmount + bet6.BetAmount +
                               bet7.BetAmount + bet8.BetAmount + bet9.BetAmount +
                               bet10.BetAmount + bet11.BetAmount + bet12.BetAmount +
                               bet13.BetAmount + bet14.BetAmount + bet15.BetAmount + bet16.BetAmount;
                betValue = betValue*GameParams.SingleChipValue;
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
                Game.Bets.Add(bet1);
                Game.Bets.Add(bet2);
                Game.Bets.Add(bet3);
                Game.Bets.Add(bet4);
                Game.Bets.Add(bet5);
                Game.Bets.Add(bet6);
                Game.Bets.Add(bet7);
                Game.Bets.Add(bet8);
                Game.Bets.Add(bet9);
                Game.Bets.Add(bet10);
                Game.Bets.Add(bet11);
                Game.Bets.Add(bet12);
                Game.Bets.Add(bet13);
                Game.Bets.Add(bet14);
                Game.Bets.Add(bet15);
                Game.Bets.Add(bet16);

                Game.Spin();
                if (Game.Winners.Count > 0)
                {
                    int winAmount = 0;
                    foreach (var x in Game.Winners)
                    {
                        winAmount += x.WinAmount * GameParams.SingleChipValue;

                    }
                    GameParams.Capital += winAmount;
                    WinCount++;
                }
                else
                {
                    LoseCount++;
                }
                CapitalHistory.Add(i, GameParams.Capital);
            }
        }
    }

    public class OneThreeTwoFourDozens : StrategyBase
    {
        public OneThreeTwoFourDozens(RouletteGame game, GameParams gameParam, Color color)
            : base(game, gameParam)
        {
            Color = color;
        }

        public Color Color { get; set; }
        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;
            var dozenBet1 = new Bet();
            var dozenBet2 = new Bet();
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = Color;

            int winCount = 0;
            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            GameParams.Capital -= bet.BetAmount*GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);
            Game.Bets.Add(bet);
            for (int i = 0; i < numSpins; i++)
            {                
                
                Game.Spin();

                var betValue = 0;
                if (Game.Winners.Count > 0)
                {
                    foreach(var b in Game.Winners)
                    {
                        var winValue = b.WinAmount*GameParams.SingleChipValue;
                        GameParams.Capital += winValue;                        
                    }

                    winCount++;
                    WinCount++;
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount*GameParams.SingleChipValue;
                    winCount = 0;
                    LoseCount++;
                }
                Game.Bets.Clear();

                CapitalHistory.Add(i + 1, GameParams.Capital);
                
                switch (winCount)
                {
                    case 1:
                        bet.BetAmount = 3;
                        break;
                    case 2:
                        bet.BetAmount = 2;
                        break;
                    case 3:
                        bet.BetAmount = 2;

                        if (Game.LastSpinResult.RouletteNumber.IsInOutsizeDozen.HasValue)
                        {                            
                            dozenBet1.BetAmount = 1;
                            dozenBet1.WinAmount = 0;
                            dozenBet2.BetAmount = 1;
                            dozenBet2.WinAmount = 0;
                            switch (Game.LastSpinResult.RouletteNumber.IsInOutsizeDozen)
                            {
                                case Roulette.OutsideDozens.FirstTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.SecondTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.ThirdTwelve;
                                    break;
                                case Roulette.OutsideDozens.SecondTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.FirstTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.ThirdTwelve;
                                    break;
                                case Roulette.OutsideDozens.ThirdTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.SecondTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.FirstTwelve;
                                    break;
                            }
                            betValue += dozenBet1.BetAmount*GameParams.SingleChipValue;
                            betValue += dozenBet2.BetAmount*GameParams.SingleChipValue;

                            if (GameParams.Capital < betValue)
                                return;

                            Game.Bets.Add(dozenBet1);
                            Game.Bets.Add(dozenBet2);

                        }

                        break;
                    default:
                        bet.BetAmount = 1;
                        break;
                }

                betValue += bet.BetAmount*GameParams.SingleChipValue;                
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
                Game.Bets.Add(bet);
                
            }
        }
    }

    public class OneThreeTwoFourDozensMartingale : StrategyBase
    {
        public OneThreeTwoFourDozensMartingale(RouletteGame game, GameParams gameParams, Color color, int martingaleLimit)
            :base(game, gameParams)
        {
            Color = color;
            MartingaleLimit = martingaleLimit;
        }

        public Color Color { get; set; }
        public int MartingaleLimit { get; set; }
        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;
            var dozenBet1 = new Bet();
            var dozenBet2 = new Bet();
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = Color;

            int winCount = 0;
            int martingaleCount = 0;
            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);
            Game.Bets.Add(bet);
            for (int i = 0; i < numSpins; i++)
            {

                Game.Spin();

                var betValue = 0;
                if (Game.Winners.Count > 0)
                {
                    foreach (var b in Game.Winners)
                    {
                        var winValue = b.WinAmount*GameParams.SingleChipValue;
                        GameParams.Capital += winValue;
                    }

                    winCount++;
                    WinCount++;
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount*GameParams.SingleChipValue;
                    winCount = 0;
                    LoseCount++;
                }
                Game.Bets.Clear();

                CapitalHistory.Add(i + 1, GameParams.Capital);

                switch (winCount)
                {
                    case 1:
                        bet.BetAmount = 3;
                        break;
                    case 2:
                        bet.BetAmount = 2;
                        break;
                    case 3:
                        bet.BetAmount = 2;

                        if (Game.LastSpinResult.RouletteNumber.IsInOutsizeDozen.HasValue)
                        {
                            dozenBet1.BetAmount = 1;
                            dozenBet1.WinAmount = 0;
                            dozenBet2.BetAmount = 1;
                            dozenBet2.WinAmount = 0;
                            switch (Game.LastSpinResult.RouletteNumber.IsInOutsizeDozen)
                            {
                                case Roulette.OutsideDozens.FirstTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.SecondTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.ThirdTwelve;
                                    break;
                                case Roulette.OutsideDozens.SecondTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.FirstTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.ThirdTwelve;
                                    break;
                                case Roulette.OutsideDozens.ThirdTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.SecondTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.FirstTwelve;
                                    break;
                            }
                            betValue += dozenBet1.BetAmount*GameParams.SingleChipValue;
                            betValue += dozenBet2.BetAmount*GameParams.SingleChipValue;

                            if (GameParams.Capital < betValue)
                                return;

                            Game.Bets.Add(dozenBet1);
                            Game.Bets.Add(dozenBet2);

                        }

                        break;
                    default:
                        if (martingaleCount >= MartingaleLimit)
                        {
                            bet.BetAmount = 1;
                            martingaleCount = 0;
                        }
                        else
                            bet.BetAmount = bet.BetAmount * 2;
                        martingaleCount++;
                        break;
                }

                betValue += bet.BetAmount*GameParams.SingleChipValue;
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
                Game.Bets.Add(bet);
            }
        }
    }

    public class OneThreeTwoFourDozensChangeColor : StrategyBase
    {
        public OneThreeTwoFourDozensChangeColor(RouletteGame game, GameParams gameParams, Color color)
            :base(game, gameParams)
        {
            Color = color;
        }

        public Color Color { get; set; }

        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;
            var dozenBet1 = new Bet();
            var dozenBet2 = new Bet();
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = Color;

            int winCount = 0;
            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);
            Game.Bets.Add(bet);
            for (int i = 0; i < numSpins; i++)
            {

                Game.Spin();

                var betValue = 0;
                if (Game.Winners.Count > 0)
                {
                    foreach (var b in Game.Winners)
                    {
                        var winValue = b.WinAmount * GameParams.SingleChipValue;
                        GameParams.Capital += winValue;
                    }

                    winCount++;
                    WinCount++;
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount*GameParams.SingleChipValue;
                    winCount = 0;
                    LoseCount++;
                }
                Game.Bets.Clear();

                CapitalHistory.Add(i + 1, GameParams.Capital);

                switch (winCount)
                {
                    case 1:
                        bet.BetAmount = 3;
                        break;
                    case 2:
                        bet.BetAmount = 2;
                        break;
                    case 3:
                        bet.BetAmount = 2;

                        if (Game.LastSpinResult.RouletteNumber.IsInOutsizeDozen.HasValue)
                        {
                            dozenBet1.BetAmount = 1;
                            dozenBet1.WinAmount = 0;
                            dozenBet2.BetAmount = 1;
                            dozenBet2.WinAmount = 0;
                            switch (Game.LastSpinResult.RouletteNumber.IsInOutsizeDozen)
                            {
                                case Roulette.OutsideDozens.FirstTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.SecondTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.ThirdTwelve;
                                    break;
                                case Roulette.OutsideDozens.SecondTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.FirstTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.ThirdTwelve;
                                    break;
                                case Roulette.OutsideDozens.ThirdTwelve:
                                    dozenBet1.OutsideDozens = Roulette.OutsideDozens.SecondTwelve;
                                    dozenBet2.OutsideDozens = OutsideDozens.FirstTwelve;
                                    break;
                            }
                            betValue += dozenBet1.BetAmount * GameParams.SingleChipValue;
                            betValue += dozenBet2.BetAmount * GameParams.SingleChipValue;

                            if (GameParams.Capital < betValue)
                                return;

                            Game.Bets.Add(dozenBet1);
                            Game.Bets.Add(dozenBet2);

                            if (bet.Color == Color.Black)
                                bet.Color = Color.Red;
                            else
                                bet.Color = Color.Red;
                        }

                        break;
                    default:
                        bet.BetAmount = 1;
                        break;
                }

                betValue += bet.BetAmount * GameParams.SingleChipValue;
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
                Game.Bets.Add(bet);

            }
        }
    }

    public class TheoryOfNineAndOneThreeTwoFour : StrategyBase
    {
        public TheoryOfNineAndOneThreeTwoFour(RouletteGame game, GameParams gameParam)
            :base(game, gameParam)
        {
            
        }

        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = GetNextColor();

            CapitalHistory.Clear();
            int winCtr = 0;
            WinCount = 0;
            LoseCount = 0;
            GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);

            for (int i = 0; i < numSpins; i++)
            {
                Game.Bets.Clear();

                Game.Bets.Add(bet);
                Game.Spin();

                if (Game.Winners.Count > 0)
                {
                    var winValue = bet.WinAmount * GameParams.SingleChipValue;
                    GameParams.Capital += winValue;
                    WinCount++;
                    winCtr++;
                    switch (winCtr)
                    {
                        case 1:
                            bet.BetAmount = 3;
                            break;
                        case 2:
                            bet.BetAmount = 2;
                            break;
                        case 3:
                            bet.BetAmount = 4;
                            break;
                        default:
                            bet.BetAmount = 1;
                            break;
                    }
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
                    winCtr = 0;
                    LoseCount++;
                }

                CapitalHistory.Add(i + 1, GameParams.Capital);

                bet.Color = GetNextColor();

                var betValue = bet.BetAmount * GameParams.SingleChipValue;
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
            }
        }

        protected Color GetNextColor()
        {
            var q = Game.SpinHistory.Skip(Game.SpinHistory.Count - 9);
            int blackCount = (from x in q
                              where x.RouletteNumber.Color.HasValue &&
                                 x.RouletteNumber.Color.Value == Color.Black
                              select x).ToList().Count();
            int redCount = (from x in q
                            where x.RouletteNumber.Color.HasValue &&
                               x.RouletteNumber.Color.Value == Color.Red
                            select x).ToList().Count();

            if (blackCount > redCount)
                return Color.Red;
            else
                return Color.Black;
        }
    }

    public class TheoryOfNine : StrategyBase
    {
        public TheoryOfNine(RouletteGame game, GameParams gameParam) 
            :base(game, gameParam)
        {
        }

        public override void Execute()
        {
            int numSpins = GameParams.SpinsPerHour * GameParams.PlayHours;
            Bet bet = new Bet();
            bet.BetAmount = 1;
            bet.Color = GetNextColor();
            
            CapitalHistory.Clear();
            WinCount = 0;
            LoseCount = 0;
            GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
            CapitalHistory.Add(0, GameParams.Capital);
            for (int i = 0; i < numSpins; i++)
            {
                Game.Bets.Clear();
                
                Game.Bets.Add(bet);
                Game.Spin();

                if (Game.Winners.Count > 0)
                {
                    var winValue = bet.WinAmount * GameParams.SingleChipValue;
                    GameParams.Capital += winValue;
                    WinCount++;
                }
                else
                {
                    //GameParams.Capital -= bet.BetAmount * GameParams.SingleChipValue;
                    LoseCount++;
                }
                CapitalHistory.Add(i + 1, GameParams.Capital);

                bet.Color = GetNextColor();
                
                var betValue = bet.BetAmount * GameParams.SingleChipValue;
                if (GameParams.Capital < betValue)
                    return;
                GameParams.Capital -= betValue;
            }

        }

        protected Color GetNextColor()
        {
            var q = Game.SpinHistory.Skip(Game.SpinHistory.Count - 9);
            int blackCount = (from x in q
                              where x.RouletteNumber.Color.HasValue &&
                                 x.RouletteNumber.Color.Value == Color.Black
                              select x).ToList().Count();
            int redCount = (from x in q
                              where x.RouletteNumber.Color.HasValue &&
                                 x.RouletteNumber.Color.Value == Color.Red
                              select x).ToList().Count();

            if (blackCount > redCount)
                return Color.Red;
            else
                return Color.Black;
        }
    }
}
