using CourseProjectClassLibrary.Models;
using CourseProjectClassLibrary.Repository;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Office.Interop.Excel;

namespace Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private bool isLoaded = false;
        //Хранилище
        IRepository repository = new DbRepository();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            //Задать источник данных для списков
            LstProductInit();
            LstClientInit();
            LstOrderInit();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region Обновление данных для списков после изменения БД или порялка сортировки или фильтрации
        void LstProductInit()
        {
            lstProducts.ItemsSource = repository.GetProducts(cbPrSort.SelectedIndex);
        }

        void LstClientInit()
        {
            lstClients.ItemsSource = repository.GetClients(cbClSort.SelectedIndex);
        }

        void LstOrderInit()
        {
            lstOrders.ItemsSource = repository.GetOrders(cbOrSort.SelectedIndex);
        }
        #endregion

        private void cbOrProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;
            Product pr = cbOrProduct.SelectedItem as Product;
            tbOrProfit.Text = Convert.ToString(pr?.Price - pr?.CostPrice);
        }

        #region Добавить
        //Добавить элемент
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (mainTab.SelectedIndex == 0) 
                AddProduct();
            else if (mainTab.SelectedIndex == 1)
                AddClient();
            else AddOrder();
        }
        //Добавить продукт
        private void AddProduct()
        {
            //Создать новый объект класса товар
            Product product = new Product()
            {
                Name = tbPrModel.Text,
                Category = tbPrCategory.Text,
                Quantity = Convert.ToDouble(tbPrQuantity.Text),
                Price = Convert.ToDecimal(tbPrPrice.Text),
                CostPrice = Convert.ToDecimal(tbPrCostPrice.Text),
                Description = tbPrDescription.Text
            };
            //Добавить объект
            repository.AddProduct(product);
            //Обновить списки
            LstProductInit();
            //Выьранный элемент в списке - новый элемент
            lstProducts.SelectedItem = product;
            MessageBox.Show($"Продукт {product.Name} создан", "Успешное действие", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AddClient()
        {
            Client client = new Client()
            {
                Name = tbClFName.Text,
                FirstName = tbClFName.Text,
                MiddleName = tbClMName.Text,
                Address = tbClAddress.Text,
                Description = tbClDescription.Text,
                IsPerson = (bool)chClIsPerson.IsChecked
            };
            repository.AddClient(client);
            //LstClientInit();
            MessageBox.Show($"Клиент {client.Name} создан", "Успешное действие", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AddOrder()
        {
            //Создание заказа
            Order order = new Order()
            {
                Name = tbOrName.Text,
                OrderProduct = cbOrProduct.SelectedItem as Product,
                OrderClient = cbOrClient.SelectedItem as Client,
                Description = tbOrDescription.Text,
                Quantity = Convert.ToDouble(tbOrQuantity.Text),
                OrderDate = calOrDate.DisplayDate,
                Profit = Convert.ToDecimal(tbOrProfit.Text)
            };
            //Если нет требуемого количества товара, заказ не создается
            if (order.OrderProduct.Quantity - order.Quantity > 0)
            {
                order.OrderProduct.Quantity -= order.Quantity;
                repository.AddOrder(order);
                repository.UpdateProduct(order.OrderProduct);
                LstOrderInit();
                LstProductInit();
            }
            //Если нет нужного количества товара вывод сообщения
            else MessageBox.Show($"Нет нужного количества товара, в наличии " +
                $"{order.OrderProduct.Quantity - order.Quantity}",
                "Нет в наличии", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show($"Продукт {order.Name} создан", "Успешное действие", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
        #region Удалить
        //Удалить элемент
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (mainTab.SelectedIndex == 0)
                RemoveProduct();
            else if (mainTab.SelectedIndex == 1)
                RemoveClient();
            else RemoveOrder();
        }

        public void RemoveProduct()
        {
            Product product = lstProducts.SelectedItem as Product;
            if (product != null)
            {
                //Вывод окна предупреждения, если нажата кнопка Yes, Удалить элемент
                if (MessageBox.Show("Удаление продукта приведет к удалению заказов!",
                    "Удаление продукта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    repository.RemoveProduct(product);
                    //Обновление списка товаров и заказов
                    LstProductInit();
                    LstOrderInit();
                }
            }
        }
        private void RemoveClient()
        {
            Client client = lstClients.SelectedItem as Client;
            if (client != null)
            {
                //Вывод окна предупреждения, если нажата кнопка Yes, Удалить элемент
                if (MessageBox.Show("Удаление клиента приведет к удалению заказов!",
                    "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    repository.RemoveClient(client);
                    //Обновление списка клиентов и заказов
                    LstClientInit();
                    LstOrderInit();
                }
            }
        }
        private void RemoveOrder()
        {
            Order order = lstOrders.SelectedItem as Order;
            if (order != null)
            {
                if (MessageBox.Show("Удалить заказ?", "Удаление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    repository.RemoveOrder(order);
                    //Обновление списка заказов
                    LstOrderInit();
                }
            }
        }
        #endregion
        #region Получить по ИД
        //Получить элемент по Id
        private void bntShow_Click(object sender, RoutedEventArgs e)
        {
            if (mainTab.SelectedIndex == 0)
                ShowProduct();
            else if (mainTab.SelectedIndex == 1)
                ShowClient();
            else ShowOrder();
        }

        private void ShowProduct()
        {
            //Получить идентификатор из поля ввода
            int.TryParse(tbPrId.Text, out int itemId);
            //Получить элемент
            var item = repository.GetProduct(itemId);
            //Если элемент не найден, то вывести сообщение
            if (item != null)
                lstProducts.SelectedItem = item;
            else MessageBox.Show("Элемент не найден!", 
                "Результаты поиска", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowClient()
        {
            int.TryParse(tbClId.Text, out int itemId); 
            var item = repository.GetClient(itemId);
            if (item != null)
                lstClients.SelectedItem = item;
            else MessageBox.Show("Элемент не найден!", "Результаты поиска", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowOrder()
        {
            int.TryParse(tbOrId.Text, out int itemId);
            var item = repository.GetOrder(itemId);
            if (item != null)
                lstOrders.SelectedItem = item;
            else MessageBox.Show("Элемент не найден!", "Результаты поиска", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
        #region Обновить данные
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)lstProducts.SelectedItem;
            if (product != null)
            {
                repository.UpdateProduct(product);
                LstProductInit();
                LstOrderInit();
            }
            MessageBox.Show($"Продукт {product?.Name} обновлен", "Успешное действие", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //Обновить данные по клиенту
        private void clientUpdate(object sender, RoutedEventArgs e)
        {
            Client client = lstClients.SelectedItem as Client;
            if (client != null)
            {
                repository.UpdateClient(client);
                LstClientInit();
                LstOrderInit();
            }
            MessageBox.Show($"Клиент {client?.Name} обновлен", "Успешное действие", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //Обновить данные по заказу
        private void orderUpdate()
        {
            Order order = lstOrders.SelectedItem as Order;
            if (order != null)
            {
                order.Name = tbOrName.Text;
                order.OrderProduct = cbOrProduct.SelectedItem as Product;
                order.OrderClient = cbOrClient.SelectedItem as Client;
                order.Description = tbOrDescription.Text;
                order.OrderDate = calOrDate.DisplayDate;
                order.Profit = Convert.ToDecimal(tbOrProfit.Text);
                repository.UpdateOrder(order);
                LstOrderInit();
            }
            MessageBox.Show($"Заказ {order?.Name} обновлен", "Успешное действие", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
        #region Сортировка и фильтрация
        //Изменение порядка сортировки списка товаров
        private void cbPrSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstProductInit();
        }

        ////Изменение порядка сортировки списка клиентов
        private void cbClSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstClientInit();
        }

        //Изменение порядка сортировки списка заказов
        private void cbOrSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstOrderInit();
        }

        //Нажатие на кнопку снять фильтр
        private void btnOrClearFilter_Click(object sender, RoutedEventArgs e)
        {
            LstOrderInit();
            //Кнопка фильтрации активна
            btnOrClearFilter.IsEnabled = false;
            btnOrFilterByDate.IsEnabled = true;
        }

        //Нажатие на кнопку фильтрации
        private void btnOrFilterByDate_Click(object sender, RoutedEventArgs e)
        {
            //Если выбрана дата для фильтра
            if (calOrDate.SelectedDate != null)
            {
                //Получение выбранной даты
                DateTime filterDate = (DateTime)calOrDate.SelectedDate;
                //Вернуть фильтрованный список
                lstOrders.ItemsSource = repository.GetOrdersByDate(filterDate);
                //Обозначить фильтрацию - кнопка фильтрации не активна
                btnOrClearFilter.IsEnabled = true;
                btnOrFilterByDate.IsEnabled = false;
            }
        }
        #endregion
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            // Создание приложения Excel
            Microsoft.Office.Interop.Excel.Application excelApp = 
                new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = true;
            if (excelApp == null)
            {
                Console.WriteLine("Excel не установлен!");
                return;
            }
            // Создание новой книги
            excelApp.Workbooks.Add();
            Worksheet worksheet = (Worksheet)excelApp.ActiveSheet;

            // Заголовки таблицы
            worksheet.Cells[1, 1] = "Id заказа";
            worksheet.Cells[1, 2] = "Имя заказа";
            worksheet.Cells[1, 3] = "Название продукта";
            worksheet.Cells[1, 4] = "ФИО клиента";
            worksheet.Cells[1, 5] = "Дата заказа";
            worksheet.Cells[1, 6] = "Примечание";
            for (int i = 0;i<lstOrders.Items.Count;i++)
            {
                Order ord = lstOrders.Items[i] as Order;
                // Запись в ячейки
                worksheet.Cells[i + 2, 1] = ord?.Id;
                worksheet.Cells[i+2, 2] = ord?.Name;
                worksheet.Cells[i+2, 3] = ord?.OrderProduct.Name;
                worksheet.Cells[i + 2, 4] = ord?.OrderClient.Name + " " 
                    + ord?.OrderClient.FirstName + " " 
                    + ord?.OrderClient.MiddleName;
                worksheet.Cells[i+2, 5] = ord?.OrderDate;
                worksheet.Cells[i+2, 6] = ord?.Description;
            }
            // Закрытие
            excelApp.Quit();
        }
        #region Ribbon

        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {
            //Вызов диалогового окна, если нажата кнопка ОК, выход из приложения
            if (MessageBox.Show("Закрыть приложение?", "Выход", MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void mainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTab.SelectedIndex == 2)
            {
                rtOrderConext.Visibility = Visibility.Visible;
                rtOrder.Visibility = Visibility.Visible;
            }
            else
            {
                rtOrderConext.Visibility = Visibility.Collapsed;
                rtOrder.Visibility = Visibility.Collapsed;
            }
        }


        private void btnGraph_Click(object sender, RoutedEventArgs e)
        {
            //Словарь клиент число заказаов
            Dictionary<Client, int> gr = new Dictionary<Client, int>();
            //Пройти по всему списку заказов,
            //посчитать число заказов каждого для клиента
            foreach(Order order in lstOrders.Items)
            {
                if (gr.ContainsKey(order.OrderClient)) gr[order.OrderClient] += 1;
                else gr[order.OrderClient] = 1;
            }
            //Создать новое окно
            Graph graph = new Graph();
            //Показать окно постоить график
            graph.Show();
            graph.GraphShow(gr);
        }
        #endregion
    }
}