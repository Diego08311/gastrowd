using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; // O System.Data.SqlClient según tu versión
using System.IO;

namespace GastroWorld.Models.Model
{
    public class Perfil
    {
        private int id;
        private string nombre;
        private string apellido;
        private string email;
        private string fotoPerfil;
        private string bio;
        private DateTime fechaRegistro;
        private List<object> amigos;
        private List<object> publicaciones;
        private readonly IDbConnection dbConnection;

        public Perfil(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            this.amigos = new List<object>();
            this.publicaciones = new List<object>();
        }

        public async Task<bool> CargarPerfilPorId(int id)
        {
            string query = "SELECT * FROM usuarios WHERE id = @Id";

            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@Id";
                parameter.Value = id;
                command.Parameters.Add(parameter);

                dbConnection.Open();
                using (var reader = await ((SqlCommand)command).ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        this.id = reader.GetInt32(reader.GetOrdinal("id"));
                        this.nombre = reader.GetString(reader.GetOrdinal("nombre"));
                        this.apellido = reader.GetString(reader.GetOrdinal("apellido"));
                        this.email = reader.GetString(reader.GetOrdinal("email"));
                        this.fotoPerfil = reader.GetString(reader.GetOrdinal("foto_perfil"));
                        this.bio = reader.GetString(reader.GetOrdinal("bio"));
                        this.fechaRegistro = reader.GetDateTime(reader.GetOrdinal("fecha_registro"));
                        dbConnection.Close();
                        return true;
                    }
                }
                dbConnection.Close();
                return false;
            }
        }

        public async Task<List<object>> ObtenerPublicaciones()
        {
            string query = "SELECT * FROM publicaciones WHERE usuario_id = @Id ORDER BY fecha DESC";
            List<object> publicaciones = new List<object>();

            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@Id";
                parameter.Value = this.id;
                command.Parameters.Add(parameter);

                dbConnection.Open();
                using (var reader = await ((SqlCommand)command).ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Crear objeto de publicación con los datos del reader
                        var publicacion = new
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Contenido = reader.GetString(reader.GetOrdinal("contenido")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                            // Añade más propiedades según la estructura de tu tabla
                        };

                        publicaciones.Add(publicacion);
                    }
                }
                dbConnection.Close();
            }

            this.publicaciones = publicaciones;
            return publicaciones;
        }

        public async Task<List<object>> ObtenerAmigos()
        {
            string query = @"SELECT u.* FROM usuarios u 
                           INNER JOIN amistades a ON (u.id = a.amigo_id AND a.usuario_id = @Id) 
                           OR (u.id = a.usuario_id AND a.amigo_id = @Id)
                           WHERE a.estado = 'aceptada'";

            List<object> amigos = new List<object>();

            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@Id";
                parameter.Value = this.id;
                command.Parameters.Add(parameter);

                dbConnection.Open();
                using (var reader = await ((SqlCommand)command).ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Crear objeto de amigo con los datos del reader
                        var amigo = new
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            FotoPerfil = reader.GetString(reader.GetOrdinal("foto_perfil")),
                            // Añade más propiedades según necesites
                        };

                        amigos.Add(amigo);
                    }
                }
                dbConnection.Close();
            }

            this.amigos = amigos;
            return amigos;
        }

        // Propiedades (getters)
        public int Id => id;
        public string Nombre => nombre;
        public string Apellido => apellido;
        public string NombreCompleto => $"{nombre} {apellido}";
        public string Email => email;
        public string FotoPerfil => fotoPerfil;
        public string Bio => bio;
        public DateTime FechaRegistro => fechaRegistro;

        // Métodos de actualización
        public async Task<bool> ActualizarPerfil(string nombre, string apellido, string bio)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.bio = bio;

            string query = "UPDATE usuarios SET nombre = @Nombre, apellido = @Apellido, bio = @Bio WHERE id = @Id";

            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var paramNombre = command.CreateParameter();
                paramNombre.ParameterName = "@Nombre";
                paramNombre.Value = nombre;
                command.Parameters.Add(paramNombre);

                var paramApellido = command.CreateParameter();
                paramApellido.ParameterName = "@Apellido";
                paramApellido.Value = apellido;
                command.Parameters.Add(paramApellido);

                var paramBio = command.CreateParameter();
                paramBio.ParameterName = "@Bio";
                paramBio.Value = bio;
                command.Parameters.Add(paramBio);

                var paramId = command.CreateParameter();
                paramId.ParameterName = "@Id";
                paramId.Value = this.id;
                command.Parameters.Add(paramId);

                dbConnection.Open();
                int result = await ((SqlCommand)command).ExecuteNonQueryAsync();
                dbConnection.Close();

                return result > 0;
            }
        }

        public async Task<bool> ActualizarFotoPerfil(string rutaFoto)
        {
            this.fotoPerfil = rutaFoto;

            string query = "UPDATE usuarios SET foto_perfil = @FotoPerfil WHERE id = @Id";

            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var paramFoto = command.CreateParameter();
                paramFoto.ParameterName = "@FotoPerfil";
                paramFoto.Value = rutaFoto;
                command.Parameters.Add(paramFoto);

                var paramId = command.CreateParameter();
                paramId.ParameterName = "@Id";
                paramId.Value = this.id;
                command.Parameters.Add(paramId);

                dbConnection.Open();
                int result = await ((SqlCommand)command).ExecuteNonQueryAsync();
                dbConnection.Close();

                return result > 0;
            }
        }
    }
}
