# GameLibrary WebApi
## Использованные пакеты:
### Для тестирования:
- FakeItEasy
- InMemory
- xUnit
### Для WebApi:
- AutoMapper
- AutoMapper.Extensions.Microsoft.DependencyInjection
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore
### В Api реализованы CRUD операции с играми.
- Создание,
- Получение списка игр с возможностью фильтрации по жанру,
- Изменение,
- Удаление.
### Запись об игре содержит данные:
- Название,
- Студия разработчик,
- 1 или более жанров, которым соответствует игра.
