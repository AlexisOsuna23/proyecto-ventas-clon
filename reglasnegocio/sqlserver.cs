using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reglasnegocio
{
    public class sqlserver
    {
        public class SQLServerClass
        {
            public String sLastError = String.Empty;

            public Boolean SiHayConexion(String NombreTab)
            {
                bool bAllok = false;
                try
                {
                    String sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT * FROM {NombreTab}", conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        conn.Close();
                    }

                    bAllok = true;
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllok = false;
                }
                return bAllok;
            }
            public string ConexionEstatica()
            {
                string SSQL = "ALEXIS";
                string INT = "CRUD_Inventario";
                string USQL = "sa";
                string CSQL = "12345";
                return $"Data Source={SSQL};Initial Catalog={INT};User ID={USQL};Password={CSQL};";
            }

            public DataTable ObtenerRegistrosDeTabla(string nombreTabla)
            {
                DataTable dataTable = new DataTable();

                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string SQLQuery = $" SELECT * FROM \"{nombreTabla}\"";

                        using (SqlDataAdapter DDA = new SqlDataAdapter(SQLQuery, conn))
                        {
                            DDA.Fill(dataTable);
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dataTable;
            }

            public DataTable ObtenerVistaVentas()
            {
                DataTable dataTable = new DataTable();

                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string SQLQuery = $" SELECT * FROM InventarioDetalle ";

                        using (SqlDataAdapter DDA = new SqlDataAdapter(SQLQuery, conn))
                        {
                            DDA.Fill(dataTable);
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dataTable;
            }

            public DataTable ObtenerDatos()
            {
                DataTable dataTable = new DataTable();

                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string SQLQuery = @"SELECT P.ProductoID, P.Descripcion, P.PVenta, VD.Cantidad, v.Total 
                                        FROM VentasDetalle VD 
                                        INNER JOIN Productos P ON P.ProductoID = VD.ProductoID
                                        INNER JOIN Ventas v ON v.Folio = VD.Folio";

                        using (SqlDataAdapter DDA = new SqlDataAdapter(SQLQuery, conn))
                        {
                            DDA.Fill(dataTable);
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dataTable;
            }

            public string FolioSugerido()
            {
                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string consultaExiste = "SELECT COUNT(*) FROM Ventas";
                        SqlCommand comandoExiste = new SqlCommand(consultaExiste, conn);
                        int count = Convert.ToInt32(comandoExiste.ExecuteScalar());

                        int siguienteIdentidad = count > 0 ? count + 1 : 1;
                        conn.Close();

                        return $"{siguienteIdentidad}";
                    }
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }


            public void InsertarInventario(string Folio, string Fecha, string Total, List<string> productoIDs, List<string> pVentas, List<string> cantidades)
            {
                bool bAllok = false;

                try
                {
                    string sConexionDB = ConexionEstatica();

                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        SqlTransaction sqlTransaction = conn.BeginTransaction();

                        try
                        {
                            using (SqlCommand cmd = conn.CreateCommand())
                            {

                                cmd.Transaction = sqlTransaction;
                                DateTime fechaTransaccion = DateTime.Parse(Fecha);

                                cmd.CommandText = "INSERT INTO Ventas(Folio, Fecha, Total) VALUES (@Folio, @Fecha, @Total)";
                                cmd.Parameters.AddWithValue("@Folio", Folio);
                                cmd.Parameters.AddWithValue("@Fecha", fechaTransaccion);
                                cmd.Parameters.AddWithValue("@Total", Total);
                                cmd.ExecuteNonQuery();
                            }

                            for (int i = 0; i < productoIDs.Count; i++)
                            {
                                using (SqlCommand VD = conn.CreateCommand())
                                {
                                    VD.Transaction = sqlTransaction;

                                    VD.CommandText = "INSERT INTO InventarioDetalle (Cantidad, Pventa, ProductoID, Folio) VALUES (@Cantidad, @Pventa, @ProductoID, @Folio)";
                                    VD.Parameters.AddWithValue("@Cantidad", cantidades[i]);
                                    VD.Parameters.AddWithValue("@Pventa", pVentas[i]);
                                    VD.Parameters.AddWithValue("@ProductoID", productoIDs[i]);
                                    VD.Parameters.AddWithValue("@Folio", Folio);
                                    VD.ExecuteNonQuery();
                                }

                                using (SqlCommand UPSaldo = conn.CreateCommand())
                                {
                                    UPSaldo.Transaction = sqlTransaction;

                                    UPSaldo.CommandText = "UPDATE Productos SET Saldo = Saldo - @Cantidad WHERE ProductoID = @ProductoID";
                                    UPSaldo.Parameters.AddWithValue("@ProductoID", productoIDs[i]);
                                    UPSaldo.Parameters.AddWithValue("@Cantidad", cantidades[i]);
                                    UPSaldo.ExecuteNonQuery();
                                }
                            }

                            sqlTransaction.Commit();
                            bAllok = true;
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Ocurrió un error durante la transacción en la base de datos.", ex);
                        }
                        finally
                        {
                            sqlTransaction.Dispose();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllok = false;
                }
            }

            public bool InsertarProductos(string ProductoID, string Descripcion, string PrecioVenta, string saldo)
            {
                try
                {
                    string sConexionDB = ConexionEstatica();

                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string insertQuery = $"INSERT INTO Productos(ProductoID, Descripcion, Pventa, Saldo) " +
                                            $"VALUES ('{ProductoID}', '{Descripcion}', '{PrecioVenta}', '{saldo}')";

                        //string EXEC = $"INSERT INTO Productos(ProductoID, Descripcion, Pventa, Saldo) " +
                          //                   $"VALUES ('{ProductoID}', '{Descripcion}', '{PrecioVenta}', '{saldo}')"; 

                        SqlCommand command = new SqlCommand(insertQuery, conn);
                        command.ExecuteNonQuery();

                        conn.Close();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    return false;
                }
            }
            public void DeleteProducto(List<string> productoIDs, List<string> cantidades)
            {
                try
                {
                    string sConexionDB = ConexionEstatica();

                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        SqlTransaction sqlTransaction = conn.BeginTransaction();

                        try
                        {
                            Tuple<string, string, string> ultimaInfo = ObtenerUltimoFolioInfo(conn, sqlTransaction);
                            string ultimoFolio = ultimaInfo.Item1;

                            List<Tuple<string, string, string>> registros = ObtenerRegistrosPorFolio(conn, sqlTransaction, ultimoFolio);

                            using (SqlCommand cmdDetalle = conn.CreateCommand())
                            {
                                cmdDetalle.Transaction = sqlTransaction;

                                cmdDetalle.CommandText = "DELETE FROM VentasDetalle WHERE Folio = @Folio";
                                cmdDetalle.Parameters.AddWithValue("@Folio", ultimoFolio);
                                cmdDetalle.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdVentas = conn.CreateCommand())
                            {
                                cmdVentas.Transaction = sqlTransaction;

                                cmdVentas.CommandText = "DELETE FROM Ventas WHERE Folio = @Folio";
                                cmdVentas.Parameters.AddWithValue("@Folio", ultimoFolio);
                                cmdVentas.ExecuteNonQuery();
                            }

                            for (int i = 0; i < registros.Count; i++)
                            {
                                using (SqlCommand UPSaldo = conn.CreateCommand())
                                {
                                    UPSaldo.Transaction = sqlTransaction;

                                    UPSaldo.CommandText = "UPDATE Productos SET Saldo = Saldo + @Cantidad WHERE ProductoID = @ProductoID";
                                    UPSaldo.Parameters.AddWithValue("@ProductoID", registros[i].Item2);
                                    UPSaldo.Parameters.AddWithValue("@Cantidad", registros[i].Item3);
                                    UPSaldo.ExecuteNonQuery();
                                }
                            }

                            sqlTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Ocurrió un error al deshacer la transacción en la base de datos.", ex);
                        }
                        finally
                        {
                            sqlTransaction.Dispose();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                }
            }

            public void Transacciones(string Folio, string Fecha, string Total, List<string> productoIDs, List<string> pVentas, List<string> cantidades)
            {
                bool bAllok = false;

                try
                {
                    string sConexionDB = ConexionEstatica();

                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        SqlTransaction sqlTransaction = conn.BeginTransaction();

                        try
                        {
                            using (SqlCommand cmd = conn.CreateCommand())
                            {

                                cmd.Transaction = sqlTransaction;
                                DateTime fechaTransaccion = DateTime.Parse(Fecha);

                                cmd.CommandText = "INSERT INTO Ventas(Folio, Fecha, Total) VALUES (@Folio, @Fecha, @Total)";
                                cmd.Parameters.AddWithValue("@Folio", Folio);
                                cmd.Parameters.AddWithValue("@Fecha", fechaTransaccion);
                                cmd.Parameters.AddWithValue("@Total", Total);
                                cmd.ExecuteNonQuery();
                            }

                            for (int i = 0; i < productoIDs.Count; i++)
                            {
                                using (SqlCommand VD = conn.CreateCommand())
                                {
                                    VD.Transaction = sqlTransaction;

                                    VD.CommandText = "INSERT INTO VentasDetalle (Cantidad, Pventa, ProductoID, Folio) VALUES (@Cantidad, @Pventa, @ProductoID, @Folio)";
                                    VD.Parameters.AddWithValue("@Cantidad", cantidades[i]);
                                    VD.Parameters.AddWithValue("@Pventa", pVentas[i]);
                                    VD.Parameters.AddWithValue("@ProductoID", productoIDs[i]);
                                    VD.Parameters.AddWithValue("@Folio", Folio);
                                    VD.ExecuteNonQuery();
                                }

                                using (SqlCommand UPSaldo = conn.CreateCommand())
                                {
                                    UPSaldo.Transaction = sqlTransaction;

                                    UPSaldo.CommandText = "UPDATE Productos SET Saldo = Saldo - @Cantidad WHERE ProductoID = @ProductoID";
                                    UPSaldo.Parameters.AddWithValue("@ProductoID", productoIDs[i]);
                                    UPSaldo.Parameters.AddWithValue("@Cantidad", cantidades[i]);
                                    UPSaldo.ExecuteNonQuery();
                                }
                            }

                            sqlTransaction.Commit();
                            bAllok = true;
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Ocurrió un error durante la transacción en la base de datos.", ex);
                        }
                        finally
                        {
                            sqlTransaction.Dispose();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllok = false;
                }
            }

            public decimal ObtenerSaldo(string ProductoID)
            {
                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string consultaSaldo = "SELECT ISNULL(SUM(Saldo), 0) FROM Productos WHERE ProductoID = @ProductoID";

                        using (SqlCommand comandoSaldo = new SqlCommand(consultaSaldo, conn))
                        {
                            comandoSaldo.Parameters.AddWithValue("@ProductoID", ProductoID);

                            using (SqlDataReader reader = comandoSaldo.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    decimal saldoActual = Convert.IsDBNull(reader[0]) ? 0 : Convert.ToDecimal(reader[0]);
                                    return saldoActual;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener el saldo: {ex.Message}");
                    return 0;
                }

                return 0;
            }

            public void DeshacerTransaccion(List<string> productoIDs, List<string> cantidades)
            {
                try
                {
                    string sConexionDB = ConexionEstatica();

                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        SqlTransaction sqlTransaction = conn.BeginTransaction();

                        try
                        {
                            Tuple<string, string, string> ultimaInfo = ObtenerUltimoFolioInfo(conn, sqlTransaction);
                            string ultimoFolio = ultimaInfo.Item1;

                            List<Tuple<string, string, string>> registros = ObtenerRegistrosPorFolio(conn, sqlTransaction, ultimoFolio);

                            using (SqlCommand cmdDetalle = conn.CreateCommand())
                            {
                                cmdDetalle.Transaction = sqlTransaction;

                                cmdDetalle.CommandText = "DELETE FROM VentasDetalle WHERE Folio = @Folio";
                                cmdDetalle.Parameters.AddWithValue("@Folio", ultimoFolio);
                                cmdDetalle.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdVentas = conn.CreateCommand())
                            {
                                cmdVentas.Transaction = sqlTransaction;

                                cmdVentas.CommandText = "DELETE FROM Ventas WHERE Folio = @Folio";
                                cmdVentas.Parameters.AddWithValue("@Folio", ultimoFolio);
                                cmdVentas.ExecuteNonQuery();
                            }

                            for (int i = 0; i < registros.Count; i++)
                            {
                                using (SqlCommand UPSaldo = conn.CreateCommand())
                                {
                                    UPSaldo.Transaction = sqlTransaction;

                                    UPSaldo.CommandText = "UPDATE Productos SET Saldo = Saldo + @Cantidad WHERE ProductoID = @ProductoID";
                                    UPSaldo.Parameters.AddWithValue("@ProductoID", registros[i].Item2);
                                    UPSaldo.Parameters.AddWithValue("@Cantidad", registros[i].Item3);
                                    UPSaldo.ExecuteNonQuery();
                                }
                            }

                            sqlTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            throw new Exception("Ocurrió un error al deshacer la transacción en la base de datos.", ex);
                        }
                        finally
                        {
                            sqlTransaction.Dispose();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                }
            }

            private Tuple<string, string, string> ObtenerUltimoFolioInfo(SqlConnection conn, SqlTransaction transaction)
            {
                string ultimoFolio = null;
                string ultimoProductoID = null;
                string ultimaCantidad = null;

                using (SqlCommand cmdUltimoFolio = conn.CreateCommand())
                {
                    cmdUltimoFolio.Transaction = transaction;

                    cmdUltimoFolio.CommandText = "SELECT MAX(Folio) FROM Ventas";
                    object result = cmdUltimoFolio.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        ultimoFolio = result.ToString();
                    }
                }

                if (ultimoFolio != null)
                {
                    List<Tuple<string, string, string>> registros = ObtenerRegistrosPorFolio(conn, transaction, ultimoFolio);

                    if (registros.Count > 0)
                    {
                        ultimoProductoID = registros[0].Item2;
                        ultimaCantidad = registros[0].Item3;
                    }
                }

                return Tuple.Create(ultimoFolio, ultimoProductoID, ultimaCantidad);
            }

            private List<Tuple<string, string, string>> ObtenerRegistrosPorFolio(SqlConnection conn, SqlTransaction transaction, string folio)
            {
                List<Tuple<string, string, string>> registros = new List<Tuple<string, string, string>>();

                using (SqlCommand cmdRegistrosPorFolio = conn.CreateCommand())
                {
                    cmdRegistrosPorFolio.Transaction = transaction;

                    cmdRegistrosPorFolio.CommandText = "SELECT ProductoID, Cantidad FROM VentasDetalle WHERE Folio = @Folio";
                    cmdRegistrosPorFolio.Parameters.AddWithValue("@Folio", folio);

                    using (SqlDataReader reader = cmdRegistrosPorFolio.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productoID = Convert.ToString(reader["ProductoID"]);
                            string cantidad = Convert.ToString(reader["Cantidad"]);
                            registros.Add(Tuple.Create(folio, productoID, cantidad));
                        }
                    }
                }

                return registros;
            }

            public DataTable ObtenerDatosVentasDetalle()
            {
                DataTable dataTable = new DataTable();

                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string SQLQuery = @"SELECT ProductoID, Cantidad, Folio FROM VentasDetalle";

                        using (SqlDataAdapter DDA = new SqlDataAdapter(SQLQuery, conn))
                        {
                            DDA.Fill(dataTable);
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dataTable;
            }

            public Tuple<string, decimal, float> ObtenerValoresPorProductID(string productID)
            {
                try
                {
                    string sConexionDB = ConexionEstatica();
                    using (SqlConnection conn = new SqlConnection(sConexionDB))
                    {
                        conn.Open();

                        string consulta = $"SELECT ProductoID, Descripcion, Saldo, PVenta FROM Productos WHERE ProductoID = @ProductID";
                        SqlCommand comando = new SqlCommand(consulta, conn);
                        comando.Parameters.AddWithValue("@ProductID", productID);

                        SqlDataReader reader = comando.ExecuteReader();

                        if (reader.Read())
                        {
                            string productoID = reader["ProductoID"].ToString();
                            string descripcion = reader["Descripcion"].ToString();
                            decimal precioVenta = Convert.ToDecimal(reader["PVenta"]);
                            float saldo = Convert.ToSingle(reader["Saldo"]);

                            conn.Close();

                            return new Tuple<string, decimal, float>(descripcion, precioVenta, saldo);
                        }
                        else
                        {
                            conn.Close();
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
