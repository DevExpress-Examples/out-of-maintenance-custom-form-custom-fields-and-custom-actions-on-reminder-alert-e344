<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyAppointmentForm.ascx.cs"
    Inherits="Templates_MyAppointmentForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v8.3" Namespace="DevExpress.Web.ASPxScheduler.Controls"
    TagPrefix="dxsc" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v8.3" Namespace="DevExpress.Web.ASPxScheduler"
    TagPrefix="dxwschs" %>
<table cellpadding="0" cellspacing="0" style="width: 100%; height: 230px;">
    <tr>
        <td style="width: 57px; padding-right: 5px;">
            <dxe:ASPxLabel ID="lblSubject" runat="server" Text="Subject:" AssociatedControlID="tbSubject">
            </dxe:ASPxLabel>
        </td>
        <td colspan="3" style="width: 100%">
            <dxe:ASPxTextBox ID="tbSubject" runat="server" Width="100%" Text='<%#((MyAppointmentFormTemplateContainer)Container).Appointment.Subject %>'>
            </dxe:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <td style="padding-right: 5px;">
            <dxe:ASPxLabel ID="lblLocation" runat="server" Text="Location:" AssociatedControlID="tbLocation">
            </dxe:ASPxLabel>
        </td>
        <td style="width: 50%">
            <dxe:ASPxTextBox ID="tbLocation" Width="100%" runat="server" Text='<%#((MyAppointmentFormTemplateContainer)Container).Appointment.Location %>'>
            </dxe:ASPxTextBox>
        </td>
        <td style="padding-left: 10px; padding-right: 5px;">
            <dxe:ASPxLabel ID="lblLabel" runat="server" Text="Price:" AssociatedControlID="tbPrice">
            </dxe:ASPxLabel>
        </td>
        <td style="width: 50%">
            <dxe:ASPxTextBox ID="tbPrice" Width="100%" runat="server" Text='<%#((MyAppointmentFormTemplateContainer)Container).Price %>'>
            </dxe:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <td style="white-space: nowrap; padding-right: 5px;">
            <dxe:ASPxLabel ID="lblStartTime" runat="server" Text="Start time:" AssociatedControlID="edtStartDate">
            </dxe:ASPxLabel>
        </td>
        <td colspan="3">
            <dxe:ASPxDateEdit ID="edtStartDate" runat="server" Date='<%#((MyAppointmentFormTemplateContainer)Container).Start %>'
                Width="100%" EditFormat="DateTime">
            </dxe:ASPxDateEdit>
        </td>
    </tr>
    <tr>
        <td style="white-space: nowrap; padding-right: 5px;">
            <dxe:ASPxLabel ID="lblEndTime" runat="server" Text="End time:" AssociatedControlID="edtEndDate">
            </dxe:ASPxLabel>
        </td>
        <td colspan="3">
            <dxe:ASPxDateEdit ID="edtEndDate" runat="server" Date='<%#((MyAppointmentFormTemplateContainer)Container).End %>'
                Width="100%" EditFormat="DateTime">
            </dxe:ASPxDateEdit>
        </td>
    </tr>
    <tr>
        <td style="white-space: nowrap; padding-right: 5px;">
            <dxe:ASPxLabel ID="lblResource" runat="server" Text="Resource:" AssociatedControlID="edtResource">
            </dxe:ASPxLabel>
        </td>
        <td colspan="3">
            <dxe:ASPxComboBox ClientInstanceName="_dx" ID="edtResource" runat="server" Width="100%"
                DataSource='<%# ((MyAppointmentFormTemplateContainer)Container).ResourceDataSource %>'
                Enabled='<%# ((MyAppointmentFormTemplateContainer)Container).CanEditResource %>'
                Value='<%#( ((MyAppointmentFormTemplateContainer)Container).Appointment.ResourceId.ToString() == "DevExpress.XtraScheduler.Resource" ? "" :((MyAppointmentFormTemplateContainer)Container).Appointment.ResourceId) %>' />
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <dxe:ASPxLabel ID="lblDescription" runat="server" Text="Description:" AssociatedControlID="memDescription">
            </dxe:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <dxe:ASPxMemo ID="memDescription" runat="server" Rows="2" Text='<%#((MyAppointmentFormTemplateContainer)Container).Appointment.Description %>'
                Width="100%">
            </dxe:ASPxMemo>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <dxe:ASPxLabel ID="lblContactInfo" runat="server" Text="Contact information:" AssociatedControlID="memContacts">
            </dxe:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <dxe:ASPxMemo ID="memContacts" runat="server" Rows="3" Text='<%#((MyAppointmentFormTemplateContainer)Container).ContactInfo %>'
                Width="100%">
            </dxe:ASPxMemo>
        </td>
    </tr>
</table>
<dxsc:AppointmentRecurrenceForm ID="AppointmentRecurrenceForm1" runat="server" IsRecurring='<%# ((AppointmentFormTemplateContainer)Container).Appointment.IsRecurring %>'
    DayNumber='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceDayNumber %>'
    End='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceEnd %>' Month='<%# ((AppointmentFormTemplateContainer)Container).RecurrenceMonth %>'
    OccurrenceCount='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceOccurrenceCount %>'
    Periodicity='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrencePeriodicity %>'
    RecurrenceRange='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceRange %>'
    Start='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceStart %>'
    WeekDays='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceWeekDays %>'
    WeekOfMonth='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceWeekOfMonth %>'
    RecurrenceType='<%# ((MyAppointmentFormTemplateContainer)Container).RecurrenceType %>'
    IsFormRecreated='<%# ((MyAppointmentFormTemplateContainer)Container).IsFormRecreated %>'>
</dxsc:AppointmentRecurrenceForm>
<table cellpadding="0" cellspacing="0" style="width: 100%; height: 35px;">
    <tr>
        <td style="width: 100%; height: 100%;" align="center">
            <table style="height: 100%;">
                <tr>
                    <td>
                        <dxe:ASPxButton runat="server" ID="btnOk" Text="OK" UseSubmitBehavior="false" AutoPostBack="false"
                            EnableViewState="false" Width="91px" OnClientClick='<%# ((MyAppointmentFormTemplateContainer)Container).SaveHandler %>' />
                    </td>
                    <td>
                        <dxe:ASPxButton runat="server" ID="btnCancel" Text="Cancel" UseSubmitBehavior="false"
                            AutoPostBack="false" EnableViewState="false" Width="91px" OnClientClick='<%# ((MyAppointmentFormTemplateContainer)Container).CancelHandler %>' />
                    </td>
                    <td>
                        <dxe:ASPxButton runat="server" ID="btnDelete" Text="Delete" UseSubmitBehavior="false"
                            AutoPostBack="false" EnableViewState="false" Width="91px" OnClientClick='<%# ((MyAppointmentFormTemplateContainer)Container).DeleteHandler %>'
                            Enabled='<%# ((DevExpress.Web.ASPxScheduler.AppointmentFormTemplateContainer)Container).CanDeleteAppointment %>' />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0" style="width: 100%;">
    <tr>
        <td style="width: 100%;" align="left">
            <dxsc:ASPxSchedulerStatusInfo runat="server" ID="schedulerStatusInfo" Priority="1"
                MasterControlId='<%# ((DevExpress.Web.ASPxScheduler.AppointmentFormTemplateContainer)Container).ControlId %>' />
        </td>
    </tr>
</table>
