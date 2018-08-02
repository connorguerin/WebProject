Imports Microsoft.VisualBasic


Public Class Employee
    Private ReadOnly staffID As Integer
    Private ReadOnly positionID As Integer
    Private ReadOnly Name As String
    Private ReadOnly DateHired As DateTime
    Private ReadOnly BaseRate As Integer
    Private AvailableHours As Integer

    Public Sub New(aStaffID As Integer, aPositionID As Integer, aName As String, aDateHired As DateTime, Base As Integer)
        staffID = aStaffID
        positionID = aPositionID
        Name = aName
        DateHired = aDateHired
        BaseRate = Base
        If positionID Mod 2 = 1 Then
            AvailableHours = 50
        Else
            AvailableHours = 30
        End If

    End Sub

    Public Function CanWorkJob(aJobID As Integer) As Integer
        If ((aJobID + 1) * 2 = positionID Or (aJobID + 1) * 2 - 1 = positionID) Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Public Function HourlyRate() As Double
        Return BaseRate * (1 + (0.05 * SkillRating()))
    End Function

    Public Function SkillRating() As Integer
        Return Math.Abs(DateDiff(DateInterval.Year, Today(), DateHired))
    End Function

    Public Function MaxHours() As Integer
        If (positionID Mod 2 = 0) Then
            Return 40
        Else
            Return 50
        End If
    End Function

    Public Function MinHours() As Integer
        If (positionID Mod 2 = 0) Then
            Return 0
        Else
            Return 40
        End If
    End Function

    Public ReadOnly Property GetHours() As Integer
        Get
            Return AvailableHours
        End Get
    End Property

    Public Sub DecreaseHours()
        AvailableHours -= 4
    End Sub

    Public ReadOnly Property GetID As Integer
        Get
            Return staffID
        End Get
    End Property

    Public ReadOnly Property GetName As String
        Get
            Return Name
        End Get
    End Property
End Class
