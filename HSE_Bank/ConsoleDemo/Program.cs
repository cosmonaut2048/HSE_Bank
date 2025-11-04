using HSE_Bank.Application.Facades;
using HSE_Bank.Application.Seeds;
using HSE_Bank.CompositionRoot;
using HSE_Bank.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank;

class Program
{
    private static AccountsFacade _accountsFacade = null!;
    private static OperationsFacade _operationsFacade = null!;
    private static CategoriesFacade _categoriesFacade = null!;
    private static AnalyticsFacade _analyticsFacade = null!;
    private static bool _running = true;

    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        // Инициализируем DI контейнер
        var serviceProvider = Bootstrapper.Build();
        
        _accountsFacade = serviceProvider.GetRequiredService<AccountsFacade>();
        _operationsFacade = serviceProvider.GetRequiredService<OperationsFacade>();
        _categoriesFacade = serviceProvider.GetRequiredService<CategoriesFacade>();
        _analyticsFacade = serviceProvider.GetRequiredService<AnalyticsFacade>();
        
        // Загружаем данные для демо
        var demoBuilder = serviceProvider.GetRequiredService<DemoDataBuilder>();
        demoBuilder.Seed();
        
        while (_running)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine() ?? "";
            await HandleMenuChoice(choice);
        }
        
        Console.WriteLine("\n✔ Спасибо за использование Банка ВШЭ!");
    }

    private static void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   Банк ВШЭ:  Модуль Учета Финансов         ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");
        
        Console.WriteLine("▶  ГЛАВНОЕ МЕНЮ");
        Console.WriteLine("1  Управление счетами");
        Console.WriteLine("2  Управление категориями");
        Console.WriteLine("3  Добавить операцию");
        Console.WriteLine("4  Просмотреть все счеты");
        Console.WriteLine("5  Просмотреть все операции");
        Console.WriteLine("6  Аналитика");
        Console.WriteLine("0  Выход");
        Console.Write("\n-> Выберите пункт: ");
    }

    private static async Task HandleMenuChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                await HandleAccountsMenu();
                break;
            case "2":
                HandleCategoriesMenu();
                break;
            case "3":
                await HandleAddOperationMenu();
                break;
            case "4":
                DisplayAllAccounts();
                break;
            case "5":
                DisplayAllOperations();
                break;
            case "6":
                await HandleAnalyticsMenu();
                break;
            case "0":
                _running = false;
                break;
            default:
                Console.WriteLine("✖ Неверный выбор. Попробуйте снова.");
                WaitForKeyPress();
                break;
        }
    }

    // ========== УПРАВЛЕНИЕ СЧЕТАМИ ==========
    private static async Task HandleAccountsMenu()
    {
        bool inAccountsMenu = true;
        while (inAccountsMenu)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║   УПРАВЛЕНИЕ СЧЕТАМИ                       ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1  Создать новый счет");
            Console.WriteLine("2  Переименовать счет");
            Console.WriteLine("3  Просмотреть все счеты");
            Console.WriteLine("0  Вернуться в главное меню");
            Console.Write("\n-> Выберите действие: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    await CreateNewAccount();
                    break;
                case "2":
                    await RenameAccount();
                    break;
                case "3":
                    DisplayAllAccounts();
                    break;
                case "0":
                    inAccountsMenu = false;
                    break;
                default:
                    Console.WriteLine("✖ Неверный выбор.");
                    WaitForKeyPress();
                    break;
            }
        }
    }

    private static async Task CreateNewAccount()
    {
        Console.Clear();
        Console.WriteLine("▶ СОЗДАНИЕ НОВОГО СЧЕТА\n");
        
        Console.Write("Введите название счета: ");
        string name = Console.ReadLine() ?? "";
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("✖ Название не может быть пустым!");
            WaitForKeyPress();
            return;
        }
        
        Console.Write("Введите начальный баланс (по умолчанию 0): ");
        decimal balance = 0;
        if (decimal.TryParse(Console.ReadLine(), out var parsed))
        {
            balance = parsed;
        }
        
        try
        {
            var accountId = await _accountsFacade.CreateAsync(name, balance);
            Console.WriteLine($"\n✔ Счет успешно создан!");
            Console.WriteLine($"   ID: {accountId}");
            Console.WriteLine($"   Название: {name}");
            Console.WriteLine($"   Начальный баланс: {balance:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка при создании счета: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    private static async Task RenameAccount()
    {
        Console.Clear();
        Console.WriteLine("▶ ПЕРЕИМЕНОВАНИЕ СЧЕТА\n");
        
        var accounts = _accountsFacade.All();
        if (accounts.Count == 0)
        {
            Console.WriteLine("✖ Нет доступных счетов!");
            WaitForKeyPress();
            return;
        }
        
        Console.WriteLine("Доступные счеты:");
        for (int i = 0; i < accounts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {accounts[i].Name} (ID: {accounts[i].Id}, Баланс: {accounts[i].Balance:C})");
        }
        
        Console.Write("\nВыберите номер счета: ");
        if (!int.TryParse(Console.ReadLine(), out int accountIdx) || accountIdx < 1 || accountIdx > accounts.Count)
        {
            Console.WriteLine("✖ Неверный номер счета!");
            WaitForKeyPress();
            return;
        }
        
        var selectedAccount = accounts[accountIdx - 1];
        Console.Write("Введите новое название: ");
        string newName = Console.ReadLine() ?? "";
        
        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("✖ Название не может быть пустым!");
            WaitForKeyPress();
            return;
        }
        
        try
        {
            await _accountsFacade.RenameAsync(selectedAccount.Id, newName);
            Console.WriteLine($"✔ Счет успешно переименован на '{newName}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    private static void DisplayAllAccounts()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   ВСЕ СЧЕТЫ                                ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");
        
        var accounts = _accountsFacade.All();
        
        if (accounts.Count == 0)
        {
            Console.WriteLine("✖ Нет доступных счетов.");
        }
        else
        {
            PrintTable(new[] { "ID", "Название", "Баланс" },
                accounts.Select(a => new[] 
                { 
                    a.Id.ToString()[..8] + "...",
                    a.Name,
                    a.Balance.ToString("C")
                }).ToList());
        }
        
        WaitForKeyPress();
    }

    // ========== УПРАВЛЕНИЕ КАТЕГОРИЯМИ ==========
    private static void HandleCategoriesMenu()
    {
        bool inCategoriesMenu = true;
        while (inCategoriesMenu)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║   УПРАВЛЕНИЕ КАТЕГОРИЯМИ                   ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1  Создать новую категорию");
            Console.WriteLine("2  Просмотреть все категории");
            Console.WriteLine("0  Вернуться в главное меню");
            Console.Write("\n-> Выберите действие: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    CreateNewCategory();
                    break;
                case "2":
                    DisplayAllCategories();
                    break;
                case "0":
                    inCategoriesMenu = false;
                    break;
                default:
                    Console.WriteLine("✖ Неверный выбор.");
                    WaitForKeyPress();
                    break;
            }
        }
    }

    private static void CreateNewCategory()
    {
        Console.Clear();
        Console.WriteLine("▶ СОЗДАНИЕ НОВОЙ КАТЕГОРИИ\n");
        
        Console.Write("Введите название категории (например, 'Еда', 'Зарплата'): ");
        string name = Console.ReadLine() ?? "";
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("✖ Название не может быть пустым!");
            WaitForKeyPress();
            return;
        }
        
        try
        {
            var categoryId = _categoriesFacade.Create(name);
            Console.WriteLine($"\n✔ Категория успешно создана!");
            Console.WriteLine($"   ID: {categoryId}");
            Console.WriteLine($"   Название: {name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка при создании категории: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    private static void DisplayAllCategories()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   ВСЕ КАТЕГОРИИ                            ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");
        
        var categories = _categoriesFacade.All();
        
        if (categories.Count == 0)
        {
            Console.WriteLine("✖ Нет доступных категорий.");
        }
        else
        {
            PrintTable(new[] { "ID", "Название" },
                categories.Select(c => new[] 
                { 
                    c.Id.ToString()[..8] + "...",
                    c.Name
                }).ToList());
        }
        
        WaitForKeyPress();
    }

    // ========== ДОБАВЛЕНИЕ ОПЕРАЦИИ ==========
    private static async Task HandleAddOperationMenu()
    {
        Console.Clear();
        Console.WriteLine("▶ ДОБАВЛЕНИЕ ОПЕРАЦИИ\n");
        
        var accounts = _accountsFacade.All();
        var categories = _categoriesFacade.All();
        
        if (accounts.Count == 0)
        {
            Console.WriteLine("✖ Нет доступных счетов! Сначала создайте счет.");
            WaitForKeyPress();
            return;
        }
        
        if (categories.Count == 0)
        {
            Console.WriteLine("✖ Нет доступных категорий! Сначала создайте категорию.");
            WaitForKeyPress();
            return;
        }
        
        // Выбор счета
        Console.WriteLine("Доступные счеты:");
        for (int i = 0; i < accounts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {accounts[i].Name}");
        }
        
        Console.Write("Выберите счет: ");
        if (!int.TryParse(Console.ReadLine(), out int accountIdx) || accountIdx < 1 || accountIdx > accounts.Count)
        {
            Console.WriteLine("✖ Неверный номер счета!");
            WaitForKeyPress();
            return;
        }
        
        var selectedAccount = accounts[accountIdx - 1];
        
        // Выбор категории
        Console.WriteLine("\nДоступные категории:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].Name}");
        }
        
        Console.Write("Выберите категорию: ");
        if (!int.TryParse(Console.ReadLine(), out int categoryIdx) || categoryIdx < 1 || categoryIdx > categories.Count)
        {
            Console.WriteLine("✖ Неверный номер категории!");
            WaitForKeyPress();
            return;
        }
        
        var selectedCategory = categories[categoryIdx - 1];
        
        // Тип операции
        Console.WriteLine("\nТип операции:");
        Console.WriteLine("1. ↓ Доход");
        Console.WriteLine("2. ↑ Расход");
        Console.Write("Выберите тип: ");
        
        string typeChoice = Console.ReadLine() ?? "";
        OperationType operationType = typeChoice == "1" ? OperationType.Income : OperationType.Expense;
        
        // Сумма
        Console.Write("Введите сумму: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("✖ Неверная сумма!");
            WaitForKeyPress();
            return;
        }
        
        // Дата
        Console.Write("Введите дату (в формате yyyy-MM-dd) или нажмите Enter для сегодня: ");
        DateTime date = DateTime.Today;
        string dateStr = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(dateStr) && !DateTime.TryParse(dateStr, out date))
        {
            Console.WriteLine("✖ Неверный формат даты!");
            WaitForKeyPress();
            return;
        }
        
        // Описание
        Console.Write("Введите описание (необязательно): ");
        string description = Console.ReadLine();
        
        try
        {
            var operationId = await _operationsFacade.AddAsync(
                selectedAccount.Id,
                selectedCategory.Id,
                operationType,
                amount,
                date,
                description);
            
            Console.WriteLine($"\n✔ Операция успешно добавлена!");
            Console.WriteLine($"   ID: {operationId}");
            Console.WriteLine($"   Счет: {selectedAccount.Name}");
            Console.WriteLine($"   Категория: {selectedCategory.Name}");
            Console.WriteLine($"   Тип: {(operationType == OperationType.Income ? "Доход" : "Расход")}");
            Console.WriteLine($"   Сумма: {amount:C}");
            Console.WriteLine($"   Дата: {date:dd.MM.yyyy}");
            if (!string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine($"   Описание: {description}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка при добавлении операции: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    // ========== ПРОСМОТР ОПЕРАЦИЙ ==========
    private static void DisplayAllOperations()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   ВСЕ ОПЕРАЦИИ                             ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");
        
        var operations = _operationsFacade.All();
        
        if (operations.Count == 0)
        {
            Console.WriteLine("✖ Нет операций.");
        }
        else
        {
            PrintTable(new[] { "Дата", "Тип", "Сумма", "Описание" },
                operations.Select(o => new[] 
                { 
                    o.Date.ToString("dd.MM.yyyy"),
                    o.Type == OperationType.Income ? "↓ Доход" : "↑ Расход",
                    o.Amount.ToString("C"),
                    o.Description ?? "—"
                }).ToList());
        }
        
        WaitForKeyPress();
    }

    // ========== АНАЛИТИКА ==========
    private static async Task HandleAnalyticsMenu()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   АНАЛИТИКА                                ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");
        
        Console.WriteLine("1  Сумма операций за период");
        Console.WriteLine("2  Сумма операций по категории");
        Console.WriteLine("3  Проверить аномалии");
        Console.WriteLine("0  Вернуться в главное меню");
        Console.Write("\n-> Выберите действие: ");
        
        string choice = Console.ReadLine() ?? "";
        
        switch (choice)
        {
            case "1":
                await AnalyzePeriod();
                break;
            case "2":
                await AnalyzeCategory();
                break;
            case "3":
                await CheckAnomalies();
                break;
            case "0":
                break;
            default:
                Console.WriteLine("✖ Неверный выбор.");
                WaitForKeyPress();
                break;
        }
    }

    private static async Task AnalyzePeriod()
    {
        Console.Clear();
        Console.WriteLine("▶ АНАЛИЗ ПЕРИОДА\n");
        
        Console.Write("Введите начальную дату (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            Console.WriteLine("✖ Неверный формат даты!");
            WaitForKeyPress();
            return;
        }
        
        Console.Write("Введите конечную дату (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
        {
            Console.WriteLine("✖ Неверный формат даты!");
            WaitForKeyPress();
            return;
        }
        
        try
        {
            var sum = await _analyticsFacade.SumByPeriodAsync(startDate, endDate);
            Console.WriteLine($"\n✔ Результат анализа:");
            Console.WriteLine($"   Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
            Console.WriteLine($"   Итоговая сумма: {sum:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    private static async Task AnalyzeCategory()
    {
        Console.Clear();
        Console.WriteLine("▶ АНАЛИЗ ПО КАТЕГОРИИ\n");
        
        var categories = _categoriesFacade.All();
        
        if (categories.Count == 0)
        {
            Console.WriteLine("✖ Нет доступных категорий!");
            WaitForKeyPress();
            return;
        }
        
        Console.WriteLine("Доступные категории:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].Name}");
        }
        
        Console.Write("Выберите категорию: ");
        if (!int.TryParse(Console.ReadLine(), out int categoryIdx) || categoryIdx < 1 || categoryIdx > categories.Count)
        {
            Console.WriteLine("✖ Неверный номер категории!");
            WaitForKeyPress();
            return;
        }
        
        var selectedCategory = categories[categoryIdx - 1];
        
        try
        {
            var sum = await _analyticsFacade.SumByCategoryAsync(selectedCategory.Id);
            Console.WriteLine($"\n✔ Результат анализа:");
            Console.WriteLine($"   Категория: {selectedCategory.Name}");
            Console.WriteLine($"   Итоговая сумма: {sum:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    private static async Task CheckAnomalies()
    {
        Console.Clear();
        Console.WriteLine("▶ ПРОВЕРКА АНОМАЛИЙ\n");
        
        Console.Write("Введите пороговое значение для аномалии: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal threshold))
        {
            Console.WriteLine("✖ Неверное значение!");
            WaitForKeyPress();
            return;
        }
        
        try
        {
            var hasAnomalies = await _analyticsFacade.HasAnomaliesAsync(threshold);
            Console.WriteLine($"\n✔ Результат проверки:");
            Console.WriteLine($"   Пороговое значение: {threshold:C}");
            Console.WriteLine($"   Обнаружены аномалии: {(hasAnomalies ? "✔ Да" : "✖ Нет")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✖ Ошибка: {ex.Message}");
        }
        
        WaitForKeyPress();
    }

    // ========== УТИЛИТЫ ==========
    private static void PrintTable(string[] headers, List<string[]> rows)
    {
        int[] columnWidths = new int[headers.Length];
        
        // Рассчитываем ширину колонок
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = headers[i].Length;
            foreach (var row in rows)
            {
                if (i < row.Length && row[i].Length > columnWidths[i])
                {
                    columnWidths[i] = row[i].Length;
                }
            }
            columnWidths[i] += 2; // отступ
        }
        
        // Печатаем заголовок
        Console.Write("┌");
        for (int i = 0; i < headers.Length; i++)
        {
            Console.Write(new string('─', columnWidths[i]));
            Console.Write(i < headers.Length - 1 ? "┬" : "┐");
        }
        Console.WriteLine();
        
        Console.Write("│");
        for (int i = 0; i < headers.Length; i++)
        {
            Console.Write(headers[i].PadRight(columnWidths[i]));
            Console.Write("│");
        }
        Console.WriteLine();
        
        // Разделитель
        Console.Write("├");
        for (int i = 0; i < headers.Length; i++)
        {
            Console.Write(new string('─', columnWidths[i]));
            Console.Write(i < headers.Length - 1 ? "┼" : "┤");
        }
        Console.WriteLine();
        
        // Печатаем строки
        foreach (var row in rows)
        {
            Console.Write("│");
            for (int i = 0; i < headers.Length; i++)
            {
                string cell = i < row.Length ? row[i] : "";
                Console.Write(cell.PadRight(columnWidths[i]));
                Console.Write("│");
            }
            Console.WriteLine();
        }
        
        // Нижняя граница
        Console.Write("└");
        for (int i = 0; i < headers.Length; i++)
        {
            Console.Write(new string('─', columnWidths[i]));
            Console.Write(i < headers.Length - 1 ? "┴" : "┘");
        }
        Console.WriteLine();
    }

    private static void WaitForKeyPress()
    {
        Console.WriteLine("\n⏎ Нажмите любую клавишу для продолжения...");
        Console.ReadKey(true);
    }
}