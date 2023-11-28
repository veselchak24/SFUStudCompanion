from flask import Flask
from flask_restful import Api
from flask import request
from bd import *


app = Flask(__name__)
api = Api()


@app.errorhandler(404)
def page_not_found(e):

    return f'''{e}<br><br><br>/tags/get - Get information about one tag.<br>Params: name.
    <br>The param "name=all" allows you to get all the tags.<br><br>
    
    /tags/del - Delete information about one tag.<br>Params: name.
    <br>The param "name=all" allows you to delete all the tags.<br><br>
    
    /tags/add - Add information about one tag.<br>Params: name, description, color and type_.<br><br>
    '''


@app.route('/tags/get', methods=['GET'])
def route_get_tag():

    tag_name = request.args.to_dict().get('name', None)

    if tag_name is not None:

        if tag_name == 'all':
            return get_tags()
        else:
            return get_tag(tag_name)

    return 'There must be a parameter name "name'


@app.route('/tags/add/', methods=['GET'])
def route_add_tag():

    args_dict = request.args.to_dict()

    if all(map(args_dict.__contains__, ('name', 'description', 'color', 'type_'))):
        add_tag(args_dict['name'], args_dict['description'], args_dict['color'], args_dict['type_'])
        return 'ok'
    else:
        return 'not all arguments are used<br><br><b>name, description, color, type_</b>'


@app.route('/tags/del', methods=['GET'])
def route_del_tag():

    tag_name = request.args.to_dict().get('name', None)

    if tag_name is not None:
        if tag_name == 'all':
            for i in get_tags():
                del_tag(i['name'])
        else:
            del_tag(tag_name)

    return 'ok'


if __name__ == '__main__':

    app.run(debug=True, port=3000, host='127.0.0.1')

# Изменить тип цвета на строку, 