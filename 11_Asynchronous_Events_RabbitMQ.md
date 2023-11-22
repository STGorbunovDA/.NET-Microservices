# Asynchronous Events c помощью RabbitMQ

* Визуализируем что должно получится между сервисами:

    ![Asynchronous_Events_RabbitMQ_33](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/33.png)

* Открываем проект ***PlatformService***:

    * Смотри коммиты: [Check client packages and config](https://github.com/STGorbunovDA/.NET-Microservices/commit/a0404f1fdf116f06a1fbfceab2af63d8a21f5fbe)

    * добавляем пакет:
        * *dotnet add package RabbitMQ.Client*
    * открываем файл *appsettings.Development.json*
        * добавляем порт и хост ***RabbitMQ***:
            * *RabbitMQHost*": "*localhost*",
            * *RabbitMQPort*": "*5672*"
    * открываем файл *appsettings.Production.json*
        * добавляем подключение хост и порт ***RabbitMQ*** для ***Kubernetes***
            * *RabbitMQHost*": "*rabbitmq-clusterip-srv*"
            * *RabbitMQPort*": "*5672*"
    * Далее добавляем в папку ***Dtos*** файл ***PlatformPublishedDto.cs:***
        * Смотри коммит: [Data Transfer Objects for eventing](https://github.com/STGorbunovDA/.NET-Microservices/commit/5a66e01e729d3231d378e79c39d9075bbb63e37f) 
        * Класс используется для передачи данных о платформе между различными компонентами или слоями приложения, например, при публикации информации о платформе в другие сервисы или системы.
    * Заходим в папку ***ProfilesAutoMapper*** или ***Profiles*** и изменяем класс ***PlatformsProfile.cs***:
         * Смотри коммит: [Data Transfer Objects for eventing](https://github.com/STGorbunovDA/.NET-Microservices/commit/5a66e01e729d3231d378e79c39d9075bbb63e37f)
         * Добавляем: *CreateMap<PlatformReadDto, PlatformPublishedDto>();*
            * Необходимо для мапинга классов из *PlatformReadDto в PlatformPublishedDto.*
    * Для асинхронной передачи сообщений между сервисами с помощью ***RabbitMQ,*** необходимо создать папку ***AsyncDataServices*** и в ней два файла, интерфейс ***IMessageBusClient.cs*** и класс ***MessageBusClient.cs*** имплементирующий данный интерфейс выполняя его протоколы. 
        * Смотри коммиты: [Message Bus Interface](https://github.com/STGorbunovDA/.NET-Microservices/commit/1f5886370d572643baff76227e36267453edc89d) и [Message Bus class and constructor and Message Bus Publish and Send](https://github.com/STGorbunovDA/.NET-Microservices/commit/7ddae5c4823179a15c46ea208aa7658a2d7ed616)
        * ***MessageBusClient.cs*** является клиентом сообщений для обмена данными между компонентами приложения с использованием ***RabbitMQ***. Он отвечает за установку соединения с ***RabbitMQ***, создание канала обмена сообщениями и отправку сообщений в виде JSON-представления объекта ***PlatformPublishedDto***. Класс используется для публикации новой платформы через шину сообщений ***RabbitMQ***.
    * В файл **Program.cs** регистрируем наш сервис для отправки асинхронных сообщений:
        * *builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();*
        * Смотри коммиты: [Update Controller](https://github.com/STGorbunovDA/.NET-Microservices/commit/7f506436b2fac29271b3290d8089dd513bd892df)
    * Внедряем наш сервис отправки сообщений в контроллер ***PlatformsController.cs***:
        * Смотри коммиты: [Update Controller](https://github.com/STGorbunovDA/.NET-Microservices/commit/7f506436b2fac29271b3290d8089dd513bd892df)
            * Здесь происходит отправка асинхронного сообщения через *MessageBusClient*. Он создает объект *platformPublishedDto* на основе *platformReadDto*, устанавливает соответствующее событие и вызывает метод *PublishNewPlatform*() для отправки сообщения. Этот блок кода нужен для отправки асинхронных сообщений о публикации новой платформы через шину сообщений *RabbitMQ*.
    * Перейдём к тестированию:
        * запусти оба сервиса ***PlatformService*** и затем ***CommandsService***
        * через *Swagger* или *Insomnia* или *Postman* протестируй отправку сообщений через RabbitMQ:
            * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
            * Обрати внимание на папку **Local Dev/ *Platform* Service** и Выполни *CreatePlatform* не забывая поменять порт
            * В консоли ***CommandsService*** должно появится это:

                ![Asynchronous_Events_RabbitMQ_39](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/39.png)
            * В консоли ***PlatformService*** появится сообщение о том что мы установили соединение и отправили сообщение в ***RabbitMQ*** а так же отправили синхронное сообщение в ***CommandsService***:

               ![Asynchronous_Events_RabbitMQ_40](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/40.png)
            * Зайдя на localhost:15672 в RabbitMQ мы увидим визуальный график нашего сообщения:

                ![Asynchronous_Events_RabbitMQ_41](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/41.png)

* Открываем проект ***CommandsService***:
    * Смотри коммит: [Preparing Service Subscription](https://github.com/STGorbunovDA/.NET-Microservices/commit/4834da773db6f30098ea933ae1e22969b86ab2f5)
    * Добавляем пакет:
        * *dotnet add package RabbitMQ.Client*
    * Открываем файл *appsettings.Development.json*
        * добавляем хост и порт ***RabbitMQ***
            * *RabbitMQHost*": "*localhost*",
            * *RabbitMQPort*": "*5672*"
        * открываем файл *appsettings.Production.json*
            * добавляем хост и порт ***RabbitMQ*** для ***Kubernetes***
                * *RabbitMQHost*": "*rabbitmq-clusterip-srv*",
                * *RabbitMQPort*": "*5672*"
    * Далее добавляем в папку ***Dtos*** файлы ***PlatformPublishedDto.cs*** и ***GenericEventDto.cs:***
        * Смотри коммит: [PlatformPublished DTO](https://github.com/STGorbunovDA/.NET-Microservices/commit/0a537958f2dd59f02a88bc9197da2c113fa52aa0)
            * класс модель ***PlatformPublishedDto.cs*** используется для передачи данных о платформе между различными компонентами или слоями приложения, например, при публикации информации о платформе в другие сервисы или системы.
            * класс модель ***GenericEventDto.cs*** необходим для обработчика асинхронных сообщений, он позволяет по приходящему сообщению мапить название сообщения и по нему выполнять соответсвующие действия исходя из названия. Например сообщение от сервиса будет создать платформу, от сюда по названию обработчик сообщения выполнит то или иное действие.
    * Заходим в папку ***ProfilesAutoMapper*** или ***Profiles*** и изменяем класс ***CommandsProfile.cs***:
        * Смотри коммит: [Automapper Config](https://github.com/STGorbunovDA/.NET-Microservices/commit/c0cb7f82f5f3bac77213d71a2b19981832226223)
        * Добавляем: *CreateMap<PlatformPublishedDto, Platform>().ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));*
            * ***Мапим*** данные из *PlatformPublishedDto* в *Platform* и добавляем условие при котором свойство *ExternalID* в типе *Platform* должно быть заполнено значением свойства *Id* из *PlatformPublishedDto*
    * Добавляем изменения в файлы репозитория *Data/Repository:*
        * *ICommandRepo.cs*
        * *CommandRepo.cs*
        * Смотри коммит: [Add new repository method](https://github.com/STGorbunovDA/.NET-Microservices/commit/06951be8660df6ab42527a932091e473ae035678) 
            * метод *bool ExternalPlatformExists(int externalPlatformId);*
                * Этот метод проверяет, существует ли в базе данных платформа с заданным внешним идентификатором, путем выполнения запроса к таблице Platforms и проверки, есть ли хотя бы одна запись, у которой значение свойства ExternalID равно указанному внешнему идентификатору. Он используется для определения наличия платформы из внешнего источника данных в приложении.
    * Добавляем в корень проекта ***CommandsService*** папку *EventProcessing* и в неё два файла *IEventProcessor.cs* и *EventProcessor.cs* обработчик асинхронных сообщений:
        * Смотри коммит: [Event Processor : DetermineEvent and ProcessEvent](https://github.com/STGorbunovDA/.NET-Microservices/commit/afdea5791a3fcfa9ab7ed06b50e34a43c2474fe8)  
            * класс *EventProcessor.cs* выполняет обработку сообщений о событиях. Он используется для преобразования и обработки сообщений о событиях, связанных с публикацией платформы. В частности, он определяет тип события на основе полученного сообщения, добавляет платформу в базу данных при публикации новой платформы и выводит соответствующие сообщения в консоль. В целом данный сервис позволяет получить из сообщения *platformPublishedMessage* *RabbitMQ* платформу и добавить её в нашу базу с помощью *IServiceScopeFactory* внутри метода *AddPlatform* из области служб *ICommandRepo* и использовать его для выполнения операций с базой данных.
            * Данный паттерн (*IServiceScopeFactory*) применяется для правильного управления его жизненным циклом:
                *  Жизненный цикл: Если *ICommandRepo* имеет *Scoped*-жизненный цикл, то получение его напрямую в конструкторе EventProcessor может привести к созданию одного экземпляра *ICommandRepo* на всю продолжительность работы *EventProcessor*. В то время как, если использовать *IServiceScopeFactory* и создать область служб внутри метода *AddPlatform*, то каждый раз, когда вызывается *AddPlatform*, будет создаваться новый экземпляр *ICommandRepo* в пределах этой области. Это может быть полезно, если требуется гарантировать изолированность операций или управление ресурсами.
                * Обработка исключений: Использование области служб через IServiceScopeFactory позволяет элегантно обрабатывать исключения, связанные с работой репозитория. Если возникает исключение при выполнении операции с репозиторием внутри метода AddPlatform, вы можете легко перехватить его, вывести соответствующие сообщения и выполнить необходимые действия, например, запись в журнал ошибок или возврат ошибки клиенту.
                * Разрешение зависимостей: Использование области служб позволяет разрешать зависимости через IServiceProvider, полученный из области. Это особенно полезно, если AddPlatform имеет свои зависимости, отличные от ICommandRepo. Вы можете легко разрешить и инъектировать эти зависимости с помощью IServiceProvider внутри области служб.
                * В целом, использование IServiceScopeFactory и создание области служб позволяет управлять жизненным циклом зависимостей, обеспечивать изолированность и обрабатывать исключения эффективным способом.
    * в файл **Program.cs** регистрируем наш сервис для обработки принимаемых асинхронных сообщений:
        * *builder.Services.AddSingleton<IEventProcessor, EventProcessor>();*
        * Смотри коммит: [Event Processor : DetermineEvent and ProcessEvent](https://github.com/STGorbunovDA/.NET-Microservices/commit/afdea5791a3fcfa9ab7ed06b50e34a43c2474fe8)  
    * Внедряем наш сервис-слушатель приёма асинхронных сообщений ***MessageBusSubscriber.cs:***
        * Смотри коммит: [Add MessageBusSubscriber](https://github.com/STGorbunovDA/.NET-Microservices/commit/4314733551b59f1c20255377a8d60b026650e431) 
        * Класс *MessageBusSubscriber* является фоновым сервисом, который выполняет подписку на события в *RabbitMQ* и обрабатывает полученные события с помощью объекта *IEventProcessor*. Он служит для организации асинхронной обработки данных из *Message* *Bus* с использованием паттерна *Publish*-*Subscribe* на основе *RabbitMQ*.
    * в файл **Program.cs** регистрируем наш сервис для приёма асинхронных сообщений:
        * *builder.Services.AddHostedService<MessageBusSubscriber>();*
        * Смотри коммит: [Implementation AddHostedService()](https://github.com/STGorbunovDA/.NET-Microservices/commit/319052bd1b5ab1250baa3e12ac89e9961cd3f657)
    
* Перейдём к тестированию:
    * запусти оба сервиса ***PlatformService*** и затем ***CommandsService***
        * в консоли ***CommandsService*** должно появится сообщение о прослушивании:

            ![Asynchronous_Events_RabbitMQ_42](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/42.png)

    * через *Swagger* или *Insomnia* или *Postman* протестируй отправку сообщений через RabbitMQ:
        * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку **Local Dev/ *Platform* Service** и Выполни *CreatePlatform* не забывая поменять порт
        * в консоли ***CommandsService*** появится сообщения о том что мы установили соединение и приняли синхронное сообщение через *HttpCommandDataClient* и асинхронное сообщение от ***RabbitMQ,*** а так же отправили сообщение для обработки, для создания платформы в БД:

             ![Asynchronous_Events_RabbitMQ_43](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/43.png)
        * Далее выполни запрос из папки **Local Dev/ *Commands Service*** и Выполни *GetPlatforms* не забывая поменять порт
        * из БД (“***InMen***”) ***CommandsService*** должны получить созданную нами Платформу полученную из ***PlatformService*** при выполнении первого запроса *CreatePlatform* в **Local Dev/ *Platform* Service:**

            ![Asynchronous_Events_RabbitMQ_44](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/44.png)
        * Далее выполни запрос на назначение данной платформе команды **Local Dev/ *Commands Service*** и Выполни *CreateCommandForPlatform* не забывая поменять порт:

            ![Asynchronous_Events_RabbitMQ_45](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/45.png)
        * Далее выполни запрос на получение всех команд у платформы **Local Dev/ *Commands Service*** и Выполни *GetCommandsForPlatform*не не забывая поменять порт:

            ![Asynchronous_Events_RabbitMQ_46](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/46.png)

* Открываем проект ***PlatformService***:
    * ***PlatformService*** *билдим* в образ **Docker** и *пушим* его на *DockerHub*
        * т.к. Docker был перезалит то необходимо перезапустить ***Kubernetes*** обновляя новый образ из ***DockerHub*** при условии если контейнер запущен:
            * *kubectl rollout restart deployment platforms-depl*
                * развертывание с именем "*platforms-depl*", обновляя все его поды на новые версии или сбрасывая текущие состояния, чтобы применить любые изменения в настройках или ресурсах
* Открываем проект ***CommandsService***:
    * ***CommandsService*** *билдим* в образ **Docker** и *пушим* его на *DockerHub*
        * т.к. Docker был перезалит то необходимо перезапустить ***Kubernetes*** обновляя новый образ из ***DockerHub*** при условии если контейнер запущен:
            * *kubectl rollout restart deployment commands-depl*
                * развертывание с именем "*commands-depl*", обновляя все его поды на новые версии или сбрасывая текущие состояния, чтобы применить любые изменения в настройках или ресурсах

* Далее перейдём к тестированию обмена сообщений между сервисами с помощью ***RabbitMQ***:
    * через *Swagger* или *Insomnia* или *Postman* протестируй отправку сообщений через RabbitMQ:
        * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку **K8S/ *Platform Service(Nginx)*** и Выполни *CreatePlatform* не забывая поменять порт и в контейнере *platforms-depl* мы увидим следующее:

            ![Asynchronous_Events_RabbitMQ_47](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/47.png)
        *  а на localhost:15672 в RabbitMQ мы увидим визуальный график нашего сообщения:

            ![Asynchronous_Events_RabbitMQ_48](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/48.png)        
        * и затем в контейнере *commands-depl должны получить сообщение о том что наша платформа добавлена в контекст БД “InMem”:*

            ![Asynchronous_Events_RabbitMQ_49](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/49.png) 

    * Далее проверь все вызовы находящиеся в папке папку **K8S/ *Commands Service(Nginx)*** с учётом добавленной платформы из сервиса ***PlatformService*** контейнера *platforms-depl*

* Визуализируем что мы получили:

    ![Asynchronous_Events_RabbitMQ_50](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/50.png) 

            