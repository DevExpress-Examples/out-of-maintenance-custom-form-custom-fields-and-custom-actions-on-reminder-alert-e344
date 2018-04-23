using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;

public class CustomFieldNames {
	public const string Price = "Price";
	public const string ContactInfo = "ContactInfo";
}


public class MyAppointmentFormTemplateContainer : AppointmentFormTemplateContainer {
	public MyAppointmentFormTemplateContainer(ASPxScheduler control)
		: base(control) {
	}
	#region Properties
	public string ContactInfo { get { return Convert.ToString(Appointment.CustomFields[CustomFieldNames.ContactInfo]); } }
	public double Price { 
		get { 
			object val = Appointment.CustomFields[CustomFieldNames.Price]; 
			return (val == DBNull.Value) ? 0 : Convert.ToDouble(val);
		} 
	}
	#endregion
}
public class MyAppointmentSaveCallbackCommand : AppointmentFormSaveCallbackCommand {
	public MyAppointmentSaveCallbackCommand(ASPxScheduler control)
		: base(control) {
	}
	protected internal new MyAppointmentFormController Controller { get { return (MyAppointmentFormController)base.Controller; } } 

	protected override void AssignControllerValues() {
		ASPxTextBox tbSubject = (ASPxTextBox)FindControlByID("tbSubject");
		ASPxTextBox tbLocation = (ASPxTextBox)FindControlByID("tbLocation");
		ASPxTextBox tbPrice = (ASPxTextBox)FindControlByID("tbPrice");
		ASPxDateEdit edtStartDate = (ASPxDateEdit)FindControlByID("edtStartDate");
		ASPxDateEdit edtEndDate = (ASPxDateEdit)FindControlByID("edtEndDate");
		ASPxMemo memDescription = (ASPxMemo)FindControlByID("memDescription");
		ASPxMemo memContacts = (ASPxMemo)FindControlByID("memContacts");
        ASPxComboBox cbResource = (ASPxComboBox)FindControlByID("edtResource");
        

		Controller.Subject = tbSubject.Text;
		Controller.Location = tbLocation.Text;
		Controller.Description = memDescription.Text;

		Controller.Start = edtStartDate.Date;
		Controller.End = edtEndDate.Date;
		// custom fields 
		Controller.ContactInfo = memContacts.Text;
		Controller.Price = Convert.ToDouble(tbPrice.Text);
        if (cbResource.SelectedItem != null)
            Controller.ResourceId = cbResource.SelectedItem.Value;
        
		DateTime clientStart = FromClientTime(Controller.Start);
		AssignControllerRecurrenceValues(clientStart);
	}
	protected override AppointmentFormController CreateAppointmentFormController(Appointment apt) {
		return new MyAppointmentFormController(Control, apt);
	}
}

public class MyAppointmentFormController : AppointmentFormController {
	private const string ContactInfoFieldName = "ContactInfo";
	private const string PriceFieldName = "Price";

	public MyAppointmentFormController(ASPxScheduler control, Appointment apt)
		: base(control, apt) {
	}
	public string ContactInfo { get { return (string)EditedAppointmentCopy.CustomFields[ContactInfoFieldName]; } set { EditedAppointmentCopy.CustomFields[ContactInfoFieldName] = value; } }
	public double Price { get { return (double)EditedAppointmentCopy.CustomFields[PriceFieldName]; } set { EditedAppointmentCopy.CustomFields[PriceFieldName] = value; } }

	string SourceContactInfo { get { return (string)SourceAppointment.CustomFields[ContactInfoFieldName]; } set { SourceAppointment.CustomFields[ContactInfoFieldName] = value; } }
	double SourcePrice { get { return (double)SourceAppointment.CustomFields[PriceFieldName]; } set { SourceAppointment.CustomFields[PriceFieldName] = value; } }

	protected override void ApplyCustomFieldsValues() {
		SourceContactInfo = ContactInfo;
		SourcePrice = Price;
	}
}
