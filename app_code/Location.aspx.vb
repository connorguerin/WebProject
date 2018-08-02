Imports Microsoft.SolverFoundation.Services
Imports System.Data
Partial Class Location
    Inherits System.Web.UI.Page
    Private myDataLoader As New DataLoader

    Private Sub DropDownList1_TextChanged(sender As Object, e As EventArgs) Handles DropDownList1.TextChanged
        lblShift.Text = Session("Location")
        If DropDownList1.Text = "Shift 1" Then
            fillGridview(Session("Location"), Session("Shift1"))
        ElseIf DropDownList1.Text = "Shift 2" Then
            fillGridview(Session("Location"), Session("Shift2"))
        ElseIf DropDownList1.Text = "Shift 3" Then
            fillGridview(Session("Location"), Session("Shift3"))
        ElseIf DropDownList1.Text = "Shift 4" Then
            fillGridview(Session("Location"), Session("Shift4"))

        End If
    End Sub
    Protected Sub fillGridview(Location As String, decisionArray As Decision(,))
        Dim totalhousekeeps As Integer = 0
        Dim outputTable As New DataTable
        Dim desiredJob As Integer
        If Location = "Housekeep" Then
            desiredJob = 0
        ElseIf Location = "Front Desk" Then
            desiredJob = 1
        ElseIf Location = "Life Guard" Then
            desiredJob = 2
        ElseIf Location = "Beach Attendant" Then
            desiredJob = 3
        ElseIf Location = "Spa" Then
            desiredJob = 4
        ElseIf Location = "Restaurant" Then
            desiredJob = 5
        End If

        outputTable.Columns.Add("Employee ID")
        outputTable.Columns.Add("Employee Name")
        Dim myemployeelist As List(Of Employee) = myDataLoader.LoadEmployeeList
        Dim isAssigned As Boolean = False
        Dim currentJob As Integer = -1
        Dim total As Integer = 0
        lblShift.Text = Location
        For i = 0 To myemployeelist.Count - 1
            isAssigned = False
            currentJob = -1
            For j = 0 To 5
                If decisionArray(i, j).ToDouble = 1 Then
                    isAssigned = True
                    currentJob = j
                    total += 1
                End If
            Next
            If isAssigned And currentJob = desiredJob Then
                Dim newrow As DataRow = outputTable.NewRow
                newrow("Employee ID") = myemployeelist.Item(i).GetID
                newrow("Employee Name") = myemployeelist.Item(i).GetName
                outputTable.Rows.Add(newrow)
            End If
        Next
        gvwEmployees.DataSource = outputTable
        gvwEmployees.DataBind()
    End Sub



End Class
