// See https://aka.ms/new-console-template for more information

EcommerceContext db = new EcommerceContext();
List<Product> products = db.Products.ToList<Product>();
if (products.Count == 0)
    StartProduct();
bool exit = false;
do
{
    Console.WriteLine("Benvenuto, sei un cliente o un dipendente?");
    string tipo = Console.ReadLine();
    if (tipo == "cliente")
    {
        Console.WriteLine("Sei un nuovo cliente?");
        string newCustomer = Console.ReadLine();
        if (newCustomer == "si")
            NewCustomer();
        ManagementCustomer();
    }
    else if (tipo != "exit")
    {
        Console.WriteLine("Sei un nuovo dipendete?");
        string newEmployee = Console.ReadLine();
        if (newEmployee == "si")
            NewEmployee();
        ManagementEmployee();
    }
    else
        exit = true;
} while (!exit);

void ManagementCustomer()
{
    List<Order> ordes = db.Orders.Where(o => o.Status == "Disponibile").ToList<Order>();
    if (ordes.Count == 0)
    {
        Console.WriteLine("Non ci sono ordini da comprare");
        return;
    }
    Console.WriteLine("Inserisci la tua email con cui sei registrato");
    string email = Console.ReadLine();
    Customer customer = db.Customers.Where( c => c.Email == email ).FirstOrDefault<Customer>();
    if (customer == null)
    {
        Console.WriteLine("Cliente non trovato");
        return;
    }
    foreach (Order order in ordes)
    {
        Console.WriteLine("Ordine numero: {0}, Prezzo: {1}", order.Id, order.Amount);
    }
    Console.WriteLine("Digita il numero dell'ordine che voui comprare");
    int idOrder = Convert.ToInt32(Console.ReadLine());
    Order myOrder = ordes.Where(c => c.Id == idOrder).First<Order>();
    NewPayment(customer,myOrder);
}

void NewCustomer()
{
    Console.WriteLine("Inserisci il nome del nuovo cliente");
    string name = Console.ReadLine();
    Console.WriteLine("Inserisci il cognome del nuovo cliente");
    string lastname = Console.ReadLine();
    Console.WriteLine("Inserisci la email del nuovo cliente");
    string email = Console.ReadLine();
    Customer customer = new Customer() { Name = name, Surname = lastname, Email = email };
    db.Customers.Add(customer);
    db.SaveChanges();
}

void ManagementEmployee()
{
    Console.WriteLine("Inserisci il tuo nome");
    string name = Console.ReadLine();
    Employee employee = db.Employees.Where(e => e.Name == name).FirstOrDefault<Employee>();
    if (employee == null)
    {
        Console.WriteLine("Dipendente non trovato");
        return;
    }
    Console.WriteLine("Scegli cosa fare:");
    Console.WriteLine("1) Creare un nuovo ordine");
    Console.WriteLine("2) Modificare un ordine");
    Console.WriteLine("3) Eliminare un ordine");
    Console.WriteLine("4) Spedisci ordini comprati");
    string scelta = Console.ReadLine();
    switch (scelta)
    {
        case "1":
            NewOrder(employee);
            break;
        case "2":
            Order order2 = SearchOrder();
            if(order2 != null)
                EditOrder(employee, order2);
            break;
        case "3":
            Order order3 = SearchOrder();
            if (order3 != null)
                DeleteOrder(order3);
            break;
        case "4":
            SendOrder(employee);
            break;
        default:
            break;
    }
}

void NewEmployee()
{
    Console.WriteLine("Inserisci il nome del nuovo dipendete");
    string name = Console.ReadLine();
    Console.WriteLine("Inserisci il cognome del nuovo dipendente");
    string lastname = Console.ReadLine();
    Employee employee = new Employee() { Name = name, Surname = lastname };
    db.Employees.Add(employee);
    db.SaveChanges();
}

void StartProduct()
{
    Product product1 = new Product() { Name = "Prodotto1", Description = "Prodotto molto bello ed utile", Price = 10.99};
    Product product2 = new Product() { Name = "Prodotto2", Description = "Prodotto molto bello ed utile", Price = 11.99 };
    Product product3 = new Product() { Name = "Prodotto3", Description = "Prodotto molto bello ed utile", Price = 12.99 };
    Product product4 = new Product() { Name = "Prodotto4", Description = "Prodotto molto bello ed utile", Price = 9.99 };
    Product product5 = new Product() { Name = "Prodotto5", Description = "Prodotto molto bello ed utile", Price = 5.99 };
    Product product6 = new Product() { Name = "Prodotto6", Description = "Prodotto molto bello ed utile", Price = 13.99 };
    Product product7 = new Product() { Name = "Prodotto7", Description = "Prodotto molto bello ed utile", Price = 22.99 };
    Product product8 = new Product() { Name = "Prodotto8", Description = "Prodotto molto bello ed utile", Price = 10 };
    Product product9 = new Product() { Name = "Prodotto9", Description = "Prodotto molto bello ed utile", Price = 4 };
    Product product10 = new Product() { Name = "Prodotto10", Description = "Prodotto molto bello ed utile", Price = 24.50 };
    db.Products.Add(product1);
    db.Products.Add(product2);
    db.Products.Add(product3);
    db.Products.Add(product4);
    db.Products.Add(product5);
    db.Products.Add(product6);
    db.Products.Add(product7);
    db.Products.Add(product8);
    db.Products.Add(product9);
    db.Products.Add(product10);
    db.SaveChanges();
}

