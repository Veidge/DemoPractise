using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoTraining
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        ApplicationContext db = new ApplicationContext();
        public MainPage(string currentUsername)
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
            ShowCurrentAccount(currentUsername);
        }

        // при загрузке страницы подгружать всю БД
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            LoadGoods();
            LoadCategoryList();
            LoadStatistics();
        }

        private void LoadGoods()
        {
            goods.ItemsSource = db.Products.ToList();
        }

        private void InsertProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(goodsName.Text) || string.IsNullOrWhiteSpace(goodsCategory.Text) ||
                string.IsNullOrWhiteSpace(goodsQuantity.Text) || string.IsNullOrWhiteSpace(goodsPrice.Text))
            {
                MessageBox.Show("Не все поля заполнены.", 
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(goodsQuantity.Text, out int quantity))
            {
                MessageBox.Show("Поле количества должно содержать число.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(goodsPrice.Text.Replace('.', ','), out decimal price))
            {
                MessageBox.Show("Поле цены должно содержать число.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            //decimal.TryParse(goodsPrice.Text.Replace('.', ','), out decimal price);
            //int.TryParse(goodsQuantity.Text, out int quantity);

            if (price < 0 || quantity < 0)
            {
                MessageBox.Show("Числовые значения не могут быть отрицательными.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Product newProduct = new Product
            {
                Name = goodsName.Text,
                Category = goodsCategory.Text,
                Price = price,
                Quantity = quantity
            };

            db.Products.Add(newProduct);
            db.SaveChanges();
            LoadGoods();
            LoadCategoryList();
            LoadStatistics();
        }

        private void SelectProduct(object sender, SelectionChangedEventArgs e)
        {
            Product selectedProduct = goods.SelectedItem as Product;

            if (selectedProduct != null)
            {
                goodsName.Text = selectedProduct.Name;
                goodsCategory.Text = selectedProduct.Category;
                goodsQuantity.Text = selectedProduct.Quantity.ToString();
                goodsPrice.Text = selectedProduct.Price.ToString("0.0");

            }
        }

        private void DeleteProduct(object sender, EventArgs e)
        {
            Product selectedProduct = goods.SelectedItem as Product;

            if (selectedProduct != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить запись?",
                    "Подтверждение",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Product product = db.Products.Find(selectedProduct.Id);

                    if (product != null)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        LoadGoods();
                        LoadCategoryList();
                        LoadStatistics();
                    }
                }
            }
        }

        private void UpdateProduct(object sender, EventArgs e)
        {
            Product selectedProduct = goods.SelectedItem as Product;

            if (selectedProduct != null)
            {
                Product product = db.Products.Find(selectedProduct.Id);

                if (product != null)
                {
                    product.Name = goodsName.Text;
                    product.Category = goodsCategory.Text;
                    product.Quantity = int.Parse(goodsQuantity.Text);
                    product.Price = decimal.Parse(goodsPrice.Text.Replace('.', ','));

                    db.SaveChanges();
                }
            }
            LoadGoods();
            LoadCategoryList();
            LoadStatistics();
        }

        private void SearchingProductName(object sender, TextChangedEventArgs e)
        {
            goods.ItemsSource = db.Products.Where(p => p.Name.ToLower().Contains(searchBox.Text.ToLower())).ToList();
        }

        private void CategoryFilter(object sender, SelectionChangedEventArgs e)
        {
            if (categoryBox.SelectedItem.ToString() == "Все категории")
            {
                goods.ItemsSource = db.Products.ToList();
            }
            else
            {
                goods.ItemsSource = db.Products.Where(p => p.Category == categoryBox.SelectedItem.ToString()).ToList();
            }
        }

        private void LoadCategoryList()
        {
            categoryBox.Items.Clear();
            categoryBox.Items.Add("Все категории");

            foreach (string category in db.Products.Select(p => p.Category).Distinct())
            {
                categoryBox.Items.Add(category);
            }
            categoryBox.SelectedIndex = 0;
        }

        private void LoadStatistics()
        {
            int goodsCount = db.Products.Count();
            decimal goodsPrice = db.Products.Sum(p => p.Price);
            int goodsCategoriesCount = db.Products.Select(p => p.Category).Distinct().Count();

            totalCategory.Text = $"Всего категорий: {goodsCategoriesCount}";
            totalQuantity.Text = $"Товаров на складе: {goodsCount}";
            totalPrice.Text = $"Общая стоимость (в рублях): {goodsPrice}";
        }

        private void ShowCurrentAccount(string account)
        {
            currentAccount.Text = $"{account}";
        }

        private void ChangeAccount(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
