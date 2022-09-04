Imports System.Data
Imports System.Data.SqlClient

Public Class Form1
    Dim conexion As New SqlConnection("Data Source=ORDENADOR;Initial Catalog=Clientes;Integrated Security=True")
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim consulta As String = "select Nombre, Empresa, Cargo from MantenedorClientes order by Nombre"
            Dim com As New SqlCommand(consulta, conexion)
            Dim dr As SqlDataReader
            If conexion.State = ConnectionState.Closed Then conexion.Open()
            dr = com.ExecuteReader(CommandBehavior.CloseConnection)
            Do While dr.Read
                DataGridView1.Rows.Add(dr.GetValue(0).ToString(), dr.GetValue(1).ToString, dr.GetValue(2).ToString())
            Loop
            dr.Close()
            'Importante: Se debe agregar las columnas en el datagriview, caso contrario te generara un error
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtEmpresa.Clear()
        txtNombre.Clear()
        txtCargo.Clear()
        txtNombre.Focus()
    End Sub

    Private Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        Dim registrar As String = String.Format("insert MantenedorClientes(Nombre, Empresa, Cargo) values ('{0}','{1}','{2}')", txtNombre.Text, txtEmpresa.Text, txtCargo.Text)
        Dim comando As New SqlCommand(registrar, conexion)
        If conexion.State = ConnectionState.Closed Then conexion.Open()
        If comando.ExecuteNonQuery() > 0 Then
            MessageBox.Show("Registro exitosamente")
            'Agregando el nuevo registro
            RegistrarClientes()
        End If
    End Sub

    Private Sub RegistrarClientes()
        Dim consulta As String = "select Nombre, Empresa, Cargo from MantenedorClientes"
        Dim comando As New SqlCommand(consulta, conexion)
        Dim dr As SqlDataReader
        If conexion.State = ConnectionState.Closed Then conexion.Open()
        dr = comando.ExecuteReader(CommandBehavior.CloseConnection)
        'Consultar registros en la data
        DataGridView1.Rows.Clear()
        Do While dr.Read
            DataGridView1.Rows.Add(dr.GetValue(0).ToString(), dr.GetValue(1).ToString(), dr.GetValue(2).ToString())
        Loop
        dr.Close()
    End Sub

    Private Sub DataGridView1_CurrentCellChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellChanged
        If DataGridView1.CurrentRow IsNot Nothing Then
            txtNombre.Text = DataGridView1.CurrentRow.Cells(0).Value
            txtEmpresa.Text = DataGridView1.CurrentRow.Cells(1).Value
            txtCargo.Text = DataGridView1.CurrentRow.Cells(2).Value
        End If
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim actualizar As String = String.Format("update MantenedorClientes set Empresa = '{0}', Cargo = '{1}' where Nombre = '{2}'", txtEmpresa.Text, txtCargo.Text, txtNombre.Text)
        Dim comando As New SqlCommand(actualizar, conexion)
        If conexion.State = ConnectionState.Closed Then conexion.Open()
        If comando.ExecuteNonQuery() > 0 Then
            MessageBox.Show("Registro modificado")
            DataGridView1.CurrentRow.Cells(0).Value = txtNombre.Text
            DataGridView1.CurrentRow.Cells(1).Value = txtEmpresa.Text
            DataGridView1.CurrentRow.Cells(2).Value = txtCargo.Text
        Else
            MessageBox.Show("No se pudo realizar registro")
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim eliminar As String = String.Format("delete MantenedorClientes where Nombre = '{0}'", txtEmpresa.Text)
            Dim comando As New SqlCommand(eliminar, conexion)
            If conexion.State = ConnectionState.Closed Then conexion.Open()
            If comando.ExecuteNonQuery() > 0 Then
                MessageBox.Show("Registro eliminado")
                DataGridView1.Rows.Remove(DataGridView1.CurrentRow)
            Else
                MessageBox.Show("No se pudo eliminar el registro")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
