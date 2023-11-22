# Создаём Synchronous Messaging между сервисами PlatformService и CommandsService

* смотри коммит: [Реализация базовой простой платформы отправки сообщений между серверами]https://github.com/STGorbunovDA/.NET-Microservices/commit/d2c6acc65fa8ec293356941b0fb3883627e46ed4)

1. Переходим к сервису ***PlatformService***
    * Создаём *SyncDataServices*/*Http*/***ICommandDataClient.cs***
    * Создаём *SyncDataServices*/*Http*/***HttpCommandDataClient.cs***
        * В классе ***HttpCommandDataClient.cs***, который является клиентом для отправки данных по ***HTTP***. В методе ***SendPlatformToCommand*** передается объект ***PlatformReadDto***, который сериализуется в формат JSON и отправляется в виде POST-запроса с использованием ***HttpClient***. Ответ сервера проверяется на успешность и выводится соответствующее сообщение в консоль.
    * Отредактируем конфигурационный файл *appsettings.Development.json:*
        * добавим маршрут сервиса ***CommandsService*** для отправки сообщения:
            * ***CommandService***": [*http://localhost:5029/api/c/platforms/*](http://localhost:5029/api/c/platforms/) - смотрим порт у ***CommandService***
    * В файл **Program.cs** регистрируем наш сервис ***HttpClient*** для отправки сообщения ***CommandsService***:
        * *builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();*
    * Внедрим в контроллер ***PlatformsController.cs => ICommandDataClient.cs*** и реализуем метод синхронной (т.к. метод ***ICommandDataClient.SendPlatformToCommand.cs*** асинхронный) отправки сообщения при добавлении какой-либо платформы.
    * Закоммитим в **Program**.**cs** //*app.UseHttpsRedirection();*
    * Добавим конфигурационный файл *appsettings.Production.json в **PlatformService:***
        * "*CommandService*": [*http://commands-clusterip-srv:80/api/c/platforms/*](http://commands-clusterip-srv:80/api/c/platforms/)
    * ***PlatformService*** Билдим в образ ***Docker*** и Пушим его на DockerHub 
         * *kubectl rollout restart deployment platforms-depl*
            * т.к. ***Docker*** был перезалит то необходимо перезапустить ***Kubernetes*** обновляя новый образ из ***DockerHub*** при условии если контейнер запущен:
                * развертывание с именем "*platforms-depl*", обновляя все его поды на новые версии или сбрасывая текущие состояния, чтобы применить любые изменения в настройках или ресурсах.

2. Переходим в папку **K8S** и создаём манифест **commands-depl.yaml:**
    * Данный манифест создает развертывание **Kubernetes** с именем "***commands-depl***", которое будет создавать и поддерживать 1 реплику подов в кластере и сервис ***ClusterIP** для общения между сервисами.* Развертывание будет управлять подами, которые имеют метку "***app***: ***commandservice***
    * смотри коммит: [Add commands-depl.yaml for Kubernetes](https://github.com/STGorbunovDA/.NET-Microservices/commit/b2774347da0667be051f4bd5b3a408f1d8037ede)
    * не забудь поменять своё зареганное доменное имя на **hub.docker.com** что бы скачать контейнер**:**
        ![SynchronousMessaging_18](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/18.png)
    * Запускаем кластер:
        * *kubectl apply -f commands-depl.yaml* 
            * команда применяет конфигурационный файл *commands-depl.yaml* для создания или обновления ресурсов (например, развертывания) в ***Kubernetes***-кластере.
    * Используем команды:
        * *kubectl get deployments*
            * команда используется для получения списка всех развертываний (**deployments**) в текущем контексте **Kubernetes**.*
        * *kubectl get pods*
            * команда используется для получения списка всех подов (поды), которые текущий пользователь имеет разрешение видеть в текущем контексте ***Kubernetes***.
3. И если два кластера и два *podsa* созданы:
        ![SynchronousMessaging_19](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/19.png)
4. Далее необходимо проверить через *Swagger* или *Insomnia* или *Postman* методы контроллера **PlatformsController.cs** через созданный ***Kubernetes***-кластер и ***ClusterIP*** между сервисами:
    * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку ***K8S / Platform Service(Node Port)*** и поменяй порт маршрута внутри используемой программы, на соответствующий созданному сервису *NodePort*:
            ![SynchronousMessaging_13](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/13.png)
        * Выполни ***CreatePlatform***
5. Должно получится в ***Kubernetes-***кластере** *platforms-depl*:
         ![SynchronousMessaging_20](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/20.png)
6. Должно получится в ***Kubernetes-***кластере ***commands-depl***:
        ![SynchronousMessaging_21](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/21.png)
7. Визуализируем, что должно получится:
        ![SynchronousMessaging_15](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/15.png)