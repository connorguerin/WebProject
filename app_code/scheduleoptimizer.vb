Imports Microsoft.VisualBasic
Imports Microsoft.SolverFoundation.Services
Public Class ScheduleOptimizer

    Private myEmployeeList As List(Of Employee) 'change to employee object
    Private myModel As Model
    Private myDecisionMatrix(,,) As Decision
    Private Day As Integer

    Public Sub New(anEmployeeList As List(Of Employee), aDay As Integer)
        myEmployeeList = anEmployeeList
        Dim openingDay As DateTime = "6/1/2018"
        Day = aDay - openingDay.DayOfYear
        Dim size(myEmployeeList.Count - 1, 5, 3) As Decision
        myDecisionMatrix = size
    End Sub

    Public Sub Solve()
        ' creates a Solver and a Model
        Dim mySolverContext As SolverContext = SolverContext.GetContext
        myModel = mySolverContext.CreateModel
        ' Decision variables
        AddDecisions()
        ' Constraints
        AddConstraints()
        ' Objective Function
        AddGoal()
        ' Solve
        mySolverContext.Solve()   ' or specifically    mySolverContext.Solve(New SimplexDirective)
    End Sub

    Public Sub AddDecisions()
        Dim i, j, k As Integer
        'employees
        For i = 0 To myEmployeeList.Count - 1
            'jobs
            For j = 0 To 5
                'shifts
                For k = 0 To 3
                    'employee i works job j for shift k
                    Dim employeeAssignment As New Decision(Domain.IntegerRange(0, 1), "Worker_" & i & "_job_" & j & "_shift_" & k)
                    myDecisionMatrix(i, j, k) = employeeAssignment
                    myModel.AddDecision(employeeAssignment)
                Next
            Next
        Next
    End Sub

    Public Sub AddConstraints()
        'employee must be qualified to work the job assigned

        For i = 0 To myEmployeeList.Count - 1
            For j = 0 To 5
                Dim currentQualification As Term = myEmployeeList.Item(i).CanWorkJob(j)
                For k = 0 To 3
                    Dim currentDecision As Decision = myDecisionMatrix(i, j, k)
                    myModel.AddConstraint("Employee_" & i & "_Qual_" & j & "_shift_" & k, currentDecision <= currentQualification)
                Next
            Next
        Next


        'employee must not be scheduled for more than one job at a time
        Dim jobSum As Term
        For i = 0 To myEmployeeList.Count - 1
            For k = 0 To 3
                jobSum = 0
                For j = 0 To 5
                    Dim currentDecision As Decision = myDecisionMatrix(i, j, k)
                    jobSum += currentDecision
                Next
                myModel.AddConstraint("One_Job_Employee_" & i & "_shift_" & k, jobSum <= 1)
            Next
        Next

        'the number of employees scheduled to a job and shift must meet demand
        Dim numberAssigned As Term
        Dim demand(,) As Integer = laborNeeds(100, SeasonFactor(Day))
        For k = 0 To 3
            For j = 0 To 5
                numberAssigned = 0
                For i = 0 To myEmployeeList.Count - 1
                    Dim currentDecision As Decision = myDecisionMatrix(i, j, k)
                    numberAssigned += currentDecision
                Next
                myModel.AddConstraint("Meet_Demand_Job_" & j & "_shift_" & k, numberAssigned >= demand(j, k))
            Next
        Next

        'minimum hours must be met for each employee
        Dim totalHours As Term
        For i = 0 To myEmployeeList.Count - 1
            totalHours = 0
            For j = 0 To 5
                For k = 0 To 3
                    Dim currentDecision As Decision = myDecisionMatrix(i, j, k)
                    totalHours += currentDecision
                Next
            Next
            totalHours *= 4
            myModel.AddConstraint("Minimum_Hours_Employee_" & i, totalHours >= myEmployeeList.Item(i).MinHours)
        Next

        'employee cannot work more than their weekly hour limit
        For i = 0 To myEmployeeList.Count - 1
            totalHours = 0
            For j = 0 To 5
                For k = 0 To 3
                    Dim currentDecision As Decision = myDecisionMatrix(i, j, k)
                    totalHours += currentDecision
                Next
            Next
            totalHours *= 4
            myModel.AddConstraint("Maximum_Hours_Employee_" & i, totalHours <= myEmployeeList.Item(i).MaxHours)
        Next


        'Dim experience As Term
        'Dim employeesum As Term
        'Dim averageExperience As Term
        'Dim averageExNeeded(,) As Double = ExperienceNeeds(SeasonFactor(Day))
        'For j = 0 To 5
        '    For k = 0 To 3
        '        experience = 0
        '        employeesum = 0
        '        For i = 0 To myEmployeeList.Count - 1
        '            Dim currentDecision As Decision = myDecisionMatrix(i, j, k)
        '            experience += currentDecision * myEmployeeList.Item(i).SkillRating()
        '            employeesum += currentDecision
        '        Next
        '        averageExperience = experience / employeesum
        '        myModel.AddConstraint("Minimum_Experience_Job_" & j & "_Shift_" & k, averageExperience >= averageExNeeded(j, k))
        '    Next
        'Next

    End Sub

    Public Sub AddGoal()
        Dim myGoal As Term = 0
        For i = 0 To myEmployeeList.Count - 1
            For j = 0 To 5
                For k = 0 To 3
                    myGoal += myDecisionMatrix(i, j, k) * myEmployeeList.Item(i).HourlyRate * 4
                Next
            Next
        Next

        myModel.AddGoal("Minimize_Labor_Costs", GoalKind.Minimize, myGoal)


    End Sub
    Public Function laborNeeds(n As Integer, seasonFactor As Integer) As Integer(,)
        'n is the number of hotel reservations
        'season factor determines volume based on what season is selected. Default should be 1

        'raw numbers of expected customers in a given place at given shift
        'restaurant
        Dim restS2 As Double = Math.Ceiling(0.5 * 0.2 * n) * seasonFactor
        Dim restS3 As Double = Math.Ceiling(0.5 * 0.2 * n) * seasonFactor
        Dim restS4 As Double = Math.Ceiling(0.5 * 0.5 * n) * seasonFactor
        'beach
        Dim beachS2 As Double = Math.Ceiling(0.7 * 0.35 * n) * seasonFactor
        Dim beachS3 As Double = Math.Ceiling(0.7 * 0.65 * n) * seasonFactor
        'spa
        Dim spaS2 As Double = Math.Ceiling(0.7 * 0.2 * n) * seasonFactor
        Dim spaS3 As Double = Math.Ceiling(0.3 * 0.2 * n) * seasonFactor

        'matrix to be returned that says how many workers needed for each job for each shift
        'locations: 0-housekeep, 1-frontdesk, 2-lifeguard, 3-beach, 4-spa, 5-restaurant
        'shifts: 0-12am to 6am, 1-6am to noon, 2-noon to 6pm, 3-6pm to midnight
        Dim WorkersPerShift(5, 3) As Integer

        'housekeeps needed
        Dim S2S3S4Housekeeps As Integer = Math.Ceiling(n * seasonFactor / 5)
        Dim S1Housekeeps As Integer = Math.Ceiling(n * seasonFactor / 10)
        WorkersPerShift(0, 0) = S1Housekeeps
        For i = 1 To 3
            WorkersPerShift(0, i) = S2S3S4Housekeeps
        Next

        'front desk needed
        Dim S2S3S4FrontDesk As Integer = Math.Ceiling(n * seasonFactor / 60)
        Dim S1FrontDesk As Integer = Math.Ceiling(n * seasonFactor / 100)
        WorkersPerShift(1, 0) = S1FrontDesk
        For i = 1 To 3
            WorkersPerShift(1, i) = S2S3S4FrontDesk
        Next

        'lifeguard needed
        Dim S2Lifeguards As Integer = Math.Ceiling(beachS2 / 50)
        Dim S3Lifeguards As Integer = Math.Ceiling(beachS3 / 50)
        WorkersPerShift(2, 1) = S2Lifeguards
        WorkersPerShift(2, 2) = S3Lifeguards

        'beach attendant needed
        Dim S2BeachAttendants As Integer = Math.Ceiling(beachS2 / 60)
        Dim S3BeachAttendants As Integer = Math.Ceiling(beachS3 / 60)
        WorkersPerShift(3, 1) = S2BeachAttendants
        WorkersPerShift(3, 2) = S3BeachAttendants

        'spa needed
        Dim S2SpaWorkers As Integer = Math.Ceiling(spaS2 / 60)
        Dim S3SpaWorkers As Integer = Math.Ceiling(spaS3 / 60)
        WorkersPerShift(4, 1) = S2SpaWorkers
        WorkersPerShift(4, 2) = S3SpaWorkers

        'restaurant needed
        Dim S2RestaurantWorkers As Integer = Math.Ceiling(restS2 / 40)
        Dim S3RestaurantWorkers As Integer = Math.Ceiling(restS3 / 40)
        Dim S4RestaurantWorkers As Integer = Math.Ceiling(restS4 / 40)
        WorkersPerShift(5, 1) = S2RestaurantWorkers
        WorkersPerShift(5, 2) = S3RestaurantWorkers
        WorkersPerShift(5, 3) = S4RestaurantWorkers

        Return WorkersPerShift

    End Function

    Public Function SeasonFactor(day As Integer) As Double
        'Assuming the resort is open for 3 months (93 days) and the peak season is the midpoint (day 47), where the default factor is 1 and the peak factor = 1.5 the season factor can be described as a function of the day, x where f(x) = -0.00023127 * x^2 + 0.021508 * x + 1
        Return -0.00023127 * day ^ 2 + 0.021508 * day + 1
    End Function

    Public Function ExperienceNeeds(seasonFactor As Double) As Double(,)
        'this determines what the minimum average of employee experience levels should be for each shift of each job
        'as a function of the season

        'initialize matrix
        Dim experienceMatrix(5, 3) As Double

        'research shows beach and restaurant dinner shift positions need higher experienced employees based on seasonal increases
        For i = 0 To 3
            experienceMatrix(0, i) = 1 * seasonFactor
            experienceMatrix(1, i) = 1 * seasonFactor
        Next

        For i = 1 To 2
            experienceMatrix(2, i) = 2 * seasonFactor
            experienceMatrix(3, i) = 2 * seasonFactor
        Next

        For i = 1 To 2
            experienceMatrix(4, i) = 1 * seasonFactor
        Next

        For i = 1 To 2
            experienceMatrix(5, i) = 1 * seasonFactor
        Next
        experienceMatrix(5, 3) = 2 * seasonFactor

        Return experienceMatrix


    End Function

    Public Function Results() As OptimizationResults
        Return New OptimizationResults(myDecisionMatrix, myEmployeeList)
    End Function
End Class
