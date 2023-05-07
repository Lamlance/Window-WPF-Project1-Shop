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
using System.Windows.Markup;

namespace WPF_Project1_Shop.ViewModel
{
  public class ReportViewModel
  {
    public static event DataBaseFinishChanges OnFinishChangesInDB;

    public event DataBaseFinishChanges OrderSumFinishGettingData;
    public event DataBaseFinishChanges ProductCountFinishGettingData;

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

    static bool DateOnlyGroupModeSmallerOrEqual(DateOnly d1, DateOnly d2, REPORT_GROUP_MODE mode)
    {
      DateOnly date1 = new DateOnly(
       d1.Year,
       mode != REPORT_GROUP_MODE.YEAR ? d1.Month : 1,
       mode == REPORT_GROUP_MODE.DATE ? d1.Day : 1);
      DateOnly date2 = new DateOnly(
       d2.Year,
       mode != REPORT_GROUP_MODE.YEAR ? d2.Month : 1,
       mode == REPORT_GROUP_MODE.DATE ? d2.Day : 1);
      return date1 <= date2;
    }

    public ReportViewModel()
    {
      var today = DateOnly.FromDateTime(DateTime.Now).AddDays(10);

      var day30Earlier = DateOnly.FromDateTime(DateTime.Now).AddDays(-30);

      GetOrderSumGroupByTime(day30Earlier, today,REPORT_GROUP_MODE.DATE);
      GetOrderItemProductGroupByTime(day30Earlier, today, REPORT_GROUP_MODE.DATE);
    }

    public async Task GetOrderSumGroupByTime(DateOnly fromDate, DateOnly toDate, REPORT_GROUP_MODE mode)
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
      OnFinishChangesInDB?.Invoke();
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
      

      var timesLabel = orderSumsByTime.Select(o =>
      {
        return (o.Date != 0 ? $"{o.Date}/" : string.Empty) + (o.Month != 0 ? $"{o.Month}/" : string.Empty) + (o.Year != 0 ? $"{o.Year}" : string.Empty);
      }).ToArray();
      orderSumGroupByTimeLabelAxis.Add(new Axis
      {
        Labels = timesLabel
      });

      OrderSumFinishGettingData?.Invoke();
    }


    public async Task GetOrderItemProductGroupByTime(DateOnly fromDate, DateOnly toDate, REPORT_GROUP_MODE mode )
    {
      var result = await Task<List<OrderItemProductCountGroupByTime>?>.Run(() =>
      {
        using (ReportRepository repository = new ReportRepository(new EFModel.RailwayContext()))
        {
          if(mode == REPORT_GROUP_MODE.DATE)
          {
            return repository.OrderItemProductCountGroupByDate(fromDate, toDate);
          }
          if(mode == REPORT_GROUP_MODE.MONTH)
          {
            return repository.OrderItemProductCountGroupByMonth(fromDate, toDate);
          }
          return repository.OrderItemProductCountGroupByYear(fromDate, toDate);
        }
      });
      if (result != null)
      {
        SetNewValueOrderItemProductGroupByTime(result,mode);
      }
      OnFinishChangesInDB?.Invoke();
    }
    public void SetNewValueOrderItemProductGroupByTime(List<OrderItemProductCountGroupByTime> data, REPORT_GROUP_MODE mode)
    {
      ProductCountGroupByTime.Clear();
      OrderCountGroupByTimeLabelAxis.Clear();

      DateOnly minDate = data.Min(d => new DateOnly(
        d.Year, 
        mode != REPORT_GROUP_MODE.YEAR ? d.Month : 1 ,
        mode == REPORT_GROUP_MODE.DATE ? d.Date : 1));

      DateOnly maxDate = data.Max(d => new DateOnly(
        d.Year,
        mode != REPORT_GROUP_MODE.YEAR ? d.Month : 1,
        mode == REPORT_GROUP_MODE.DATE ? d.Date : 1));

      List<DateOnly> dates = new List<DateOnly>();
      for (DateOnly date = minDate; DateOnlyGroupModeSmallerOrEqual(date,maxDate,mode);)
      {
        dates.Add(date);
        if (mode == REPORT_GROUP_MODE.DATE)
        {
          date = date.AddDays(1);
        }
        if(mode == REPORT_GROUP_MODE.MONTH)
        {
          date = date.AddMonths(1);
        }
        if(mode == REPORT_GROUP_MODE.YEAR)
        {
          date = date.AddYears(1);
        }
      }

      var labels = dates.Select(d =>
      {
        return (mode == REPORT_GROUP_MODE.DATE ? $"{d.Day}/" : string.Empty) + (mode != REPORT_GROUP_MODE.YEAR ? $"{d.Month}/" : string.Empty) + $"{d.Year}";
      }).ToArray();
      OrderCountGroupByTimeLabelAxis.Add(new Axis
      {
        Labels = labels,
        LabelsRotation = 90
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
        for (DateOnly date = minDate; DateOnlyGroupModeSmallerOrEqual(date,maxDate,mode); )
        {
          var found = item.Find(i => i.Year == date.Year && (i.Month == date.Month || i.Month == 0) && (i.Date == date.Day || i.Date == 0));
          if(found != null)
          {
            countArray.Add(found.Count);
          }
          else
          {
            countArray.Add(0);
          }
          if (mode == REPORT_GROUP_MODE.DATE)
          {
            date = date.AddDays(1);
          }
          if (mode == REPORT_GROUP_MODE.MONTH)
          {
            date = date.AddMonths(1);
          }
          if (mode == REPORT_GROUP_MODE.YEAR)
          {
            date = date.AddYears(1);
          }
        }
        ProductCountGroupByTime.Add(new LineSeries<int>()
        {
          Values = countArray.ToArray(),
          //Values = item.Select(i => i.Count).ToArray(),
          Name = item.ElementAt(0).ProductName
        }); 
      }
      ProductCountFinishGettingData?.Invoke();
    }
  }
}
