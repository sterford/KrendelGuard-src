from flask import Flask, render_template, request, jsonify, redirect, url_for, session
from flask_sqlalchemy import SQLAlchemy
import os
from datetime import datetime
import logging

app = Flask(__name__)
app.secret_key = 'your_secret_key_here'
app.config['UPLOAD_FOLDER'] = 'static/uploads/'
app.config['SQLALCHEMY_DATABASE_URI'] = 'postgresql://krendelguard:THImzhfs0Uk9V6E1kf1JNuMexvPfIqLy@dpg-cs9qt488fa8c73cfcukg-a.oregon-postgres.render.com/craken'
db = SQLAlchemy(app)

# Настройка логирования
logging.basicConfig(level=logging.DEBUG)

@app.errorhandler(404)
def page_not_found(e):
    return render_template('404.html'), 404

# Модели базы данных
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True, nullable=False)
    password = db.Column(db.String(80), nullable=False)
    rol = db.Column(db.String(50), nullable=False)
    hwid = db.Column(db.String(255), nullable=True)
    licence = db.Column(db.String(255), nullable=True)
    regdate = db.Column(db.DateTime, nullable=False)
    is_activated = db.Column(db.Boolean, default=False)
    privyazka_hwid = db.Column(db.Boolean, default=False)

class Key(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    key = db.Column(db.String(255), unique=True, nullable=False)
    status = db.Column(db.String(50), nullable=False)

# Создание таблиц
with app.app_context():
    db.create_all()

# Загрузка пользователей из базы данных
def load_users():
    return User.query.all()

# Сохранение пользователей в базу данных
def save_users(users):
    for user in users:
        db.session.add(user)
    db.session.commit()

# Загрузка ключей из базы данных
def load_keys():
    return Key.query.all()

# Сохранение ключей в базу данных
def save_keys(keys):
    for key in keys:
        db.session.add(key)
    db.session.commit()

# Function to activate a key
def activate_key(key, username):
    keys = load_keys()
    users = load_users()
    key_activated = False

    for k in keys:
        if k.key == key and k.status == 'not activated':
            k.status = 'activated'
            key_activated = True
            break

    if key_activated:
        for user in users:
            if user.username == username:
                user.licence = key
                user.is_activated = True
                break

        save_keys(keys)
        save_users(users)
        return True

    return False

# Register a new user
@app.route('/register', methods=['GET', 'POST'])
def register():
    if request.method == 'POST':
        username = request.form['username']
        email = request.form['email']
        password = request.form['password']

        existing_user = User.query.filter_by(username=username).first()

        if existing_user:
            return redirect(url_for('register'))

        new_user = User(
            username=username,
            email=email,
            password=password,
            rol='User',
            hwid='No data...',
            licence='No data...',
            regdate=datetime.now(),
            is_activated=False,
            privyazka_hwid=False
        )

        db.session.add(new_user)
        db.session.commit()

        return redirect(url_for('login'))

    return render_template('register.html')

# API-эндпоинт для изменения параметра is_activated
@app.route('/api/update_is_activated', methods=['PUT'])
def update_is_activated():
    data = request.json
    username = data.get('username')
    is_activated = data.get('is_activated')

    user = User.query.filter_by(username=username).first()
    if user:
        user.is_activated = is_activated
        db.session.commit()
        return jsonify({'status': 'success', 'message': 'is_activated updated successfully'})

    return jsonify({'status': 'error', 'message': 'User not found'})

# Login an existing user
@app.route('/login', methods=['GET', 'POST'])
def login():
    if 'logged_in' in session:
        return redirect(url_for('user_panel'))

    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']

        user = User.query.filter_by(username=username, password=password).first()

        if user:
            session['logged_in'] = True
            session['userdata'] = {
                'id': user.id,
                'username': user.username,
                'email': user.email,
                'rol': user.rol,
                'hwid': user.hwid,
                'licence': user.licence,
                'regdate': user.regdate,
                'is_activated': user.is_activated,
                'privyazka_hwid': user.privyazka_hwid
            }
            session['is_activated'] = user.is_activated
            if user.rol == 'Admin':
                return redirect(url_for('admin_panel'))
            else:
                return redirect(url_for('user_panel'))
        else:
            return redirect(url_for('login'))

    return render_template('login.html')

# API endpoint for user login
@app.route('/api/login', methods=['POST'])
def api_login():
    username = request.form['username']
    password = request.form['password']
    user = User.query.filter_by(username=username, password=password).first()
    if user:
        return jsonify({
            'status': 'success',
            'message': 'Авторизация прошла успешно'
        })
    else:
        return jsonify({
            'status': 'error',
            'message': 'Неверный логин или пароль'
        })

@app.route('/api/check_subscription', methods=['POST'])
def api_check_subscription():
    username = request.form['username']
    user = User.query.filter_by(username=username).first()
    if user and user.is_activated:
        return jsonify({'status': 'success', 'is_activated': True})
    else:
        return jsonify({'status': 'error', 'is_activated': False, 'message': 'Подписка не активна'})

@app.route('/api/update_hwid', methods=['POST'])
def api_update_hwid():
    username = request.form['username']
    hwid = request.form['hwid']
    user = User.query.filter_by(username=username).first()
    if user:
        if user.hwid == hwid:
            return jsonify({'status': 'success', 'message': 'HWID уже привязан и совпадает'})
        if not user.privyazka_hwid:
            user.hwid = hwid
            user.privyazka_hwid = True
            db.session.commit()
            return jsonify({'status': 'success', 'message': 'HWID успешно обновлен'})
        else:
            return jsonify({'status': 'error', 'message': 'HWID уже привязан'})
    else:
        return jsonify({'status': 'error', 'message': 'Пользователь не найден'})

@app.route('/api/check_hwid', methods=['POST'])
def api_check_hwid():
    username = request.form['username']
    hwid = request.form['hwid']
    user = User.query.filter_by(username=username).first()
    if user and user.hwid == hwid:
        return jsonify({'status': 'success', 'message': 'HWID проверен успешно'})
    else:
        return jsonify({'status': 'error', 'message': 'Неверный HWID'})

@app.route('/user_panel', methods=['GET', 'POST'])
def user_panel():
    activation_message = None
    activation_message_type = None

    if request.method == 'POST':
        key = request.form['key']
        if activate_key(key, session['userdata']['username']):
            activation_message = "Ключ успешно активирован!"
            activation_message_type = "success"
        else:
            activation_message = "Ключ уже активирован или не существует"
            activation_message_type = "danger"

    return render_template('user_panel.html', user=session['userdata']['username'], id=session['userdata']['id'],
                           mail=session['userdata']['email'], prezik=session['userdata']['regdate'],
                           rol=session['userdata']['rol'], hwid=session['userdata']['hwid'],
                           licence=session['userdata']['licence'], activation_message=activation_message,
                           activation_message_type=activation_message_type)

@app.route('/logout')
def logout():
    session.clear()
    return redirect(url_for('login'))

@app.route('/admin/users')
def admin_users():
    if 'logged_in' not in session or session['userdata']['rol'] != 'Admin':
        return redirect(url_for('login'))

    users = User.query.all()
    return render_template('admin_users.html', users=users)

@app.route('/admin/user/<int:user_id>', methods=['GET', 'POST'])
def admin_user(user_id):
    if 'logged_in' not in session or session['userdata']['rol'] != 'Admin':
        return redirect(url_for('login'))

    user = User.query.get_or_404(user_id)

    if request.method == 'POST':
        user.is_activated = request.form.get('is_activated') == 'True'
        user.rol = request.form.get('rol')
        db.session.commit()
        return redirect(url_for('admin_users'))

    return render_template('admin_user.html', user=user)

@app.route('/admin_panel')
def admin_panel():
    if session['userdata']['rol'] != 'Admin':
        users = load_users()
        if 'logged_in' not in session:
            loggedl = 'Войти / Зарегистрироваться'
        else:
            loggedl = 'Личный кабинет'
        return render_template('index.html', member=len(users), cabinetlabel=loggedl)

    if 'logged_in' not in session:
        return redirect(url_for('login'))
    return render_template('admin_panel.html', user=session['userdata']['username'], id=session['userdata']['id'],
                           mail=session['userdata']['email'], prezik=session['userdata']['regdate'],
                           rol=session['userdata']['rol'], hwid=session['userdata']['hwid'],
                           licence=session['userdata']['licence'])

@app.route('/api/get_user_data', methods=['POST'])
def api_get_user_data():
    username = request.form['username']
    user = User.query.filter_by(username=username).first()
    if user:
        return jsonify({
            'status': 'success',
            'username': user.username,
            'email': user.email,
            'hwid': user.hwid,
            'licence': user.licence,
            'regdate': user.regdate,
            'is_activated': user.is_activated,
            'privyazka_hwid': user.privyazka_hwid
        })
    else:
        return jsonify({'status': 'error', 'message': 'Пользователь не найден'})

@app.route('/shop')
def shop():
    if 'logged_in' not in session:
        return redirect(url_for('login'))
    return render_template('shop.html', user=session['userdata']['username'], id=session['userdata']['id'],
                           mail=session['userdata']['email'], prezik=session['userdata']['regdate'],
                           rol=session['userdata']['rol'], hwid=session['userdata']['hwid'],
                           licence=session['userdata']['licence'])

@app.route('/download_page')
def download_page():
    if 'logged_in' not in session:
        return redirect(url_for('login'))
    return render_template('download_page.html', user=session['userdata']['username'], id=session['userdata']['id'],
                           mail=session['userdata']['email'], prezik=session['userdata']['regdate'],
                           rol=session['userdata']['rol'], hwid=session['userdata']['hwid'],
                           licence=session['userdata']['licence'])

@app.route('/')
def index():
    users = load_users()
    if 'logged_in' not in session:
        loggedl = 'Войти / Зарегистрироваться'
    else:
        loggedl = 'Личный кабинет'
    return render_template('index.html', member=len(users), cabinetlabel=loggedl)

if __name__ == '__main__':
    app.run(debug=True)
