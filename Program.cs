using System;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows;
using System.Threading;
using System.IO.Pipelines;
using System.Data;
using System.Collections.Generic;
using DBVentana;
using System.Linq.Expressions;

namespace DBProgram
{
    
    class Gestor_de_biblioteca
    {
        static void Main(string[] args)
        {
            Ventana ventana = new Ventana(213, 50);

            Console.Write("Introduzca el nombre de la base de datos: ");
            string nomdb = Console.ReadLine();
            string connectionString = $"host=localhost;user=root;database={nomdb};port=3306";

            MySqlConnection connection = new MySqlConnection(connectionString);
            int opcion;

            try
            {
                Console.WriteLine("Conectando a la base de datos...");
                connection.Open();
                Thread.Sleep(3000);
                Console.WriteLine("¡Conexion establecida correctamente!");
                TablaUsuarios(connectionString);
                TablaLibros(connectionString);
                TablaGenero(connectionString);
                TablaPrestamos(connectionString);

                Console.WriteLine("\nPresione una tecla para continuar");
                Console.ReadKey();

                do
                {
                    Console.Clear();

                    Console.WriteLine("Seleccione la accion que desea realizar");
                    Console.WriteLine("\n\tUSUARIOS \n(1) Crear usuario \n(2) Actualizar usuario \n(3) Borrar usuario " +
                        "\n\n\tLIBROS \n(4) Agregar libro \n(5) Borrar libro \n\n\tGENEROS \n(6) Agregar genero \n\n\tPRESTAMOS " +
                        "\n(7) Crear prestamo \n(8) Actualizar prestamo \n\n(0) SALIR");
                    Console.Write("\nElija una opcion: ");

                    if (!int.TryParse(Console.ReadLine(), out opcion))
                    {
                        Console.WriteLine("Por favor ingrese un número válido.");
                        continue;
                    }

                    switch (opcion)
                    {
                        case 1:// CREAR USUARIO
                            Console.Clear();

                            try
                            {
                                string nombre = SolicitarDatoNoVacio("Ingrese nombre: ");
                                string apellido = SolicitarDatoNoVacio("Ingrese apellido: ");
                                string direccion = SolicitarDatoNoVacio("Ingrese direccion: ");
                                string telefono = SolicitarDatoNoVacio("Ingrese telefono: ");
                                string email = SolicitarDatoNoVacio("Ingrese email: ");

                                CrearUsuario(connectionString, nombre, apellido, direccion, telefono, email);

                                Console.Write("\nPresione cualquier tecla para volver al menu");
                                Console.ReadKey();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }

                            break;

                        case 2:// ACTUALIZAR USUARIO
                            try
                            {
                                Console.Clear();
                                string queryMostrar = "SELECT * FROM usuarios";
                                MySqlCommand cmd = new MySqlCommand(queryMostrar, connection);

                                List<Dictionary<string, object>> datos = ObtenerDatosDB(connectionString, queryMostrar);

                                var nombreColumnas = new Dictionary<string, string>
                        {
                            {"idusuario", "ID"},{"nombre", "NOMBRE"}, {"apellido", "APELLIDO"}, {"direccion", "DIRECCION"},{"telefono", "TELEFONO"},
                            {"email", "EMAIL"}, {"creado_el", "CREADO"}, {"actualizado_el", "ACTUALIZADO"}, {"estado_u", "ESTADO"}
                        };

                                if (datos.Count > 0)
                                {
                                    MostrarTabla(datos, nombreColumnas);
                                }
                                else
                                {
                                    Console.WriteLine("No hay datos que mostrar");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }

                            Console.Write("\nIngrese el ID del usuario que desea actualizar: ");
                            int idUsuario = int.Parse(Console.ReadLine());
                            Console.WriteLine("\nIntroduzca los nuevos valores para actualizar o presione cualquier tecla para omitir el campo no deseado");
                            string nomAct = SolicitarDatoVacio("\nIngrese el nuevo nombre: ");
                            string apeAct = SolicitarDatoVacio("Ingrese el nuevo apellido: ");
                            string direcAct = SolicitarDatoVacio("Ingrese la nueva direccion: ");
                            string telefAct = SolicitarDatoVacio("Ingrese el nuevo telefono: ");
                            string emailAct = SolicitarDatoVacio("Ingrese el nuevo email: ");

                            ActualizarUsuario(connectionString, idUsuario, nomAct, apeAct, direcAct, telefAct, emailAct);

                            Console.Write("\nPresione cualquier tecla para volver al menu");
                            Console.ReadKey();

                            break;

                        case 3:// BORRAR USUARIO
                            try
                            {
                                Console.Clear();
                                string queryMostrar = "SELECT * FROM usuarios WHERE estado_u = 1";
                                MySqlCommand cmd = new MySqlCommand(queryMostrar, connection);

                                List<Dictionary<string, object>> datos = ObtenerDatosDB(connectionString, queryMostrar);

                                var nombreColumnas = new Dictionary<string, string>
                        {
                            {"idusuario", "ID"},{"nombre", "NOMBRE"}, {"apellido", "APELLIDO"}, {"direccion", "DIRECCION"},{"telefono", "TELEFONO"},
                            {"email", "EMAIL"}, {"creado_el", "CREADO"}, {"actualizado_el", "ACTUALIZADO"}, {"estado_u", "ESTADO"}
                        };

                                if (datos.Count > 0)
                                {
                                    MostrarTabla(datos, nombreColumnas);
                                }
                                else
                                {
                                    Console.WriteLine("No hay datos que mostrar");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }

                            Console.Write("\nIngrese el ID del usuario que desea cambiar su estado: ");
                            int usuarioid = int.Parse(Console.ReadLine());

                            EliminarUsuario(connectionString, usuarioid);

                            Console.Write("\nPresione cualquier tecla para volver al menu");
                            Console.ReadKey();

                            break;

                        case 4:// AGREGAR LIBRO
                            Console.Clear();

                            try
                            {
                                string titulo = SolicitarDatoNoVacio("Ingrese el titulo: ");
                                string autor = SolicitarDatoNoVacio("Ingrese el autor: ");
                                string generoid = SolicitarDatoNoVacio("Ingrese el ID del genero: ");
                                string fecha_public = SolicitarDatoNoVacio("Ingrese la fecha de publicacion (AAAA-MM-DD): ");

                                AgregarLibro(connectionString, titulo, autor, generoid, fecha_public);

                                Console.Write("\nPresione cualquier tecla para volver al menu");
                                Console.ReadKey();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }

                        break;

                        case 5:// BORRAR LIBRO
                            try
                            {
                                Console.Clear();
                                string queryMostrar = "SELECT * FROM libros WHERE estado_l = 1";
                                MySqlCommand cmd = new MySqlCommand(queryMostrar, connection);

                                List<Dictionary<string, object>> datos = ObtenerDatosDB(connectionString, queryMostrar);

                                var nombreColumnas = new Dictionary<string, string>
                        {
                            {"idlibro", "ID"},{"titulo", "TITULO"}, {"autor", "AUTOR"}, {"generoid", "IDGENERO"}, {"fecha_public", "FECHA PUBLIC"},
                            {"creado_el", "CREADO"}, {"actualizado_el", "ACTUALIZADO"}, {"estado_l", "ESTADO"}
                        };

                                if (datos.Count > 0)
                                {
                                    MostrarTabla(datos, nombreColumnas);
                                }
                                else
                                {
                                    Console.WriteLine("No hay datos que mostrar");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }

                            Console.Write("\nIngrese el ID del libro que desea cambiar su estado: ");
                            int idlibro = int.Parse(Console.ReadLine());

                            EliminarLibro(connectionString, idlibro);

                            Console.Write("\nPresione cualquier tecla para volver al menu");
                            Console.ReadKey();

                            break;

                        case 6:// AGREGAR GENERO
                            Console.Clear();

                            try
                            {
                                string genero = SolicitarDatoNoVacio("Ingrese el nuevo genero: ");

                                AgregarGenero(connectionString, genero);

                                Console.Write("\nPresione cualquier tecla para volver al menu");
                                Console.ReadKey();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }
                            
                            break;

                        case 7:// CREAR PRESTAMO
                            Console.Clear();

                            try
                            {
                                Console.Write("Ingrese el ID del usuario: ");
                                int idusuario = int.Parse(Console.ReadLine());
                                Console.Write("Ingrese el ID del libro: ");
                                int libroid = int.Parse(Console.ReadLine());

                                CrearPrestamo(connectionString, idusuario, libroid);

                                Console.Write("\nPresione cualquier tecla para volver al menu");
                                Console.ReadKey();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }
                            
                            break;

                        case 8:// ACTUALIZAR PRESTAMO
                            Console.Clear();

                            try
                            {
                                string queryMostrar = "SELECT * FROM prestamos";
                                MySqlCommand cmd = new MySqlCommand(queryMostrar, connection);

                                List<Dictionary<string, object>> datos = ObtenerDatosDB(connectionString, queryMostrar);

                                var nombreColumnas = new Dictionary<string, string>
                        {
                            {"idprestamo", "ID"},{"fecha_prestamo", "PRESTAMO"}, {"fecha_estimada", "ESTIMADA"}, {"fecha_devolucion", "DEVOLUCION"},
                            {"libroid", "IDLIBRO"}, {"usuarioid", "IDUSUARIO"}
                        };

                                if (datos.Count > 0)
                                {
                                    MostrarTabla(datos, nombreColumnas);
                                }
                                else
                                {
                                    Console.WriteLine("No hay datos que mostrar");
                                }

                                Console.Write("\nIngrese el ID del préstamo: ");
                                int idprestamo = int.Parse(Console.ReadLine());

                                ActualizarPrestamo(connectionString, idprestamo);

                                Console.Write("\nPresione cualquier tecla para volver al menu");
                                Console.ReadKey();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ocurrio un error: " + ex.Message);
                            }

                            break;

                        case 0:
                            Console.WriteLine("\nSaliendo del programa...");

                        break;

                        default:
                            Console.WriteLine("\nOpción no válida, intente de nuevo.");
                            Console.WriteLine("\nPresione cualquier tecla para reintentar");
                            Console.ReadKey();

                        break;
                    }
                }
                while (opcion != 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connection.Close();
                Console.WriteLine("Conexion cerrada");
            }
        }

        // FUNC SOLICITAR DATOS NO VACIOS
        static string SolicitarDatoNoVacio(string mensaje)
        {
            string dato;

            do
            {
                Console.Write($"{mensaje}");
                dato = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(dato))
                {
                    Console.WriteLine("Este campo no puede estar vacío, inténtalo de nuevo");
                }
            }
            while (string.IsNullOrWhiteSpace(dato));

            return dato;
        }

        // FUNC SOLICITAR DATOS VACIOS
        static string SolicitarDatoVacio(string mensaje)
        {
            Console.Write($"{mensaje}");
            return Console.ReadLine();
        }

        //FUNC CREAR USUARIO
        static void CrearUsuario(string connectionString, string nombre, string apellido, string direccion, string telefono, string email)
        {
            string queryCrear = "INSERT INTO usuarios (nombre, apellido, direccion, telefono, email) VALUES (@nombre, @apellido, @direccion, " +
                "@telefono, @email)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryCrear, connection);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@apellido", apellido);
                cmd.Parameters.AddWithValue("@direccion", direccion);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@email", email);

                try
                {
                    connection.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(filasAfectadas > 0 ? "\nUsuario creado correctamente" : "\nNo se pudo crear el usuario");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }
        }

        //FUNC OBTENER DATOS DE LA BASE DE DATOS
        static List<Dictionary<string, object>> ObtenerDatosDB(string connectionString, string queryShow)
        {
            var filas = new List<Dictionary<string, object>>();

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(queryShow, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var fila = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            fila[reader.GetName(i)] = reader.IsDBNull(i) ? "" : reader.GetValue(i);
                        }

                        filas.Add(fila);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }

            return filas;
        }

        // FUNC MOSTRAR TABLA
        static void MostrarTabla(List<Dictionary<string, object>> filas, Dictionary<string, string> nombreColumnas)
        {
            if (filas == null || filas.Count == 0)
            {
                Console.WriteLine("No hay datos que mostrar");
                return;
            }

            var columnas = filas[0].Keys.ToList();

            var anchoColumnas = new Dictionary<string, int>();

            foreach (var columna in columnas)
            {
                string nombreColumna = nombreColumnas.ContainsKey(columna) ? nombreColumnas[columna] : columna;
                int maxAncho = Math.Max(columna.Length, filas.Max(fila => fila[columna]?.ToString().Length ?? 0));
                anchoColumnas[columna] = maxAncho;
            }

            foreach (var columna in columnas)
            {
                string nombreColumna = nombreColumnas.ContainsKey(columna) ? nombreColumnas[columna] : columna;
                Console.Write($"| {nombreColumna.PadRight(anchoColumnas[columna])}");
            }
            Console.WriteLine("|");

            Console.WriteLine(new string('-', anchoColumnas.Values.Sum() + (columnas.Count * 3) + 1));

            foreach (var fila in filas)
            {
                foreach (var columna in columnas)
                {
                    string valor = fila[columna]?.ToString() ?? "";
                    Console.Write($"| {valor.PadRight(anchoColumnas[columna])}");
                }
                Console.WriteLine("|");
            }
        }

        // FUNC ACTUALIZAR USUARIO
        static void ActualizarUsuario(string connectionString, int idsuario, string nombre, string apellido, string direccion, string telefono, string email)
        {
            string queryActualizar = "UPDATE usuarios SET actualizado_el = @fechaActual";

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                queryActualizar += ", nombre = @nombre";
            }
            if (!string.IsNullOrWhiteSpace(apellido))
            {
                queryActualizar += ", apellido = @apellido";
            }
            if (!string.IsNullOrWhiteSpace(direccion))
            {
                queryActualizar += ", direccion = @direccion";
            }
            if (!string.IsNullOrWhiteSpace(telefono))
            {
                queryActualizar += ", telefono = @telefono";
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                queryActualizar += ", email = @email";
            }
            queryActualizar += " WHERE idusuario = @idusuario";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryActualizar, connection);
                cmd.Parameters.AddWithValue("@idusuario", idsuario);
                cmd.Parameters.AddWithValue("@fechaActual", DateTime.Now);

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                }
                if (!string.IsNullOrWhiteSpace(apellido))
                {
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                }
                if (!string.IsNullOrWhiteSpace(direccion))
                {
                    cmd.Parameters.AddWithValue("@direccion", direccion);
                }
                if (!string.IsNullOrWhiteSpace(telefono))
                {
                    cmd.Parameters.AddWithValue("@telefono", telefono);
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                }

