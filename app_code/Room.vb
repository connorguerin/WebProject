Imports Microsoft.VisualBasic

Public Class Room
    Private myNumber As Double
    Private myType As Integer
    Private myOccupancy As Integer
    Private myBathroomCount As Integer
    Private myCost As Double
    Private isAvailable As Boolean

    Public Sub New(ByVal RoomNum As Double, ByVal Type As Integer, ByVal Occupancy As Integer,
                   ByVal BathCount As Integer, ByVal Cost As Double)
        myNumber = RoomNum
        myType = Type
        myOccupancy = Occupancy
        myBathroomCount = BathCount
        myCost = Cost
        isAvailable = True
    End Sub

    Public ReadOnly Property RoomNum As Double
        Get
            Return myNumber
        End Get
    End Property

    Public ReadOnly Property Type As Integer
        Get
            Return myType
        End Get
    End Property

    Public ReadOnly Property Occupancy As Integer
        Get
            Return myOccupancy
        End Get
    End Property

    Public ReadOnly Property BathCount As Integer
        Get
            Return myBathroomCount
        End Get
    End Property

    Public ReadOnly Property Rate As Double
        Get
            Return myCost
        End Get
    End Property

    Public ReadOnly Property Available As Boolean
        Get
            Return isAvailable
        End Get
    End Property

    Public Sub noLongerAvailable()
        isAvailable = False
    End Sub

    Public Sub isNowAvailable()
        isAvailable = True
    End Sub
End Class
