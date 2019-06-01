using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DynamicListView
{   
    public partial class MainPage : ContentPage
    {
        #region Inner Classes

        class ListItemViewModel : BaseViewModel
        {
            private decimal _price;
            private int _quantity;

            public decimal Price
            {
                get { return _price; }
                set { SetProperty(ref _price, value); }
            }
            public int Quantity
            {
                get { return _quantity; }
                set { SetProperty(ref _quantity, value); }
            }
        }

        class MainPageViewModel : BaseViewModel
        {
            private decimal _totalPrice;
            private int _totalQuantity;            

            private ObservableCollection<ListItemViewModel> _items;

            private ICommand _loadItemsCommand;
            private ICommand _refreshItemsCommand;
            private ICommand _addItemCommand;            

            public decimal TotalPrice
            {
                get { return _totalPrice; }
                set { SetProperty(ref _totalPrice, value); }
            }
            public int TotalQuantity
            {
                get { return _totalQuantity; }
                set { SetProperty(ref _totalQuantity, value); }
            }

            public MainPageViewModel()
            {
                this.Items = new ObservableCollection<ListItemViewModel>();
            }

            public ObservableCollection<ListItemViewModel> Items
            {
                get { return _items; }
                set { SetProperty(ref _items, value); }
            }

            public ICommand LoadItemsCommand
            {
                get
                {
                    if(_loadItemsCommand == null)
                    {
                        _loadItemsCommand = new Command(
                            () =>
                            {
                                this.Items.Add(new ListItemViewModel { Price = 1.50m, Quantity = 10 });
                                this.Items.Add(new ListItemViewModel { Price = 5.70m, Quantity = 3 });
                                this.Items.Add(new ListItemViewModel { Price = 10m, Quantity = 1});

                                RefreshTotals();
                            });

                    }

                    return _loadItemsCommand;
                }
            }

            public ICommand AddItemCommand
            {
                get
                {
                    if(_addItemCommand == null)
                    {
                        _addItemCommand = new Command(
                            () =>
                            {
                                this.Items.Add(new ListItemViewModel { Price = 1m, Quantity = 0 });
                                this.TotalQuantity = this.Items.Sum(x => x.Quantity);
                                this.TotalPrice = this.Items.Sum(x => x.Price);
                            });
                    }

                    return _addItemCommand;
                }
            }

            public ICommand RefreshItemsCommand
            {
                get
                {
                    if(_refreshItemsCommand == null)
                        _refreshItemsCommand = new Command((p) => RefreshTotals());

                    return _refreshItemsCommand;
                }
            }

            private void RefreshTotals()
            {
                this.TotalQuantity = this.Items.Sum(x => x.Quantity);
                this.TotalPrice = this.Items.Sum(x => x.Price * x.Quantity);
            }
        }

        #endregion

        #region Constants and Fields

        private MainPageViewModel _vm;

        #endregion


        public MainPage()
        {
            InitializeComponent();

            _vm = new MainPageViewModel();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadItemsCommand.Execute(null);
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _vm.RefreshItemsCommand.Execute(null);
        }
    }
}
