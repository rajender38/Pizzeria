using System;
using System.Collections.Generic;
using System.Linq;

namespace LOR.Pizzeria
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Store> storeList = new List<Store>();
            storeList.Add(new Store() { StoreId = 1, Name = "Brisbane" });
            storeList.Add(new Store() { StoreId = 2, Name = "Sydney" });
            storeList.Add(new Store() { StoreId = 3, Name = "Gold Coast" });

            List<Pizza> pizzaList = new List<Pizza>();
            pizzaList.Add(new Pizza()
            {
                PizzaId = 1,
                Name = "Capriciosa",
                Ingredients = new List<string> { "mushrooms", "cheese", "ham", "mozarella" },
                Bake = "Baking pizza for 15 minutes at 200 degrees...",
                CutPiecesCount = 6
            });
            pizzaList.Add(new Pizza()
            {
                PizzaId = 2,
                Name = "Florenza",
                Ingredients = new List<string> { "olives", "pastrami", "mozarella", "onion" },
                Bake = "Baking pizza for 15 minutes at 300 degrees...",
                CutPiecesCount = 8
            });
            pizzaList.Add(new Pizza()
            {
                PizzaId = 3,
                Name = "Margherita",
                Ingredients = new List<string> { "mozarella", "onion", "garlic", "oregano" },
                Bake = "Baking pizza for 20 minutes at 200 degrees...",
                CutPiecesCount = 6
            });
            pizzaList.Add(new Pizza()
            {
                PizzaId = 4,
                Name = "Inferno",
                Ingredients = new List<string> { "chili peppers", "mozzarella", "chicken", "cheese" },
                Bake = "Baking pizza for 15 minutes at 250 degrees...",
                CutPiecesCount = 8
            });

            List<StorePizza> storePizzaList = new List<StorePizza>();
            storePizzaList.Add(new StorePizza() { StorePizzaId = 1, StoreId = 1, PizzaId = 1, Cost = 20.50, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 2, StoreId = 1, PizzaId = 2, Cost = 18.50, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 3, StoreId = 1, PizzaId = 3, Cost = 22, Quantity = 3 });

            storePizzaList.Add(new StorePizza() { StorePizzaId = 4, StoreId = 2, PizzaId = 1, Cost = 20.50, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 5, StoreId = 2, PizzaId = 3, Cost = 20, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 6, StoreId = 2, PizzaId = 4, Cost = 18.50, Quantity = 3 });

            storePizzaList.Add(new StorePizza() { StorePizzaId = 7, StoreId = 3, PizzaId = 1, Cost = 20.50, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 8, StoreId = 3, PizzaId = 2, Cost = 15.50, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 9, StoreId = 3, PizzaId = 3, Cost = 12.50, Quantity = 3 });
            storePizzaList.Add(new StorePizza() { StorePizzaId = 10, StoreId = 3, PizzaId = 4, Cost = 43.50, Quantity = 3 });

            Cart cart = new Cart();
            List<StorePizza> storePizzaListCart = new List<StorePizza>();

            bool trySelectStore = true;
            do
            {

                Console.WriteLine("Welcome to LOR Pizzeria! Please select the store location: Brisbane OR Sydney OR Gold Coast");
                var storeInput = Console.ReadLine();

                Console.WriteLine("MENU");
                var storeDetails = storeList.Where(s => s.Name.ToUpper() == storeInput.ToUpper()).FirstOrDefault();
                if (storeDetails != null)
                {
                    trySelectStore = false;
                    bool addPizza = true;
                    do
                    {
                        var getStorePizza = storePizzaList.Where(s => s.StoreId == storeDetails.StoreId);
                        foreach (StorePizza storePizza in getStorePizza)
                        {
                            var pizzaDetails = pizzaList.Where(s => s.PizzaId == storePizza.PizzaId).FirstOrDefault();
                            Console.WriteLine(pizzaDetails.Name + " - " + string.Join(", ", pizzaDetails.Ingredients) + " - " + storePizza.Cost);
                        }
                        Console.WriteLine("What can I get you?");
                        var pizzaType = Console.ReadLine();
                        var storePizzaDetails = getStorePizza.Where(s => s.PizzaId == (pizzaList.Where(s => s.Name.ToUpper() == pizzaType.ToUpper()).FirstOrDefault().PizzaId)).FirstOrDefault();
                        if (storePizzaDetails != null)
                        {
                            storePizzaListCart.Add(storePizzaDetails);
                            cart.StorePizza = storePizzaListCart;
                            cart.TotalCost = cart.TotalCost + storePizzaDetails.Cost;
                        }
                        Console.WriteLine("Do you want to add another Pizza? Reply 'Yes' else enter any key to continue.");
                        addPizza = Console.ReadLine().ToUpper() == "Yes".ToUpper() ? true : false;
                    } while (addPizza);

                    foreach (StorePizza cartItem in cart.StorePizza)
                    {
                        var cartPizza = pizzaList.Where(s => s.PizzaId == cartItem.PizzaId).FirstOrDefault();
                        cartPizza.Prepare();
                        cartPizza.BakeProcess();
                        cartPizza.Cut();
                        cartPizza.Box();

                    }
                    cart.PrintReceipt();
                    Console.WriteLine("\nYour pizza is ready!");

                }
                else
                {
                    Console.WriteLine("Selected store does not exists, do you want to try again? Reply 'Yes' else enter any key to Exit.");
                    trySelectStore  = Console.ReadLine().ToUpper() == "Yes".ToUpper() ? true : false; ;
                }

            } while (trySelectStore);
        }


    }

    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
    }

    public class StorePizza
    {
        public int StorePizzaId { get; set; }
        public int StoreId { get; set; }
        public int PizzaId { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }


    }

    public class Cart
    {
        public List<StorePizza> StorePizza { get; set; }
        public double TotalCost { get; set; }
        public void PrintReceipt()
        {
            Console.WriteLine("\nTotal price: " + TotalCost);
        }

    }

    public class Pizza
    {
        public int PizzaId { get; set; }
        public string Name { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        //public decimal BasePrice { get; set; }
        public string Bake { get; set; }
        public int CutPiecesCount { get; set; }


        public void Prepare()
        {
            Console.WriteLine("\nPreparing " + Name + "...");
            Console.Write("Adding ");
            foreach (var i in Ingredients)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }

        public void BakeProcess()
        {

            Console.WriteLine(Bake);
        }

        public void Cut()
        {

            Console.WriteLine($"Pizza is cut into {CutPiecesCount} Pieces");

        }

        public void Box()
        {
            Console.WriteLine("Putting pizza into a nice box...");
        }



    }
}
