Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler
Imports DevExpress.Web.ASPxScheduler.Internal

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private ReadOnly Property Storage() As ASPxSchedulerStorage
		Get
			Return Me.ASPxScheduler1.Storage
		End Get
	End Property
	Public Shared RandomInstance As New Random()



	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		SetupMappings()
		ResourceFiller.FillResources(Me.ASPxScheduler1.Storage, 3)

		ASPxScheduler1.AppointmentDataSource = appointmentDataSource
		ASPxScheduler1.DataBind()

	End Sub

	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		ASPxScheduler1.Start = DateTime.Today
	End Sub


	#Region "Data Fill"


	Private Function GetCustomEvents() As CustomEventList
		Dim events As CustomEventList = TryCast(Session("ListBoundModeObjects"), CustomEventList)
		If events Is Nothing Then
			events = GenerateCustomEventList()
			Session("ListBoundModeObjects") = events
		End If
		Return events
	End Function

	#Region "Random events generation"
	Private Function GenerateCustomEventList() As CustomEventList
		Dim eventList As New CustomEventList()
		Dim count As Integer = Storage.Resources.Count
		For i As Integer = 0 To count - 1
			Dim resource As Resource = Storage.Resources(i)
			Dim subjPrefix As String = resource.Caption & "'s "

			eventList.Add(CreateEvent(resource.Id, subjPrefix & "meeting", 2, 5))
			eventList.Add(CreateEvent(resource.Id, subjPrefix & "travel", 3, 6))
			eventList.Add(CreateEvent(resource.Id, subjPrefix & "phone call", 0, 10))
		Next i
		Return eventList
	End Function
	Private Function CreateEvent(ByVal resourceId As Object, ByVal subject As String, ByVal status As Integer, ByVal label As Integer) As CustomEvent
		Dim customEvent As New CustomEvent()
		customEvent.Subject = subject
		customEvent.OwnerId = resourceId
		Dim rnd As Random = RandomInstance
		Dim rangeInHours As Integer = 48
		customEvent.StartTime = DateTime.Today + TimeSpan.FromHours(rnd.Next(0, rangeInHours))
		customEvent.EndTime = customEvent.StartTime + TimeSpan.FromHours(rnd.Next(0, rangeInHours \ 8))
		customEvent.Status = status
		customEvent.Label = label
		customEvent.Id = "ev" & customEvent.GetHashCode()
		Return customEvent
	End Function
	#End Region


	Private Sub SetupMappings()
		Dim mappings As ASPxAppointmentMappingInfo = Storage.Appointments.Mappings
		Storage.BeginUpdate()
		Try
			mappings.AppointmentId = "Id"
			mappings.Start = "StartTime"
			mappings.End = "EndTime"
			mappings.Subject = "Subject"
			mappings.AllDay = "AllDay"
			mappings.Description = "Description"
			mappings.Label = "Label"
			mappings.Location = "Location"
			mappings.RecurrenceInfo = "RecurrenceInfo"
			mappings.ReminderInfo = "ReminderInfo"
			mappings.ResourceId = "OwnerId"
			mappings.Status = "Status"
			mappings.Type = "EventType"
		Finally
			Storage.EndUpdate()
		End Try
	End Sub
	#End Region


	Protected Sub ASPxScheduler1_AppointmentFormShowing(ByVal sender As Object, ByVal e As AppointmentFormEventArgs)
		e.Container = New MyAppointmentFormTemplateContainer(CType(sender, ASPxScheduler))
	End Sub
	Protected Sub ASPxScheduler1_BeforeExecuteCallbackCommand(ByVal sender As Object, ByVal e As SchedulerCallbackCommandEventArgs)

		If e.CommandId = SchedulerCallbackCommandId.AppointmentSave Then
			e.Command = New MyAppointmentSaveCallbackCommand(CType(sender, ASPxScheduler))
		End If
		If e.CommandId = "CREATAPTWR" Then
			e.Command = New CreateAppointmentWithReminderCallbackCommand(ASPxScheduler1)
		End If



	End Sub



	Protected Sub ASPxScheduler1_AppointmentInserting(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
		Dim storage As ASPxSchedulerStorage = CType(sender, ASPxSchedulerStorage)
		Dim apt As Appointment = CType(e.Object, Appointment)
		storage.SetAppointmentId(apt, "a" & apt.GetHashCode())
	End Sub

	Protected Sub appointmentsDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
		e.ObjectInstance = New CustomEventDataSource(GetCustomEvents())
	End Sub

	#Region "CreateAppointmentWithReminderCallbackCommand"
	Public Class CreateAppointmentWithReminderCallbackCommand
		Inherits SchedulerCallbackCommand
		Public Sub New(ByVal control As ASPxScheduler)
			MyBase.New(control)
		End Sub

		Public Overrides ReadOnly Property Id() As String
			Get
				Return "CREATAPTWR"
			End Get
		End Property
		Public Overrides ReadOnly Property RequiresControlHierarchy() As Boolean
			Get
				Return True
			End Get
		End Property

		Protected Overrides Sub ParseParameters(ByVal parameters As String)
			' do nothing
		End Sub
		Protected Overrides Sub ExecuteCore()
			Dim nowtime As DateTime = DateTime.Now.AddMinutes(16)
			Dim aptPattern As Appointment = Control.Storage.CreateAppointment(AppointmentType.Pattern)
			aptPattern.Subject = "Appointment with Reminder"
			aptPattern.Description = "Recurrence Appointment with reminder"
			aptPattern.Duration = TimeSpan.FromHours(2)
			aptPattern.ResourceId = Control.Storage.Resources(0).Id
			aptPattern.StatusId = CInt(Fix(AppointmentStatusType.Busy))
			aptPattern.LabelId = 1
			aptPattern.CustomFields("Price") = 15.00

			aptPattern.RecurrenceInfo.Type = RecurrenceType.Daily
			aptPattern.RecurrenceInfo.Periodicity = 2
			aptPattern.RecurrenceInfo.Range = RecurrenceRange.OccurrenceCount
			aptPattern.RecurrenceInfo.OccurrenceCount = 10
			aptPattern.RecurrenceInfo.Start = nowtime.AddDays(-4)

			aptPattern.HasReminder = True
			aptPattern.Reminder.TimeBeforeStart = TimeSpan.FromMinutes(15)


			Control.Storage.Appointments.Add(aptPattern)
			Control.Start = DateTime.Today
		End Sub
	End Class
	#End Region


	 Protected Sub ASPxScheduler1_ReminderAlert(ByVal sender As Object, ByVal e As ReminderEventArgs)

		 Dim app As Appointment= ASPxScheduler1.Storage.CreateAppointment(AppointmentType.Normal)
		 app.Subject = "Created from the appointment with customfield Price = " & e.AlertNotifications(0).ActualAppointment.CustomFields("Price") & " on alert"
		 app.Start = e.AlertNotifications(0).ActualAppointment.Start.AddHours(2)
		 app.Duration = TimeSpan.FromHours(4)
		 ASPxScheduler1.Storage.Appointments.Add(app)
		 e.AlertNotifications(0).Reminder.Dismiss()
		 e.AlertNotifications(0).Handled = True

	 End Sub
	Protected Sub ASPxScheduler1_AppointmentViewInfoCustomizing(ByVal sender As Object, ByVal e As AppointmentViewInfoCustomizingEventArgs)

	Dim apt As Appointment = e.ViewInfo.Appointment
	If apt.HasReminder AndAlso apt.Type = AppointmentType.Occurrence AndAlso apt.RecurrencePattern IsNot Nothing Then
		Dim pattern As Appointment = apt.RecurrencePattern
		Dim reminder As RecurringReminder = CType(pattern.Reminder, RecurringReminder)
		e.ViewInfo.ShowBell = reminder.AlertOccurrenceIndex <= apt.RecurrenceIndex
	End If



	End Sub

End Class
