using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectClassLibrary.Models
{
    //Класс товар
    public class Product : Subject
    {
        //Категория
        public string? Category {  get; set; }
        //Цена
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        //[PrimaryKey]
        //Имеемое количество
        public double Quantity { get; set; }
        //Переопределенный метод определения эквивалентности объектов Product
        public override bool Equals(object? obj)
        {
            //Определение эквивалентоности по Id
            return this.Id == (obj as Product)?.Id;
        }
        //Переопределение метода преобразования класса в строку
        public override string ToString()
        {
            return $"{Name}, {Price}р, {CostPrice}р, id:{Id} ";
        }
        //Можно не переопределять - используется в словарях
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
