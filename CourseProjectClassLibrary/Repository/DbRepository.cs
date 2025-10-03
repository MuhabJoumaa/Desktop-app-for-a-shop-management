using CourseProjectClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectClassLibrary.Repository
{
    //Хранилище - простая БД SQLite
    public class DbRepository : DbContext, IRepository
    {
        #region Настройка хранилища (контекста)
        //Каждая DbSet - отдельная таблица данных БД
        DbSet<Product> Products => Set<Product>();
        DbSet<Client> Clients => Set<Client>();
        DbSet<Order> Orders => Set<Order>();
        //Конструктор - создании контекста автоматически проверить наличие базы данных и,
        //если она отсуствует, создать ее
        public DbRepository()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
            ////Тестовые данные, в реальном приложении - убрать
            /* Products.AddRange(
                new Product()
                {
                    Id = 1,
                    Name = "Стеклоомыватель Blueone",
                    Price = 25.12m,
                    CostPrice = 20.03m,
                    Quantity = 50,
                    Description = "Отличное средство для чистки ваших стекол"
                },
                new Product()
                {
                    Id = 2,
                    Name = "Свеча зажигания Nickel100",
                    Price = 125.12m,
                    CostPrice = 95.5m,
                    Quantity = 10,
                    Description = "Абсолютно из никеля"
                },
                new Product()
                {
                    Id = 3,
                    Name = "Шестерня MAXIMUM",
                    Price = 150.12m,
                    CostPrice = 143.0m,
                    Quantity = 30,
                    Description = "Отличная шестеренка"
                },
                new Product()
                {
                    Id = 4,
                    Name = "Передняя фара ВАЗ 2107",
                    Price = 1125.10m,
                    CostPrice = 1000.123m,
                    Quantity = 5,
                    Description = "Почти новая"
                });

            Clients.AddRange(
                new Client()
                {
                    Id = 1,
                    Name = "Ivanov",
                    FirstName = "Ivan",
                    MiddleName = "Ivanovich",
                    Address = "Dostoevskij street 8",
                    Description = "Vip",
                    IsPerson = true
                },
            new Client()
            {
                Id = 2,
                Name = "Petrov",
                FirstName = "Peter",
                MiddleName = "Petrovich",
                Address = "Moscow street 7",
                Description = "Loser",
                IsPerson = true
            },
            new Client()
            {
                Id = 3,
                Name = "Sidorov",
                FirstName = "Sidor",
                MiddleName = "Sidorovich",
                Address = "Peter street 32",
                Description = "Big Boss",
                IsPerson = true
            }
            );
            ////Сохранить изменения в файле БД
            this.SaveChanges();
            Orders.AddRange(
                new Order
                {
                    Id = 1,
                    Name = $"{Clients.ElementAt(0)}: {DateTime.Now}",
                    OrderProduct = Products.ElementAt(0),
                    OrderDate = new DateTime(2025, 12, 12),
                    OrderClient = Clients.ElementAt(0),
                    Profit = Products.ElementAt(0).Price - Products.ElementAt(0).CostPrice,
                    Quantity = 3
                },
                new Order
                {
                    Id = 2,
                    Name = $"{Clients.ElementAt(1)}: {DateTime.Now}",
                    OrderProduct = Products.ElementAt(1),
                    OrderDate = new DateTime(2024, 11, 12),
                    OrderClient = Clients.ElementAt(1),
                    Profit = Products.ElementAt(1).Price - Products.ElementAt(1).CostPrice,
                    Quantity = 2
                });
            this.SaveChanges(); */
        }
        //Устанавливает параметры подключения и создания БД

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Файл базы данных - в данном случае shop.db
            optionsBuilder.UseSqlite("DataSource = shop.db");
        }
        #endregion

        #region Реализация интерфейса IRepository
        #region Добавить элемент
        //Добавить нового клиента
        public bool AddClient(Client client)
        {
            //Добавить в таблицу
            Clients.Add(client);
            //Сохранить изменения
            var res = this.SaveChanges();
            if (res > 0) return true;
            return false;
        }
        public bool AddOrder(Order order)
        {
            Orders.Add(order);
            var res = this.SaveChanges();
            if (res > 0) return true;
            return false;
        }
        public bool AddProduct(Product product)
        {
            Products.Add(product);
            var res = this.SaveChanges();
            if (res > 0) return true;
            return false;

        }
        #endregion

        #region Удалить элемент
        public bool RemoveProduct(Product product)
        {
            //Удалить элемент из таблицы
            Products.Remove(product);
            var res = this.SaveChanges();
            if (res > 0) return true;
            return false;
        }

        public bool RemoveClient(Client client)
        {
            Clients.Remove(client);
            var res = this.SaveChanges();
            if (res > 0) return true;
            return false;
        }

        public bool RemoveOrder(Order order)
        {
            Orders.Remove(order);
            var res = this.SaveChanges();
            if (res > 0) return true;
            return false;
        }
        #endregion

        #region Получить элемент по Id


        public Product GetProduct(int productId)
        {
            //Linq+Лямбда выражение получитm первый элемент в коллекции с productId
            //Если элементов нет вернуть null
            return Products.FirstOrDefault(p => p.Id == productId);
        }

        public Order GetOrder(int orderId)
        {
            return Orders.FirstOrDefault(p => p.Id == orderId);
        }

        public Client GetClient(int clientId)
        {
            return Clients.FirstOrDefault(p => p.Id == clientId);
        }
        #endregion

        #region Обновить элемент
        public bool UpdateProduct(Product product)
        {
            //Обновить элемент в коллекции
            Products.Update(product);
            var result = this.SaveChanges();
            if (result > 0) return true;
            return false;
        }

        public bool UpdateClient(Client client)
        {
            Clients.Update(client);
            var result = this.SaveChanges();
            if (result > 0) return true;
            return false;
        }

        public bool UpdateOrder(Order order)
        {
            Orders.Update(order);
            var result = this.SaveChanges();
            if (result > 0) return true;
            return false;
        }
        #endregion

        #region Фильтрация и сортировка таблиц
        public ObservableCollection<Product> GetProducts(int sort = 0)
        {
            if (sort == 0)
                return new ObservableCollection<Product>(Products.OrderBy(p => p.Id));
            else if (sort == 1)
                return new ObservableCollection<Product>(Products.OrderBy(p => p.Name));
            else
                return new ObservableCollection<Product>(Products.OrderBy(p => p.Price));
        }


        public ObservableCollection<Client> GetClients(int sort = 0)
        {
            if (sort == 0)
                return new ObservableCollection<Client>(Clients.OrderBy(p => p.Id));
            else if (sort == 1)
                return new ObservableCollection<Client>(Clients.OrderBy(p => p.Name));
            else
                return new ObservableCollection<Client>(Clients.OrderBy(p => p.IsPerson));
        }

        public ObservableCollection<Order> GetOrders(int sort = 0)
        {
            if (sort == 0)
                return new ObservableCollection<Order>(Orders.OrderBy(p => p.Id));
            else if (sort == 1)
                return new ObservableCollection<Order>(Orders.OrderBy(p => p.Name));
            else
                return new ObservableCollection<Order>(Orders.OrderBy(p => p.OrderDate));
        }

        //Получить заказы по дате
        public ObservableCollection<Order> GetOrdersByDate(DateTime dateTime)
        {
            return new ObservableCollection<Order>(Orders.Where(p => p.OrderDate.Date == dateTime.Date));
        }
        #endregion
        #endregion
    }
}
