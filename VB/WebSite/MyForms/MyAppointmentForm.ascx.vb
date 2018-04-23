Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler

Partial Public Class Templates_MyAppointmentForm
	Inherits System.Web.UI.UserControl
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

	End Sub
	Public Overrides Overloads Sub DataBind()
		MyBase.DataBind()
		Dim container As MyAppointmentFormTemplateContainer = CType(Parent, MyAppointmentFormTemplateContainer)
		AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence
		btnOk.ClientSideEvents.Click = container.SaveHandler
		btnCancel.ClientSideEvents.Click = container.CancelHandler
		btnDelete.ClientSideEvents.Click = container.DeleteHandler
		'btnDelete.Enabled = !container.IsNewAppointment;
	End Sub
End Class
