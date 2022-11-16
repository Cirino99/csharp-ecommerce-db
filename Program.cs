// See https://aka.ms/new-console-template for more information

EcommerceContext db = new EcommerceContext();
List<Product> products = db.Products.ToList<Product>();
if (products.Count == 0)
    StartProduct();
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
else
{
    Console.WriteLine("Sei un nuovo dipendete?");
    string newEmployee = Console.ReadLine();
    if (newEmployee == "si")
        NewEmployee();
    ManagementEmployee();
}

void ManagementCustomer()
{
    List<Order> ordes = db.Orders.ToList<Order>();
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
    NewOrder(employee);
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
    order.Status = true;
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