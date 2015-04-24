using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Roulette.Strategies;

namespace Roulette
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int NumSpins;
        private RouletteGame Game = new RouletteGame();
        private void button1_Click(object sender, EventArgs e)
        {
            Game.Reset();
            NumSpins = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < NumSpins; i++)
            {
                Game.Spin();
            }

            GetStats();
           
        }

        private void GetStats()
        {
            CalculateColors();
            CalculateDozens();
            CalculateColumns();
            CalculateZeros();
            CalculateLowHigh();
            CalculateOddEven();
            GetHotNumbers();
            GetColorStreak();
        }

        private void GetColorStreak()
        {
            int blackStreakResult = 0;
            int blackStreakCount = 0;
            int blackIter = 1;

            int redStreakResult = 0;
            int redStreakCount = 0;
            int redIter = 1;

            foreach (var q in Game.SpinHistory)
            {                                
                // black
                if (q.RouletteNumber.Color.HasValue)
                {
                    if (q.RouletteNumber.Color == Color.Black)
                    {
                        blackStreakCount++;
                        redStreakCount = 0;
                    }
                    else
                    {
                        blackStreakCount = 0;
                        redStreakCount++;
                    }

                    //if (blackStreakResult == blackStreakCount)
                    //    blackIter++;
                    //if (redStreakResult == redStreakCount)
                    //    redIter++;

                    if (blackStreakCount > blackStreakResult)
                    {
                        blackStreakResult = blackStreakCount;
                    }

                    if (redStreakCount > redStreakResult)
                        redStreakResult = redStreakCount;
                }
                else
                {
                    blackStreakCount = 0;
                    redStreakCount = 0;
                }
            }

            labelBlackStreak.Text = blackStreakResult.ToString(); // +"(" + blackIter.ToString() + ")";
            labelRedStreak.Text = redStreakResult.ToString();// +"(" + redIter.ToString() + ")";
        }

        private void GetHotNumbers()
        {
            var query = (   
                        from q in Game.SpinHistory
                            group q by q.RouletteNumber.Number into g
                         select new  { Number = g.Key, Count = g.Count()}
                        )
                        .OrderBy(x => x.Count)
                        .ToList();

            labelHotNumbers.Text = string.Format("{0}, {1}, {2}", 
                query[query.Count - 1].Number,
                query[query.Count - 2].Number,
                query[query.Count - 3].Number);
                        
        }

        private void CalculateOddEven()
        {
            int result = (from r in Game.SpinHistory
                          where r.RouletteNumber.OddEven.HasValue == true &&
                            r.RouletteNumber.OddEven.Value == Roulette.OddEven.Odd
                          select r).ToList().Count();
            labelOdd.Text = ((double)result / (double)NumSpins).ToString("0.00%");

            result = (from r in Game.SpinHistory
                          where r.RouletteNumber.OddEven.HasValue == true &&
                            r.RouletteNumber.OddEven.Value == Roulette.OddEven.Even
                          select r).ToList().Count();
            labelEven.Text = ((double)result / (double)NumSpins).ToString("0.00%");
        }

        private void CalculateLowHigh()
        {
            int result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInLowerHalf
                          select r).ToList().Count();
            labelLowerHalf.Text = ((double)result / (double)NumSpins).ToString("0.00%");

            result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInUpperHalf
                          select r).ToList().Count();
            labelUpperHalf.Text = ((double)result / (double)NumSpins).ToString("0.00%");
        }

        private void CalculateColumns()
        {
            int result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInOutsideColumn.HasValue == true &&
                            r.RouletteNumber.IsInOutsideColumn.Value == Roulette.OutsideColumns.FirstColumn
                          select r).ToList().Count();
            labelCol_1.Text = ((double)result / (double)NumSpins).ToString("0.00%");

            result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInOutsideColumn.HasValue == true &&
                            r.RouletteNumber.IsInOutsideColumn.Value == Roulette.OutsideColumns.SecondColumn
                          select r).ToList().Count();
            labelCol_2.Text = ((double)result / (double)NumSpins).ToString("0.00%");

            result = (from r in Game.SpinHistory
                      where r.RouletteNumber.IsInOutsideColumn.HasValue == true &&
                        r.RouletteNumber.IsInOutsideColumn.Value == Roulette.OutsideColumns.ThirdColumn
                      select r).ToList().Count();
            labelCol_3.Text = ((double)result / (double)NumSpins).ToString("0.00%");
        }

        private void CalculateZeros()
        {
            int zeros = (from z in Game.SpinHistory
                         where z.RouletteNumber.IsZero()
                         select z).ToList().Count();
            var result = (double)zeros / (double)NumSpins;
            labelZero.Text = result.ToString("0.00%");
        }

        private void CalculateDozens()
        {
            int result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInOutsizeDozen.HasValue == true &&
                            r.RouletteNumber.IsInOutsizeDozen.Value == Roulette.OutsideDozens.FirstTwelve
                          select r).ToList().Count();
            label12_1.Text = ((double)result / (double)NumSpins).ToString("0.00%");

            result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInOutsizeDozen.HasValue == true &&
                            r.RouletteNumber.IsInOutsizeDozen.Value == Roulette.OutsideDozens.SecondTwelve
                          select r).ToList().Count();
            label12_2.Text = ((double)result / (double)NumSpins).ToString("0.00%");

            result = (from r in Game.SpinHistory
                          where r.RouletteNumber.IsInOutsizeDozen.HasValue == true &&
                            r.RouletteNumber.IsInOutsizeDozen.Value == Roulette.OutsideDozens.ThirdTwelve
                          select r).ToList().Count();
            label12_3.Text = ((double)result / (double)NumSpins).ToString("0.00%");
        }

        private void CalculateColors()
        {
            int blacks = (from b in Game.SpinHistory
                          where b.RouletteNumber.Color == Color.Black
                          select b).ToList().Count();
            var result = (double)blacks / (double)NumSpins;
            labelBlack.Text = result.ToString("0.00%");

            int reds = (from b in Game.SpinHistory
                          where b.RouletteNumber.Color == Color.Red
                          select b).ToList().Count();
            result = (double)reds / (double)NumSpins;
            labelRed.Text = result.ToString("0.00%");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StrategyBase strategy = null;
            var gameParams = new GameParams();
            gameParams.Capital = Convert.ToDecimal(textCapital.Text);
            gameParams.PlayHours = (int)playHours.Value;
            gameParams.SingleChipValue = Convert.ToInt32(textSingleChipValue.Text);
            gameParams.SpinsPerHour = Convert.ToInt32(textSpinPerHour.Text);
            
            Color color;
            switch (tabControl1.SelectedIndex)
            {
                case 0: 
                    strategy = new TheoryOfNine(Game, gameParams);
                    break;
                case 1: 
                    if (radioBlack.Checked)
                        color = Color.Black;
                    else
                        color = Color.Red;
                    strategy = new OneThreeTwoFour(Game, gameParams, color);                      
                    break;
                case 2: 
                    strategy = new Martingale(Game, gameParams);
                    break;
                case 3:
                    
                    if (radioTab4Black.Checked)
                        color = Color.Black;
                    else
                    {
                        color = Color.Red;
                    }
                    strategy = new OneThreeTwoFourDozens(Game, gameParams, color);
                    break;
                case 4:
                    if (radioTab5Black.Checked)
                        color = Color.Black;
                    else
                    {
                        color = Color.Red;
                    }
                    strategy = new OneThreeTwoFourDozensChangeColor(Game, gameParams, color);
                    break;
                case 5:
                    if (radioTab6Black.Checked)
                        color = Color.Black;
                    else
                    {
                        color = Color.Red;
                    }
                    strategy = new OneThreeTwoFourDozensMartingale(Game, gameParams, color, Convert.ToInt32(textMartingaleLimit.Text));
                    break;
                case 6: 
                    strategy = new TheoryOfNineAndOneThreeTwoFour(Game, gameParams);
                    break;
                case 7:
                    strategy = new KamoteStrategy(Game, gameParams);
                    break;
            }

            if (strategy != null)
                strategy.Execute();

            NumSpins += gameParams.PlayHours * gameParams.SpinsPerHour;
            GetStats();

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    PlotChart(chartTheory9, strategy.CapitalHistory);
                    textTab1Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab1WinCount.Text = strategy.WinCount.ToString();
                    textTab1LoseCount.Text = strategy.LoseCount.ToString();
                    break;
                case 1:
                    textTab2Capital.Text = gameParams.Capital.ToString("#,##0");
                    PlotChart(chart1324, strategy.CapitalHistory);
                    textTab2Win.Text = strategy.WinCount.ToString();
                    textTab2Lose.Text = strategy.LoseCount.ToString();
                    break;
                case 2:
                    textTab3Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab3Win.Text = strategy.WinCount.ToString();
                    textTab3Lose.Text = strategy.LoseCount.ToString();
                    PlotChart(chartMartingale, strategy.CapitalHistory);
                    break;
                case 3:
                    textTab4Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab4Win.Text = strategy.WinCount.ToString();
                    textTab4Lose.Text = strategy.LoseCount.ToString();
                    PlotChart(chartTab4, strategy.CapitalHistory);
                    break;
                case 4:
                    textTab5Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab5Win.Text = strategy.WinCount.ToString();
                    textTab5Lose.Text = strategy.LoseCount.ToString();
                    PlotChart(chartTab5, strategy.CapitalHistory);
                    break;
                case 5:
                    textTab6Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab6Win.Text = strategy.WinCount.ToString();
                    textTab6Lose.Text = strategy.LoseCount.ToString();
                    PlotChart(chartTab6, strategy.CapitalHistory);
                    break;
                case 6:
                    textTab7Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab7Win.Text = strategy.WinCount.ToString();
                    textTab7Lose.Text = strategy.LoseCount.ToString();
                    PlotChart(chart1, strategy.CapitalHistory);
                    break;
                case 7:
                    textTab8Capital.Text = gameParams.Capital.ToString("#,##0");
                    textTab8Win.Text = strategy.WinCount.ToString();
                    textTab8Lose.Text = strategy.LoseCount.ToString();
                    PlotChart(chart8, strategy.CapitalHistory);
                    break;
            }

        }

        private void PlotChart(System.Windows.Forms.DataVisualization.Charting.Chart chartSeries, Dictionary<int, decimal> capitalHistory)
        {
            chartSeries.Series[0].Points.Clear();
            foreach(var capital  in capitalHistory)
            {
                chartSeries.Series[0].Points.AddXY(capital.Key, capital.Value);
            }
        }

       
    }
}
