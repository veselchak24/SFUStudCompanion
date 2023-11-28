import sqlite3
import time

db = sqlite3.connect('tags.sql', check_same_thread=False)
cursor = db.cursor()

cursor.execute('''CREATE TABLE IF NOT EXISTS tags (
    name TEXT NOT NULL,
    description TEXT,
    date INTEGER,
    color TEXT,
    type TEXT NOT NULL)''')
db.commit()


# Getting information about one tag
def get_tag(name) -> dict:

    """Получение данных о метке по ее имени -> {name, description, date, color, type_}."""

    data = cursor.execute('SELECT * FROM tags WHERE name = ?', (name,)).fetchone()
    if data is not None:
        return {'name': data[0], 'description': data[1], 'date': data[2], 'color': data[3], 'type_': data[4]}
    else:
        return {}


# Getting information about all tags
def get_tags() -> list:

    """Получение данных обо всех метках -> [{name, description, date, color, type_},]."""

    data = cursor.execute('SELECT name FROM tags').fetchall()
    if data is not None:
        return [get_tag(tag[0]) for tag in data]
    else:
        return []


# Adding the new tag
def add_tag(name, description, color, type_) -> None:

    """Добавление информации о метке."""

    if get_tag(name) == {}:
        cursor.execute('INSERT INTO tags VALUES (?, ?, ?, ?, ?)', (name, description, int(time.time()), color, type_))
        db.commit()


def del_tag(name):

    """Удаление метки по ее имени."""

    cursor.execute('DELETE FROM tags WHERE name = ?', (name,))
    db.commit()