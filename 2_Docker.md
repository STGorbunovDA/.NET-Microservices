# Docker

1. создаем ***Dockerfile*:**
    * смотри коммит: 
     [add Dockerfile](https://github.com/STGorbunovDA/.NET-Microservices/commit/f3ca477033c73d29d91b164f6d82b7e766a92124)
2. Регаемся на **https://hub.docker.com/**
3. Cкачиваем и устанавливаем **Docker**
4. Проверяем установлен ли **Docker:**
    * *docker –-version*
5. Проверяем включен ли ***Kubernetes***:
    ![Docker_2](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/2.png)
6. Создаём ***Docker***-образ проверяя находимся ли мы в папке ***PlatformService***:
    * *docker build -t ***proxtreeme***/platformservice .* 
    * Исправь выделенный тег ***proxtreeme***, необходимо указать своё зареганное доменное имя на **hub.docker.com**
    * команду необходимо писать строчными буквами
    * не забывай про «.» в конце команды
        * она указывает на текущую директорию как контекст сборки, это означает, что все файлы и папки, находящиеся в текущей директории, будут доступны 
        в процессе сборки **Docker**-образа.
    * убедись что библиотека *obj\Debug\net7.0\ ***PlatformService.dll*** соответствует названию в файле **Docker**:
        ![Docker_16](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/16.png)
        * установи ***Docker*** расширение в ***VS Code***
        * запускаем ***Docker***-контейнер командой:
            * *docker run -p 8080:80 -d ***proxtreeme***/platformservice*
                * из образа "***proxtreeme***/***platformservice***", пробрасывая порт 8080 на хостовую машину к порту 80 контейнера, в фоновом режиме (*detached mode*)
            * Команды **Docker:** 
                * *docker ps* выводим запущенные контейнеры
                * *docker stop e9aa89472296* останавливаем **Docker-контейнер,** где выделенное красным цветом идентификатор контейнера который необходимо смотреть командой *docker ps*
                * *docker start e9aa89472296* запуск **Docker-контейнера,** где выделенное красным цветом идентификатор контейнера который необходимо смотреть командой *docker ps*
       
        * Пушим (отправляем в [**https://hub.docker.com**](https://hub.docker.com/)) созданный контейнер:
            * *docker push ***proxtreeme***/platformservice*
            * Исправь выделенный тег ***proxtreeme***, необходимо указать своё зареганное доменное имя на **https://hub.docker.com**
            * команду необходимо писать строчными буквами
        * Далее необходимо проверить через *Swagger* или *Insomnia* или *Postman* методы контроллера ***PlatformsController*.cs** через созданный **Docker**-контейнер:
            * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
            * Обрати внимание на папку ***Docker Env/ Platform Service*,** выполни запрос
