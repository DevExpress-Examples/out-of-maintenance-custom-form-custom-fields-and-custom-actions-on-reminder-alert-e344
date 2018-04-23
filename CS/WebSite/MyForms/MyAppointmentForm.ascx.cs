using System;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;

public partial class Templates_MyAppointmentForm : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {

	}
	public override void DataBind() {
		base.DataBind();
		MyAppointmentFormTemplateContainer container = (MyAppointmentFormTemplateContainer)Parent;
		AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence;
		//btnDelete.Enabled = !container.IsNewAppointment;
	}
}
