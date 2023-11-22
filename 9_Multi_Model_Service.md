# Создаём Multi-Model Service в CommandsService

* открываем проект ***CommandsService***:
    1. Cоздаём папку Models и добавляем в неё два файла:
        * Platform.cs
        * Command.cs
            * Это модели данных с использованием атрибутов [*Key*] и [*Required*], которые служат для описания свойств и отношений между платформами и командами в рамках приложения "***CommandsService***". Они помогают определить структуру данных, требующихся для хранения информации о платформах и соответствующих им командах, а также задают требования к обязательности заполнения определенных полей.
        * смотри коммит: [Add models to the Command Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/b276ca44dd07ea7ccffb2ef177f1ff03a4bae60a)
    2. Визуализируем представление моделей к БД:
    
         ![Multi-Model_Service_29](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/29.png)
    3.  Создаём контекст БД, путём добавления файла *AppDbContext.cs* в папку *Data*
        * Смотри коммит: [Add Database Context](https://github.com/STGorbunovDA/.NET-Microservices/commit/1ba620d84408f406cbfca43f57f65bdc5ad0b698)
        * Переопределяем метод *OnModelCreating* его базовую реализацию в классе *DbContext*. В данном конкретном случае, он используется для настройки отношений (связей) между таблицами/сущностями в базе данных, с помощью объекта *ModelBuilder*
        * В данном коде, метод *OnModelCreating* настраивает связь "один-ко-многим" (*one-to-many*) между сущностями *Platform* и *Command*. Это выполняется с помощью следующих вызовов методов:
            * *modelBuilder.Entity<Platform>().HasMany(p => p.Commands).WithOne(p => p.Platform!).HasForeignKey(p => p.PlatformId)*
                * Здесь определяется, что сущность *Platform* может иметь много команд (*Commands*) связанных с ней. Связь устанавливается с помощью свойства *Platform* в сущности *Command* и внешнего ключа *PlatformId* в таблице команд.
            * *modelBuilder.Entity<Command>().HasOne(p => p.Platform).WithMany(p => p.Commands).HasForeignKey(p => p.PlatformId)*
                * Здесь определяется обратная связь, что каждая команда (*Command*) имеет только одну платформу (*Platform*). Связь устанавливается с помощью свойства *Commands* в сущности *Platform* и внешнего ключа *PlatformId* в таблице команд.
        * данные настройки определяют связь между таблицами и обеспечивают целостность данных при работе с базой данных через ***EF Core***.
    4. Добавляем репозиторий путем добавления файлов в папку *Data/Repository:*
        * Смотри коммит: [Add Concrete Repository Class](https://github.com/STGorbunovDA/.NET-Microservices/commit/d65cd25bf4901b03ab65f27d549515b13765ce74)
        * *ICommandRepo.cs*
        * *CommandRepo.cs*
            * нужны для выполнения операций создания и получения команд, получения списка всех платформ, проверки наличия платформы по её идентификатору, а также сохранения изменений в контексте базы данных. Класс CommandRepo взаимодействует с контекстом базы данных AppDbContext, который передается через конструктор.
    5. Визуализируем что должно получится:

            ![Multi-Model_Service_30](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/30.png)
    6. В файл **Program.cs** регистрируем наш сервис для доступа к БД, сам контекст и AutoMapper:
        * *builder.Services.AddScoped<ICommandRepo, CommandRepo>();*
        * *builder.Services.AddDbContext<AppDbContext>(opt 
        => opt.UseInMemoryDatabase("InMen"));*
        * *builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());*
        * Смотри коммит: [Update Platform Controller](https://github.com/STGorbunovDA/.NET-Microservices/commit/c5c8c1812cd572b216f6f778b9f5baa149d76248) 
    7. Добавляем папку **Dtos** и в неё файлы **CommandCreateDto.cs**, **CommandReadDto.cs** и **PlatformReadDto.cs:**
        * **CommandCreateDto** представляет собой объект передачи данных (DTO) для создания новой команды. Он содержит два свойства: *HowTo* (описание команды) и *CommandLine* (командная строка). Атрибуты [*Required*] указывают на то, что эти свойства являются обязательными при создании команды. 
        * **CommandReadDto** представляет собой объект передачи данных (*DTO*) для чтения информации об уже существующей команде. Он содержит свойства, которые включают идентификатор команды (*Id*), описание команды (*HowTo*), командную строку (*CommandLine*) и идентификатор платформы (*PlatformId*). 
        * **PlatformReadDto** представляет собой объект передачи данных (*DTO*) для чтения информации о платформе. Он содержит свойства, которые включают идентификатор платформы (*Id*) и название платформы (*Name*).
        * Соблюдаем паттерн **Data Transfer Object (DTO)** — один из шаблонов проектирования, используется для передачи данных между подсистемами приложения.
        * Смотри коммит: [Data Transfer Object (DTOs)](https://github.com/STGorbunovDA/.NET-Microservices/commit/4be3503dfcef3cfaf3c85a823abcdfcdd16a5ac1) 
    8. Добавляем папку **ProfilesAutoMapper** и в неё файл **CommandsProfile.cs:**
        * Определяет отображение (маппинг) между моделями и **Data Transfer Object (DTO)** в приложении с использованием AutoMapper.
        * Смотри коммит: [Automapper Profiles](https://github.com/STGorbunovDA/.NET-Microservices/commit/62fd650c9cb98643413a3de4d18b8a9b5f71046c)
    9. Обновляем наш контроллер ***PlatformsController.cs:***
        * Смотри коммит: [Update Platform Controller](https://github.com/STGorbunovDA/.NET-Microservices/commit/c5c8c1812cd572b216f6f778b9f5baa149d76248)
        * добавляем метод получения всех платформ из БД ***InMem***
    10. Добавляем методы для контроллера **CommandsController.cs:**
        * Смотри коммит: [Create Commands Controller](https://github.com/STGorbunovDA/.NET-Microservices/commit/2ccc6230c8babd1804feff1eb2ab7dc6b205cfd0)
        * **CommandsController** представляет контроллер API для управления командами на определенной платформе. Контроллер взаимодействует с репозиторием команд (*ICommandRepo*) и использует *AutoMapper* для маппинга объектов между моделями и *DTO* (*Data* *Transfer* *Objects*) и содержит следующие методы:
            * *GetCommandsForPlatform*: Получает список команд для определенной платформы по идентификатору платформы.
            * *GetCommandForPlatform*: Получает информацию о конкретной команде на определенной платформе по идентификаторам платформы и команды.
            * *CreateCommandForPlatform*: Создает новую команду на определенной платформе, используя данные из объекта *CommandCreateDto*.
                * Каждый метод выполняет определенные проверки и возвращает соответствующие результаты (например, *NotFound* или *Ok*) в зависимости от успешности операции. Кроме того, метод *CreateCommandForPlatform* также возвращает созданную команду в ответе, включая ссылку на ее получение (*GetCommandForPlatform*).
    11. Перейдём к тестированию:
        *  через *Swagger* или *Insomnia* или *Postman* протестируй методы контроллера **PlatformsController.cs** и **CommandsController.cs:**
            * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
            * Обрати внимание на папку **Local Dev/ Commands Service,** тебе необходимо заменить порт в маршрутах во всех файлах на соответствующий твоему приложению. http://localhost:**5029**/api/c/platforms/ - обрати внимание на консоль твоего приложения после запуска или в файле конфигурации **launchSettings.json** в *profiles* => "*applicationUrl*": "*http://localhost:5029*"

                ![Multi-Model_Service_31](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/31.png)
            * Выполни ***CreateCommandForPlatform***
            * Результат в консоли должен вывести сообщение:

                ![Multi-Model_Service_32](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/32.png)
    
        