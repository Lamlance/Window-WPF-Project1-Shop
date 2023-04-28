using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Project1_Shop.EFModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WPF_Project1_Shop.View
{
  /// <summary>
  /// Interaction logic for ImportProductFromAccess.xaml
  /// </summary>
  public partial class ImportProductFromAccess : UserControl
  {
    public delegate void OnProductDataImported(List<Product> products);
    public event OnProductDataImported? OnDataImported;

    public ImportProductFromAccess()
    {
      InitializeComponent();
    }

    string AccessDBPath = "";
    ObservableCollection<Product> readProduct = new ObservableCollection<Product>();
    private void BrowseBtnClick(object sender, RoutedEventArgs e)
    {
      using (var dialog = new System.Windows.Forms.OpenFileDialog())
      {
        dialog.Filter = "Files | *.accdb;";
        var res = dialog.ShowDialog();
        if(res == System.Windows.Forms.DialogResult.OK)
        {
          if (dialog.CheckFileExists)
          {
            AccessDBPath = dialog.FileName;
            this.txtAcessFilePath.Content = dialog.FileName;
          }
        }
      };
    }
    
    public void ReadAccessDB()
    {
      string connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={AccessDBPath}";
      string tableName = this.txtBoxTableName.Text;

      string productNameCol = this.txtBoxProductNameCol.Text;
      string productAmountCol = this.txtBoxProductQuantityCol.Text;
      string productPriceCol = this.txtBoxProductPriceCol.Text;
      string productDescCol = this.txtBoxProductDescCol.Text;
      string productImgCol = txtBoxProductImageCol.Text;

      using (OleDbConnection connection = new OleDbConnection(connString))
      {
        connection.Open();
        OleDbCommand command = new OleDbCommand($"SELECT * FROM {tableName} ;", connection);
        using (OleDbDataReader reader = command.ExecuteReader())
        {
          readProduct.Clear();
          while (reader.Read())
          {
            string? name = (reader[productNameCol] != null && reader[productNameCol] is string) ? (string)reader[productNameCol] : null;
            int? amount = (reader[productAmountCol] != null && reader[productAmountCol] is int) ? (int)reader[productAmountCol] : null;
            int? price = (reader[productPriceCol] != null && reader[productPriceCol] is int) ? (int)reader[productPriceCol] : null;
            string? desc = (reader[productDescCol] != null && reader[productDescCol] is string) ? (string)reader[productDescCol] : null;
            string? img = (reader[productImgCol] != null && reader[productImgCol] is string) ? (string)reader[productImgCol] : null;
            readProduct.Add(new Product()
            {
              ProductName = name ?? "Unknow product",
              Numbers = amount ?? 0,
              Price = price ?? 0,
              Descriptions = desc,
              ImagePath = img ?? ""
            });
          }
        }
      }
    }

    private void AcceptBtnClick(object sender, RoutedEventArgs e)
    {
      OnDataImported?.Invoke(readProduct.ToList());
    }

    private void ProductDataGridLoaded(object sender, RoutedEventArgs e)
    {
      this.ImportedProductDataGrid.ItemsSource = readProduct;
    }

    private void ReadBtnClick(object sender, RoutedEventArgs e)
    {
      try
      {
        ReadAccessDB();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Cant read db check your inputs: {ex.Message}");
      }
    }
  }
}
