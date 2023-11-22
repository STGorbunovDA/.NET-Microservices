# Kubernetes для PlatformService

1. Переходим в корневую папку нашего решения ***.NET-Microservices.sln***
2. Создаём папку ***K8S***
3. В папке ***K8S*** создаём манифест ***platforms-depl.yaml:***
    * Данный манифест создает развертывание ***Kubernetes*** с именем "***platforms-depl***", которое будет создавать и поддерживать 1 реплику подов в кластере и сервис ***ClusterIP** для общения между сервисами*. Развертывание будет управлять подами, которые имеют метку 
    "***app***: ***platformservice***
    * смотри коммиты: 
     [Creating Kubernetes Manifests](https://github.com/STGorbunovDA/.NET-Microservices/commit/ae50b1dab63d93da2613c3213fdfb4539c0f91f2) и
     [Add ClusterIP in platforms-depl.yaml](https://github.com/STGorbunovDA/.NET-Microservices/commit/bd8db5ed301d3c3873c69ca1d6931d53ec7c3464)
     * Не забудь поменять своё зареганное доменное имя на **https://hub.docker.com** что бы скачать контейнер**:

        ![Kubernetes_6](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/6.png)

    * Визуализируем, что должно получится:

         ![Kubernetes_7](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/7.png)
            
    * Проверяем командой версию ***Kubernetes*:**
        * *kubectl version*
        * должно получится:

         ![Kubernetes_8](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/8.png)
4. Переходим в папку **K8S** и используем команду: 
    * *kubectl apply -f platforms-depl.yaml*
    * команда применяет конфигурационный файл ***platforms-depl.yaml*** для создания или обновления ресурсов (например, развертывания) в **Kubernetes**-кластере
5. Используем командЫ: 
    * *kubectl get deployments*
        * команда используется для получения списка всех развертываний (**deployments**) в текущем контексте **Kubernetes**
    * *kubectl get pods*
        * команда используется для получения списка всех подов (поды), которые текущий пользователь имеет разрешение видеть в текущем контексте ***Kubernetes***
6. Удаляем предыдущие не запущенные контейнеры
7. Открываем программу ***Docker*** должно отобразится два файла:

    ![Kubernetes_9](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/9.png)
    * Первый это кластер работающего контейнера
    * Второй сам работающий контейнер:

        ![Kubernetes_10](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/10.png)
        * Если удалить контейнер или кластер то ***Kubernetes*** его автоматически перезапустит без потери данных. Для того что ***Kubernetes*** создавал больше дубликатов контейнеров в файле ***platforms-depl.yaml*** необходимо указать больше реплик (например когда наш сервис перегружен входящим трафиком):

            ![Kubernetes_11](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/11.png)
        * и заново пересобрать наш манифест командой:
            * *kubectl apply -f platforms-depl.yaml*
    * Что бы полностью удалить контейнер ***Kubernetes*** необходимо воспользоваться командой:
        * *kubectl delete deployment ***platforms-depl****
            * где выделено название нашего кластера pods-ов(контейнеров), которое можно посмотреть введя команду:
                * *kubectl get deployments*
    * В папке ***K8S*** создаём манифест ***platforms-np-srv.yaml*:**
        * данный манифест создает службу ***Kubernetes*** с именем "***platformnpservice-srv***", типом ***NodePort***, прослушивающую порт 80 и направляющую трафик на порт 80 подов с меткой "***app***: ***platformservice***
        * смотри коммит: 
        [Creating Kubernetes Manifests](https://github.com/STGorbunovDA/.NET-Microservices/commit/ae50b1dab63d93da2613c3213fdfb4539c0f91f2)
8. Визуализируем, что должно получится:

    ![Kubernetes_4](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/4.png)
9. Переходим в папку **K8S** и используем команду:
    * *kubectl apply -f platforms-np-srv.yaml*
    * команда создаёт службу ***Kubernetes*** с именем "***platformnpservice-srv***", типом ***NodePort***, прослушивающая порт 80 и направляющая трафик на порт 80 подов с меткой "***app***: ***platformservice***
    * Используем команду:
        * *kubectl get services*
        * команда выводит список всех служб **Kubernetes**, которые были созданы в кластере.
        * должно получится: 

        ![Kubernetes_12](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/12.png)
        * что бы полностью удалить сервис ***Kubernetes*** необходимо воспользоваться командой:
            * *kubectl delete service ***platformnpservice-srv****
            * где выделено название нашей сервиса контейнеров Node Port, которое можно посмотреть введя команду:
                * *kubectl get services*
10. Далее необходимо проверить через *Swagger* или *Insomnia* или *Postman* методы контроллера **PlatformsController.cs** через созданный ***Kubernetes***-кластер:
    * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку ***K8S / Platform Service(Node Port)*** и поменяй порт маршрута внутри используемой программы, на соответствующий созданному сервису *NodePort*:

            ![Kubernetes_13](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/13.png)
            
            ![Kubernetes_14](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/14.png)
            * выполни запрос *GetPlatforms*