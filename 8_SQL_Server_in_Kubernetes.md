# Создаём SQL Server in Kubernetes

* Добавим *Persistent Volume Claim (PVC)* - это объявление, которое используется в *Kubernetes* для запроса доступа к постоянному тому (*Persistent Volume*). *PVC* определяет требования к хранилищу данных, такие как доступный размер, режим доступа (*Read/Write*), класс хранилища и т.д. Он позволяет приложениям в *Kubernetes* запрашивать и использовать постоянное хранилище независимо от конкретной реализации хранилища. После создания *PVC*, *Kubernetes* будет искать и связывать его с соответствующим *Persistent* *Volume*, который соответствует требованиям, описанным в PVC.
1. Зайдём в папку **K8S** и создадим манифест ***local-pvc.yaml:***
    * Он определит *Persistent Volume Claim (PVC)* для использования с ресурсом хранения данных в Kubernetes.
    * смотри коммит: [Add PersistentVolumeClaim](https://github.com/STGorbunovDA/.NET-Microservices/commit/3764be93434a38a3d3d7f49021cbf875e705078a)
    * Выполним развёртывание контейнера(кластера):
        * *kubectl apply -f local-pvc.yaml*
    * Запускаем команду:
        * *kubectl get pvc*
            * возвращает список всех Persistent Volume Claims, которые были созданы 
            в Kubernetes кластере.
2. Далее создадим секрет с именем "mssql" и сохраним в нем литеральное значение пароля (SA\_PASSWORD) для использования в приложении или развертывании базы данных Microsoft SQL Server в Kubernetes кластере:
    * *kubectl create secret generic mssql --from-literal=SA\_PASSWORD="***pa55w0rd!***"*
        * пароль можно указать свой
3. Далее создадим ещё один манифест *mssql-plat-depl.yaml* в папке ***K8S***:
    * Данный манифест описывает конфигурацию развертывания и сервиса для базы данных ***Microsoft SQL Server*** в ***Kubernetes*** кластере. Развертывание создает одну реплику контейнера с образом *mcr.microsoft.com/mssql/server:2017-latest*, настроенной с необходимыми переменными окружения, включая пароль *SA\_PASSWORD* из ранее созданного секрета "mssql". Также определяется точка монтирования для постоянного хранения данных базы данных. Сервисы *mssql-clusterip-srv* и *mssql-loadbalancer* предоставляют доступ к контейнеру с использованием *ClusterIP* и *LoadBalancer* соответственно.
    * смотри коммит: [Create a SQL Sever deployment](https://github.com/STGorbunovDA/.NET-Microservices/commit/69c124ff6f3f8c63c15ef535ece2e22a4228838f)
4. Выполним развёртывание созданного контейнера:
    * *kubectl apply -f mssql-plat-depl.yaml*
5. Запускаем команду:
    * *kubectl get services*
    * должно получится:

    ![SQL_Server_in_Kubernetes_26](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/26.png)
    * *kubectl get pods*
    * должно получится:

    ![SQL_Server_in_Kubernetes_27](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/27.png)
    * Ждем когда **Kubernetes** скачает все необходимые пакеты и развернётся кластер ***MSSQL***
6.  После того как контейнер развернётся необходимо зайти в *MSSQL Server Management Studio* ввести:
    * login: ***sa***
    * Password: ***pa55w0rd!***
        * *Очень плохая практика при использовании такого логина и пароля.*
7. Проверим работоспособность ***Kubernetes*** и сохранения БД:
    * в *MSSQL Server Management Studio* создадим тестовую БД.
    * в программе докер удали контейнер развёртывания *mssql-plat-depl:*

    ![SQL_Server_in_Kubernetes_28](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/28.png)

    * ждём восстановления контейнера.
    * заходим в *MSSQL Server Management Studio* и убеждаемся что тестовая БД присутствует.
    * Удалим тестовую БД.
8. Открываем проект ***PlatformService***:
    * Добавляем строку подключения для доступа к контейнеру ***Kubernetes*** в файле *appsettings.Production.json:*
        * смотри коммит: [Create a SQL Sever deployment 2](https://github.com/STGorbunovDA/.NET-Microservices/commit/a48ddbed6741ffd742e1677a50b047a045dd78f0)
        * нельзя указывать логин и пароль для доступа к БД в открытом доступе.
    * Далее отредактируем файл *Program.cs*:
        * Добавим выбор для доступа к базе данных, за счёт конфигурации web-приложения, которое использует ***Entity Framework***. В зависимости от того, находится ли приложение в производственной среде или нет, оно будет использовать либо ***SQL Server***, используя соответствующую строку подключения "*PlatformsConn*", либо встроенную (в памяти) базу данных с именем "*InMem*". Это необходимо для миграции БД, когда развернём контейнер в ***Kubernetes.***
        * смотри коммит: [Create a SQL Sever deployment 2](https://github.com/STGorbunovDA/.NET-Microservices/commit/a48ddbed6741ffd742e1677a50b047a045dd78f0)
    * Далее отредактируем метод статического класса PrepDb. PrepPopulation:
        * Добавим isProd - флаг, указывающий, находится ли приложение в продакшн-окружении и если истина то выполним миграцию.
        * смотри коммит: [Update Database Preparation](https://github.com/STGorbunovDA/.NET-Microservices/commit/8199c2646571dace4d70a56f80098fa69979a169)
    * Так же внесём изменения в *Program.cs* путем добавления:
        * *PrepDb.PrepPopulation(app, **app.Environment.IsProduction()**);*
    * Добавляем строку подключения для доступа к контейнеру ***Kubernetes*** в файле    *appsettings.Development.json:*
        * нельзя указывать напрямую логин и пароль для доступа к БД.
        * смотри коммит: [Create Migrations](https://github.com/STGorbunovDA/.NET-Microservices/commit/c8c2129dfa91a12689a2a68b5639590180b6e283)
    * Убеждаемся, что всё ещё в проекте ***PlatformService*** и сгенерируем initialmigration:
        * Временно оставим только этот код для доступа к ***SQL Server***, а не к ***"InMem"***:     
            ```
            builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
            ```
        * и закомментируем в классе *Program.cs* строчку кода:
            ```
            PrepDb.PrepPopulation(app, app.Environment.IsProduction());
            ```
        * Перейдем в консоль и выполним эту команду:
            * *dotnet ef migrations add initialmigration*
                * должно получится два файла, список инструкций, указывающий *SQL Server* как создавать таблицы и тд.
        * восставливаем исходное состояние кода.
    * Убеждаемся, что всё ещё в проекте ***PlatformService:***
        * ***PlatformService*** *билдим* в образ **Docker** и *пушим* его на *DockerHub*
        * т.к. Docker был перезалит то необходимо перезапустить ***Kubernetes*** обновляя новый образ из ***DockerHub*** при условии если контейнер запущен:
            * *kubectl rollout restart deployment platforms-depl*
                * развертывание контейнера с именем "*platforms-depl*", обновляя все его поды на новые версии или сбрасывая текущие состояния, чтобы применить любые изменения в настройках или ресурсах
9. Перейдём к тестированию:
    * через *Swagger* или *Insomnia* или *Postman* протестируй методы контроллера **PlatformsController.cs** через созданный ***Kubernetes*** и *ClusterIP* между сервисами:
        * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку ***K8S / Platform Service(Nginx)***:
            * Выполни GetPlatforms
            * Выполни CreatePlatform
        * после выполнения запроса CreatePlatform в БД(mssqldb) должна создаться новая платформа.
        * заходим в *MSSQL Server Management Studio* и убеждаемся в этом.
10. Визуализируем что должно получится, выделено красным цветом:

     ![SQL_Server_in_Kubernetes_25](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/25.png)
         

