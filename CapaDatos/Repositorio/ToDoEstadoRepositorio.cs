using CapaDatos.Entidades;
using CapaDatos.Interfaces;
using CapaDatos.Persistencia;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio
{
    public class ToDoEstadoRepositorio : IToDoEstado
    {
        private readonly DBConexion _conexion;

        public ToDoEstadoRepositorio(IOptions<DBConexion> conexion)
        {
            this._conexion = conexion.Value;
        }

        public async Task<List<ToDoEstado>> ObtenerTodos()
        {
            try
            {
                List<ToDoEstado> lista = new List<ToDoEstado>();

                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    conexion.Open();

                    var sql = "SP_Seleccionar_Estado_ToDo";

                    SqlCommand cmd = new SqlCommand(sql, conexion);

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ToDoEstado ObjToDoEstado = new ToDoEstado()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Descripcion = reader["descripcion"].ToString()
                            };

                            lista.Add(ObjToDoEstado);
                        }
                    }

                    return lista;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ToDoEstado> ObtenerPorId(int codigo)
        {
            try
            {
                using (var conexion = new SqlConnection(_conexion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var sql = "SELECT * FROM TODO_ESTADO WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(sql, conexion);

                    cmd.Parameters.AddWithValue("@id", codigo);

                    cmd.CommandType = CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ToDoEstado todoEstado = new ToDoEstado
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Descripcion = reader["descripcion"].ToString(),
                            };

                            return todoEstado;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
