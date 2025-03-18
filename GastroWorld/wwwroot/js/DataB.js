// database.js
const mysql = require('mysql');

// Configuración de la conexión a la base de datos
const connection = mysql.createConnection({
    host: 'localhost',
    user: 'tu_usuario',
    password: 'tu_contraseña',
    database: 'gastroworld'
});

// Función para obtener el nombre de usuario actual
async function getCurrentUsername() {
    const user = await getLoggedInUser();
    if (user) {
        return user.username;
    }
    return 'Usuario Anónimo';
}

// Función para obtener el usuario actual
function getLoggedInUser() {
    // Aquí debes implementar la lógica para obtener el usuario actual
    // (por ejemplo, a través de una sesión o un token de autenticación)
    return new Promise((resolve) => {
        // Ejemplo de usuario hardcodeado
        resolve({ uid: '1234', username: 'JuanPerez' });
    });
}

// Función para guardar una publicación en la base de datos
async function savePost(content, image) {
    const username = await getCurrentUsername();
    const postData = {
        username,
        content,
        image,
        comments: [],
        timestamp: new Date(),
        likes: 0
    };

    return new Promise((resolve, reject) => {
        connection.query(
            'INSERT INTO posts (username, content, image, comments, timestamp, likes) VALUES (?, ?, ?, ?, ?, ?)',
            [username, content, image, JSON.stringify([]), postData.timestamp, 0],
            (error, result) => {
                if (error) {
                    reject(error);
                } else {
                    resolve(result.insertId);
                }
            }
        );
    });
}

// Función para cargar las publicaciones de la base de datos
async function loadPosts() {
    return new Promise((resolve, reject) => {
        connection.query(
            'SELECT * FROM posts ORDER BY timestamp DESC',
            (error, results) => {
                if (error) {
                    reject(error);
                } else {
                    resolve(results);
                }
            }
        );
    });
}

export { getCurrentUsername, savePost, loadPosts };
