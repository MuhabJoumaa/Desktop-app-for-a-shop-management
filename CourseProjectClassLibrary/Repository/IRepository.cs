using CourseProjectClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectClassLibrary.Repository
{
    //Интерфейс хранилища, необходим для создания гибкого приложения
    //Класс реализующий интерфейс в приложении можно изменить
    public interface IRepository
    {
        #region Методы  возрщают коллекцию объектов в порядке определяемом значеним параметра сортировки
        //ObservableCollection - коллекция с уведомленим об изменении
        ObservableCollection<Product> GetProducts(int sort = 0);
        ObservableCollection<Client> GetClients(int sort = 0);
        ObservableCollection<Order> GetOrders(int sort = 0);
        #endregion

        #region Добавление элементов в коллекцию
        bool AddProduct(Product product);
        bool AddClient(Client client);
        bool AddOrder(Order order);
        #endregion

        #region Обновление элементов коллекции
        bool UpdateProduct(Product product);
        bool UpdateClient(Client client);
        bool UpdateOrder(Order order);
        #endregion

        #region Удаление элементов коллекции
        bool RemoveProduct(Product product);
        bool RemoveClient(Client client);
        bool RemoveOrder(Order order);
        #endregion

        #region Получить элемент по Id
        Product GetProduct(int productId);
        Order GetOrder(int orderId);
        Client GetClient(int clientId);
        #endregion

        //Получить заказы по дате
        ObservableCollection<Order> GetOrdersByDate(DateTime dateTime);
    }
}
