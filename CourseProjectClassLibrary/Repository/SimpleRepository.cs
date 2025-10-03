using CourseProjectClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseProjectClassLibrary.Repository
{
    //Тестовое хранилище в памяти
    public class SimpleRepository : IRepository
    {
        //Коллекции имитируют БД
        ObservableCollection<Product> products = new ObservableCollection<Product>()
        {
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
            }
        };
        ObservableCollection<Client> clients = new ObservableCollection<Client>()
        {
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
                Id =3,
                Name = "Sidorov",
                FirstName = "Sidor",
                MiddleName = "Sidorovich",
                Address = "Peter street 32",
                Description = "Big Boss",
                IsPerson= true
            }
        };
        ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public SimpleRepository()
        {
            Product pr = products.ElementAt(0);
            orders.Add(
                new Order
                {
                    Id = 1,
                    OrderProduct = pr,
                    OrderDate = DateTime.Now,
                    OrderClient = clients[0],
                    Profit = pr.Price - pr.CostPrice
                });

            orders.Add(
                new Order
                {
                    Id = 2,
                    OrderProduct = pr,
                    OrderDate = DateTime.Now,
                    OrderClient = clients[0],
                    Profit = pr.Price - pr.CostPrice
                });
        }


        public ObservableCollection<Client> GetClients(int sort = 0)
        {
            return new ObservableCollection<Client>(clients);
        }
        public ObservableCollection<Order> GetOrders(int sort = 0)
        {
            return orders;
        }
        public ObservableCollection<Product> GetProducts(int sort = 0)
        {
            return products;
        }

        public bool AddClient(Client client)
        {
            throw new NotImplementedException();
        }
        public bool AddOrder(Order order)
        {
            throw new NotImplementedException();
        }
        public bool AddProduct(Product product)
        {
            product.Id = products.OrderBy(p => p.Id).Last().Id + 1;
            products.Add(product);
            return true;
        }

        public bool RemoveProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public bool RemoveClient(Client client)
        {
            throw new NotImplementedException();
        }
        public bool RemoveOrder(Order order)
        {
            throw new NotImplementedException();
        }


        public Product GetProduct(int productId)
        {
            throw new NotImplementedException();
        }
        public Order GetOrder(int orderId)
        {
            throw new NotImplementedException();
        }
        public Client GetClient(int clientId)
        {
            throw new NotImplementedException();
        }


        public bool UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public bool UpdateClient(Client client)
        {
            throw new NotImplementedException();
        }
        public bool UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Order> GetOrdersByDate(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
