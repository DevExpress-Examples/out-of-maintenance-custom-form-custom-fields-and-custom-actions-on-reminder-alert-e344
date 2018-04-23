using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler.Internal;

public partial class _Default : System.Web.UI.Page {
    ASPxSchedulerStorage Storage { get { return this.ASPxScheduler1.Storage; } }
    public static Random RandomInstance = new Random();



    protected void Page_Load(object sender, EventArgs e) {
        DataHelper.SetupMappings(this.ASPxScheduler1);
        ResourceFiller.FillResources(this.ASPxScheduler1.Storage, 3);
        DataHelper.ProvideRowInsertion(ASPxScheduler1, appointmentDataSource);

        ASPxScheduler1.AppointmentDataSource = appointmentDataSource;
        ASPxScheduler1.DataBind();
    }

    protected void Page_Init(object sender, EventArgs e) {
        ASPxScheduler1.Start = DateTime.Today;
    }


    #region Data Fill


    CustomEventList GetCustomEvents() {
        CustomEventList events = Session["ListBoundModeObjects"] as CustomEventList;
        if (events == null) {
            events = GenerateCustomEventList();
            Session["ListBoundModeObjects"] = events;
        }
        return events;
    }

    #region Random events generation
    CustomEventList GenerateCustomEventList() {
        CustomEventList eventList = new CustomEventList();
        int count = Storage.Resources.Count;
        for (int i = 0; i < count; i++) {
            Resource resource = Storage.Resources[i];
            string subjPrefix = resource.Caption + "'s ";

            eventList.Add(CreateEvent(resource.Id, subjPrefix + "meeting", 2, 5));
            eventList.Add(CreateEvent(resource.Id, subjPrefix + "travel", 3, 6));
            eventList.Add(CreateEvent(resource.Id, subjPrefix + "phone call", 0, 10));
        }
        return eventList;
    }
    CustomEvent CreateEvent(object resourceId, string subject, int status, int label) {
        CustomEvent customEvent = new CustomEvent();
        customEvent.Subject = subject;
        customEvent.OwnerId = resourceId;
        Random rnd = RandomInstance;
        int rangeInHours = 48;
        customEvent.StartTime = DateTime.Today + TimeSpan.FromHours(rnd.Next(0, rangeInHours));
        customEvent.EndTime = customEvent.StartTime + TimeSpan.FromHours(rnd.Next(0, rangeInHours / 8));
        customEvent.Status = status;
        customEvent.Label = label;
        customEvent.Id = "ev" + customEvent.GetHashCode();
        return customEvent;
    }
    #endregion

   #endregion


    protected void ASPxScheduler1_AppointmentFormShowing(object sender, AppointmentFormEventArgs e) {
        e.Container = new MyAppointmentFormTemplateContainer((ASPxScheduler)sender);
    }
    protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e) {

        if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
            e.Command = new MyAppointmentSaveCallbackCommand((ASPxScheduler)sender);
        if (e.CommandId == "CREATAPTWR")
            e.Command = new CreateAppointmentWithReminderCallbackCommand(ASPxScheduler1);
    }

    protected void appointmentsDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
        e.ObjectInstance = new CustomEventDataSource(GetCustomEvents());
    }

    #region CreateAppointmentWithReminderCallbackCommand
    public class CreateAppointmentWithReminderCallbackCommand : SchedulerCallbackCommand {
        public CreateAppointmentWithReminderCallbackCommand(ASPxScheduler control)
            : base(control) {
        }

        public override string Id { get { return "CREATAPTWR"; } }
        public override bool RequiresControlHierarchy { get { return true; } }

        protected override void ParseParameters(string parameters) {
            // do nothing
        }
        protected override void ExecuteCore() {
            DateTime nowtime = DateTime.Now.AddMinutes(16);
            Appointment aptPattern = Control.Storage.CreateAppointment(AppointmentType.Pattern);
            aptPattern.Subject = "Appointment with Reminder";
            aptPattern.Description = "Recurrence Appointment with reminder";
            aptPattern.Duration = TimeSpan.FromHours(2);
            aptPattern.ResourceId = Control.Storage.Resources[0].Id;
            aptPattern.StatusId = (int)AppointmentStatusType.Busy;
            aptPattern.LabelId = 1;
            aptPattern.CustomFields["Price"] = 15.00;

            aptPattern.RecurrenceInfo.Type = RecurrenceType.Daily;
            aptPattern.RecurrenceInfo.Periodicity = 2;
            aptPattern.RecurrenceInfo.Range = RecurrenceRange.OccurrenceCount;
            aptPattern.RecurrenceInfo.OccurrenceCount = 10;
            aptPattern.RecurrenceInfo.Start = nowtime.AddDays(-4);

            aptPattern.HasReminder = true;
            aptPattern.Reminder.TimeBeforeStart = TimeSpan.FromMinutes(15);


            Control.Storage.Appointments.Add(aptPattern);
            Control.Start = DateTime.Today;
        }
    }
    #endregion


     protected void ASPxScheduler1_ReminderAlert(object sender, ReminderEventArgs e) {
         
         Appointment app= ASPxScheduler1.Storage.CreateAppointment(AppointmentType.Normal);
         app.Subject = "Created from the appointment with customfield Price = " + e.AlertNotifications[0].ActualAppointment.CustomFields["Price"]
             + " on alert";
         app.Start = e.AlertNotifications[0].ActualAppointment.Start.AddHours(2);
         app.Duration = TimeSpan.FromHours(4);
         ASPxScheduler1.Storage.Appointments.Add(app);
         e.AlertNotifications[0].Reminder.Dismiss();
         e.AlertNotifications[0].Handled = true;

    }
    protected void ASPxScheduler1_AppointmentViewInfoCustomizing(object sender, AppointmentViewInfoCustomizingEventArgs e) {

    Appointment apt = e.ViewInfo.Appointment;
    if (apt.HasReminder && apt.Type == AppointmentType.Occurrence && apt.RecurrencePattern != null)
    {
        Appointment pattern = apt.RecurrencePattern;
        RecurringReminder reminder = (RecurringReminder)pattern.Reminder;
        e.ViewInfo.ShowBell = reminder.AlertOccurrenceIndex <= apt.RecurrenceIndex;
    }

    }

}
