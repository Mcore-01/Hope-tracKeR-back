# Hope-tracKeR-back
авторизация - админ|гость
помещения - склад|кабинеты
адресация помещений - филиал-корпус-этаж-кабинет
хозтовары - техника|расходники
особенности техники - клавиатура - новая/бу|картридж - пустой/заправленный

складской учет - приход|перемещение|выдача|списание
док-оборот - акт приема-передачи в ремонт|отчеты по технике - общее количество по филиалам, распределение по возрасту(до 1года, 1-3года, более 3 лет)
не подходит под инвентаризацию - провода, гарнитура

Hope-tracKeR-back
Hope-tracKeR-front

users - id, name, login, password
categories - id, name
brands - id, name
items - id, serial_id, name, status, added_date, address_id, category_id, brand_id
addresses - id, branch, building, floor, room
item_attributes - id, item_id, name, value
repairs - id, date, desc_failure, item_id, user_id, from_address_id, to_address_id
