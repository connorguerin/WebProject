Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb

Public Class DataLoader
    Private myConnectionStrBooking As String = ConfigurationManager.ConnectionStrings("ConnectionStrACCDB").ToString
    Private myConnectionStrStaffing As String = ConfigurationManager.ConnectionStrings("ConnectionStrACCDB1").ToString
    Private myConnection As OleDbConnection   ' only a reference variable
    Private myCommand As OleDbCommand         ' only a reference variable
    Private myReader As OleDbDataReader       ' only a reference variable

    'Get dataTable of all occupied rooms for a given date range


    'STAFFING
    'Return dataTable of all staff

    'list of rooms becoming vacant on a given day (for housekeeping)

    Public Function AllRooms() As DataTable
        Dim myTable As New DataTable
        myConnection = New OleDbConnection(myConnectionStrBooking)
        myCommand = New OleDbCommand("SELECT Rooms.* From Rooms", myConnection)
        myConnection.Open()
        myReader = myCommand.ExecuteReader
        myTable.Load(myReader)
        myReader.Close()
        myConnection.Close()
        Return myTable
    End Function

    Public Sub CreateReservation(ByVal anID As Integer, ByVal aRoom As Double)
        myConnection = New OleDbConnection(myConnectionStrBooking)
        Dim paramStr As String = anID & aRoom
        myCommand = New OleDbCommand("INSERT INTO Reservation(VisitID, RoomNum) VALUES (" & paramStr & ")", myConnection)
        myConnection.Open()
        myCommand.ExecuteNonQuery()
        myConnection.Close()
    End Sub

    Public Function OccupiedRoomNumbers(ByVal aDate As String) As List(Of Double)
        'RETURN list of occupied room numbers

        myConnection = New OleDbConnection(myConnectionStrBooking)
        Dim paramStr As String = "#" & aDate & "#"
        myCommand = New OleDbCommand("SELECT Rooms.[Room Num] " &
                                     "FROM Visit INNER JOIN (Rooms INNER JOIN Reservation ON Rooms.[Room Num] = Reservation.RoomNum) " &
                                     "ON Visit.VisitID = Reservation.VisitID " &
                                     "WHERE (((Visit.[Check In])<=" & paramStr & ") AND ((Visit.[Check Out])>" & paramStr & "))", myConnection)
        myConnection.Open()
        myReader = myCommand.ExecuteReader
        Dim myList As New List(Of Double)
        Do While myReader.Read
            myList.Add(myReader.Item("Room Num"))
        Loop
        myReader.Close()
        myConnection.Close()
        Return myList
    End Function

    Public Function VisitsInProgress(ByVal aDate As String) As List(Of Integer)
        'returns list of VisitID's currently at the hotel on a given day

        Dim myTable As New DataTable
        myConnection = New OleDbConnection(myConnectionStrBooking)
        Dim paramStr As String = "#" & aDate & "#"
        myCommand = New OleDbCommand("SELECT Visit.VisitID FROM Visit " &
                                     "WHERE (((Visit.[Check In])<=" & paramStr & ") AND ((Visit.[Check Out])>=" & paramStr & "))", myConnection)
        myConnection.Open()
        myReader = myCommand.ExecuteReader
        Dim myList As New List(Of Integer)
        Do While myReader.Read
            myList.Add(myReader.Item("VisitID"))
        Loop
        myReader.Close()
        myConnection.Close()
        Return myList
    End Function


    'METRICS
    Public Function AvgRoomRate() As Double
        myConnection = New OleDbConnection(myConnectionStrBooking)
        myCommand = New OleDbCommand("SELECT Avg(Rooms.[Seasonal Rate]) AS " &
                                     "[AvgOfSeasonal Rate] FROM Rooms", myConnection)
        myConnection.Open()
        Dim scalar As Double = myCommand.ExecuteScalar
        myConnection.Close()
        Return scalar
    End Function

    Public Function TotalCurrentGuests(ByVal aDate As String) As Double
        myConnection = New OleDbConnection(myConnectionStrBooking)
        Dim paramStr As String = "#" & aDate & "#"
        myCommand = New OleDbCommand("SELECT Sum(Visit.[Party Size]) FROM Visit " &
                                     "HAVING (((Visit.[Check In])<=" & paramStr & ") AND ((Visit.[Check Out])>" & paramStr & "))", myConnection)
        myConnection.Open()
        Dim scalar As Double = myCommand.ExecuteScalar
        myConnection.Close()
        Return scalar
    End Function

    Public Function TotalDepartingRev(ByVal aDate As String) As Double
        myConnection = New OleDbConnection(myConnectionStrBooking)
        Dim paramStr As String = "#" & aDate & "#"
        myCommand = New OleDbCommand("SELECT Count(Visit.VisitID) FROM Visit " &
                                     "HAVING Visit.[Check Out]=" & paramStr, myConnection)
        myConnection.Open()
        Dim scalar As Double = myCommand.ExecuteScalar
        myConnection.Close()
        Return scalar
    End Function

    Public Function TotalArrivingRev(ByVal aDate As String) As Double
        myConnection = New OleDbConnection(myConnectionStrBooking)
        Dim paramStr As String = "#" & aDate & "#"
        myCommand = New OleDbCommand("SELECT Count(Visit.VisitID) FROM Visit " &
                                     "HAVING Visit.[Check In]=" & paramStr, myConnection)
        myConnection.Open()
        Dim scalar As Double = myCommand.ExecuteScalar
        myConnection.Close()
        Return scalar
    End Function

    Public Function ExistingEmployee(ByVal User As Integer, ByVal Pass As String) As Boolean
        Dim myTable As New DataTable
        myConnection = New OleDbConnection(myConnectionStrStaffing)
        myCommand = New OleDbCommand("SELECT [Staff Info].[Staff ID], [Staff Info].Password FROM [Staff Info];", myConnection)
        myConnection.Open()
        myReader = myCommand.ExecuteReader
        myTable.Load(myReader)
        myReader.Close()
        myConnection.Close()
        Dim i As Integer
        i = 0
        Do Until User = myTable.Rows(i).Item("Staff ID") And Pass = myTable.Rows(i).Item("Password")
            i = i + 1
            If i > myTable.Rows.Count Then
                Exit Do
            End If
        Loop

        If i > myTable.Rows.Count Then
            ExistingEmployee = False
        Else
            ExistingEmployee = True
        End If

    End Function

    Public Function LoadStaffInfo() As DataTable
        Dim myTable As New DataTable
        myConnection = New OleDbConnection(myConnectionStrStaffing)
        myCommand = New OleDbCommand("SELECT* FROM [Staff Info]", myConnection)
        myConnection.Open()
        myReader = myCommand.ExecuteReader
        myTable.Load(myReader)
        myReader.Close()
        myConnection.Close()
        Return myTable
    End Function

    Public Sub AddGuest(ByVal aName As String, ByVal anAge As String, ByVal anEmail As String, ByVal aPref As String)
        myConnection = New OleDbConnection(myConnectionStrBooking)   ' no connection yet
        Dim paramStr As String = "'" & aName & "'," & anAge & ", '" & anEmail & "'," & aPref
        myCommand = New OleDbCommand("INSERT INTO Guest(Name, Age, Email, RoomPref) VALUES (" & paramStr & ")", myConnection)
        myConnection.Open()
        myCommand.ExecuteNonQuery()
        myConnection.Close()
    End Sub


    Public Sub AddVisit(ByVal aGuestID As String, ByVal aPartySize As String, ByVal CheckIn As String, ByVal CheckOut As String, ByVal CardNum As String)
        myConnection = New OleDbConnection(myConnectionStrBooking)   ' no connection yet
        Dim paramStr As String = aGuestID & "," & aPartySize & ", #" & CheckIn & "#, #" & CheckOut & "#,'" & CardNum & "'"
        myCommand = New OleDbCommand("INSERT INTO Visit(GuestID, [Party Size], [Check In], [Check Out], Payment) VALUES (" & paramStr & ")", myConnection)
        myConnection.Open()
        myCommand.ExecuteNonQuery()
        myConnection.Close()
    End Sub

    Public Function GuestMax() As Integer
        Dim maxID As Integer
        myConnection = New OleDbConnection(myConnectionStrBooking)
        myCommand = New OleDbCommand("SELECT Max(Guest.GuestID) FROM Guest", myConnection)
        myConnection.Open()
        maxID = myCommand.ExecuteScalar
        myConnection.Close()
        Return maxID
    End Function

    Public Function VisitMax() As Integer
        Dim maxID As Integer
        myConnection = New OleDbConnection(myConnectionStrBooking)
        myCommand = New OleDbCommand("SELECT Max(Visit.VisitID) FROM Visit", myConnection)
        myConnection.Open()
        maxID = myCommand.ExecuteScalar
        myConnection.Close()
        Return maxID
    End Function

End Class
