# Создаём PlatformService


1. Cоздаём решение ***sln***
    * *dotnet new sln*
    * создаться решение *.NET-Microservices.sln*
2. Cоздаём **WebApi** и добавляем к нашему решению *.NET-Microservices.sln*:
    * *dotnet new webapi -n PlatformService*** 
3. Открываем проект:
    * Code -r ***PlatformService***
4. Удаляем все встроенные контроллеры и сущности из проекта ***PlatformService***
5. Добавляем пакеты в ***PlatformService***:
    - dotnet add package *AutoMapper.Extensions.Microsoft.DependencyInjection*
    - dotnet add package *Microsoft.EntityFrameworkCore*
    - dotnet add package *Microsoft.EntityFrameworkCore.Design*
    - dotnet add package *Microsoft.EntityFrameworkCore.InMemory*
    - dotnet add package *Microsoft.EntityFrameworkCore.SqlServer*
6. Добавляем папку **Models** в *PlatformService и в неё файл **Platform.cs:***
    * **Platform.cs** является моделью данных для платформы, содержащей информацию об идентификаторе, имени, издателе и стоимости.
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
7. Добавляем папку **Data** в ***PlatformService*** и в неё файл **AppDbContext.cs:**
    * **AppDbContext.cs** является контекстом базы данных и определяет связь с моделью данных, включая таблицу ***Platform.cs*** для работы с платформами.
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
8. В файле ***Program.cs*** удаляем все комментарии и добавляем контекст подключения к БД, пока в данном случае только для хранения данных в оперативной памяти:
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
9. Добавляем папку ***Data*** интерфейс ***IPlatformRepo.cs*:**
    * Определяет контракт для репозитория платформ, включая методы для получения всех платформ(по Id), создания и сохранения данных платформы.
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
10. Добавляем папку ***Data*** класс ***PlatformRepo.cs*:**
    * реализует интерфейс ***IPlatformRepo.cs*** и содержит реализации методов для работы с данными платформы, включая создание, получение, сохранение и загрузку данных из базы данных.
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
11. В файл ***Program.cs*** регистрируем наш сервис для доступа к БД:
    * *builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();*
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
12. Добавляем папку ***Data*** статический класс **PrepDb.cs:**
    * Он содержит метод ***PrepPopulation***, используемый для подготовки и заполнения базы данных приложения. Метод ***SeedData*** выполняет миграции базы данных и добавляет начальные данные (платформы) в базу, если они еще не существуют. 
Без **DI**, только для теста. В производство внедрять нельзя.
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
13. Добавляем в файл ***Program.cs*** метод ***PrepDb.PrepPopulation(app):***
    * смотри коммит: 
     [Implementing migration for InMemory](https://github.com/STGorbunovDA/.NET-Microservices/commit/50a8ea2b42fc6f540d872cd35a2664df8d148afe)
14. Добавляем папку ***Dtos*** и в неё файлы ***PlatformCreateDto.cs*** и ***PlatformReadDto.cs:***
    * ***PlatformCreateDto*** используется для передачи данных при создании новой платформы. Он содержит свойства для имени, издателя и стоимости платформы, которые являются обязательными для заполнения.

    * ***PlatformReadDto*** используется для передачи данных при чтении информации о платформе. Он содержит свойства для идентификатора, имени, издателя и стоимости платформы.

    * Соблюдаем паттерн ***Data Transfer Object (DTO)*** — один из шаблонов проектирования, используется для передачи данных между подсистемами приложения.
    * смотри коммит: 
     [Implementation AutoMapper](https://github.com/STGorbunovDA/.NET-Microservices/commit/877ba0d5d72c936933e611ca7d68a78ae84fceca)
15. В файл ***Program.cs*** регистрируем наш сервис *AutoMapper*:
    *  *builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());*
    * смотри коммит: 
     [Implementation AutoMapper](https://github.com/STGorbunovDA/.NET-Microservices/commit/877ba0d5d72c936933e611ca7d68a78ae84fceca)
16. Добавляем папку ***Profiles*** и в неё файл ***PlatformsProfile.cs*:**
    * Определяет отображение (маппинг) между моделями и ***Data Transfer Object (DTO)*** в приложении с использованием ***AutoMapper***.
    * смотри коммит: 
     [Implementation AutoMapper](https://github.com/STGorbunovDA/.NET-Microservices/commit/877ba0d5d72c936933e611ca7d68a78ae84fceca)
17. Добавляем папку ***Controllers*** и в неё файл ***PlatformsController*.*cs*:**
    * ***PlatformsController*.*cs*** обрабатывает HTTP-запросы, связанные с платформами, используя ***AutoMapper*** для отображения моделей и ***Data Transfer Object (DTO)*** и взаимодействует с репозиторием ***IPlatformRepo***.***cs***
    * смотри коммит: 
     [Creating a controller and transferring files to the Repository folder](https://github.com/STGorbunovDA/.NET-Microservices/commit/5d7a5182142ed6d8fee434957ea17577b2fd9acf)
18. Общая картинка сервиса ***PlatformService*** и проделанных действий:
    ![PlatformService_1-1](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/1-1.png)
19. Далее необходимо проверить через *Swagger* или *Insomnia* или *Postman* методы контроллера *PlatformsController.cs:*
    * Далее необходимо проверить через *Swagger* или *Insomnia* или *Postman* методы контроллера ***PlatformsController*.*cs*:**
        * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку ***Local Dev/ Platform Service*,** тебе необходимо заменить порт в маршрутах во всех файлах на соответствующий твоему приложению. *http://localhost:**5230**/api/platforms/* - обрати внимание на консоль твоего приложения после запуска или в файле конфигурации ***launchSettings.json*** 
в *profiles* => "*applicationUrl*": "*http://localhost:5230*"