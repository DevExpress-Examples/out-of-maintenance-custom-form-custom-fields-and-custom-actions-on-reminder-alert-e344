﻿Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler

Public Class CustomFieldNames
    Public Const Price As String = "Price"
    Public Const ContactInfo As String = "ContactInfo"
End Class


Public Class MyAppointmentFormTemplateContainer
    Inherits AppointmentFormTemplateContainer

    Public Sub New(ByVal control As ASPxScheduler)
        MyBase.New(control)
    End Sub
    #Region "Properties"
    Public ReadOnly Property ContactInfo() As String
        Get
            Return Convert.ToString(Appointment.CustomFields(CustomFieldNames.ContactInfo))
        End Get
    End Property
    Public ReadOnly Property Price() As Double
        Get
            Dim val As Object = Appointment.CustomFields(CustomFieldNames.Price)
            Return If(val Is DBNull.Value, 0, Convert.ToDouble(val))
        End Get
    End Property
    #End Region
End Class
Public Class MyAppointmentSaveCallbackCommand
    Inherits AppointmentFormSaveCallbackCommand

    Public Sub New(ByVal control As ASPxScheduler)
        MyBase.New(control)
    End Sub
    Protected Friend Shadows ReadOnly Property Controller() As MyAppointmentFormController
        Get
            Return CType(MyBase.Controller, MyAppointmentFormController)
        End Get
    End Property

    Protected Overrides Sub AssignControllerValues()
        Dim tbSubject As ASPxTextBox = CType(FindControlByID("tbSubject"), ASPxTextBox)
        Dim tbLocation As ASPxTextBox = CType(FindControlByID("tbLocation"), ASPxTextBox)
        Dim tbPrice As ASPxTextBox = CType(FindControlByID("tbPrice"), ASPxTextBox)
        Dim edtStartDate As ASPxDateEdit = CType(FindControlByID("edtStartDate"), ASPxDateEdit)
        Dim edtEndDate As ASPxDateEdit = CType(FindControlByID("edtEndDate"), ASPxDateEdit)
        Dim memDescription As ASPxMemo = CType(FindControlByID("memDescription"), ASPxMemo)
        Dim memContacts As ASPxMemo = CType(FindControlByID("memContacts"), ASPxMemo)
        Dim cbResource As ASPxComboBox = CType(FindControlByID("edtResource"), ASPxComboBox)


        Controller.Subject = tbSubject.Text
        Controller.Location = tbLocation.Text
        Controller.Description = memDescription.Text

        Controller.Start = edtStartDate.Date
        Controller.End = edtEndDate.Date
        ' custom fields 
        Controller.ContactInfo = memContacts.Text
        Controller.Price = Convert.ToDouble(tbPrice.Text)
        If cbResource.SelectedItem IsNot Nothing Then
            Controller.ResourceId = cbResource.SelectedItem.Value
        End If

        Dim clientStart As Date = FromClientTime(Controller.Start)
        AssignControllerRecurrenceValues(clientStart)
    End Sub
    Protected Overrides Function CreateAppointmentFormController(ByVal apt As Appointment) As AppointmentFormController
        Return New MyAppointmentFormController(Control, apt)
    End Function
End Class

Public Class MyAppointmentFormController
    Inherits AppointmentFormController

    Private Const ContactInfoFieldName As String = "ContactInfo"
    Private Const PriceFieldName As String = "Price"

    Public Sub New(ByVal control As ASPxScheduler, ByVal apt As Appointment)
        MyBase.New(control, apt)
    End Sub
    Public Property ContactInfo() As String
        Get
            Return CStr(EditedAppointmentCopy.CustomFields(ContactInfoFieldName))
        End Get
        Set(ByVal value As String)
            EditedAppointmentCopy.CustomFields(ContactInfoFieldName) = value
        End Set
    End Property
    Public Property Price() As Double
        Get
            Return CDbl(EditedAppointmentCopy.CustomFields(PriceFieldName))
        End Get
        Set(ByVal value As Double)
            EditedAppointmentCopy.CustomFields(PriceFieldName) = value
        End Set
    End Property

    Private Property SourceContactInfo() As String
        Get
            Return CStr(SourceAppointment.CustomFields(ContactInfoFieldName))
        End Get
        Set(ByVal value As String)
            SourceAppointment.CustomFields(ContactInfoFieldName) = value
        End Set
    End Property
    Private Property SourcePrice() As Double
        Get
            Return CDbl(SourceAppointment.CustomFields(PriceFieldName))
        End Get
        Set(ByVal value As Double)
            SourceAppointment.CustomFields(PriceFieldName) = value
        End Set
    End Property

    Protected Overrides Sub ApplyCustomFieldsValues()
        SourceContactInfo = ContactInfo
        SourcePrice = Price
    End Sub
End Class