                try
                {
                    connection.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(filasAfectadas > 0 ? "\nUsuario actualizado corectamente" : "\nNo se pudo actualizar el usuario");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }
        }

        // FUNC ELIMINAR USUARIO (CAMBIAR SU ESTADO)
        static void EliminarUsuario(string connectionString, int idusuario)
        {
            string queryEliminar = "UPDATE usuarios SET estado_u = 0 WHERE idusuario = @idusuario AND estado_u = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryEliminar, connection);
                cmd.Parameters.AddWithValue("@idusuario", idusuario);

                try
                {
                    connection.Open();

                    int filaAfectada = cmd.ExecuteNonQuery();
                    Console.WriteLine(filaAfectada > 0 ? "\nEl estado del usuario ha sido cambiado a inactivo." : "\nNo se pudo cambiar el estado del usuario.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }
        }

        //FUNC AGREGAR LIBRO
        static void AgregarLibro(string connectionString, string titulo, string autor, string generoid, string feche_public)
        {
            string queryCrear = "INSERT INTO libros (titulo, autor, generoid, fecha_public) VALUES (@titulo, @autor, @generoid, " +
                "@fechapublic)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryCrear, connection);
                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@autor", autor);
                cmd.Parameters.AddWithValue("@generoid", generoid);
                cmd.Parameters.AddWithValue("@fechapublic", feche_public);

                try
                {
                    connection.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(filasAfectadas > 0 ? "\nLibro agregado con éxito." : "\nNo se pudo agregar el libro.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }
        }

        //FUNC AGREGAR GENERO
        static void AgregarGenero(string connectionString, string genero)
        {
            string queryAgregar = "INSERT INTO genero (genero) VALUES (@genero)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryAgregar, connection);
                cmd.Parameters.AddWithValue("@genero", genero);

                try
                {
                    connection.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(filasAfectadas > 0 ? "\nGenero agregado con éxito." : "\nNo se pudo agregar el nuevo genero.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }
        }

        //FUNC ELIMINAR LIBRO (CAMBIAR SU ESTADO)
        static void EliminarLibro(string connectionString, int idlibro)
        {
            string queryEliminar = "UPDATE libros SET estado_l = 0 WHERE idlibro = @idlibro AND estado_l = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryEliminar, connection);
                cmd.Parameters.AddWithValue("@idlibro", idlibro);

                try
                {
                    connection.Open();

                    int filaAfectada = cmd.ExecuteNonQuery();
                    Console.WriteLine(filaAfectada > 0 ? "\nEl estado del libro ah sido cambiado a inactivo." : "\nNo se pudo cambiar el estado del libro.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                }
            }
        }

        // FUNC CREAR PRESTAMO
        static void CrearPrestamo(string connectionString, int usuarioid, int libroid)
        {
            string queryVerificar = @"SELECT usuarios.estado_u AS UsuarioEstado, libros.estado_l AS LibroEstado FROM usuarios CROSS JOIN libros WHERE idusuario = @usuarioid AND idlibro = @libroid";

            string queryInsertar = "INSERT INTO prestamos (fecha_estimada, fecha_devolucion, libroid, usuarioid) VALUES " +
                "(@fechaestimada, NULL, @libroid, @usuarioid)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryVerificar, connection);
                cmd.Parameters.AddWithValue("@usuarioid", usuarioid);
                cmd.Parameters.AddWithValue("@libroid", libroid);

                try
                {
                    connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sbyte usuarioEstado = reader["UsuarioEstado"] != DBNull.Value ? Convert.ToSByte(reader["UsuarioEstado"]) : (sbyte)-1;
                            sbyte libroEstado = reader["LibroEstado"] != DBNull.Value ? Convert.ToSByte(reader["LibroEstado"]) : (sbyte)-1;

                            if (usuarioEstado == 1 && libroEstado == 1)
                            {
                                reader.Close();

                                MySqlCommand cmdinsert = new MySqlCommand(queryInsertar, connection);
                                cmdinsert.Parameters.AddWithValue("@usuarioid", usuarioid);
                                cmdinsert.Parameters.AddWithValue("@libroid", libroid);
                                cmdinsert.Parameters.AddWithValue("@fechaestimada", DateTime.Now.AddDays(7));

                                int filaAfectada = cmdinsert.ExecuteNonQuery();
                                Console.WriteLine(filaAfectada > 0 ? "\nPréstamo creado con éxito" : "\nNo se pudo crear el préstamo");
                            }
                            else
                            {
                                Console.WriteLine("\nEl préstamo no se puede realizar porque el usuario o el libro no estan activos.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron registros para el usuario o el libro proporcionados");
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    Console.WriteLine("Error de SQL: " + sqlEx.Message);
                    Console.WriteLine("Detalles: " + sqlEx.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error: " + ex.Message);
                    Console.WriteLine("Detalles: " + ex.StackTrace);
                }
            }
        }

        // FUNC ACTUALIZAR PRESTAMO
        static void ActualizarPrestamo(string connectionString, int idprestamo)
        {
            string queryActualizar = "UPDATE prestamos SET fecha_devolucion = @fechadevolucion WHERE idprestamo = @idprestamo";
            string queryCheckFecha = "SELECT fecha_estimada, fecha_devolucion, usuarioid FROM prestamos WHERE idprestamo = @idprestamo";
            string queryActEstado = "UPDATE usuarios SET estado_u = 0 WHERE idusuario = @usuarioid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmdActualizar = new MySqlCommand(queryActualizar, connection);
                    cmdActualizar.Parameters.AddWithValue("@fechadevolucion", DateTime.Now);
                    cmdActualizar.Parameters.AddWithValue("@idprestamo", idprestamo);
                    cmdActualizar.ExecuteNonQuery();

                    MySqlCommand cmdCheck = new MySqlCommand(queryCheckFecha, connection);
                    cmdCheck.Parameters.AddWithValue("@idprestamo", idprestamo);

                    using (MySqlDataReader reader = cmdCheck.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int usuarioid = (int)reader["usuarioid"];
                            DateTime fechaEstimada = (DateTime)reader["fecha_estimada"];
                            DateTime fechaDevolucion = (DateTime)reader["fecha_devolucion"];

                            if (fechaDevolucion > fechaEstimada)
                            {
                                reader.Close();

                                MySqlCommand cmdActEstado = new MySqlCommand(queryActEstado, connection);
                                cmdActEstado.Parameters.AddWithValue("@usuarioid", usuarioid);
                                cmdActEstado.ExecuteNonQuery();

                                Console.WriteLine("\nEl estado del usuario se desactivó debido a la devolución tardía");
                            }
                            else
                            {
                                Console.WriteLine("Préstamo actualizado con éxito");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el préstamo con el ID especificado");
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    Console.WriteLine("Ocurrió un error SQL: " + sqlEx.Message);
                    Console.WriteLine("Detalles: " + sqlEx.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                    Console.WriteLine("Detalles: " + ex.StackTrace);
                }
            }
        }

        static void TablaUsuarios(string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryCrearTablaUsuario = @"CREATE TABLE IF NOT EXISTS usuarios(
                                                  idusuario INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                                                  nombre VARCHAR(30) NOT NULL,
                                                  apellido VARCHAR(40) NOT NULL,
                                                  direccion VARCHAR(50),
                                                  telefono VARCHAR(12),
                                                  email VARCHAR(60),
                                                  creado_el TIMESTAMP DEFAULT NOW(),
                                                  actualizado_el TIMESTAMP DEFAULT NOW(),
                                                  estado_u TINYINT DEFAULT 1);";

                MySqlCommand cmdCrearTabla = new MySqlCommand(queryCrearTablaUsuario, connection);
                try
                {
                    connection.Open();
                    cmdCrearTabla.ExecuteNonQuery();
                    Console.WriteLine("La tabla 'usuarios' se ha creado exitosamente o ya existía en el contexto");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No se ha podido crear la tabla 'usuarios': " + ex.Message);
                }
            }
        }

        static void TablaLibros (string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryCrearTablaLibro = @"CREATE TABLE IF NOT EXISTS libros(
                                                idlibro INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                                                titulo VARCHAR(30) NOT NULL,
                                                autor VARCHAR(60) NOT NULL,
                                                generoid INT,
                                                fecha_public DATE,
                                                creado_el TIMESTAMP DEFAULT NOW(),
                                                actualizado_el TIMESTAMP DEFAULT NOW(),
                                                estado_l TINYINT DEFAULT 1,
                                                CONSTRAINT FK_GENERO FOREIGN KEY (generoid) REFERENCES genero (idgenero));";

                MySqlCommand cmdCrearTabla = new MySqlCommand(queryCrearTablaLibro, connection);
                try
                {
                    connection.Open();
                    cmdCrearTabla.ExecuteNonQuery();
                    Console.WriteLine("La tabla 'libros' se ha creado exitosamente o ya existía en el contexto");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No se ha podido crear la tabla 'libros': " + ex.Message);
                }
            }
        }

        static void TablaGenero (string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryCrearTablaGenero = @"CREATE TABLE IF NOT EXISTS generos(
                                                 idgenero INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                                                 genero VARCHAR(30) NOT NULL UNIQUE);";

                MySqlCommand cmdCrearTabla = new MySqlCommand(queryCrearTablaGenero, connection);
                try
                {
                    connection.Open();
                    cmdCrearTabla.ExecuteNonQuery();
                    Console.WriteLine("La tabla 'generos' se ha creado exitosamente o ya existía en el contexto");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No se ha podido crear la tabla 'generos': " + ex.Message);
                }
            }
        }

        static void TablaPrestamos (string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryCrearTablaPrestamos = @"CREATE TABLE IF NOT EXISTS prestamos(
                                                    idprestamo INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                                                    fecha_prestamo TIMESTAMP DEFAULT NOW(),
                                                    fecha_estimada DATE,
                                                    fecha_devolucion TIMESTAMP DEFAULT NOW(),
                                                    libroid INT,
                                                    usuarioid INT,
                                                    CONSTRAINT fk_libroprest FOREIGN KEY (libroid) REFERENCES libros (idlibro),
                                                    CONSTRAINT fk_usuarioprest FOREIGN KEY (usuarioid) REFERENCES usuarios (idusuario));";

                MySqlCommand cmdCrearTabla = new MySqlCommand(queryCrearTablaPrestamos, connection);
                try
                {
                    connection.Open();
                    cmdCrearTabla.ExecuteNonQuery();
                    Console.WriteLine("La tabla 'prestamos' se ha creado exitosamente o ya existía en el contexto");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No se ha podido crear la tabla 'prestamos': " + ex.Message);
                }
            }
        }
    }
}
