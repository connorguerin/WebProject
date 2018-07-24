
Imports System.Data
Imports System.Data.OleDb

Public Class DataLoader
    Private myConnectionStrBooking As String = ConfigurationManager.ConnectionStrings("ConnectionStrACCDB").ToString
    'Private myConnectionStrStaffing As String = ConfigurationManager.ConnectionStrings("ConnectionStrMDB").ToString
    Private myConnection As OleDbConnection   ' only a reference variable
    Private myCommand As OleDbCommand         ' only a reference variable
    Private myReader As OleDbDataReader       ' only a reference variable

    'Get dataTable of all occupied rooms for a given date range

    'return information for visits

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

End Class
