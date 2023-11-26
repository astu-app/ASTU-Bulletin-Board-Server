# Содержимое объявлений

Объявление содержит следующие элементы:
- текст
- автор
- набор получателей
- флаг автоматического сокрытия
	- срок автоматического сокрытия
- флаг отложенной публикации
	- срок отложенной публикации
- набор прикрепленных файлов
- прикрепленный опрос

## Структура в БД
- id
- текст
- id автора
- набор id групп получателей (отдельная таблица многие-ко-многим)
- флаг автоматического сокрытия
- срок автоматического сокрытия
- дата отложенной публикации
- срок отложенной публикации
- набор id прикрепленных файлов (отдельная таблица многие-ко-многим)
- id прикрепленного опроса



