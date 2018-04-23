<%@ Page Language="vb" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v8.2, Version=8.2.2.0, Culture=neutral, PublicKeyToken=9B171C9FD64DA1D1"
	Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.2, Version=8.2.2.0, Culture=neutral, PublicKeyToken=9B171C9FD64DA1D1"
	Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register assembly="DevExpress.Web.ASPxScheduler.v8.2, Version=8.2.2.0, Culture=neutral, PublicKeyToken=9B171C9FD64DA1D1" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v8.2.Core, Version=8.2.2.0, Culture=neutral, PublicKeyToken=9B171C9FD64DA1D1" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>

	</div>
		<dxp:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
			<PanelCollection>
				<dxp:PanelContent runat="server" _designerRegion="0">
					<dxe:aspxbutton id="btnCreateAptRem" runat="server" text="Create appointment with reminder" AutoPostBack="False" ClientInstanceName="btnCreateAptRem">
					<ClientSideEvents Click="function(s, e) { OnBtnCreateAptRemClick(); }" />
					</dxe:aspxbutton>
					&nbsp;
					<br />
		<dxwschs:ASPxScheduler id="ASPxScheduler1" runat="server" Width="100%" 
		OnAppointmentFormShowing="ASPxScheduler1_AppointmentFormShowing" 
		OnBeforeExecuteCallbackCommand="ASPxScheduler1_BeforeExecuteCallbackCommand" GroupType="Resource" Start="2008-08-08" ClientInstanceName="scheduler" OnAppointmentInserting="ASPxScheduler1_AppointmentInserting" OnAppointmentViewInfoCustomizing="ASPxScheduler1_AppointmentViewInfoCustomizing" OnReminderAlert="ASPxScheduler1_ReminderAlert">
		<Storage>
			<Appointments>
				<Mappings AllDay="AllDay" AppointmentId="Id" Description="Description" 
					End="End" Label="BranchId" Location="Location" RecurrenceInfo="RecurrenceInfo" 
					ReminderInfo="ReminderInfo" ResourceId="ResourceId" Start="Start" 
					Status="Status" Subject="Subject" Type="type" />
				<CustomFieldMappings>
					<dxwschs:ASPxAppointmentCustomFieldMapping Member="Price" Name="Price" />
					<dxwschs:ASPxAppointmentCustomFieldMapping Name="ContactInfo" Member="ContactInfo" />
				</CustomFieldMappings>
			</Appointments>
		</Storage>            
		<OptionsForms AppointmentFormTemplateUrl="~/MyForms/MyAppointmentForm.ascx" />
		<Views>
			<DayView ResourcesPerPage="1" DayCount="3"><TimeRulers><cc1:TimeRuler /></TimeRulers></DayView>
			<WorkWeekView><TimeRulers><cc1:TimeRuler /></TimeRulers></WorkWeekView>
			<WeekView ResourcesPerPage="1">
			</WeekView>
		</Views>
			<OptionsBehavior RemindersFormDefaultAction="Custom" ShowRemindersForm="False" />
	</dxwschs:ASPxScheduler>
				</dxp:PanelContent>
			</PanelCollection>
		</dxp:ASPxPanel>
		&nbsp;
<asp:ObjectDataSource ID="appointmentDataSource" runat="server" DataObjectTypeName="CustomEvent" TypeName="CustomEventDataSource" DeleteMethod="DeleteMethodHandler" SelectMethod="SelectMethodHandler" InsertMethod="InsertMethodHandler" UpdateMethod="UpdateMethodHandler" OnObjectCreated="appointmentsDataSource_ObjectCreated" />

	</form>
	<script type="text/javascript">
	function OnBtnCreateAptRemClick() {
		scheduler.RaiseCallback("CREATAPTWR|");
	}
	</script>

</body>
</html>
