# LaFinance – фінансовий планувальник

Майданюк Софія ІПЗ-23-1

## Функціональність: 
LaFinance — це настільний застосунок для обліку особистих фінансів, розроблений на основі архітектури MVVM. Він дозволяє користувачу:
- Переглядати аналітику доходів і витрат у вигляді стовпчикового графіку з фільтром за датами.
- Відстежувати витрати по категоріях — категорії з перевищеним лімітом підсвічуються червоним.
- Керувати транзакціями: додавати, редагувати, видаляти та фільтрувати за типом, категорією, датою або назвою.
- Керувати балансами: облік готівки, карт і депозитів.
- Створювати та редагувати категорії для доходів і витрат з можливістю вибору іконки. Також є можливість видалення
- Встановлювати бюджетні ліміти на витрати по категоріях для кожного місяця. Також є можливість видалення та редагування

Інтерфейс простий і зручний, усі вікна додавання та редагування виконані в єдиному стилі. Додаток забезпечує повний цикл управління особистими фінансами.

## Programming Principles
У цьому проєкті дотримано ключових принципів програмування, які підвищують якість коду, спрощують підтримку та сприяють масштабованості:

### SOLID
#### Single Responsibility Principle (SRP)
Кожен клас відповідає лише за одну дію. Наприклад:
- [TransactionRepository](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Repositories/TransactionRepository.cs) - тільки для доступу до транзакцій
- [TransactionsViewModel](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/TransactionsViewModel.cs) - лише для логіки представлення транзакцій.
- [AddTransactionWindow](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/Views/transaction/AddTransactionWindow.xaml.cs) - лише для взаємодії з користувачем під час створення транзакції.

#### Open/Closed Principle (OCP)
Архітектура проєкту побудована таким чином, що дозволяє легко додавати нову функціональність без необхідності змінювати вже написаний код. Наприклад, якщо у майбутньому потрібно буде додати новий тип транзакцій, новий формат аналітики або окремий модуль для планування витрат, це можна реалізувати через нові класи або ViewModel без зміни існуючих компонентів. Такий підхід забезпечує зручне масштабування та знижує ризик порушення стабільної логіки додатку.

#### Dependency Inversion Principle (DIP)
Клас `TransactionsViewModel` та `AddTransactionWindow` не залежать напряму від [TransactionRepository](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Repositories/TransactionRepository.cs), а працюють з абстракцією [ITransactionRepository](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Interfaces/ITransactionRepository.cs).

### Dependency Injection
Використано Microsoft.Extensions.DependencyInjection:
- Усі сервіси ([AppDbContext](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Data/AppDbContext.cs), [Repositories](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/ClassLibrary-FinancialPlanner/Repositories)], [ViewModels](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/WpfApp-FinancialPlanner/ViewModels)) передаються через конструктор
- Підвищено гнучкість та можливість підміни залежностей під час тестування або масштабування

### Separation of Concerns (SoC)
Функціональність чітко розмежована:
- [Models](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/ClassLibrary-FinancialPlanner/Models) — бізнес-логіка ([Transaction](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Models/Transaction.cs), [BudgetLimit](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Models/BudgetLimit.cs), [Category](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Models/Category.cs) і тд.).
- [ViewModels](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/WpfApp-FinancialPlanner/ViewModels) — логіка представлення даних ([TransactionsViewModel](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/TransactionsViewModel.cs), [AnalyticsViewModel](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs) і тд.).
- [Views](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/WpfApp-FinancialPlanner/Views) — XAML + Code-Behind, без зайвої логіки.
- [Repositories](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/ClassLibrary-FinancialPlanner/Repositories)] — доступ до БД.

### DRY (Don't Repeat Yourself)
Принцип уникнення дублювання реалізовано в основних частинах бізнес-логіки:
- Повторне використання коду забезпечується через централізовані репозиторії (`ITransactionRepository`, `IBudgetLimitRepository`).
- Завантаження та оновлення списків винесено в методи `LoadData` / `LoadAsync`.
- Обробка подій розділена між ViewModel та репозиторіями, що дозволяє уникнути дублювання логіки.

У UI-частині (наприклад, вікна `AddCategoryWindow` і `EditCategoryWindow`) структура частково повторюється, але це зроблено навмисно для простоти підтримки та відокремлення сценаріїв. У разі масштабування ці компоненти можна об’єднати або перетворити на спільний контрол.

