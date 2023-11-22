# Создаём CommandsService

1. Переходим в корневую папку нашего решения *.NET-Microservices.sln*
2. Создаём **WebApi** и добавляем к нашему решению *.NET-Microservices.sln:*
    * *dotnet new webapi -n ***CommandsService****
3. Открываем проект:
    * *Code -r CommandsService*
4. Удаляем все встроенные контроллеры и сущности из проекта ***CommandsService***
5. Добавляем пакеты в ***CommandsService***:
    - dotnet add package *AutoMapper.Extensions.Microsoft.DependencyInjection*
    - dotnet add package *Microsoft.EntityFrameworkCore*
    - dotnet add package *Microsoft.EntityFrameworkCore.Design*
    - dotnet add package *Microsoft.EntityFrameworkCore.InMemory*
    - dotnet add package *Microsoft.EntityFrameworkCore.SqlServer*
6. Проверяем в конфигурационном файле ***launchSettings**.**json** назначение портов, главное что бы они отличались от портов **PlatformService**, если что меняем на свои.*
7. Создаём ***Controllers/ PlatformsController.cs*** 
    * смотри коммит: [Add CommandsService](https://github.com/STGorbunovDA/.NET-Microservices/commit/9bb187f375a85fb4657e7f8ff82142f1554b92f9)
8. Закоммитим в ***Program.cs*** //app.UseHttpsRedirection();
9. Далее необходимо проверить через *Swagger* или *Insomnia* или *Postman* методы контроллера   ***PlatformsController*.*cs*:**
     * Имплементируй файл [.NET-Microservices.postman\_collection](https://github.com/STGorbunovDA/.NET-Microservices/tree/dev/postman) в любую из вышеперечисленных программ.
        * ООбрати внимание на папку ***Local Dev/ Commands Service*,** тебе необходимо заменить порт в маршрутах во всех файлах на соответствующий твоему приложению. *http://localhost:**5029**/api/c/platforms/* - обрати внимание на консоль твоего приложения после запуска или в файле конфигурации ***launchSettings*.*json*** 
        в *profiles* => "*applicationUrl*": "*http://localhost:5029*"
10. Создаём **Docker**-образ:
    * *docker build -t proxtreeme/ commandservice .* 
    * смотри коммит: [Add Dockerfile for CommandService](https://github.com/STGorbunovDA/.NET-Microservices/commit/e3fd13d58d4c694640584043729b2c3e11253bcf)
    * Исправь выделенный тег ***proxtreeme***, необходимо указать своё зареганное доменное имя на **hub.docker.com**
    * команду необходимо писать строчными буквами
    * не забывай про «.» в конце команды
    * она указывает на текущую директорию как контекст сборки, это означает, что все файлы и папки, находящиеся в текущей директории, будут доступны 
    в процессе сборки ***Docker***-образа.
    * Убедись что библиотека *obj\Debug\net7.0\ ***CommandsService.dll*** соответствует названию в файле ***Docker***:

         ![CommandsService_17](https://github.com/STGorbunovDA/.NET-Microservices/blob/dev/img/17.png)
11. Запускаем ***Docker***-контейнер командой:
    * *docker run -p 8080:80 proxtreeme/commandservice*
    * из образа "***proxtreeme***/ ***commandservice***", пробрасывая порт 8080 на хостовую машину к порту 80 контейнера, проверяя развёртывание в консоли(не в фоновом режиме).
    * останавливаем контейнер **Docker**
12. Пушим (отправляем в [**https://hub.docker.com**](https://hub.docker.com/)) созданный контейнер:
    * *docker push proxtreeme/commandservice*
    * Исправь выделенный тег «proxtreeme», необходимо указать своё зареганное доменное имя на **https://hub.docker.com**
    * команду необходимо писать строчными буквами
