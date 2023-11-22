# Nginx Ingress Load balancer

* Далее нам необходимо внедрить шлюз *Nginx Ingress Load balancer*
    * Переходим на <https://github.com/kubernetes/ingress-nginx>
    * Ищем ссылку *Get started*:
        * <https://kubernetes.github.io/ingress-nginx/deploy/>
    * Переходим на *Docker Desktop* и копируем команду:
        * kubectl apply -f <https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/aws/deploy.yaml>
1. Убеждаемся что мы в папке **K8S**:
    * в терминале вводим скопированную ссылку.
    * должны получить: 
        ![SynchronousMessaging_22](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/22.png)
    * Дождаться развёртывания контейнера и для того что бы увидеть в консоли все развёртывания необходимо обратится к команде:
        * *kubectl get namespace*
    * и далее путём сопоставления названия обратится к этому пространству имён:*
        * *kubectl get pods -–namespace=ingress-nginx*
    * после того как кластеры и подсы nginx развернуться необходимо удалить в приложении Docker неактивные контейнеры.*
    * смотрим какой порт у сервиса *nginx:*
        * *kubectl get services -–namespace=ingress-nginx*
    * В папке **K8S** создаём манифест ***ingress-srv.yaml:***
        *  файл маршрутизации который контроллер nginx будет использовать для определения маршрута к нашим сервисам
        * смотри коммит: [Add to configaration Ingress](https://github.com/STGorbunovDA/.NET-Microservices/commit/1f1c2aec4aca089266fd7d99d27f9b5b125920fb)
        * Необходимо в файле C:\Windows\System32\drivers\etc\hosts прописать маршрут:
        ![SynchronousMessaging_23](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/23.png)
        * Запускаем контейнер:
            * *kubectl apply -f ingress-srv.yaml* 
                * этот файл маршрутизации Ingress определяет, как контроллер Nginx будет направлять трафик к сервисам в зависимости от пути URL, задавая для каждого пути соответствующую службу и порт для перенаправления трафика.
2. Перейдём к тестированию *Nginx Ingress Load balancer и Pod Ingress Nginx Container*:
    * через *Swagger* или *Insomnia* или *Postman* протестируй методы контроллера **PlatformsController.cs** через созданный ***Kubernetes*** и *ClusterIP* между сервисами:
        * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * Обрати внимание на папку ***K8S / Platform Service(Nginx)***:
            * Выполни GetPlatforms
* Должно получится: 
     ![SynchronousMessaging_24](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/24.png)