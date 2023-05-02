using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFCustomRepository;
using static WPF_Project1_Shop.EFCustomRepository.ReportRepository;

namespace WPF_Project1_Shop.ViewModel
{
  public class ReportViewModel
  {
    ObservableCollection<ISeries<double>> orderSumGroupByTime = new ObservableCollection<ISeries<double>>();
    ObservableCollection<Axis> orderSumGroupByTimeLabelAxis = new ObservableCollection<Axis>();

    ObservableCollection<ISeries<int>> productCountGroupByTime = new ObservableCollection<ISeries<int>>();
    ObservableCollection<Axis> orderCountGroupByTimeLabelAxis = new ObservableCollection<Axis>();

    public ObservableCollection<ISeries<double>> OrderSumGroupByTime { get => orderSumGroupByTime; }
    public ObservableCollection<Axis> OrderSumGroupByTimeLabelAxis { get => orderSumGroupByTimeLabelAxis; }

    public ObservableCollection<ISeries<int>> ProductCountGroupByTime { get => productCountGroupByTime; }
    public ObservableCollection<Axis> OrderCountGroupByTimeLabelAxis { get => orderCountGroupByTimeLabelAxis; }

    public enum REPORT_GROUP_MODE
    {
      DATE,MONTH,YEAR
    }

    public ReportViewModel()
    {
      var today = DateOnly.FromDateTime(DateTime.Now).AddDays(10);

      var day30Earlier = DateOnly.FromDateTime(DateTime.Now).AddDays(-30);

      GetOrderSumGroupByDate(day30Earlier, today,REPORT_GROUP_MODE.MONTH);
      GetOrderItemProductGroupByDate(day30Earlier, today);
    }

    public async Task GetOrderSumGroupByDate(DateOnly fromDate, DateOnly toDate, REPORT_GROUP_MODE mode = REPORT_GROUP_MODE.DATE)
    {
      var result = await Task<List<ReportRepository.OrderSumProfitGroupByTime>?>.Run(() =>
      {
        using (ReportRepository repository = new ReportRepository(new EFModel.RailwayContext()))
        {
          if(mode == REPORT_GROUP_MODE.DATE)
          {
            return repository.OrderSubTotalByDate(fromDate, toDate);
          }
          if(mode == REPORT_GROUP_MODE.MONTH)
          {
            return repository.OrderSubTotalByMonth(fromDate, toDate);
          }
          return repository.OrderSubTotalByYear(fromDate, toDate);
        }
      });
      if (result != null)
      {
        SetNewValueOrderSumGroupByTime(result);
      }
    }



    public void SetNewValueOrderSumGroupByTime(List<ReportRepository.OrderSumProfitGroupByTime> orderSumsByTime)
    {
      orderSumGroupByTime.Clear();
      orderSumGroupByTimeLabelAxis.Clear();
      orderSumsByTime.Reverse();
      var sums = orderSumsByTime.Select(o => o.Sum).ToArray();
      orderSumGroupByTime.Add(new ColumnSeries<double>()
      {
        Name = "Total",
        Values = sums
      });
      orderSumGroupByTime.Add(new ColumnSeries<double>()
      {
        Name = "Profit",
        Values = sums.Select(s => s * (0.1)).ToArray()
      });

      StringBuilder sb = new StringBuilder();
      

      var timesLabel = orderSumsByTime.Select(o =>
      {
        return (o.Date != 0 ? $"{o.Date}/" : string.Empty) + (o.Month != 0 ? $"{o.Month}/" : string.Empty) + (o.Year != 0 ? $"{o.Year}" : string.Empty);
      }).ToArray();
      orderSumGroupByTimeLabelAxis.Add(new Axis
      {
        Labels = timesLabel,
        LabelsRotation = 0,
        SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
        SeparatorsAtCenter = false,
        TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
        TicksAtCenter = true
      });
    }


    public async Task GetOrderItemProductGroupByDate(DateOnly fromDate, DateOnly toDate)
    {
      var result = await Task<List<OrderItemProductCountGroupByTime>?>.Run(() =>
      {
        using (ReportRepository repository = new ReportRepository(new EFModel.RailwayContext()))
        {
          return repository.OrderItemProductCountGroupByDate(fromDate, toDate);
        }
      });
      if (result != null)
      {
        SetNewValueOrderItemProductGroupByTime(result);
      }
    }

    public void SetNewValueOrderItemProductGroupByTime(List<OrderItemProductCountGroupByTime> data)
    {
      ProductCountGroupByTime.Clear();
      OrderCountGroupByTimeLabelAxis.Clear();

      DateOnly minDate = data.Min(d => new DateOnly(d.Year, d.Month, d.Date));
      DateOnly maxDate = data.Max(d => new DateOnly(d.Year, d.Month, d.Date));

      List<DateOnly> dates = new List<DateOnly>();
      for (DateOnly date = minDate; date <= maxDate;)
      {
        dates.Add(date);
        date = date.AddDays(1);
      }
      OrderCountGroupByTimeLabelAxis.Add(new Axis()
      {
        Labels = dates.Select(d=>$"{d.Day}/{d.Month}/{d.Year}").ToArray()
      });
      var groupByProduct = data
        .GroupBy(d => new
        {
          d.ProductName
        })
        .Select(g => g.ToList())
        .ToList();
      
      foreach (var item in groupByProduct)
      {
        List<int> countArray = new List<int>();
        for (DateOnly date = minDate; date <= maxDate; )
        {
          var found = item.Find(i => i.Year == date.Year && i.Month == date.Month && i.Date == date.Day);
          if(found != null)
          {
            countArray.Add(found.Count);
          }
          else
          {
            countArray.Add(0);
          }
          date = date.AddDays(1);
        }
        ProductCountGroupByTime.Add(new LineSeries<int>()
        {
          Values = countArray.ToArray(),
          Name = item.ElementAt(0).ProductName
        });
      }
    }
  }
}
