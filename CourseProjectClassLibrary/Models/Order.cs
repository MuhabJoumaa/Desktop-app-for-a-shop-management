using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectClassLibrary.Models
{
    //Класс - заказ
    public class Order : Subject
    {
        //Дата заказа
        public DateTime OrderDate { get; set; }
        //Количество элементов в заказе
        public double Quantity { get; set; }
        //Заказанный продукт
        public Product OrderProduct { get; set; }
        //Клиент сделавший заказ
        public Client OrderClient { get; set; }
        public decimal Profit { get; set; }
        //Переопределение метода установления эквивалентности объектов 
        public override bool Equals(object? obj)
        {
            return this.Id == (obj as Order)?.Id;
        }
        //Переопределение метода преобразования класса в строку
        public override string ToString()
        {
            return $"{OrderClient.Name} - {OrderProduct.Name} : {Quantity} / {OrderDate.Date.ToString("D")}, id:{Id}";
        }
        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
