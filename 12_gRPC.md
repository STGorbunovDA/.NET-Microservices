# gRPC

* ***gRPC (gRPC Remote Procedure Call)*** - это современная технология удаленного вызова процедур (*RPC*), разработанная *Google*. Она предоставляет простой и эффективный способ для клиента и сервера обмениваться данными и вызывать методы на удаленных машинах.

* Вот некоторые ключевые особенности ***gRPC***:
    * Протокол с открытым исходным кодом: ***gRPC*** является открытым стандартом и доступен на разных платформах и языках программирования.
    * Мультиплатформенность: ***gRPC*** поддерживает множество языков программирования, включая ***C++***, ***C#***, ***Go***, ***Java***, ***JavaScript***, ***Kotlin***, ***Python***, ***Ruby*** и другие. Это означает, что вы можете создавать клиент-серверные приложения с использованием ***gRPC*** на различных платформах.
    * Универсальный протокол сериализации: ***gRPC*** использует ***Protocol*** ***Buffers*** (***Protobuf***) в качестве универсального протокола сериализации данных. ***Protobuf*** - это компактный, быстрый и расширяемый формат сериализации данных, который позволяет определять структуру сообщений и генерировать код для разных языков программирования.
    * Мощная система определения интерфейсов: ***gRPC*** использует язык с описанием службы ***Protocol*** ***Buffers*** (***Proto***) для определения интерфейсов API и сообщений данных. Описания ***Proto*** обеспечивают ясное и строгое определение доступных методов и типов данных в API.
    * Поддержка различных паттернов коммуникации: ***gRPC*** поддерживает как унарный (однократный запрос-ответ), так и дуплексный (потоковый) обмен сообщениями между клиентом и сервером. Это позволяет создавать эффективные взаимодействия в режиме реального времени и обрабатывать потоковые данные.
    * Поддержка различных протоколов передачи: ***gRPC*** может использовать различные протоколы передачи данных, включая ***HTTP/2***, для обеспечения эффективного и безопасного обмена данными между клиентом и сервером. ***HTTP/2*** - современный протокол передачи данных, который обеспечивает множество преимуществ по сравнению с ***HTTP/1***.
    * Автоматическая генерация кода: ***gRPC*** предоставляет инструменты для автоматической генерации кода клиента и сервера на основе описаний ***Proto***. Это упрощает процесс разработки и интеграции клиентской и серверной частей приложения.
    * Использование ***gRPC*** позволяет создавать эффективные и масштабируемые клиент-серверные системы с минимальными затратами на разработку и обмен данных. Благодаря использованию ***Protobuf*** и ***HTTP***/***2***, ***gRPC*** обеспечивает высокую производительность и низкую задержку при обмене сообщениями.
* Служба gRPC в нашем примере необходима для получения всех платформ синхронно из ***PlatformService*** в ***CommandsService.***

    ![Asynchronous_Events_RabbitMQ_51](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/51.png)

* в папке **K8S** добавим в манифест **platforms-depl.yaml:**
    * порт для ***gRPC***
    * Смотри коммит: [Update Platforms Service Cluster IP](https://github.com/STGorbunovDA/.NET-Microservices/commit/33c1b012698d81b2135861509ae5860b9e86ea4e) 

* открываем проект ***PlatformService***:
    *  Добавим в конфигурационный файл ***appsettings.Production.json***:
    * внедряем сервер ***Kestrel*** со двумя конечными точками (***endpoints***): одна для ***gRPC*** протокола, использующая ***HTTP/2*** на порту ***666***, и другая для веб-API, использующая ***HTTP/1*** на порту ***80***, оба адресованы на [***http://platforms-clusterip-srv***](http://platforms-clusterip-srv)
    * Смотри коммит: [Production Configuration](https://github.com/STGorbunovDA/.NET-Microservices/commit/2d3e904ff2dbcfbd51356092f74d7544cbc3ef9f) 
    * Далее добавим:
        * *dotnet add package Grpc.AspNetCore*
        * Смотри коммит: [Add gRPC Packages](https://github.com/STGorbunovDA/.NET-Microservices/commit/1bc8bcc19c5e8da4aa0ee907621a229f621bd40c) 
    * Создадим папку *Protos* и добавим в неё конфигурационный файл ***platforms.proto*** службы ***gRPC**:*
        * Смотри коммит: [Create Proto Platforms Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/39804b8b01d1c869777e1a86cf21d9218c4e26b6)
        * Это протокол взаимодействия для получения информации о платформах с использованием ***gRPC***. Серверная сторона должна реализовать метод ***GetAllPlatforms*** для обработки запросов, а клиентская сторона может использовать этот протокол для отправки запросов и получения ответов о платформах
        * Как я понял файл proto генератор кода в нашем случае для ***C#*** для службы ***gRPC*** и находится он после билдинга сервиса здесь:

            ![Asynchronous_Events_RabbitMQ_52](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/52.png)

    * В корне сервиса в конфигурационный файл ***PlatformService.csproj*** добавим упоминание о нахождении*** файла ***Proto:***
         * Смотри коммит: [Create Proto Platforms Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/39804b8b01d1c869777e1a86cf21d9218c4e26b6)
    * В папке ***SyncDataServices*** создадим папку ***Grpc*** и в ней файл ***GrpcPlatformService.cs:***
        * Смотри коммит: [gRPC in the Platforms Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/308e99c74ead7952e237f0390672623c9e66439e)
        * ***GrpcPlatformService*** является реализацией ***gRPC*** сервиса ***GrpcPlatform.GrpcPlatformBase***, и содержит метод ***GetAllPlatforms***, который получает все платформы из репозитория ***IPlatformRepo***, отображает их с использованием ***AutoMapper*** и возвращает ответ в виде ***PlatformResponse***.
    * Далее отредактируем файл ***PlatformsProfile.cs*** который находится в папке ***ProfilesAutoMapper:***
         * Смотри коммит: [gRPC in the Platforms Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/308e99c74ead7952e237f0390672623c9e66439e)
         * происходит уточнённый мапинг *Platform* в *GrpcPlatformModel*
    * В файл **Program.cs** регистрируем наш сервис ***gRPC***:
        * *builder.Services.AddGrpc();*
* открываем проект ***CommandsService***:
    * И добавляем пакеты:
        * *dotnet add package Google.Protobuf*
        * *dotnet add package Grpc.Net.Client*
        * *dotnet add package Grpc.Tools*
        * Смотри коммит: [Add gRPC Packages](https://github.com/STGorbunovDA/.NET-Microservices/commit/1bc8bcc19c5e8da4aa0ee907621a229f621bd40c)
    * Добавим в конфигурационный файл ***appsettings.Production.json***:
        * порт ***gRPC*** сервиса откуда будем получать данные
        * Смотри коммит: [Config Creation](https://github.com/STGorbunovDA/.NET-Microservices/commit/b4c92da0238762a6959991d3c4daa744ccda752a)
    * Добавим в конфигурационный файл ***appsettings.Development.json***:
        * порт ***gRPC*** сервиса откуда будем получать данные
        * Смотри коммит: [change config](https://github.com/STGorbunovDA/.NET-Microservices/commit/1c2e51267c7bc15d4b399e5b4b18e954b7f9e10c)
    * Создадим папку *Protos и* добавим в неё конфигурационный файл ***platforms.proto*** службы ***gRPC**:*
        * Смотри коммит: [Create Proto Commands Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/9bdffea147fce93429c462ce3ab2a54d97a0e8d0) 
        * Это протокол взаимодействия для получения информации о платформах с использованием ***gRPC***. Серверная сторона должна реализовать метод ***GetAllPlatforms*** для обработки запросов, а клиентская сторона может использовать этот протокол для отправки запросов и получения ответов о платформах
        * Как я понял файл proto генератор кода в нашем случае для ***C#*** для службы ***gRPC*** и находится он после билдинга сервиса здесь:

            * ![Asynchronous_Events_RabbitMQ_53](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/53.png)
    * В корне сервиса в конфигурационный файл ***CommandsService.csproj*** добавим упоминание о нахождении*** файла ***Proto:***
        * Смотри коммит: [Create Proto Commands Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/9bdffea147fce93429c462ce3ab2a54d97a0e8d0)
    * Далее отредактируем файл ***CommandsProfile.cs*** который находится в папке ***ProfilesAutoMapper:***
        * Смотри коммит: [Create gRPC Data Service in the Commands Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/395cdbd65460352a568e4be516aab82d276b0cd4)
        * ***PlatformDataClient***.***cs***, представляет клиента для взаимодействия с ***gRPC*** сервисом ***GrpcPlatform***.***GrpcPlatformClient***. Метод ***ReturnAllPlatforms*** вызывает ***gRPC*** сервис, получает все платформы и отображает их с использованием ***AutoMapper***, возвращая результат в виде коллекции Platform. Простыми словами ***PlatformDataClient***.***cs*** является получателем наших платформ из сервиса ***PlatformService*** с использованием ***gRPC***
    * В файл **Program.cs** регистрируем наш сервисы по синхронному получению всех платформ с использованием ***gRPC***:
        * *builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();* 
        * Смотри коммит: [Create gRPC Data Service in the Commands Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/395cdbd65460352a568e4be516aab82d276b0cd4)
    * В корне сервиса ***CommandsService*** в папке ***Data*** создадим метод который будет вызывать ***IPlatformDataClient*** и добавлять получившие платформы в БД “***InMem***”:
        * Смотри коммит: [Create DB Preparation in Commands Service](https://github.com/STGorbunovDA/.NET-Microservices/commit/3d77e157da06d2c9aa8e6d80f190a9f142cadf1c)
            * в файл **Program.cs** добавим этот метод