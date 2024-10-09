using SysCafeteriasweetcoffee.Models;
using BCrypt.Net; // Importar la librería BCrypt

namespace SysCafeteriasweetcoffee.Services
{
    public class UsuarioService
    {
        // Método para registrar un nuevo usuario
        public void RegistrarUsuario(string login, string password)
        {
            // Encriptar la contraseña con BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Crear el nuevo usuario con el login y la contraseña encriptada
            Usuario nuevoUsuario = new Usuario
            {
                Login = login, // Usar Login en lugar de Username
                Password = passwordHash // Usar Password en lugar de PasswordHash
            };

            // Guardar el usuario en la base de datos (Lógica para guardar en DB)
        }

        // Método para verificar si la contraseña ingresada es correcta
        public bool VerificarPassword(string passwordIngresada, string passwordHash)
        {
            // Verificar si la contraseña en texto plano corresponde con el hash almacenado
            return BCrypt.Net.BCrypt.Verify(passwordIngresada, passwordHash);
        }

        // Ejemplo de método para obtener el usuario por login
        public Usuario ObtenerUsuarioPorLogin(string login)
        {
            // Aquí deberías tener tu lógica para obtener el usuario desde la base de datos
            // Por ejemplo: dbContext.Usuarios.FirstOrDefault(u => u.Login == login);
            return null; // Simulación de la lógica
        }
    }
}
