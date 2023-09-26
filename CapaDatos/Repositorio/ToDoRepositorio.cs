using CapaDatos.Entidades;
using CapaDatos.Interfaces;
using CapaDatos.Persistencia;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio
{
    public class ToDoRepositorio : IToDo
    {
        private readonly DBConexion _conexion;
        private readonly IToDoEstado _toDoEstado;

        public ToDoRepositorio(IOptions<DBConexion> conexion, IToDoEstado toDoEstado)
        {
            this._conexion = conexion.Value;
            this._toDoEstado = toDoEstado;
        }

        public async Task ActualizarEstado(int id)
        {
            try
            {
                var fechaFinalizada = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy"));
                var estadoId = 1;

                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SP_Modificar_Estado_ToDo";

                    using (var cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType =  CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@fechaFinalizada", fechaFinalizada);
                        cmd.Parameters.AddWithValue("@estadoId", estadoId);

                        await cmd.ExecuteNonQueryAsync();

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Crear(ToDo toDo)
        {
            try
            {
                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SP_Insertar_ToDo";

                    using (var cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@titulo", toDo.Titulo);
                        cmd.Parameters.AddWithValue("@descripcion", toDo.Descripcion);
                        cmd.Parameters.AddWithValue("@fechaRegistro", toDo.FechaRegistro);
                        cmd.Parameters.AddWithValue("@estadoId", toDo.EstadoId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ToDo> Editar(ToDo toDo)
        {
            try
            {
                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SP_Modificar_ToDo";

                    using (var cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", toDo.Id);
                        cmd.Parameters.AddWithValue("@titulo", toDo.Titulo);
                        cmd.Parameters.AddWithValue("@descripcion", toDo.Descripcion);

                        await cmd.ExecuteNonQueryAsync();

                    }
                }
                return toDo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "DELETE TODO WHERE id = @id";

                    using (var cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@id", id);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<ToDo>> ObtenerPorEstadoId(int id)
        {
            try
            {
                List<ToDo> lista = new List<ToDo>();
                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SELECT * FROM TODO WHERE estadoId = @estadoId ORDER BY id DESC";

                    var cmd = new SqlCommand(sql, conexion);

                    cmd.Parameters.AddWithValue("@estadoId", id);

                    cmd.CommandType = CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ToDo ObjToDo = new ToDo()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titulo = reader["titulo"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                                FechaFinalizada = (reader["fechaFinalizada"] != DBNull.Value) ? Convert.ToDateTime(reader["fechaFinalizada"]) : (DateTime?)null,
                                EstadoId = Convert.ToInt32(reader["estadoId"])
                            };

                            var idTodoEstado = ObjToDo.EstadoId;
                            ObjToDo.ToDoEstado = await _toDoEstado.ObtenerPorId(idTodoEstado);


                            lista.Add(ObjToDo);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ToDo> ObtenerPorId(int id)
        {
            try
            {
                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SELECT * FROM TODO WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(sql, conexion);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ToDo ObjToDo = new ToDo
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Titulo = reader["titulo"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                                FechaFinalizada = (reader["fechaFinalizada"] != DBNull.Value) ? Convert.ToDateTime(reader["fechaFinalizada"]) : (DateTime?)null,
                                EstadoId = reader.GetInt32(reader.GetOrdinal("estadoId"))
                            };

                            var idTodoEstado = ObjToDo.EstadoId;
                            ObjToDo.ToDoEstado = await _toDoEstado.ObtenerPorId(idTodoEstado);

                            return ObjToDo;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<ToDo>> ObtenerTodos()
        {
            try
            {
                List<ToDo> lista = new List<ToDo>();

                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SP_Seleccionar_ToDo ";

                    SqlCommand cmd = new SqlCommand(sql, conexion);

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ToDo ObjToDo = new ToDo()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Titulo = reader["titulo"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaRegistro = Convert.ToDateTime(reader["fechaRegistro"]),
                                FechaFinalizada = (reader["fechaFinalizada"] != DBNull.Value) ? Convert.ToDateTime(reader["fechaFinalizada"]) : (DateTime?)null,
                                EstadoId = Convert.ToInt32(reader["estadoId"])
                            };

                            var idTodoEstado = ObjToDo.EstadoId;
                            ObjToDo.ToDoEstado = await _toDoEstado.ObtenerPorId(idTodoEstado);


                            lista.Add(ObjToDo);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