void NewOrder(Employee employee)
{
    List<Product> products = db.Products.ToList<Product>();
    Order order = new Order();
    order.Employee = employee;
    order.Products = new List<Product>();
    order.Amount = 0;
    order.Status = "Disponibile";
    bool succes = false;
    foreach(Product product in products)
    {
        Console.WriteLine("Nome: {0}, prezzo: {1}",product.Name,product.Price);
        Console.WriteLine("Vuoi aggiungere questo prodotto all'ordine?");
        string addProduct = Console.ReadLine();
        if (addProduct == "si")
        {
            order.Products.Add(product);
            order.Amount += product.Price;
            succes = true;
        }
    }
    if (succes)
    {
        db.Orders.Add(order);
        db.SaveChanges();
    }
}

void NewPayment(Customer customer, Order order)
{
    int succes;
    do
    {
        Random rand = new Random();
        succes = rand.Next(2);
        Payment payment = new Payment();
        payment.Amount = order.Amount;
        payment.Order = order;
        if(succes == 1)
            payment.Status = true;
        else
            payment.Status = false;
        order.Customer = customer;
        order.Status = "Comprato";
        db.Payments.Add(payment);
        db.SaveChanges();
    } while (succes == 0);
    Console.WriteLine("Ordine acquistato con successo!");
}

void EditOrder(Employee employee, Order order)
{
    order.Employee = employee;
    Console.WriteLine("L'ordine ha {0} prodotti al suo interno",order.Products.Count);
    Console.WriteLine("Vuoi aggiungere un prodotto o eliminare un prodotto dall'ordine?(add/remove)");
    string addRemove = Console.ReadLine();
    if (addRemove == "remove")
    {
        foreach (Product product in order.Products)
        {
            Console.WriteLine("Nome: {0}, prezzo: {1}", product.Name, product.Price);
            Console.WriteLine("Vuoi rimuovere questo prodotto all'ordine?");
            string addProduct = Console.ReadLine();
            if (addProduct == "si")
            {
                order.Products.Remove(product);
                order.Amount -= product.Price;
            }
        }
        if (order.Products.Count == 0)
            DeleteOrder(order);
    } else
    {
        List<Product> products = db.Products.ToList<Product>();
        foreach (Product product in products)
        {
            if (!order.Products.Contains(product))
            {
                Console.WriteLine("Nome: {0}, prezzo: {1}", product.Name, product.Price);
                Console.WriteLine("Vuoi aggiungere questo prodotto all'ordine?");
                string addProduct = Console.ReadLine();
                if (addProduct == "si")
                {
                    order.Products.Add(product);
                    order.Amount += product.Price;
                }
            }
        }
    }
    db.SaveChanges();
}

void DeleteOrder(Order order)
{
    db.Orders.Remove(order);
    db.SaveChanges();
}

Order SearchOrder()
{
    List<Order> ordes = db.Orders.Where(o => o.Status == "Disponibile").ToList<Order>();
    if (ordes.Count == 0)
    {
        Console.WriteLine("Non ci sono ordini presenti");
        return null;
    }
    foreach (Order order in ordes)
    {
        Console.WriteLine("Ordine numero: {0}, Prezzo: {1}", order.Id, order.Amount);
    }
    Console.WriteLine("Digita il numero dell'ordine che voui selezionare");
    int idOrder = Convert.ToInt32(Console.ReadLine());
    Order myOrder = ordes.Where(c => c.Id == idOrder).First<Order>();
    return myOrder;
}

void SendOrder(Employee employee)
{
    List<Order> ordes = db.Orders.Where(o => o.Status == "Comprato").ToList<Order>();
    if (ordes.Count == 0)
    {
        Console.WriteLine("Non ci sono ordini da spedire");
        return;
    }
    foreach (Order order in ordes)
    {
        Console.WriteLine("Ordine numero: {0}, Prezzo: {1}", order.Id, order.Amount);
        Console.WriteLine("Vuoi spedire questo ordine?");
        string send = Console.ReadLine();
        if (send == "si")
        {
            order.Status = "Spedito";
        }
    }
    db.SaveChanges();
}