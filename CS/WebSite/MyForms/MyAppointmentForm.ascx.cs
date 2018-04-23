
using System;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;

public partial class Templates_MyAppointmentForm : SchedulerFormControl {
	protected void Page_Load(object sender, EventArgs e) {

	}
	public override void DataBind() {
		base.DataBind();
		MyAppointmentFormTemplateContainer container = (MyAppointmentFormTemplateContainer)Parent;
		AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence;
        btnOk.ClientSideEvents.Click = container.SaveHandler;
        btnCancel.ClientSideEvents.Click = container.CancelHandler;
        btnDelete.ClientSideEvents.Click = container.DeleteHandler;
        //btnDelete.Enabled = !container.IsNewAppointment;
	}
    protected override DevExpress.Web.ASPxEditors.ASPxEditBase[] GetChildEditors()
    {
        return new DevExpress.Web.ASPxEditors.ASPxEditBase[] { };
    }
    protected override DevExpress.Web.ASPxEditors.ASPxButton[] GetChildButtons()
    {
        return new DevExpress.Web.ASPxEditors.ASPxButton[] { };
    }
}
