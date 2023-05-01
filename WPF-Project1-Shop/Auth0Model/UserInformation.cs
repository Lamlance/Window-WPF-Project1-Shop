using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Auth0Model
{
  public class UserInformation: INotifyPropertyChanged
  {
    private string nickname = "";
    private string name = "";
    private string pricturePath = "";
    private string email = "";
    private bool isEmailVerified = false;
    private string role = "";

    public string Nickname { get => nickname; set => nickname = value; }
    public string Name { get => name; set => name = value; }
    public string PricturePath { get => pricturePath; set => pricturePath = value; }
    public string Email { get => email; set => email = value; }
    public bool IsEmailVerified { get => isEmailVerified; set => isEmailVerified = value; }

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}
