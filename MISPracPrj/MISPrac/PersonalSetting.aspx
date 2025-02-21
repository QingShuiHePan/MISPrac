<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PersonalSetting.aspx.cs" Inherits="MISPrac.PersonalSetting" %>

<%@ Register Assembly="SSPUCore" Namespace="SSPUCore.Controls" TagPrefix="sspu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="float: left; width: 100%; margin-top: 5px;" >
        <sspu:ThemeRoller ID="ThemeRoller1" CssClass="ThemeRoller" runat="server" LabelText="系统样式" AutoPostBack="True"></sspu:ThemeRoller>
    </div>
    
    <br/>
    <div class=".ui-widget" style="float: left; width: 100%; text-align: center; font-weight: bold; font-size: 1.3em;">个人信息：</div>
    <sspu:JDataTable ID="JDataTable1" runat="server" />
</asp:Content>
