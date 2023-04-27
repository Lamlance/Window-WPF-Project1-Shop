using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public class CategoryViewModel
  {
    private ObservableCollection<Category> categories;
    private HashSet<Category> selectedCategories;


    public CategoryViewModel()
    {
      selectedCategories = new HashSet<Category>();
      categories = new ObservableCollection<Category>();

      GetManyCategories();
    }

    public ObservableCollection<Category> Categories { get => categories; }
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
        categories.Add(c);
      });
    }
  }
}