### KISS (Keep It Simple, Stupid)
Код простий і читається без зайвих умовностей:
- Зрозуміле і лаконічне управління UI.
- Мінімум вкладеності та перевантаження логіки у вікнах.
- Простий механізм реакції на події (через делегати або оновлення контексту).

### Fail Fast
Програма реагує на помилки якомога раніше, ще до того, як дані потрапляють у систему. Один з прикладів: при створенні транзакції вікно [AddTransactionWindow](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/Views/transaction/AddTransactionWindow.xaml) перевіряє:
- чи введено правильну суму (decimal.TryParse);
- чи вибрано категорію, баланс та опис;
- чи не перевищує витрата встановлений ліміт.
Якщо хоча б одна перевірка не проходить — транзакція не створюється, а користувач отримує повідомлення. Це дозволяє запобігти накопиченню помилок, забезпечує надійність і передбачувану поведінку програми.

### YAGNI (You Aren’t Gonna Need It)
Код проєкту реалізує лише ту функціональність, яка потрібна зараз, без написання зайвих методів чи класів “про всяк випадок”. ViewModel-логіка (TransactionsViewModel, BudgetLimitViewModel) містить тільки необхідні методи: LoadAsync, Add, Update, Delete, без підготовлених, але ще не використаних функцій (типу “завантажити з CSV” чи “експортувати в PDF”).

