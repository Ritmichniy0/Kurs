using System;
using System.Collections.Generic;
//---------------------------------------------1---------------------------------------------------------
// Базовый класс "ГорячийНапиток"
class ГорячийНапиток
{
    public string Name { get; set; }
    public int Volume { get; set; }

    public ГорячийНапиток(string name, int volume)
    {
        Name = name;
        Volume = volume;
    }
}

// Наследник "ГорячийНапиток" с полем "Температура"
class ГорячийНапитокСТемпературой : ГорячийНапиток
{
    public int Temperature { get; set; }

    public ГорячийНапитокСТемпературой(string name, int volume, int temperature) : base(name, volume)
    {
        Temperature = temperature;
    }
}
//---------------------------------------------2---------------------------------------------------------
// Интерфейс "ТорговыйАвтомат"
interface ТорговыйАвтомат
{
    ГорячийНапиток getProduct(string name, int volume, int temperature);
}

// Реализация "ГорячихНапитковАвтомат"
class ГорячихНапитковАвтомат : ТорговыйАвтомат
{
    private List<ГорячийНапиток> beverages;

    public ГорячихНапитковАвтомат()
    {
        beverages = new List<ГорячийНапиток>();
    }

    public void addProduct(ГорячийНапиток beverage)
    {
        beverages.Add(beverage);
    }

    public ГорячийНапиток getProduct(string name, int volume, int temperature)
    {
        foreach (var beverage in beverages)
        {
            if (beverage.name == name && beverage.volume == volume && beverage is ГорячийНапитокСТемпературой)
            {
                var напитокСТемпературой = (ГорячийНапитокСТемпературой)beverage;
                if (напитокСТемпературой.temperature == temperature)
                {
                    return напитокСТемпературой;
                }
            }
        }
        return null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ГорячихНапитковАвтомат автомат = new ГорячихНапитковАвтомат();

        ГорячийНапитокСТемпературой чай = new ГорячийНапитокСТемпературой("Чай", 200, 80);
        ГорячийНапитокСТемпературой кофе = new ГорячийНапитокСТемпературой("Кофе", 250, 90);

        автомат.addProduct(чай);
        автомат.addProduct(кофе);

        string name = "Чай";
        int volume = 200;
        int temperature = 80;

        ГорячийНапиток выбранныйНапиток = автомат.getProduct(name, volume, temperature);
        
        if (выбранныйНапиток != null)
        {
            Console.WriteLine($"Возьмите ваш {выбранныйНапиток.Name}");
        }
        else
        {
            Console.WriteLine("Такого напитка в автомате нет.");
        }
    }
}
