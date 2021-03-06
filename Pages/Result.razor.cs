﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ChartJs.Blazor.ChartJS.BarChart;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes.Ticks;
using ChartJs.Blazor.ChartJS.Common.Enums;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.Common.Wrappers;
using ChartJs.Blazor.ChartJS.PieChart;
using ChartJs.Blazor.Charts;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using SampleQuestionnaire.Models;

namespace SampleQuestionnaire.Pages
{
    public partial class Result
    {
        [Inject]
        private HttpClient _http { get; set; }

        [Inject]
        private AppSettings _settings { get; set; }

        private string year_month = "";
        private QuestionnaireResult json;

        private BarConfig _q1Config;
        private ChartJsBarChart _q1Chart;
        private BarDataset<DoubleWrapper> _q1DataSet;

        private PieConfig _q5Config;
        private ChartJsPieChart _q5Chart;

        protected async Task GetData()
        {
            var result = await _http.GetStringAsync(_settings.GasApi + year_month);
            //json = JsonConvert.DeserializeObject<QuestionnaireResult>(result);
            json = JsonSerializer.Deserialize<QuestionnaireResult>(result);

            if (_q1Chart == null)
            {
                await GenerateBarChart(json.Question1.Question,
                    json.Question1.Answer.Select(x => x.Answer).ToArray(),
                    json.Question1.Answer.Select(x => Convert.ToDouble(x.Votes)).ToArray());
            }
            else
            {
                await UpdateBarChart(json.Question1.Question,
                    json.Question1.Answer.Select(x => x.Answer).ToArray(),
                    json.Question1.Answer.Select(x => Convert.ToDouble(x.Votes)).ToArray());
            }

            if (_q5Chart == null)
            {
                GeneratePieChart(json.Question5.Question,
                    json.Question5.Answer.Select(x => x.Answer).ToArray(),
                    json.Question5.Answer.Select(x => Convert.ToDouble(x.Votes)).ToArray());
            }
            else
            {
                await UpdatePieChart(json.Question5.Question,
                    json.Question5.Answer.Select(x => x.Answer).ToArray(),
                    json.Question5.Answer.Select(x => Convert.ToDouble(x.Votes)).ToArray());
            }
        }

        protected Task GenerateBarChart(string question, string[] answers, double[] votes)
        {
            // Note the constructor argument
            _q1Config = new BarConfig(ChartType.HorizontalBar)
            {
                Options = new BarOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = question
                    },
                    Responsive = true,
                    Scales = new BarScales
                    {
                        XAxes = new List<CartesianAxis>
            {
                        new LinearCartesianAxis
                        {
                            Ticks = new LinearCartesianTicks
                            {
                                AutoSkip = false,
                                Min = 0 // Otherwise the lowest value in the dataset won't be visible
                            }
                        }
                    }
                    }
                }
            };

            _q1Config.Data.Labels.AddRange(answers);

            //Note the constructor argument
            _q1DataSet = new BarDataset<DoubleWrapper>(ChartType.HorizontalBar)
            {
                Label = question,
                BackgroundColor = Enumerable.Range(0, answers.Length).Select(i => ColorUtil.FromDrawingColor(System.Drawing.Color.BlueViolet)).ToArray(),
                BorderColor = ColorUtil.FromDrawingColor(System.Drawing.Color.Black),
                BorderWidth = 1
            };

            _q1DataSet.AddRange(votes.Wrap());
            _q1Config.Data.Datasets.Add(_q1DataSet);

            return Task.CompletedTask;
        }
        protected Task UpdateBarChart(string question, string[] answers, double[] votes)
        {
            _q1Config.Data.Datasets.Clear();
            _q1Config.Data.Labels.Clear();

            // Note the constructor argument
            _q1Config.Options = new BarOptions
            {
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = question
                },
                Responsive = true,
                Scales = new BarScales
                {
                    XAxes = new List<CartesianAxis>
        {
                    new LinearCartesianAxis
                    {
                        Ticks = new LinearCartesianTicks
                        {
                            AutoSkip = false,
                            Min = 0 // Otherwise the lowest value in the dataset won't be visible
                        }
                    }
                }
                }
            };

            _q1Config.Data.Labels.AddRange(answers);

            //Note the constructor argument
            _q1DataSet = new BarDataset<DoubleWrapper>(ChartType.HorizontalBar)
            {
                Label = question,
                BackgroundColor = Enumerable.Range(0, answers.Length).Select(i => ColorUtil.FromDrawingColor(System.Drawing.Color.BlueViolet)).ToArray(),
                BorderColor = ColorUtil.FromDrawingColor(System.Drawing.Color.Black),
                BorderWidth = 1
            };

            _q1DataSet.AddRange(votes.Wrap());
            _q1Config.Data.Datasets.Add(_q1DataSet);

            return Task.CompletedTask;
        }


        protected void GeneratePieChart(string question, string[] answers, double[] votes)
        {
            _q5Config = new PieConfig
            {
                Options = new PieOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = question
                    },
                    Responsive = true,
                    Animation = new ArcAnimation
                    {
                        AnimateRotate = true,
                        AnimateScale = true
                    }
                }
            };

            _q5Config.Data.Labels.AddRange(answers);

            var pieSet = new PieDataset
            {
                BackgroundColor = answers
                    .Select(i => i == "はい" ? ColorUtil.FromDrawingColor(System.Drawing.Color.Blue)
                                                        : ColorUtil.FromDrawingColor(System.Drawing.Color.Red))
                    .ToArray(),
                BorderWidth = 0,
                HoverBackgroundColor = answers
                    .Select(i => i.ToString() == "はい" ? ColorUtil.FromDrawingColor(System.Drawing.Color.DarkBlue)
                                                        : ColorUtil.FromDrawingColor(System.Drawing.Color.DarkRed))
                    .ToArray(),
                HoverBorderColor = ColorUtil.FromDrawingColor(System.Drawing.Color.Black),
                HoverBorderWidth = 1,
                BorderColor = "#ffffff",
            };

            pieSet.Data.AddRange(votes);
            _q5Config.Data.Datasets.Add(pieSet);
        }

        protected async Task UpdatePieChart(string question, string[] answers, double[] votes)
        {

            _q5Config.Data.Datasets.Clear();
            _q5Config.Data.Labels.Clear();
            await _q5Chart.Update();

            _q5Config.Options = new PieOptions
            {
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = question
                },
                Responsive = true,
                Animation = new ArcAnimation
                {
                    AnimateRotate = true,
                    AnimateScale = true
                }
            };

            _q5Config.Data.Labels.AddRange(answers);

            var pieSet = new PieDataset
            {
                BackgroundColor = answers
                    .Select(i => i.ToString() == "はい" ? ColorUtil.FromDrawingColor(System.Drawing.Color.Blue)
                                                        : ColorUtil.FromDrawingColor(System.Drawing.Color.Red))
                    .ToArray(),
                BorderWidth = 0,
                HoverBackgroundColor = answers
                    .Select(i => i.ToString() == "はい" ? ColorUtil.FromDrawingColor(System.Drawing.Color.DarkBlue)
                                                        : ColorUtil.FromDrawingColor(System.Drawing.Color.DarkRed))
                    .ToArray(),
                HoverBorderColor = ColorUtil.FromDrawingColor(System.Drawing.Color.Black),
                HoverBorderWidth = 1,
                BorderColor = "#ffffff",
            };

            pieSet.Data.AddRange(votes);
            _q5Config.Data.Datasets.Add(pieSet);

            await _q5Chart.Update();
        }
    }
}