## Design Patterns
### MVVM (Model-View-ViewModel)
Це архітектурний патерн, що чітко розділяє інтерфейс користувача (View), логіку відображення (ViewModel) та структуру даних (Model).
У проєкті цей патерн реалізовано за допомогою таких частин:
- Model — класи в [ClassLibrary_FinancialPlanner.Models](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/ClassLibrary-FinancialPlanner/Models) (Transaction.cs, Category.cs, Balance.cs, BudgetLimit.cs), що описують структуру даних.
- [ViewModels](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/WpfApp-FinancialPlanner/ViewModels) — класи, як-от TransactionsViewModel.cs, AnalyticsViewModel.cs, BudgetLimitViewModel.cs, які містять бізнес-логіку та обробку даних.
-  [Views](https://github.com/sofia-maidaniuk/FinancialPlanner/tree/main/financial_planner/WpfApp-FinancialPlanner/Views) — XAML-сторінки (TransactionsPage.xaml, AnalyticsPage.xaml, BudgetLimitsPage.xaml), які зв’язуються з ViewModel через DataContext.
Цей підхід дозволяє незалежно змінювати логіку й інтерфейс, що покращує підтримку, тестованість і масштабованість додатку.

### Dependency Injection (DI)
Це структурний патерн, який дозволяє об’єктам отримувати свої залежності (класи або сервіси), не створюючи їх самостійно.
- У проєкті реалізовано DI через вбудований механізм Microsoft.Extensions.DependencyInjection у [App.xaml.cs](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/App.xaml.cs):
- У методі [OnStartup](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/App.xaml.cs#L19-L42) всі сервіси (контексти БД, репозиторії, ViewModel-и) реєструються через services.AddScoped(...) або AddTransient(...).
Залежності потім "впроваджуються" у вікна та сторінки за допомогою App.Services.GetRequiredService<...>().

### Singleton
Це патерн, що гарантує існування лише одного екземпляра класу й надає глобальну точку доступу до нього.
У проєкті [App.xaml.cs](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/App.xaml.cs) реалізовано патерн Singleton для доступу до сервісів через змінну 
`public static IServiceProvider Services`. 
Цей екземпляр Services ініціалізується один раз у методі OnStartup, і після цього використовується по всьому проєкту для отримання зареєстрованих залежностей: 
`var context = App.Services.GetRequiredService<AppDbContext>();`
Таким чином, це забезпечує централізований і єдиний доступ до всіх зареєстрованих сервісів додатку.

### Repository
Патерн Repository інкапсулює логіку доступу до джерела даних, відокремлюючи бізнес-логіку від деталей зберігання. Це дозволяє легко змінювати джерело даних без впливу на інші частини коду. У проєкті реалізовано інтерфейси репозиторіїв - [ITransactionRepository](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Interfaces/ITransactionRepository.cs) та їх конкретні реалізації [TransactionRepository](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/ClassLibrary-FinancialPlanner/Repositories/TransactionRepository.cs). Це дозволяє у ViewModel працювати з транзакціями через абстракцію, а не напряму з контекстом БД.

## Refactoring Techniques
### Extract Method
У [AnalyticsViewModel](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs) метод `GenerateMonthlyChart()` був занадто довгим і складався з кількох логічно відокремлених частин: фільтрація транзакцій, побудова графіка, групування витрат. Тому ці частини були винесені в окремі методи:
- [FilterTransactionsByDate(List<Transaction>)](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs#L86-L95): відповідає лише за фільтрацію транзакцій за вказаним діапазоном дат DateFrom – DateTo.
- [CreateMonthlyPlot(List<Transaction>)](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs#L97-L127): будує OxyPlot-графік на основі переданих транзакцій.
- [GetExpensesByCategory(List<Transaction>)](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs#L129-L142): (додатково використовується) для формування списку витрат за категоріями.

### Extract Class
У проєкті створено окремий [клас CategoryExpense](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs#L16-L22) у AnalyticsViewModel.cs, який був виділений для зберігання структурованих даних про витрати по категоріях. Це дозволяє не перевантажувати ViewModel додатковими полями й логікою, а натомість винести цю відповідальність у спеціалізований клас.

### Encapsulate Field
Замість прямого доступу до полів класу, використовується властивість з get/set. Це дозволяє краще контролювати доступ до даних, додавати валідацію, оновлення UI через INotifyPropertyChanged тощо. У AnalyticsViewModel усі поля, що відображаються у View (наприклад, MonthlyPlotModel, ExpensesByCategory), інкапсульовані через публічні властивості, які викликають [OnPropertyChanged()](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/AnalyticsViewModel.cs#L29-L34) для оновлення інтерфейсу. Це дозволяє інтерфейсу автоматично реагувати на зміну значень та дає змогу легко додати додаткову логіку (наприклад, валідацію) у майбутньому без зміни структури класу.

### Move method
У BudgetLimitViewModel.cs був реалізований метод [LoadData](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/ViewModels/BudgetLimitViewModel.cs#L35-L45), який завантажує категорії та ліміти. Раніше подібна логіка могла знаходитися у BudgetLimitsPage.xaml.cs — тобто безпосередньо в коді сторінки. Але вона була переміщена у ViewModel. Це — приклад класичного переносу методу з View до ViewModel у рамках MVVM. Такий хід дозволяє тестувати LoadData() незалежно від інтерфейсу користувача, спрощує повторне використання, зменшує зв’язність між UI і логікою.

###  Rename Method
Назви методів та змінних відображають їх призначення. LoadData, AddLimitAsync, NotifyTransactionsToReloadAsync — назви не абстрактні, а описують дії чітко.

###  Split Variable
У [Save_Click()](https://github.com/sofia-maidaniuk/FinancialPlanner/blob/main/financial_planner/WpfApp-FinancialPlanner/Views/transaction/AddTransactionWindow.xaml.cs#L43-L120) методах, наприклад у AddTransactionWindow.xaml.cs, використовуються окремі змінні для кожного типу даних, замість багаторазового використання однієї. Це покращує читабельність, зменшує ризик помилок при повторному використанні змінної для різних задач.

###  Replace Nested Conditional with Guard Clauses
Замість вкладених if-else структур використовуються “ранні виходи” (return), що одразу обробляють невалідні дані. Це робить код лінійним, чистим і легким для читання. Наприклад, у AddTransactionWindow.xaml.cs у методі Save_Click():
```
if (!decimal.TryParse(AmountBox.Text, out decimal amount))
{
    MessageBox.Show("Будь ласка, введіть коректну суму.", "Помилка");
    return;
}

if (BalanceComboBox.SelectedItem is not Balance selectedBalance)
{
    MessageBox.Show("Будь ласка, оберіть баланс.", "Помилка");
    return;
}

if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
{
    MessageBox.Show("Будь ласка, введіть опис транзакції.", "Помилка");
    return;
}
```
Це зменшує глибину вкладеності і стає зрозуміло одразу, що відбувається.

## Результат та вигляд
<img src="result img/analytics.png"  alt="Головна сторінка з статистикою" width="500">
<img src="result img/balance.png"  alt="Баланси" width="500">
<img src="result img/transaction.png"  alt="Транзакції" width="500">
<img src="result img/transaction_filter.png"  alt="Транзакції" width="500"> 
<img src="result img/category.png"  alt="Категорії" width="500"> 
<img src="result img/category_add.png"  alt="Категорії" width="500"> 
<img src="result img/category_edit.png"  alt="Категорії" width="500"> 
<img src="result img/category_delete.png"  alt="Категорії" width="500"> 
<img src="result img/limit.png"  alt="Ліміти" width="500"> 
<img src="result img/limit_edit.png"  alt="Ліміти" width="500"> 