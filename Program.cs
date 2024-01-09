using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Program
    {
        class Person
        {
            public string FIO { get; set; }
            public object ID { get; set; }
        }

        class Employee : Person
        {
            public string PhoneNumber { get; set; }
            public string Department { get; set; }
        }

        class Client : Person
        {
            public decimal Balance { get; set; }
            public List<decimal> Accounts { get; set; }

            public Client()
            {
                Accounts = new List<decimal> { 0 }; 
            }
        }

        static List<Employee> employees = new List<Employee>();
        static List<Client> clients = new List<Client>();

        static void Main(string[] args)
        {
            
            employees.Add(new Employee { FIO = "Иванов Иван", ID = 123456, PhoneNumber = "111-222-3333", Department = "бухгалтерия" });
            employees.Add(new Employee { FIO = "Петров Петр", ID = 789012, PhoneNumber = "444-555-6666", Department = "охрана" });

            clients.Add(new Client { FIO = "Сидоров Сидор", ID = "ABCD1234", Balance = 100 });
            clients.Add(new Client { FIO = "Иванова Анна", ID = "EFGH5678", Balance = 50 });

            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Найти клиента");
                Console.WriteLine("2. Добавить клиента");
                Console.WriteLine("3. Добавить счет клиенту");
                Console.WriteLine("4. Управление счетами клиента");
                Console.WriteLine("5. Удалить клиента");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        FindClient();
                        break;
                    case 2:
                        AddClient();
                        break;
                    case 3:
                        AddAccountToClient();
                        break;
                    case 4:
                        ManageClientAccounts();
                        break;
                    case 5:
                        RemoveClient();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                    
                }
            }
        }

        static void FindClient()
        {
            Console.Write("Введите имя клиента для поиска: ");
            string name = Console.ReadLine();

            var foundClients = clients.Where(c => c.FIO.Contains(name)).ToList();

            if (foundClients.Any())
            {
                foreach (var client in foundClients)
                {
                    Console.WriteLine($"Найден клиент: {client.FIO}, ID: {client.ID}");
                }
                
            }
            else
            {
                Console.WriteLine("Клиент не найден. Хотите зарегистрировать нового клиента? (да/нет)");
                string response = Console.ReadLine().ToLower();

                if (response == "да")
                {
                    AddClient();
                }
            }
        }

        static void AddClient()
        {
            Console.Write("Введите имя нового клиента: ");
            string name = Console.ReadLine();

            Console.Write("Введите ID нового клиента: ");
            string id = Console.ReadLine();

         

            var newClient = new Client { FIO = name, ID = id };
            clients.Add(newClient);

            Console.WriteLine("Новый клиент успешно зарегистрирован.");
        }

        static void AddAccountToClient()
        {
            Console.Write("Введите ID клиента, для которого добавить счет: ");
            string id = Console.ReadLine();

            var client = clients.FirstOrDefault(c => c.ID.ToString() == id);

            if (client != null)
            {
                if (client.Accounts.Count < 3)
                {
                    client.Accounts.Add(0); 
                    Console.WriteLine("Новый счет успешно добавлен.");
                }
                else
                {
                    Console.WriteLine("У клиента уже есть максимальное количество счетов.");
                }
            }
            else
            {
                Console.WriteLine("Клиент не найден.");
            }
        }

        static void ManageClientAccounts()
        {

            Console.Write("Введите ID клиента, у которого хотите управлять счетами: ");
            string id = Console.ReadLine();

            var client = clients.FirstOrDefault(c => c.ID.ToString() == id);

            if (client != null)
            {
                Console.WriteLine($"Выберите действие для клиента {client.FIO}:");
                Console.WriteLine("1. Добавить деньги на счет");
                Console.WriteLine("2. Снять деньги со счета");
                Console.WriteLine("3. Завести новый счет");
                Console.Write("Выберите действие: ");

                int action = int.Parse(Console.ReadLine());

                switch (action)
                {
                    case 1:
                        AddMoneyToAccount(client);
                        break;
                    case 2:
                        WithdrawMoneyFromAccount(client);
                        break;
                    case 3:
                        AddAccountToClient();
                        break;
                    
                }
            }
            else
            {
                Console.WriteLine("Клиент не найден.");
            }
        }

        static void AddMoneyToAccount(Client client)
        {
            Console.Write("Выберите счет для пополнения (от 1 до 3): ");
            int accountNumber = int.Parse(Console.ReadLine());

            if (accountNumber >= 1 && accountNumber <= client.Accounts.Count)
            {
                Console.Write("Введите сумму для пополнения: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                client.Accounts[accountNumber - 1] += amount;
                Console.WriteLine($"Счет {accountNumber} успешно пополнен на {amount}.");
            }
            else
            {
                Console.WriteLine("Неверный номер счета.");
            }
        }

        static void WithdrawMoneyFromAccount(Client client)
        {
            Console.Write("Выберите счет для снятия денег (от 1 до 3): ");
            int accountNumber = int.Parse(Console.ReadLine());

            if (accountNumber >= 1 && accountNumber <= client.Accounts.Count)
            {
                Console.Write("Введите сумму для снятия: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                if (client.Accounts[accountNumber - 1] >= amount)
                {
                    client.Accounts[accountNumber - 1] -= amount;
                    Console.WriteLine($"Со счета {accountNumber} снято {amount}.");
                }
                else
                {
                    Console.WriteLine("Недостаточно средств на счете.");
                }
            }
            else
            {
                Console.WriteLine("Неверный номер счета.");
            }
        }

        static void RemoveClient()
        {
            Console.Write("Введите ID клиента, которого нужно удалить: ");
            string id = Console.ReadLine();

            var client = clients.FirstOrDefault(c => c.ID.ToString() == id);

            if (client != null)
            {
                if (client.Accounts.Any(acc => acc > 0))
                {
                    Console.WriteLine("На счетах клиента есть деньги. их надо снять.");
                }
                else
                {
                    clients.Remove(client);
                    Console.WriteLine("Клиент успешно удален.");
                }
            }
            else
            {
                Console.WriteLine("Клиент не найден.");
            }
        }
    }
}
