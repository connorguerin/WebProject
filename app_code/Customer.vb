Imports Microsoft.VisualBasic

Public Class Customer
    Private ID As Integer
    Private name As String
    Private roomPref As Integer
    Private partySize As Integer
    Private checkin As Integer
    Private checkout As Integer
    Private roomAssignment As Integer

    Public Sub New(anID As Integer, aName As String, aRoomPref As Integer, aPartySize As Integer, aCheckin As DateTime, aCheckout As DateTime)
        ID = anID
        name = aName
        roomPref = aRoomPref
        partySize = aPartySize
        checkin = aCheckin.DayOfYear
        checkout = aCheckout.DayOfYear
        roomAssignment = 0
    End Sub

    Public ReadOnly Property getID As Integer
        Get
            Return ID
        End Get
    End Property

    Public ReadOnly Property getPreference As Integer
        Get
            Return roomPref
        End Get
    End Property

    Public ReadOnly Property getSize As Integer
        Get
            Return partySize
        End Get
    End Property

    Public ReadOnly Property getCheckin As Integer
        Get
            Return checkin
        End Get
    End Property

    Public ReadOnly Property getcheckout As Integer
        Get
            Return checkout
        End Get
    End Property

    Public ReadOnly Property getAssignment As Integer
        Get
            Return roomAssignment
        End Get
    End Property

    Public Sub setRoomAssignment(assignment As Integer)
        roomAssignment = assignment
    End Sub

    Public ReadOnly Property getName As String
        Get
            Return name
        End Get
    End Property
End Class
