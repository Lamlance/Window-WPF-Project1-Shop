using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class BaseCustomRepository:IDisposable
  {
    RailwayContext context;

    protected BaseCustomRepository(RailwayContext context)
    {
      this.context = context;
    }

    public RailwayContext dbContext { get => context; set => context = value; }

    public void Dispose()
    {
      context.SaveChanges();
      context.Dispose();
    }
  }
}
