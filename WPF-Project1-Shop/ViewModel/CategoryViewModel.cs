using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public class CategoryViewModel
  {
    public enum MODIFY_MODE
    {
      NONE, ADD, EDIT, DELETE
    }
    MODIFY_MODE _modifyMode = MODIFY_MODE.NONE;
    public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }

    private int _curPage = 1;
    private int _itemPerPage = 15;
    private bool _isSearching = false;

    public class CheckableCategory : Category, INotifyPropertyChanged
    {
      public int totalAmount { get; set; } = 0;
      public bool IsChecked { get; set; } = true;

      public event PropertyChangedEventHandler? PropertyChanged;
    }


    private readonly static Dictionary<int,CategoryViewModel> createdInstance = new Dictionary<int, CategoryViewModel>();
    private static int idCount = 0;
    public static CategoryViewModel NewInstance()
    {
      idCount++;
      var category = new CategoryViewModel(idCount);
      createdInstance.Add(idCount, category);
      return category;
    }

    public delegate void OnCatogryDataction(Category? category);
    public event OnCatogryDataction OnNewCategoryAdded;

    private static void RemoveInstanceFromDict(int id)
    {
      createdInstance.Remove(id);
    }
    private static void NotifyNewCateogryAdded(Category newCategory)
    {
      foreach(var key in createdInstance.Keys)
      {
        createdInstance[key].Categories.Add(new CheckableCategory
        {
          IsChecked = false,
          CategoryName = newCategory.CategoryName,
          Id = newCategory.Id
        });
      }
    }
    
    private ObservableCollection<CheckableCategory> categories;
    private HashSet<Category> selectedCategories;
    private readonly int myId;

    private CategoryViewModel(int id)
    {
      selectedCategories = new HashSet<Category>();
      categories = new ObservableCollection<CheckableCategory>();
      myId = id;
      GetManyCategories();
    }
    ~CategoryViewModel()
    {
      RemoveInstanceFromDict(myId);
    }
    public ObservableCollection<CheckableCategory> Categories { get => categories; }
    public HashSet<Category> SelectedCategories { get => selectedCategories; }

    public async Task GetManyCategories()
    {
      var result = await Task<List<Category>>.Run(() =>
      {
        using (CategoryRepository repository = new CategoryRepository(new RailwayContext()))
        {
          var result = repository.GetAllCategories().ToList();
          return result;
        }
      });
      categories.Clear();
      result.ForEach((c) =>
      {
        categories.Add(new CheckableCategory() { 
          CategoryName = c.CategoryName,
          totalAmount = c.Products.Count,
          Id = c.Id,
          Products = c.Products,
          IsChecked = false
        });
      });
    }

    public async Task AddCategory(Category category)
    {
      var result = await Task<Category?>.Run(() =>
      {
        try
        {
          using (CategoryRepository repository = new CategoryRepository(new RailwayContext()))
          {
            return repository.AddCategory(category);
          }
        }
        catch (Exception)
        {
          return null;
        }
      });

      if(result != null)
      {
        NotifyNewCateogryAdded(result);
      }
      OnNewCategoryAdded?.Invoke(result);

    }

    public async Task SearchCategories(string? name)
    {
      if (_isSearching)
      {
        return;
      }
      _isSearching = true;
      //var result = await Task<List<Order>?>.Run(() =>
      //{
      //  using (CustomerRepository repository = new CustomerRepository(new RailwayContext()))
      //  {
      //    return repository.SearchCategories(name)!.ToList();
      //  }
      //});
      //customersSet = result;
      //SetPage(1);
      //_isSearching = false;
      //OnDataSetReset?.Invoke((int)Math.Ceiling((double)(customersSet != null ? customersSet.Count() : 0) / _itemPerPage));
    }

  }
}
