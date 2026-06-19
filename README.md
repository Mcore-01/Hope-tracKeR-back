**Hope-tracKeR-back**  
REST API для учёта хозяйственных товаров (техника и расходники)  
Фронтенд: **Hope-tracKeR-front** 

Стек: .NET 8, PostgreSQL, EF Core, Swagger
-------------------
**Запуск**  
Docker
```
docker-compose up --build
```
* API: http://localhost:8080
* PostgreSQL: localhost:5433
* Миграции применяются автоматически при старте
* Swagger: http://localhost:8080/swagger

Локально
* Поднять PostgreSQL (порт 5433, параметры как в appsettings.json)
* API: http://localhost:5154
* Swagger: http://localhost:5154/swagger
-----------------
