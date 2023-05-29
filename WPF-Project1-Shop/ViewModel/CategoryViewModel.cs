using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
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

    public delegate void ModifyCategoryCallBackType(Category? category);

    public event ModifyCategoryCallBackType OnDataAdd;
    public event ModifyCategoryCallBackType OnDataRemove;
    public event ModifyCategoryCallBackType OnDataUpdate;

    private int _curPage = 1;
    private int _itemPerPage = 15;
    private bool _isSearching = false;

    public int ItemPerPage { get => _itemPerPage; set => _itemPerPage = value; }
    public delegate void CategoryDataSetChanged(int totalPage);

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

    private static void NotifyNewCateogryDeleted(Category deleteCategory)
    {
      foreach (var key in createdInstance.Keys)
      {
        createdInstance[key].Categories.Remove(new CheckableCategory
        {
          Id = deleteCategory.Id
        });
      }
    }


    private ObservableCollection<CheckableCategory> categories;
    private HashSet<Category> selectedCategories;
    private readonly int myId;
    List<Category>? categorySet;
    Dictionary<long, int> idToPagePos;
    HashSet<Category> skipCategories;
    public event CategoryDataSetChanged? OnDataSetReset;

    private CategoryViewModel(int id)
    {
      skipCategories = new HashSet<Category>();
      selectedCategories = new HashSet<Category>();
      categories = new ObservableCollection<CheckableCategory>();
      myId = id;
      idToPagePos = new Dictionary<long, int>();

      GetManyCategories();
    }
    ~CategoryViewModel()
    {
      RemoveInstanceFromDict(myId);
    }
    public ObservableCollection<CheckableCategory> Categories { get => categories; }
    public HashSet<Category> SelectedCategories { get => selectedCategories; }

    public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }
    public string GetStatusString()
    {
      return $"IS {_modifyMode}";
    }

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

    public async Task UpdateCategory(Category category)
    {
      var result = await Task<Category?>.Run(() =>
      {
        try
        {
          using (CategoryRepository repository = new CategoryRepository(new RailwayContext()))
          {
            return repository.UpdateCategory(category);
          }
        }
        catch (Exception e)
        {
          return null;
        }
      });
      if (result != null)
      {
        OnDataUpdate?.Invoke(result);
      }

    }

    public async Task SearchCategories(string? name)
    {
      var result = await Task<List<Category>?>.Run(() =>
      {
        using (CategoryRepository repository = new CategoryRepository(new RailwayContext()))
        {
          var categories = repository.SearchCategories(name);
          return categories?.ToList();
        }
      });
      if(result != null)
      {
        categorySet = result.ToList();
        setPage(1);
        OnDataSetReset?.Invoke((int)Math.Ceiling((double)(categorySet != null ? categorySet.Count() : 0) / _itemPerPage));
      }
    }

    public async Task RemoveCategory(Category category)
    {
      var result = await Task.Run(() =>
      {
        using (CategoryRepository repository = new CategoryRepository(new RailwayContext()))
        {
          return repository.DeleteCategory(category);
        }
      });
      if(result != null)
      {
        categories.Remove(new CheckableCategory
        {
          Id = category.Id,
        });
        NotifyNewCateogryDeleted(result);
      }
      OnDataRemove?.Invoke(result);
    }


    public void setPage(int page = 1)
    {
      if (categorySet == null)
      {
        return;
      }
      _curPage = page > 0 ? page : 1;
      int start = (_curPage * _itemPerPage) - _itemPerPage;
      int end = Math.Min(start + _itemPerPage, categorySet.Count());
      categories.Clear();
      idToPagePos.Clear();
      for (int i = start; i < end; i++)
      {

        Category? c = categorySet.ElementAtOrDefault(i);
        if(c != null)
        {
          categories.Add(new CheckableCategory()
          {
            Id = c.Id,
            CategoryName = c.CategoryName,
            Products = c.Products,
            totalAmount = c.Products.Count(),
          });

        }
      }
    }


  }
}
